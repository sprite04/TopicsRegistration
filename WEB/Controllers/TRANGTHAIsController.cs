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
    public class TRANGTHAIsController : ApiController
    {
        private WEBDbContext db = new WEBDbContext();

        // GET: api/TRANGTHAIs
        public IQueryable<TRANGTHAI> GetTRANGTHAIs()
        {
            return db.TRANGTHAIs;
        }

        // GET: api/TRANGTHAIs/5
        [ResponseType(typeof(TRANGTHAI))]
        public IHttpActionResult GetTRANGTHAI(int id)
        {
            TRANGTHAI tRANGTHAI = db.TRANGTHAIs.Find(id);
            if (tRANGTHAI == null)
            {
                return NotFound();
            }

            return Ok(tRANGTHAI);
        }

        // PUT: api/TRANGTHAIs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTRANGTHAI(int id, TRANGTHAI tRANGTHAI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tRANGTHAI.IdTT)
            {
                return BadRequest();
            }

            db.Entry(tRANGTHAI).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TRANGTHAIExists(id))
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

        // POST: api/TRANGTHAIs
        [ResponseType(typeof(TRANGTHAI))]
        public IHttpActionResult PostTRANGTHAI(TRANGTHAI tRANGTHAI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TRANGTHAIs.Add(tRANGTHAI);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tRANGTHAI.IdTT }, tRANGTHAI);
        }

        // DELETE: api/TRANGTHAIs/5
        [ResponseType(typeof(TRANGTHAI))]
        public IHttpActionResult DeleteTRANGTHAI(int id)
        {
            TRANGTHAI tRANGTHAI = db.TRANGTHAIs.Find(id);
            if (tRANGTHAI == null)
            {
                return NotFound();
            }

            db.TRANGTHAIs.Remove(tRANGTHAI);
            db.SaveChanges();

            return Ok(tRANGTHAI);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TRANGTHAIExists(int id)
        {
            return db.TRANGTHAIs.Count(e => e.IdTT == id) > 0;
        }
    }
}