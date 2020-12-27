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
    public class SINHVIEN_DETAIController : ApiController
    {
        private WEBDbContext db = new WEBDbContext();

        // GET: api/SINHVIEN_DETAI
        public IQueryable<SINHVIEN_DETAI> GetSINHVIEN_DETAI()
        {
            return db.SINHVIEN_DETAI;
        }

        // GET: api/SINHVIEN_DETAI/5
        [ResponseType(typeof(SINHVIEN_DETAI))]
        public IHttpActionResult GetSINHVIEN_DETAI(int idDT,int idSV)
        {
            SINHVIEN_DETAI sINHVIEN_DETAI = db.SINHVIEN_DETAI.SingleOrDefault(x => x.SinhVien==idSV && x.DeTai == idDT);
            if (sINHVIEN_DETAI == null)
            {
                return NotFound();
            }

            return Ok(sINHVIEN_DETAI);
        }

        // PUT: api/SINHVIEN_DETAI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSINHVIEN_DETAI(int idDT, int idSV, SINHVIEN_DETAI sINHVIEN_DETAI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idDT != sINHVIEN_DETAI.DeTai || idSV!=sINHVIEN_DETAI.SinhVien)
            {
                return BadRequest();
            }

            db.Entry(sINHVIEN_DETAI).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SINHVIEN_DETAIExists(idDT, idSV))
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

        // POST: api/SINHVIEN_DETAI
        [ResponseType(typeof(SINHVIEN_DETAI))]
        public IHttpActionResult PostSINHVIEN_DETAI(SINHVIEN_DETAI sINHVIEN_DETAI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SINHVIEN_DETAI.Add(sINHVIEN_DETAI);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SINHVIEN_DETAIExists(sINHVIEN_DETAI.DeTai,sINHVIEN_DETAI.SinhVien))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtRoute("DefaultApi", new { id = sINHVIEN_DETAI.DeTai }, sINHVIEN_DETAI);
        }

        // DELETE: api/SINHVIEN_DETAI/5
        [ResponseType(typeof(SINHVIEN_DETAI))]
        public IHttpActionResult DeleteSINHVIEN_DETAI(int idDT, int idSV)
        {
            SINHVIEN_DETAI sINHVIEN_DETAI = db.SINHVIEN_DETAI.SingleOrDefault(x => x.SinhVien == idSV && x.DeTai == idDT);
            if (sINHVIEN_DETAI == null)
            {
                return NotFound();
            }

            db.SINHVIEN_DETAI.Remove(sINHVIEN_DETAI);
            db.SaveChanges();

            return Ok(sINHVIEN_DETAI);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SINHVIEN_DETAIExists(int idDT, int idSV)
        {
            return db.SINHVIEN_DETAI.Count(e => e.DeTai == idDT && e.SinhVien==idSV) > 0;
        }
    }
}