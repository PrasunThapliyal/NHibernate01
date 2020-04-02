
namespace ORMWebAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using ORM_NHibernate;
    using ORM_NHibernate.BusinessObjects;


    [Route("api/[controller]")]
    [ApiController]
    public class NetworkController : ControllerBase
    {
        private readonly NHibernate.ISession _session;

        public NetworkController(NHibernate.ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        // GET: api/OnepNetwork
        [HttpGet]
        public IEnumerable<OnepNetwork> Get()
        {
            // This works, btw
            //using (var tx = _session.BeginTransaction())
            //{
            //    var students = _session.CreateCriteria<Student>().List<Student>();
            //    tx.Commit();
            //    return students;
            //}

            // This is another way
            // Notable: The .Where is not applied at client side .. the SQL query itself has .Where applied .. cool !!
            //
            // This is the localdb dialect
            // NHibernate:
            // /* [expression] */select
            //    student0_.Id as id1_0_,
            //    student0_.Lastname as lastname2_0_,
            //    student0_.Firstname as firstname3_0_
            //from
            //    Student student0_
            //where
            //    student0_.Firstname = @p0;
            //        @p0 = 'Prasun'[Type: String(4000:0:0)]
            //
            // --
            // MYSQL Dialect
            // NHibernate:
            //        select
            //    student0_.id as id1_0_,
            //    student0_.firstname as firstname2_0_,
            //    student0_.lastname as lastname3_0_
            //from
            //    student student0_
            //where
            //    student0_.firstname =? p0;
            //?p0 = 'Prasun'[Type: String(6:0:0)]

            /////////////////////

            using (var tx = _session.BeginTransaction())
            {
                var networks = _session
                    .Query<OnepNetwork>()
                    //.Where(s => s.Name == "Test")
                    .ToList();
                foreach (var network in networks)
                {
                    Console.Out.WriteLine("Network: " + network.Name);
                }
                tx.Commit();

                return networks;
            }

            /*
             * 
                NHibernate:
                    select
                        product0_.id as id1_0_,
                        product0_.Name as name2_0_,
                        product0_.description as description3_0_,
                        product0_.UnitPrice as unitprice4_0_,
                        product0_1_.ISBN as isbn2_1_,
                        product0_1_.Author as author3_1_,
                        product0_2_.Director as director2_2_,
                        case
                            when product0_1_.Id is not null then 1
                            when product0_2_.Id is not null then 2
                            when product0_.id is not null then 0
                        end as clazz_
                    from
                        product product0_
                    left outer join
                        Book product0_1_
                            on product0_.id=product0_1_.Id
                    left outer join
                        Movie product0_2_
                            on product0_.id=product0_2_.Id
             * 
             * 
             * */


            /////////////////////
        }


        // GET: api/OnepNetwork/5
        [HttpGet("{id}", Name = "Get")]
        public OnepNetwork Get(uint id)
        {
            using (var tx = _session.BeginTransaction())
            {
                var onepNetwork = _session.Get<OnepNetwork>(id);
                tx.Commit();
                Debug.WriteLine(ReferenceEquals(onepNetwork.OnepTerminationpoints[0].OnepAmpRole, onepNetwork.OnepAmptps[0]));
                Debug.WriteLine($"{onepNetwork.OnepTerminationpoints[0].Id}, {onepNetwork.OnepAmptps[0].Id}");
                return null;
            }
        }

        // POST: api/OnepNetwork
        [HttpPost]
        public object Post()
        {
            OnepNetwork onepNetwork = null;
            {
                onepNetwork = new OnepNetwork()
                {
                    McpProjectId = Guid.NewGuid().ToString(),
                    Name = "Test 01"
                };
                var onepFiberTL1 = new OnepFibertl()
                {
                    Name = "FS01_1",
                    Length = 30,
                    Loss = 30 * 0.25,
                    OnepNetwork = onepNetwork
                };
                var onepFiberTL2 = new OnepFibertl()
                {
                    Name = "FS01_2",
                    Length = 30,
                    Loss = 30 * 0.25,
                    OnepNetwork = onepNetwork
                };
                onepFiberTL1.OnepTopologicallinkMemberByUniMate = onepFiberTL2;
                onepFiberTL2.OnepTopologicallinkMemberByUniMate = onepFiberTL1;

                onepNetwork.OnepFibertls.Add(onepFiberTL1);
                onepNetwork.OnepFibertls.Add(onepFiberTL2);

                /////////

                var onepTP1 = new OnepTerminationpoint()
                {
                    Name = "TP 01",
                    Ptp = 1,
                    Role = 2,
                    OnepNetwork = onepNetwork
                };
                var onepTP2 = new OnepTerminationpoint()
                {
                    Name = "TP 02",
                    Ptp = 1,
                    Role = 2,
                    OnepNetwork = onepNetwork
                };

                onepNetwork.OnepTerminationpoints.Add(onepTP1);
                onepNetwork.OnepTerminationpoints.Add(onepTP2);

                onepTP1.OnepTopologicallinksForAEndTP.Add(onepFiberTL1);
                onepTP1.OnepTopologicallinksForZEndTP.Add(onepFiberTL2);
                onepFiberTL1.OnepTerminationpointByAEndTP = onepTP1;
                onepFiberTL2.OnepTerminationpointByAEndTP = onepTP2;

                onepTP2.OnepTopologicallinksForAEndTP.Add(onepFiberTL2);
                onepTP2.OnepTopologicallinksForZEndTP.Add(onepFiberTL1);
                onepFiberTL1.OnepTerminationpointByZEndTP = onepTP2;
                onepFiberTL2.OnepTerminationpointByZEndTP = onepTP1;

                var onepAmpTP1 = new OnepAmptp()
                {
                    TargetGain = 2.0,
                    OnepNetwork = onepNetwork,
                    OnepTerminationpoint = onepTP1
                };
                onepTP1.OnepAmpRole = onepAmpTP1;
                onepNetwork.OnepAmptps.Add(onepAmpTP1);

                var onepAmpTP2 = new OnepAmptp()
                {
                    TargetGain = 2.0,
                    OnepNetwork = onepNetwork,
                    OnepTerminationpoint = onepTP2
                };
                onepTP2.OnepAmpRole = onepAmpTP2;
                onepNetwork.OnepAmptps.Add(onepAmpTP2);

            }

            // TODO : Json serialization not working due to circular references

            //var onepNetworkAsJsonString = JsonConvert.SerializeObject(onepNetwork);
            //Debug.WriteLine(onepNetworkAsJsonString);

            using (var tx = _session.BeginTransaction())
            {
                try
                {
                    //var table_name = "onep_amptp";
                    //int maxoid = 2;
                    //int count = 2;
                    //foreach (var amptp in onepNetwork.OnepAmptps)
                    //{
                    //    _session.CreateSQLQuery($"insert into {table_name}  (oid) select oid + {maxoid} from onep_insertoid where oid <= {count}").ExecuteUpdate();
                    //}
                    //_session.Save(onepNetwork.OnepTerminationpoints[0]);
                    //_session.Save(onepNetwork.OnepTerminationpoints[1]);
                    //_session.Save(onepNetwork.OnepAmptps[0]);
                    //_session.Save(onepNetwork.OnepAmptps[1]);
                    _session.Save(onepNetwork);
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    tx.Rollback();
                }
            }

            return new { Id = onepNetwork.Id };
        }

        // PUT: api/OnepNetwork/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] OnepNetwork product)
        {
        }

        // DELETE: api/OnepNetwork/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var tx = _session.BeginTransaction())
            {
                var onepNetwork = _session.Get<OnepNetwork>(id);
                _session.Delete(onepNetwork);
                tx.Commit();
            }
        }

    }
}
