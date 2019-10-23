
namespace ORMWebAPI.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using ORM_NHibernate;
    using ORM_NHibernate.BusinessObjects;


    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        // GET: api/Student
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            DBSession dBSession = new DBSession();
            var sefact = dBSession.GetSessionFactory();

            using (var session = sefact.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    var students = session.CreateCriteria<Student>().List<Student>();
                    tx.Commit();
                    return students;
                }
            }
        }

        // GET: api/Student/5
        [HttpGet("{id}", Name = "Get")]
        public Student Get(int id)
        {
            DBSession dBSession = new DBSession();
            var sefact = dBSession.GetSessionFactory();

            using (var session = sefact.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    var student = session.Get<Student>(id);
                    tx.Commit();
                    return student;
                }
            }
        }

        // POST: api/Student
        [HttpPost]
        public void Post([FromBody] Student student)
        {
            DBSession dBSession = new DBSession();
            var sefact = dBSession.GetSessionFactory();

            using (var session = sefact.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    session.Save(student);
                    tx.Commit();
                }
            }
        }
    

        // PUT: api/Student/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Student student)
        {
            DBSession dBSession = new DBSession();
            var sefact = dBSession.GetSessionFactory();

            using (var session = sefact.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    var studentStorage = session.Get<Student>(id);

                    studentStorage.Firstname = student.Firstname;
                    studentStorage.Lastname = student.Lastname;
                    session.Update(studentStorage);
                    tx.Commit();
                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            DBSession dBSession = new DBSession();
            var sefact = dBSession.GetSessionFactory();

            using (var session = sefact.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    var student = session.Get<Student>(id);
                    session.Delete(student);
                    tx.Commit();
                }
            }
        }
    }
}
