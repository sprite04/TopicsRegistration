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
    public class CAUHINHsController : ApiController
    {
        private WEBDbContext db = new WEBDbContext();

        // GET: api/CAUHINHs
        public IQueryable<CAUHINH> GetCAUHINHs()
        {
            return db.CAUHINHs;
        }

        // GET: api/CAUHINHs/5
        [ResponseType(typeof(CAUHINH))]
        public IHttpActionResult GetCAUHINH(int id)
        {
            CAUHINH cAUHINH = db.CAUHINHs.Find(id);
            if (cAUHINH == null)
            {
                return NotFound();
            }

            return Ok(cAUHINH);
        }

        // PUT: api/CAUHINHs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCAUHINH(int id, CAUHINH cAUHINH)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cAUHINH.IdCauHinh)
            {
                return BadRequest();
            }

            db.Entry(cAUHINH).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CAUHINHExists(id))
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

        // POST: api/CAUHINHs
        [ResponseType(typeof(CAUHINH))]
        public IHttpActionResult PostCAUHINH(CAUHINH cAUHINH)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CAUHINHs.Add(cAUHINH);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cAUHINH.IdCauHinh }, cAUHINH);
        }

        // DELETE: api/CAUHINHs/5
        [ResponseType(typeof(CAUHINH))]
        public IHttpActionResult DeleteCAUHINH(int id)
        {
            CAUHINH cAUHINH = db.CAUHINHs.Find(id);
            if (cAUHINH == null)
            {
                return NotFound();
            }

            db.CAUHINHs.Remove(cAUHINH);
            db.SaveChanges();

            return Ok(cAUHINH);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CAUHINHExists(int id)
        {
            return db.CAUHINHs.Count(e => e.IdCauHinh == id) > 0;
        }
    }
}