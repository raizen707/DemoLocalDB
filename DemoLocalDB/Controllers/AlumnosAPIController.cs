using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using DemoLocalDB.Models;

namespace DemoLocalDB.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using DemoLocalDB.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Alumno>("AlumnosAPI");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class AlumnosAPIController : ODataController
    {
        private UniversidadEntities db = new UniversidadEntities();

        // GET: odata/AlumnosAPI
        [EnableQuery]
        public IQueryable<Alumno> GetAlumnosAPI()
        {
            return db.Alumnoes;
        }

        // GET: odata/AlumnosAPI(5)
        [EnableQuery]
        public SingleResult<Alumno> GetAlumno([FromODataUri] int key)
        {
            return SingleResult.Create(db.Alumnoes.Where(alumno => alumno.Id == key));
        }

        // PUT: odata/AlumnosAPI(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Alumno> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Alumno alumno = db.Alumnoes.Find(key);
            if (alumno == null)
            {
                return NotFound();
            }

            patch.Put(alumno);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlumnoExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(alumno);
        }

        // POST: odata/AlumnosAPI
        public IHttpActionResult Post(Alumno alumno)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Alumnoes.Add(alumno);
            db.SaveChanges();

            return Created(alumno);
        }

        // PATCH: odata/AlumnosAPI(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Alumno> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Alumno alumno = db.Alumnoes.Find(key);
            if (alumno == null)
            {
                return NotFound();
            }

            patch.Patch(alumno);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlumnoExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(alumno);
        }

        // DELETE: odata/AlumnosAPI(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Alumno alumno = db.Alumnoes.Find(key);
            if (alumno == null)
            {
                return NotFound();
            }

            db.Alumnoes.Remove(alumno);
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

        private bool AlumnoExists(int key)
        {
            return db.Alumnoes.Count(e => e.Id == key) > 0;
        }
    }
}
