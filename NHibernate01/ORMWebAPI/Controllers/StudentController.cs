
namespace ORMWebAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

            using (var tx = _session.BeginTransaction())
            {
                var students = _session
                    .Query<Student>()
                    .Where(s => s.Firstname == "Prasun")
                    .ToList();
                foreach (var student in students)
                {
                    Console.Out.WriteLine("Student: " + student.Firstname);
                }
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
