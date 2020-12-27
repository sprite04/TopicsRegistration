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
    public class XINVAONHOMsController : ApiController
    {
        private WEBDbContext db = new WEBDbContext();

        // GET: api/XINVAONHOMs
        public IQueryable<XINVAONHOM> GetXINVAONHOMs()
        {
            return db.XINVAONHOMs;
        }

        // GET: api/XINVAONHOMs/5
        [ResponseType(typeof(XINVAONHOM))]
        public IHttpActionResult GetXINVAONHOM(int idDT,int idSV)
        {
            XINVAONHOM xINVAONHOM = db.XINVAONHOMs.SingleOrDefault(x => x.NguoiGui == idSV && x.DeTai == idDT);
            if (xINVAONHOM == null)
            {
                return NotFound();
            }

            return Ok(xINVAONHOM);
        }

        // PUT: api/XINVAONHOMs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutXINVAONHOM(int idDT,int idSV, XINVAONHOM xINVAONHOM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idSV != xINVAONHOM.NguoiGui || idDT!=xINVAONHOM.DeTai)
            {
                return BadRequest();
            }

            db.Entry(xINVAONHOM).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!XINVAONHOMExists(idDT,idSV))
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

        // POST: api/XINVAONHOMs
        [ResponseType(typeof(XINVAONHOM))]
        public IHttpActionResult PostXINVAONHOM(XINVAONHOM xINVAONHOM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.XINVAONHOMs.Add(xINVAONHOM);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (XINVAONHOMExists(xINVAONHOM.DeTai, xINVAONHOM.NguoiGui))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = xINVAONHOM.NguoiGui }, xINVAONHOM);
        }

        // DELETE: api/XINVAONHOMs/5
        [ResponseType(typeof(XINVAONHOM))]
        public IHttpActionResult DeleteXINVAONHOM(int idDT, int idSV)
        {
            XINVAONHOM xINVAONHOM = db.XINVAONHOMs.SingleOrDefault(x => x.NguoiGui == idSV && x.DeTai == idDT);
            if (xINVAONHOM == null)
            {
                return NotFound();
            }

            db.XINVAONHOMs.Remove(xINVAONHOM);
            db.SaveChanges();

            return Ok(xINVAONHOM);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool XINVAONHOMExists(int idDT,int idSV)
        {
            return db.XINVAONHOMs.Count(e => e.NguoiGui == idSV && e.DeTai==idDT) > 0;
        }
    }
}