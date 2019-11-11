using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.Prism.Events;
using OnePCommon;
using OnePlanner.Commands;
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

        public OnePlannerController(
            IEventAggregator eventAggregator,
            IUndoManager undoManager)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            _undoManager = undoManager ?? throw new ArgumentNullException(nameof(undoManager));
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
            IFormFile file
            //, 
            //[FromServices] NHibernate.ISession nHibernateSession = null,
            //[FromServices] OnePlannerORM orm,
            //[FromServices] DatabaseCommands databaseCommands
            )
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            //if (orm is null)
            //{
            //    throw new ArgumentNullException(nameof(orm));
            //}

            //if (databaseCommands is null)
            //{
            //    throw new ArgumentNullException(nameof(databaseCommands));
            //}

            //if (nHibernateSession is null)
            //{
            //    throw new ArgumentNullException(nameof(nHibernateSession));
            //}

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
                    //if (nHibernateSession != null)
                    //{
                    //    using (var tx = nHibernateSession.BeginTransaction())
                    //    {
                    //        nHibernateSession.Save(network);
                    //        tx.Commit();
                    //    }
                    //}


                    {
                        {
                            DBConnection dbConnectionSetting = new DBConnection()
                            {
                                DbName = "oneplanner_cdd08082011",
                                Host = "localhost",
                                Password = "password",
                                Port = 3306,
                                User = "root"
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


        private static string GetFileName(IFormFile file) =>
            file.ContentDisposition.Split(';')
                .Select(x => x.Trim())
                .Where(x => x.StartsWith("filename="))
                .Select(x => x.Substring(9).Trim('"'))
                .First();

    }
}
