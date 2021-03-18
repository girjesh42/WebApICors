using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using WebApiMoviesOdataCF.Data;
using WebApiMoviesOdataCF.Models;

namespace WebApiMoviesOdataCF.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebApiMoviesOdataCF.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Movie>("Movies");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [EnableCors(origins: "https://localhost:44326", headers: "*", methods: "*")]
    public class MoviesController : ODataController
    {
        private MovieDbContext db = new MovieDbContext();

        // GET: odata/Movies
        [EnableQuery]
        public IQueryable<Movie> GetMovies()
        {
            return db.Movies;
        }

        // GET: odata/Movies(5)
        [EnableQuery]
        public SingleResult<Movie> GetMovie([FromODataUri] int key)
        {
            return SingleResult.Create(db.Movies.Where(movie => movie.MId == key));
        }

        // PUT: odata/Movies(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Movie> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Movie movie = db.Movies.Find(key);
            if (movie == null)
            {
                return NotFound();
            }

            patch.Put(movie);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(movie);
        }

        // POST: odata/Movies
        public IHttpActionResult Post(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Movies.Add(movie);
            db.SaveChanges();

            return Created(movie);
        }

        // PATCH: odata/Movies(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Movie> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Movie movie = db.Movies.Find(key);
            if (movie == null)
            {
                return NotFound();
            }

            patch.Patch(movie);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(movie);
        }

        // DELETE: odata/Movies(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Movie movie = db.Movies.Find(key);
            if (movie == null)
            {
                return NotFound();
            }

            db.Movies.Remove(movie);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MovieExists(int key)
        {
            return db.Movies.Count(e => e.MId == key) > 0;
        }
    }
}
