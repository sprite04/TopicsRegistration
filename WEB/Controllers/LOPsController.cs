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
    public class LOPsController : ApiController
    {
        private WEBDbContext db = new WEBDbContext();

        // GET: api/LOPs
        public IQueryable<LOP> GetLOPs()
        {
            return db.LOPs;
        }

        // GET: api/LOPs/5
        [ResponseType(typeof(LOP))]
        public IHttpActionResult GetLOP(int id)
        {
            LOP lOP = db.LOPs.Find(id);
            if (lOP == null)
            {
                return NotFound();
            }

            return Ok(lOP);
        }

        // PUT: api/LOPs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLOP(int id, LOP lOP)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lOP.IdLop)
            {
                return BadRequest();
            }

            db.Entry(lOP).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LOPExists(id))
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

        // POST: api/LOPs
        [ResponseType(typeof(LOP))]
        public IHttpActionResult PostLOP(LOP lOP)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LOPs.Add(lOP);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = lOP.IdLop }, lOP);
        }

        // DELETE: api/LOPs/5
        [ResponseType(typeof(LOP))]
        public IHttpActionResult DeleteLOP(int id)
        {
            LOP lOP = db.LOPs.Find(id);
            if (lOP == null)
            {
                return NotFound();
            }

            db.LOPs.Remove(lOP);
            db.SaveChanges();

            return Ok(lOP);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LOPExists(int id)
        {
            return db.LOPs.Count(e => e.IdLop == id) > 0;
        }
    }
}