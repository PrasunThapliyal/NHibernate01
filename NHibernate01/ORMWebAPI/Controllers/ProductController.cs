
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


        // GET: api/Product/Movie
        [HttpGet("Movie")]
        public IEnumerable<Movie> GetMovies()
        {
            using (var tx = _session.BeginTransaction())
            {
                var movies = _session
                    .Query<Movie>()
                    .ToList();
                foreach (var movie in movies)
                {
                    Console.Out.WriteLine("Student: " + movie.Name);
                }
                tx.Commit();

                return movies;
            }

            /*
             * 
                NHibernate:
                    select
                        movie0_.Id as id1_0_,
                        movie0_1_.Name as name2_0_,
                        movie0_1_.description as description3_0_,
                        movie0_1_.UnitPrice as unitprice4_0_,
                        movie0_.Director as director2_2_
                    from
                        Movie movie0_
                    inner join
                        product movie0_1_
                            on movie0_.Id=movie0_1_.id
             * 
             * 
             * */


            /////////////////////
        }


        // GET: api/Product/5
        [HttpGet("{id}", Name = "Get")]
        public Product Get(int id)
        {
            using (var tx = _session.BeginTransaction())
            {
                var student = _session.Get<Book>(id);
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


        // POST: api/Product/Movie
        [HttpPost("Movie")]
        public void Post([FromBody] Movie movie)
        {
            /*
             * 
                {
	                "Name": "Movie with Actors",
	                "Description": "Test Movie",
	                "UnitPrice": 320,
	                "Director": "FF DD",
	                "Actors":
	                [
		                {
			                "Actor":"Actor 1",
			                "Role": "Villian"
		                },
		                {
			                "Actor":"Actor 2",
			                "Role": "Heroine"
		                }
	                ]
                }
             * 
             * 
             * */

            using (var tx = _session.BeginTransaction())
            {
                _session.Save(movie);
                tx.Commit();
            }

            /*
             * 
             * 
                NHibernate:
                    INSERT
                    INTO
                        product
                        (Name, description, UnitPrice, id)
                    VALUES
                        (?p0, ?p1, ?p2, ?p3);
                    ?p0 = 'Movie with Actors' [Type: String (17:0:0)], ?p1 = 'Test Movie' [Type: String (10:0:0)], ?p2 = 320 [Type: Currency (0:0:0)], ?p3 = 32769 [Type: Int32 (0:0:0)]
                NHibernate:
                    INSERT
                    INTO
                        Movie
                        (Director, Id)
                    VALUES
                        (?p0, ?p1);
                    ?p0 = 'FF DD' [Type: String (5:0:0)], ?p1 = 32769 [Type: Int32 (0:0:0)]
                NHibernate:
                    INSERT
                    INTO
                        ActorRole
                        (Actor, Role, id)
                    VALUES
                        (?p0, ?p1, ?p2);
                    ?p0 = 'Actor 1' [Type: String (7:0:0)], ?p1 = 'Villian' [Type: String (7:0:0)], ?p2 = 65536 [Type: Int32 (0:0:0)]
                NHibernate:
                    INSERT
                    INTO
                        ActorRole
                        (Actor, Role, id)
                    VALUES
                        (?p0, ?p1, ?p2);
                    ?p0 = 'Actor 2' [Type: String (7:0:0)], ?p1 = 'Heroine' [Type: String (7:0:0)], ?p2 = 65537 [Type: Int32 (0:0:0)]
                NHibernate:
                    UPDATE
                        ActorRole
                    SET
                        MovieId = ?p0,
                        ActorIndex = ?p1
                    WHERE
                        id = ?p2;
                    ?p0 = 32769 [Type: Int32 (0:0:0)], ?p1 = 0 [Type: Int32 (0:0:0)], ?p2 = 65536 [Type: Int32 (0:0:0)]
                NHibernate:
                    UPDATE
                        ActorRole
                    SET
                        MovieId = ?p0,
                        ActorIndex = ?p1
                    WHERE
                        id = ?p2;
                    ?p0 = 32769 [Type: Int32 (0:0:0)], ?p1 = 1 [Type: Int32 (0:0:0)], ?p2 = 65537 [Type: Int32 (0:0:0)]
             * 
             * */
        }


        // POST: api/Product/TestMovie
        [HttpPost("TestMovie")]
        public void Post()
        {
            var actor1 = new ActorRole()
            {
                Actor = "Shakti Kapoor",
                Role = "Hero"
            };
            var actor2 = new ActorRole()
            {
                Actor = "AB",
                Role = "Villian"
            };

            var movie1 = new Movie()
            {
                Name = "The Burning Train",
                Description = "Test",
                UnitPrice = 310,
                Director = "Quinten Tarantino",
                Actors = new List<ActorRole>() { actor1, actor2 }
            };


            //var book = new Book()
            //{
            //    Name = "Book1",
            //    Description = "N",
            //    UnitPrice = (decimal)200.0,
            //    ISBN = "SGF",
            //    Author = "PPT"
            //};

            using (var tx = _session.BeginTransaction())
            {
                //_session.Save(actor1);
                //_session.Save(actor2);
                _session.Save(movie1);
                //_session.Save(book);
                tx.Commit();
            }

            //using (var tx = _session.BeginTransaction())
            //{
            //    book.Name = "TTT";
            //    _session.Save(book);
            //    tx.Commit();
            //}
        }


        // POST: api/Product/Movie
        [HttpPost("Book")]
        public void Post([FromBody] Book book)
        {
            using (var tx = _session.BeginTransaction())
            {
                _session.Save(book);
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
