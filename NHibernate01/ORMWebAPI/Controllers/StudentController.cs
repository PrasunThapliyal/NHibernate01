
namespace ORMWebAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using ORM_NHibernate;
    using ORM_NHibernate.BusinessObjects;


    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly NHibernate.ISession _session;

        public StudentController(NHibernate.ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        // GET: api/Student
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            using (var tx = _session.BeginTransaction())
            {
                var students = _session.CreateCriteria<Student>().List<Student>();
                tx.Commit();
                return students;
            }
        }

        // GET: api/Student/5
        [HttpGet("{id}", Name = "Get")]
        public Student Get(int id)
        {
            using (var tx = _session.BeginTransaction())
            {
                var student = _session.Get<Student>(id);
                tx.Commit();
                return student;
            }
        }

        // POST: api/Student
        [HttpPost]
        public void Post([FromBody] Student student)
        {
            using (var tx = _session.BeginTransaction())
            {
                _session.Save(student);
                tx.Commit();
            }
        }


        // PUT: api/Student/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Student student)
        {
            using (var tx = _session.BeginTransaction())
            {
                var studentStorage = _session.Get<Student>(id);

                studentStorage.Firstname = student.Firstname;
                studentStorage.Lastname = student.Lastname;
                _session.Update(studentStorage);
                tx.Commit();
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var tx = _session.BeginTransaction())
            {
                var student = _session.Get<Student>(id);
                _session.Delete(student);
                tx.Commit();
            }
        }

    }
}
