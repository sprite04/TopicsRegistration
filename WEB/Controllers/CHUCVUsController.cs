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
    public class CHUCVUsController : ApiController
    {
        private WEBDbContext db = new WEBDbContext();

        // GET: api/CHUCVUs
        public IQueryable<CHUCVU> GetCHUCVUs()
        {
            return db.CHUCVUs;
        }

        // GET: api/CHUCVUs/5
        [ResponseType(typeof(CHUCVU))]
        public IHttpActionResult GetCHUCVU(int id)
        {
            CHUCVU cHUCVU = db.CHUCVUs.Find(id);
            if (cHUCVU == null)
            {
                return NotFound();
            }

            return Ok(cHUCVU);
        }

        // PUT: api/CHUCVUs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCHUCVU(int id, CHUCVU cHUCVU)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cHUCVU.IdCVu)
            {
                return BadRequest();
            }

            db.Entry(cHUCVU).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CHUCVUExists(id))
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

        // POST: api/CHUCVUs
        [ResponseType(typeof(CHUCVU))]
        public IHttpActionResult PostCHUCVU(CHUCVU cHUCVU)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CHUCVUs.Add(cHUCVU);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cHUCVU.IdCVu }, cHUCVU);
        }

        // DELETE: api/CHUCVUs/5
        [ResponseType(typeof(CHUCVU))]
        public IHttpActionResult DeleteCHUCVU(int id)
        {
            CHUCVU cHUCVU = db.CHUCVUs.Find(id);
            if (cHUCVU == null)
            {
                return NotFound();
            }

            db.CHUCVUs.Remove(cHUCVU);
            db.SaveChanges();

            return Ok(cHUCVU);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CHUCVUExists(int id)
        {
            return db.CHUCVUs.Count(e => e.IdCVu == id) > 0;
        }
    }
}