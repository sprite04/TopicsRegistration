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
    public class CHUYENNGANHsController : ApiController
    {
        private WEBDbContext db = new WEBDbContext();

        // GET: api/CHUYENNGANHs
        public IQueryable<CHUYENNGANH> GetCHUYENNGANHs()
        {
            return db.CHUYENNGANHs;
        }

        // GET: api/CHUYENNGANHs/5
        [ResponseType(typeof(CHUYENNGANH))]
        public IHttpActionResult GetCHUYENNGANH(int id)
        {
            CHUYENNGANH cHUYENNGANH = db.CHUYENNGANHs.Find(id);
            if (cHUYENNGANH == null)
            {
                return NotFound();
            }

            return Ok(cHUYENNGANH);
        }

        // PUT: api/CHUYENNGANHs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCHUYENNGANH(int id, CHUYENNGANH cHUYENNGANH)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cHUYENNGANH.IdCNganh)
            {
                return BadRequest();
            }

            db.Entry(cHUYENNGANH).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CHUYENNGANHExists(id))
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

        // POST: api/CHUYENNGANHs
        [ResponseType(typeof(CHUYENNGANH))]
        public IHttpActionResult PostCHUYENNGANH(CHUYENNGANH cHUYENNGANH)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CHUYENNGANHs.Add(cHUYENNGANH);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cHUYENNGANH.IdCNganh }, cHUYENNGANH);
        }

        // DELETE: api/CHUYENNGANHs/5
        [ResponseType(typeof(CHUYENNGANH))]
        public IHttpActionResult DeleteCHUYENNGANH(int id)
        {
            CHUYENNGANH cHUYENNGANH = db.CHUYENNGANHs.Find(id);
            if (cHUYENNGANH == null)
            {
                return NotFound();
            }

            db.CHUYENNGANHs.Remove(cHUYENNGANH);
            db.SaveChanges();

            return Ok(cHUYENNGANH);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CHUYENNGANHExists(int id)
        {
            return db.CHUYENNGANHs.Count(e => e.IdCNganh == id) > 0;
        }
    }
}