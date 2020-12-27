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
    public class LOAIDETAIsController : ApiController
    {
        private WEBDbContext db = new WEBDbContext();

        // GET: api/LOAIDETAIs
        public IQueryable<LOAIDETAI> GetLOAIDETAIs()
        {
            return db.LOAIDETAIs;
        }

        // GET: api/LOAIDETAIs/5
        [ResponseType(typeof(LOAIDETAI))]
        public IHttpActionResult GetLOAIDETAI(int id)
        {
            LOAIDETAI lOAIDETAI = db.LOAIDETAIs.Find(id);
            if (lOAIDETAI == null)
            {
                return NotFound();
            }

            return Ok(lOAIDETAI);
        }

        // PUT: api/LOAIDETAIs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLOAIDETAI(int id, LOAIDETAI lOAIDETAI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lOAIDETAI.IdLoai)
            {
                return BadRequest();
            }

            db.Entry(lOAIDETAI).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LOAIDETAIExists(id))
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

        // POST: api/LOAIDETAIs
        [ResponseType(typeof(LOAIDETAI))]
        public IHttpActionResult PostLOAIDETAI(LOAIDETAI lOAIDETAI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LOAIDETAIs.Add(lOAIDETAI);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = lOAIDETAI.IdLoai }, lOAIDETAI);
        }

        // DELETE: api/LOAIDETAIs/5
        [ResponseType(typeof(LOAIDETAI))]
        public IHttpActionResult DeleteLOAIDETAI(int id)
        {
            LOAIDETAI lOAIDETAI = db.LOAIDETAIs.Find(id);
            if (lOAIDETAI == null)
            {
                return NotFound();
            }

            db.LOAIDETAIs.Remove(lOAIDETAI);
            db.SaveChanges();

            return Ok(lOAIDETAI);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LOAIDETAIExists(int id)
        {
            return db.LOAIDETAIs.Count(e => e.IdLoai == id) > 0;
        }
    }
}