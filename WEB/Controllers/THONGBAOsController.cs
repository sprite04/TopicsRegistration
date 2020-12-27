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
    public class THONGBAOsController : ApiController
    {
        private WEBDbContext db = new WEBDbContext();

        // GET: api/THONGBAOs
        public IQueryable<THONGBAO> GetTHONGBAOs()
        {
            return db.THONGBAOs;
        }

        // GET: api/THONGBAOs/5
        [ResponseType(typeof(THONGBAO))]
        public IHttpActionResult GetTHONGBAO(int id)
        {
            THONGBAO tHONGBAO = db.THONGBAOs.Find(id);
            if (tHONGBAO == null)
            {
                return NotFound();
            }

            return Ok(tHONGBAO);
        }

        // PUT: api/THONGBAOs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTHONGBAO(int id, THONGBAO tHONGBAO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tHONGBAO.IdTB)
            {
                return BadRequest();
            }

            db.Entry(tHONGBAO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!THONGBAOExists(id))
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

        // POST: api/THONGBAOs
        [ResponseType(typeof(THONGBAO))]
        public IHttpActionResult PostTHONGBAO(THONGBAO tHONGBAO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.THONGBAOs.Add(tHONGBAO);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tHONGBAO.IdTB }, tHONGBAO);
        }

        // DELETE: api/THONGBAOs/5
        [ResponseType(typeof(THONGBAO))]
        public IHttpActionResult DeleteTHONGBAO(int id)
        {
            THONGBAO tHONGBAO = db.THONGBAOs.Find(id);
            if (tHONGBAO == null)
            {
                return NotFound();
            }

            db.THONGBAOs.Remove(tHONGBAO);
            db.SaveChanges();

            return Ok(tHONGBAO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool THONGBAOExists(int id)
        {
            return db.THONGBAOs.Count(e => e.IdTB == id) > 0;
        }
    }
}