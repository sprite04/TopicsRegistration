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
    public class QUYENsController : ApiController
    {
        private WEBDbContext db = new WEBDbContext();

        // GET: api/QUYENs
        public IQueryable<QUYEN> GetQUYENs()
        {
            return db.QUYENs;
        }

        // GET: api/QUYENs/5
        [ResponseType(typeof(QUYEN))]
        public IHttpActionResult GetQUYEN(string id)
        {
            QUYEN qUYEN = db.QUYENs.Find(id);
            if (qUYEN == null)
            {
                return NotFound();
            }

            return Ok(qUYEN);
        }

        // PUT: api/QUYENs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQUYEN(string id, QUYEN qUYEN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != qUYEN.IdQuyen)
            {
                return BadRequest();
            }

            db.Entry(qUYEN).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QUYENExists(id))
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

        // POST: api/QUYENs
        [ResponseType(typeof(QUYEN))]
        public IHttpActionResult PostQUYEN(QUYEN qUYEN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.QUYENs.Add(qUYEN);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (QUYENExists(qUYEN.IdQuyen))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = qUYEN.IdQuyen }, qUYEN);
        }

        // DELETE: api/QUYENs/5
        [ResponseType(typeof(QUYEN))]
        public IHttpActionResult DeleteQUYEN(string id)
        {
            QUYEN qUYEN = db.QUYENs.Find(id);
            if (qUYEN == null)
            {
                return NotFound();
            }

            db.QUYENs.Remove(qUYEN);
            db.SaveChanges();

            return Ok(qUYEN);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QUYENExists(string id)
        {
            return db.QUYENs.Count(e => e.IdQuyen == id) > 0;
        }
    }
}