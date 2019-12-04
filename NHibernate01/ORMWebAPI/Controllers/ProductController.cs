
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
    public class ProductController : ControllerBase
    {
        private readonly NHibernate.ISession _session;

        public ProductController(NHibernate.ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        // GET: api/Product
        [HttpGet]
        public IEnumerable<Product> Get()
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
                var products = _session
                    .Query<Product>()
                    //.Where(s => s.Name == "Test")
                    .ToList();
                foreach (var product in products)
                {
                    Console.Out.WriteLine("Student: " + product.Name);
                }
                tx.Commit();

                return products;
            }

            /////////////////////
        }

        // GET: api/Product/5
        [HttpGet("{id}", Name = "Get")]
        public Product Get(int id)
        {
            using (var tx = _session.BeginTransaction())
            {
                var student = _session.Get<Product>(id);
                tx.Commit();
                return student;
            }
        }

        // POST: api/Product
        [HttpPost]
        public void Post([FromBody] Product product)
        {
            using (var tx = _session.BeginTransaction())
            {
                _session.Save(product);
                tx.Commit();
            }
        }


        // PUT: api/Product/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Product product)
        {
            using (var tx = _session.BeginTransaction())
            {
                var productStorage = _session.Get<Product>(id);

                productStorage.Name = product.Name;
                productStorage.Description = product.Description;
                productStorage.UnitPrice = product.UnitPrice;
                _session.Update(productStorage);
                tx.Commit();
            }
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var tx = _session.BeginTransaction())
            {
                var product = _session.Get<Product>(id);
                _session.Delete(product);
                tx.Commit();
            }
        }

    }
}
