using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DOAN.Common;
using DOAN.Models;

namespace DOAN.Controllers
{
    [Authorize(Roles = "*,quanlysinhvien,quanlygiangvien,xemdsgiangvien,xemdssinhvien")]
    public class QLNguoiDungController : Controller
    {
        WEBDbContext db = new WEBDbContext();
        // GET: QLNguoiDung
        public ActionResult Index()
        {
            var list = db.NGUOIDUNGs.Where(x => x.Block == false && x.ChucVu ==1);
            return View(list);
        }

        public ActionResult IndexGV()
        {
            var list = db.NGUOIDUNGs.Where(x => x.Block == false && x.ChucVu>1);
            return View(list);
        }

        [Authorize(Roles = "*,quanlysinhvien")]
        public ActionResult Create()
        {
            ViewBag.ChuyenNganh =new SelectList(db.CHUYENNGANHs,"IdCNganh","TenCNganh");
            ViewBag.Lop=new SelectList(db.LOPs, "IdLop", "TenLop");
            
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public ActionResult Create(NGUOIDUNG nd, HttpPostedFileBase Avatar)
        {
            if (Avatar != null)
            {
                if (Avatar.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Avatar.FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/hinhnd"), fileName);
                    nd.Avatar = fileName;
                    if (!System.IO.File.Exists(path))
                    {
                        Avatar.SaveAs(path);
                    }
                }
            }

            nd.Password = Encryptor.MD5Hash(nd.Username);
            nd.RegisterDate = DateTime.Now;
            nd.Block = false;
            nd.ChucVu = 1;
            nd.IdUT = 1;

            db.NGUOIDUNGs.Add(nd);
            try
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin.");
                ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh",nd.ChuyenNganh);
                ViewBag.Lop = new SelectList(db.LOPs, "IdLop", "TenLop",nd.Lop);
                return View(nd);
            }
        }

        [Authorize(Roles = "*,quanlygiangvien")]
        public ActionResult CreateGV()
        {
            ViewBag.ChucVu = new SelectList(db.CHUCVUs.Where(x =>x.IdCVu>1), "IdCVu", "TenCVu");
            ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("CreateGV")]
        public ActionResult CreateGV(NGUOIDUNG nd, HttpPostedFileBase Avatar, FormCollection f)
        {
            if (Avatar != null)
            {
                if (Avatar.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Avatar.FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/hinhnd"), fileName);
                    nd.Avatar = fileName;
                    if (!System.IO.File.Exists(path))
                    {
                        Avatar.SaveAs(path);
                    }
                }
            }
            var kq = f["admin"];
            if (kq == null)
                nd.IdUT = nd.ChucVu;
            else
                nd.IdUT = 5;
            nd.Password = Encryptor.MD5Hash(nd.Username);
            nd.RegisterDate = DateTime.Now;
            nd.Block = false;

            db.NGUOIDUNGs.Add(nd);
            try
            {
                db.SaveChanges();
                return RedirectToAction("IndexGV");
            }

            catch (Exception)
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin.");
                ViewBag.ChucVu = new SelectList(db.CHUCVUs.Where(x => x.IdCVu > 1), "IdCVu", "TenCVu",nd.ChucVu);
                ViewBag.ChuyenNganh = new SelectList(db.CHUCVUs, "IdCNganh", "TenCNganh",nd.ChuyenNganh);
                return View(nd);
            }
        }

        [Authorize(Roles = "*,quanlysinhvien")]
        public ActionResult Edit(int id)
        {
            NGUOIDUNG nd = db.NGUOIDUNGs.SingleOrDefault(x => x.IdUser == id);
            if (nd == null)
                return HttpNotFound();

            ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh", nd.ChuyenNganh);
            ViewBag.Lop = new SelectList(db.LOPs, "IdLop", "TenLop", nd.Lop);
            ViewBag.AnhCu = nd.Avatar;
            return View(nd);
        }

        // POST: NGUOIDUNGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Edit")]
        public ActionResult Edit(NGUOIDUNG nd, HttpPostedFileBase Avatar, string AnhCu)
        {
            if (Avatar != null)
            {
                if (Avatar.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Avatar.FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/hinhnd"), fileName);
                    nd.Avatar = fileName;
                    if (!System.IO.File.Exists(path))
                    {
                        Avatar.SaveAs(path);
                    }
                }
            }
            else
                nd.Avatar = AnhCu;

            

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(nd).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {
                        return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
                ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh", nd.ChuyenNganh);
                ViewBag.Lop = new SelectList(db.LOPs, "IdLop", "TenLop", nd.Lop);
            }
            return View(nd);
        }

        [Authorize(Roles = "*,quanlygiangvien")]
        public ActionResult EditGV(int id)
        {
            NGUOIDUNG nd = db.NGUOIDUNGs.Find(id);
            if (nd == null)
                return HttpNotFound();

            ViewBag.ChucVu = new SelectList(db.CHUCVUs.Where(x => x.IdCVu > 1), "IdCVu", "TenCVu", nd.ChucVu);
            ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh", nd.ChuyenNganh);
            ViewBag.AnhCu = nd.Avatar;
            ViewBag.UserType = nd.IdUT;
            return View(nd);
        }

        // POST: NGUOIDUNGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Edit")]
        public ActionResult EditGV(NGUOIDUNG nd, HttpPostedFileBase Avatar, string AnhCu, FormCollection f)
        {
            if (Avatar != null)
            {
                if (Avatar.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Avatar.FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/hinhnd"), fileName);
                    nd.Avatar = fileName;
                    if (!System.IO.File.Exists(path))
                    {
                        Avatar.SaveAs(path);
                    }
                }
            }
            else
                nd.Avatar = AnhCu;

            var admin = f["admin"];
            if (admin == null)
                nd.IdUT = nd.ChucVu;
            else
                nd.IdUT = 5;

            if (ModelState.IsValid)
            {
                db.Entry(nd).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("IndexGV");
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
                ViewBag.ChucVu = new SelectList(db.CHUCVUs.Where(x => x.IdCVu > 1), "IdCVu", "TenCVu", nd.ChucVu);
                ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh", nd.ChuyenNganh);
            }
            return View(nd);
        }


        [Authorize(Roles = "*,quanlygiangvien")]
        public ActionResult Delete(int id)
        {
            NGUOIDUNG nd = db.NGUOIDUNGs.Find(id);
            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            try
            {
                nd.Block = true;
                //nd.LOP1 = null;
                //nd.CHUYENNGANH1 = null;
                //nd.CHUCVU1 = null;
                //nd.USERTYPE = null;

                db.Entry(nd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
            }
        }
    }
}