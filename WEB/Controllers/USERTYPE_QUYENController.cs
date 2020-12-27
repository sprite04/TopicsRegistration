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
    public class USERTYPE_QUYENController : ApiController
    {
        private WEBDbContext db = new WEBDbContext();

        // GET: api/USERTYPE_QUYEN
        public IQueryable<USERTYPE_QUYEN> GetUSERTYPE_QUYEN()
        {
            return db.USERTYPE_QUYEN;
        }

        // GET: api/USERTYPE_QUYEN/5
        [ResponseType(typeof(USERTYPE_QUYEN))]
        public IHttpActionResult GetUSERTYPE_QUYEN(int idUT, string quyen)
        {
            USERTYPE_QUYEN uSERTYPE_QUYEN = db.USERTYPE_QUYEN.SingleOrDefault(x => x.IdUT == idUT && x.Quyen == quyen);
            if (uSERTYPE_QUYEN == null)
            {
                return NotFound();
            }

            return Ok(uSERTYPE_QUYEN);
        }

        // PUT: api/USERTYPE_QUYEN/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUSERTYPE_QUYEN(int idUT,string quyen, USERTYPE_QUYEN uSERTYPE_QUYEN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idUT != uSERTYPE_QUYEN.IdUT || quyen!= uSERTYPE_QUYEN.Quyen)
            {
                return BadRequest();
            }

            db.Entry(uSERTYPE_QUYEN).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!USERTYPE_QUYENExists(idUT,quyen))
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

        // POST: api/USERTYPE_QUYEN
        [ResponseType(typeof(USERTYPE_QUYEN))]
        public IHttpActionResult PostUSERTYPE_QUYEN(USERTYPE_QUYEN uSERTYPE_QUYEN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.USERTYPE_QUYEN.Add(uSERTYPE_QUYEN);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (USERTYPE_QUYENExists(uSERTYPE_QUYEN.IdUT,uSERTYPE_QUYEN.Quyen))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = uSERTYPE_QUYEN.IdUT }, uSERTYPE_QUYEN);
        }

        // DELETE: api/USERTYPE_QUYEN/5
        [ResponseType(typeof(USERTYPE_QUYEN))]
        public IHttpActionResult DeleteUSERTYPE_QUYEN(int idUT, string quyen)
        {
            USERTYPE_QUYEN uSERTYPE_QUYEN = db.USERTYPE_QUYEN.SingleOrDefault(x => x.IdUT == idUT && x.Quyen == quyen);
            if (uSERTYPE_QUYEN == null)
            {
                return NotFound();
            }

            db.USERTYPE_QUYEN.Remove(uSERTYPE_QUYEN);
            db.SaveChanges();

            return Ok(uSERTYPE_QUYEN);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool USERTYPE_QUYENExists(int idUT,string quyen)
        {
            return db.USERTYPE_QUYEN.Count(e => e.IdUT == idUT && e.Quyen==quyen) > 0;
        }
    }
}