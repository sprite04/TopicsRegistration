using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WEB.Models;

namespace WEB.Controllers
{
    public class NIENKHOAsController : ApiController
    {
        private WEBDbContext db = new WEBDbContext();

        // GET: api/NIENKHOAs
        public IQueryable<NIENKHOA> GetNIENKHOAs()
        {
            return db.NIENKHOAs;
        }

        // GET: api/NIENKHOAs/5
        [ResponseType(typeof(NIENKHOA))]
        public IHttpActionResult GetNIENKHOA(int id)
        {
            NIENKHOA nIENKHOA = db.NIENKHOAs.Find(id);
            if (nIENKHOA == null)
            {
                return NotFound();
            }

            return Ok(nIENKHOA);
        }

        // PUT: api/NIENKHOAs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNIENKHOA(int id, NIENKHOA nIENKHOA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != nIENKHOA.IdNK)
            {
                return BadRequest();
            }

            db.Entry(nIENKHOA).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NIENKHOAExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/NIENKHOAs
        [ResponseType(typeof(NIENKHOA))]
        public IHttpActionResult PostNIENKHOA(NIENKHOA nIENKHOA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.NIENKHOAs.Add(nIENKHOA);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = nIENKHOA.IdNK }, nIENKHOA);
        }

        // DELETE: api/NIENKHOAs/5
        [ResponseType(typeof(NIENKHOA))]
        public IHttpActionResult DeleteNIENKHOA(int id)
        {
            NIENKHOA nIENKHOA = db.NIENKHOAs.Find(id);
            if (nIENKHOA == null)
            {
                return NotFound();
            }

            db.NIENKHOAs.Remove(nIENKHOA);
            db.SaveChanges();

            return Ok(nIENKHOA);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NIENKHOAExists(int id)
        {
            return db.NIENKHOAs.Count(e => e.IdNK == id) > 0;
        }
    }
}