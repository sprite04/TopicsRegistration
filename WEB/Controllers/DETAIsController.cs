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
    public class DETAIsController : ApiController
    {
        private WEBDbContext db = new WEBDbContext();

        // GET: api/DETAIs
        public IQueryable<DETAI> GetDETAIs()
        {
            return db.DETAIs;
        }

        // GET: api/DETAIs/5
        [ResponseType(typeof(DETAI))]
        public IHttpActionResult GetDETAI(int id)
        {
            DETAI dETAI = db.DETAIs.Find(id);
            if (dETAI == null)
            {
                return NotFound();
            }

            return Ok(dETAI);
        }

        // PUT: api/DETAIs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDETAI(int id, DETAI dETAI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dETAI.IdDeTai)
            {
                return BadRequest();
            }

            db.Entry(dETAI).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DETAIExists(id))
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

        // POST: api/DETAIs
        [ResponseType(typeof(DETAI))]
        public IHttpActionResult PostDETAI(DETAI dETAI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DETAIs.Add(dETAI);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dETAI.IdDeTai }, dETAI);
        }

        // DELETE: api/DETAIs/5
        [ResponseType(typeof(DETAI))]
        public IHttpActionResult DeleteDETAI(int id)
        {
            DETAI dETAI = db.DETAIs.Find(id);
            if (dETAI == null)
            {
                return NotFound();
            }

            db.DETAIs.Remove(dETAI);
            db.SaveChanges();

            return Ok(dETAI);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DETAIExists(int id)
        {
            return db.DETAIs.Count(e => e.IdDeTai == id) > 0;
        }
    }
}