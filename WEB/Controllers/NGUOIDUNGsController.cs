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
    public class NGUOIDUNGsController : ApiController
    {
        private WEBDbContext db = new WEBDbContext();

        // GET: api/NGUOIDUNGs
        public IQueryable<NGUOIDUNG> GetNGUOIDUNGs()
        {
            return db.NGUOIDUNGs;
        }

        // GET: api/NGUOIDUNGs/5
        [ResponseType(typeof(NGUOIDUNG))]
        public IHttpActionResult GetNGUOIDUNG(int id)
        {
            NGUOIDUNG nGUOIDUNG = db.NGUOIDUNGs.Find(id);
            if (nGUOIDUNG == null)
            {
                return NotFound();
            }

            return Ok(nGUOIDUNG);
        }

        // PUT: api/NGUOIDUNGs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNGUOIDUNG(int id, NGUOIDUNG nGUOIDUNG)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != nGUOIDUNG.IdUser)
            {
                return BadRequest();
            }

            db.Entry(nGUOIDUNG).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NGUOIDUNGExists(id))
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

        // POST: api/NGUOIDUNGs
        [ResponseType(typeof(NGUOIDUNG))]
        public IHttpActionResult PostNGUOIDUNG(NGUOIDUNG nGUOIDUNG)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.NGUOIDUNGs.Add(nGUOIDUNG);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = nGUOIDUNG.IdUser }, nGUOIDUNG);
        }

        // DELETE: api/NGUOIDUNGs/5
        [ResponseType(typeof(NGUOIDUNG))]
        public IHttpActionResult DeleteNGUOIDUNG(int id)
        {
            NGUOIDUNG nGUOIDUNG = db.NGUOIDUNGs.Find(id);
            if (nGUOIDUNG == null)
            {
                return NotFound();
            }

            db.NGUOIDUNGs.Remove(nGUOIDUNG);
            db.SaveChanges();

            return Ok(nGUOIDUNG);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NGUOIDUNGExists(int id)
        {
            return db.NGUOIDUNGs.Count(e => e.IdUser == id) > 0;
        }
    }
}