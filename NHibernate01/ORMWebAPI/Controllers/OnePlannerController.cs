using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ciena.BluePlanet.OnePlannerRestLibrary.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.Prism.Events;
using OnePCommon;
using OnePlanner.Commands;
using OnePlanner.CommonCS.Base;
using OnePlanner.CommonCS.Commands;
using OnePlanner.CommonCS.DummyWrappers.DbWrapper;
using OnePlanner.CommonCS.Logging;
using OnePlanner.DataAbstractionLayer.DbWrapperImplementations;
using OnePlanner.OrmNhib;
using OnePlanner.OrmNhib.BusinessObjects;

namespace ORMWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnePlannerController : ControllerBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IUndoManager _undoManager;
        private readonly IRptVersionUtility _rptVersionUtility;

        public OnePlannerController(
            IEventAggregator eventAggregator,
            IUndoManager undoManager,
            IRptVersionUtility rptVersionUtility)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            _undoManager = undoManager ?? throw new ArgumentNullException(nameof(undoManager));
            _rptVersionUtility = rptVersionUtility ?? throw new ArgumentNullException(nameof(rptVersionUtility));
        }

        // GET: api/OnePlanner
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/OnePlanner/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/OnePlanner
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/OnePlanner/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/OnePlanner/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("submitNetwork")]
        public async Task<IActionResult> SubmitNetwork(
            IFormFile file,
            [FromForm] string ProjectName,
            [FromForm] string NetworkName,
            [FromForm] string MySQLHost,
            [FromForm] string MySQLUser,
            [FromForm] string MySQLPassword,
            [FromForm] int MySQLPort)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            //if (ProjectName is null)
            //{
            //    throw new ArgumentNullException(nameof(ProjectName));
            //}

            if (NetworkName is null)
            {
                throw new ArgumentNullException(nameof(NetworkName));
            }

            if (MySQLHost is null)
            {
                throw new ArgumentNullException(nameof(MySQLHost));
            }

            if (MySQLUser is null)
            {
                throw new ArgumentNullException(nameof(MySQLUser));
            }

            if (MySQLPassword is null)
            {
                throw new ArgumentNullException(nameof(MySQLPassword));
            }

            if (file != null && file.Length > 0)
            {
                var fileName = GetFileName(file);

                if (!fileName.EndsWith(".onep") && !fileName.EndsWith(".opnc"))
                {
                    throw new InvalidOperationException("File must be a OnePlanner or OnePlanner Capture file (*.onep, *.opnc)");
                }

                try
                {
                    bool upgradeWarnings;
                    Version fileVersion;
                    string layerType;
                    bool oneControlImport;
                    OnepNetwork network = null;

                    using (var fileStream = file.OpenReadStream())
                    {
                        if (fileName.ToLower().EndsWith(".opnc"))
                        {
                            //it's an encrypted file, need to decrypt it first
                            using (var streamReader = new BinaryReader(fileStream))
                            {
                                byte[] encryptedBuff = null;
                                using (var memstream = new MemoryStream())
                                {
                                    streamReader.BaseStream.CopyTo(memstream);
                                    encryptedBuff = memstream.ToArray();
                                }

                                byte[] decryptedBuff = null;
                                EncryptionFunctions.DecryptByteArray(encryptedBuff, ref decryptedBuff);

                                using (var memStream = new MemoryStream(decryptedBuff))
                                {
                                    network = OrmCommands.OpenFromStream(memStream, _undoManager, _eventAggregator, out upgradeWarnings, out fileVersion, out layerType, out oneControlImport);
                                    network.IsPncNetwork = true;
                                }
                            }
                        }
                        else
                        {
                            network = OrmCommands.OpenFromStream(fileStream, _undoManager, _eventAggregator, out upgradeWarnings, out fileVersion, out layerType, out oneControlImport);
                        }
                    }

                    //McpProjectId is used in the 1P client only and is reset once the network comes to MCP so that it does not get saved in the MCP.
                    network.McpProjectId = null;

                    {
                        // Save to Database
                        network.Name = NetworkName;
                        DBConnection dbConnectionSetting = new DBConnection()
                        {
                            DbName = string.IsNullOrEmpty(ProjectName) ? $"onep_{DateTime.Now.ToString("yyyyMMddHHmmss")}" : ProjectName,
                            Host = MySQLHost,
                            User = MySQLUser,
                            Password = MySQLPassword,
                            Port = MySQLPort
                        };

                        var serverFactory = new CDBInterfaceFactory();
                        var dbServer = serverFactory.Create(
                            UEDBServerType.MySQL,
                            dbConnectionSetting.Host,
                            dbConnectionSetting.User,
                            dbConnectionSetting.Password,
                            dbConnectionSetting.Port);

                        bool blnValidationFailed = false;
                        bool bNeedToUpdateSchema = false;
                        var theOrm = new OnePlannerORM(dbServer, dbConnectionSetting.DbName, out blnValidationFailed, bNeedToUpdateSchema);
                        var databaseCommands = new DatabaseCommands(theOrm, _undoManager, _eventAggregator);
                        if (databaseCommands != null)
                        {
                            databaseCommands.SaveOrUpdateNetwork(network);
                        }
                    }

                }
                catch (FileLoadException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    throw;
                }
            }

            await Task.Delay(0);

            return Ok();
        }


        [HttpGet("GetNetwork")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetNetwork(
            string ProjectName,
            //string NetworkName,
            int NetworkId,
            string MySQLHost,
            string MySQLUser,
            string MySQLPassword,
            int MySQLPort)
        {
            DBConnection dbConnectionSetting = new DBConnection()
            {
                DbName = string.IsNullOrEmpty(ProjectName) ? $"onep_{DateTime.Now.ToString("yyyyMMddHHmmss")}" : ProjectName,
                Host = MySQLHost,
                User = MySQLUser,
                Password = MySQLPassword,
                Port = MySQLPort
            };

            var serverFactory = new CDBInterfaceFactory();
            var dbServer = serverFactory.Create(
                UEDBServerType.MySQL,
                dbConnectionSetting.Host,
                dbConnectionSetting.User,
                dbConnectionSetting.Password,
                dbConnectionSetting.Port);

            bool blnValidationFailed = false;
            bool bNeedToUpdateSchema = false;
            var theOrm = new OnePlannerORM(dbServer, dbConnectionSetting.DbName, out blnValidationFailed, bNeedToUpdateSchema);
            var databaseCommands = new DatabaseCommands(theOrm, _undoManager, _eventAggregator);
            if (databaseCommands != null)
            {
                var onepNetwork = databaseCommands.LoadNetwork((uint)NetworkId);
                var result = GetFileStreamResult(onepNetwork);
                await Task.Yield();
                return result;
            }
            return null;
        }

        private static string GetFileName(IFormFile file) =>
            file.ContentDisposition.Split(';')
                .Select(x => x.Trim())
                .Where(x => x.StartsWith("filename="))
                .Select(x => x.Substring(9).Trim('"'))
                .First();



        private FileStreamResult GetFileStreamResult(OnepNetwork onepNetwork, LayerType layerType = LayerType.PHOTONIC_LAYER, string fileSourceWithVersion = "")
        {
            if (!string.IsNullOrEmpty(onepNetwork.Notes))
            {
                Response.Headers.Add("Warning", "File contains some issues.");
            }

            // stream the onep file
            var tempFileName = Guid.NewGuid().ToString() + ".onep";
            var tempFilePath = Path.Combine(Path.GetTempPath(), tempFileName);
            var uiVersion = OnePlanner.CommonCS.Utils.CVersion.AsVersion();
            string layerTypeStr = Enum.GetName(typeof(LayerType), layerType);

            if (string.IsNullOrEmpty(fileSourceWithVersion))
                fileSourceWithVersion = _rptVersionUtility.RptAppVersion();

            OrmCommands.SaveToFile(onepNetwork, tempFilePath, uiVersion, _eventAggregator, _undoManager, false, layerTypeStr, false, fileSourceWithVersion);

            var fs = new FileStream(tempFilePath, FileMode.Open);

            var result = new FileStreamResult(fs, "application/octet-stream")
            {

                // This field is set so that the mvc filter in DownloadFileResultFilter.cs can use it to delete
                // this temporary file. This is actually a misuse of the field since it's supposed to represent
                // what the client should name the file on its client computer. But this is an internal service
                // used by onep-proxy and viability and they ignore this field.
                FileDownloadName = tempFileName
            };

            return result;
        }

    }
}
