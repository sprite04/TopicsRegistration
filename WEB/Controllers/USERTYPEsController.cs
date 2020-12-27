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
    public class USERTYPEsController : ApiController
    {
        private WEBDbContext db = new WEBDbContext();

        // GET: api/USERTYPEs
        public IQueryable<USERTYPE> GetUSERTYPEs()
        {
            return db.USERTYPEs;
        }

        // GET: api/USERTYPEs/5
        [ResponseType(typeof(USERTYPE))]
        public IHttpActionResult GetUSERTYPE(int id)
        {
            USERTYPE uSERTYPE = db.USERTYPEs.Find(id);
            if (uSERTYPE == null)
            {
                return NotFound();
            }

            return Ok(uSERTYPE);
        }

        // PUT: api/USERTYPEs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUSERTYPE(int id, USERTYPE uSERTYPE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uSERTYPE.IdUT)
            {
                return BadRequest();
            }

            db.Entry(uSERTYPE).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!USERTYPEExists(id))
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

        // POST: api/USERTYPEs
        [ResponseType(typeof(USERTYPE))]
        public IHttpActionResult PostUSERTYPE(USERTYPE uSERTYPE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.USERTYPEs.Add(uSERTYPE);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = uSERTYPE.IdUT }, uSERTYPE);
        }

        // DELETE: api/USERTYPEs/5
        [ResponseType(typeof(USERTYPE))]
        public IHttpActionResult DeleteUSERTYPE(int id)
        {
            USERTYPE uSERTYPE = db.USERTYPEs.Find(id);
            if (uSERTYPE == null)
            {
                return NotFound();
            }

            db.USERTYPEs.Remove(uSERTYPE);
            db.SaveChanges();

            return Ok(uSERTYPE);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool USERTYPEExists(int id)
        {
            return db.USERTYPEs.Count(e => e.IdUT == id) > 0;
        }
    }
}