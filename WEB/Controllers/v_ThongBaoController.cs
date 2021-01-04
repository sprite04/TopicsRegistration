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
    public class v_ThongBaoController : ApiController
    {
        private WEBDbContext db = new WEBDbContext();

        // GET: api/v_ThongBao
        public IQueryable<v_ThongBao> Getv_ThongBao()
        {
            return db.v_ThongBao;
        }

        // GET: api/v_ThongBao/5
        [ResponseType(typeof(v_ThongBao))]
        public IHttpActionResult Getv_ThongBao(int id)
        {
            v_ThongBao v_ThongBao = db.v_ThongBao.Find(id);
            if (v_ThongBao == null)
            {
                return NotFound();
            }

            return Ok(v_ThongBao);
        }

        // PUT: api/v_ThongBao/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putv_ThongBao(int id, v_ThongBao v_ThongBao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != v_ThongBao.IdTB)
            {
                return BadRequest();
            }

            db.Entry(v_ThongBao).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!v_ThongBaoExists(id))
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

        // POST: api/v_ThongBao
        [ResponseType(typeof(v_ThongBao))]
        public IHttpActionResult Postv_ThongBao(v_ThongBao v_ThongBao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.v_ThongBao.Add(v_ThongBao);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = v_ThongBao.IdTB }, v_ThongBao);
        }

        // DELETE: api/v_ThongBao/5
        [ResponseType(typeof(v_ThongBao))]
        public IHttpActionResult Deletev_ThongBao(int id)
        {
            v_ThongBao v_ThongBao = db.v_ThongBao.Find(id);
            if (v_ThongBao == null)
            {
                return NotFound();
            }

            db.v_ThongBao.Remove(v_ThongBao);
            db.SaveChanges();

            return Ok(v_ThongBao);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool v_ThongBaoExists(int id)
        {
            return db.v_ThongBao.Count(e => e.IdTB == id) > 0;
        }
    }
}