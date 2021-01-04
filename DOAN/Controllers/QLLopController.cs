using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Common;
using DOAN.Models;

namespace WEB.Controllers
{
    [Authorize(Roles = "xemdslophoc,*")]
    public class QLLopController : Controller
    {
        WEBDbContext db = new WEBDbContext();
        // GET: QLLop
        public ActionResult Index()
        {
            var list = db.LOPs;
            return View(list);
        }

        [Authorize(Roles = "*")]
        public ActionResult Create()
        {
            ViewBag.NienKhoa = new SelectList(db.NIENKHOAs,"IdNK","TenNK");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public ActionResult Create(LOP lop)
        {
            try
            {
                db.LOPs.Add(lop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin bạn đã nhập");
                ViewBag.NienKhoa = new SelectList(db.NIENKHOAs, "IdNK", "TenNK", lop.NIENKHOA.IdNK);
                return View(lop);
            }
        }

        [Authorize(Roles = "*")]
        public ActionResult Edit(int id)
        {
            LOP lop = db.LOPs.Find(id);
            if (lop == null)
                return HttpNotFound();

            if (lop.NIENKHOA != null)
                ViewBag.NienKhoa = new SelectList(db.NIENKHOAs, "IdNK", "TenNK", lop.NIENKHOA.IdNK);
            else
                ViewBag.NienKhoa = new SelectList(db.NIENKHOAs, "IdNK", "TenNK");
            return View(lop);
        }

        // POST: NGUOIDUNGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Edit")]
        public ActionResult Edit(LOP lop)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(lop).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

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
                ViewBag.NienKhoa = new SelectList(db.NIENKHOAs, "IdNK", "TenNK",lop.IdNK);
            }
            return View(lop);
        }

        [Authorize(Roles = "quanlylophoc,*")]
        public ActionResult NhapLop(int id)
        {
            LOP lop = db.LOPs.Find(id);
            if(lop==null)
                return HttpNotFound();

            ViewBag.Lop = lop;
            ViewBag.ChuyenNganh = db.CHUYENNGANHs;
            return View();
        }

        [HttpPost]
        public ActionResult NhapLop(IEnumerable<NGUOIDUNG> Model, int Lop)
        {
            try
            {
                foreach (var item in Model)
                {
                    NGUOIDUNG nd = new NGUOIDUNG();
                    nd.RegisterDate = DateTime.Now;
                    nd.Block = false;
                    nd.Name = item.Name;
                    nd.Username = item.Username;
                    nd.ChuyenNganh = item.ChuyenNganh;
                    nd.Diem = item.Diem;
                    nd.TongTC = item.TongTC;
                    nd.Lop = Lop;
                    nd.ChucVu = 1;
                    nd.IdUT = 1;
                    nd.Password = Encryptor.MD5Hash(item.Username);
                    try
                    {
                        db.NGUOIDUNGs.Add(nd);
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
                ViewBag.Lop = Lop;
                ViewBag.ChuyenNganh = db.CHUYENNGANHs;
                return View();
            }

        }

        public ActionResult HienThiDSLop(int id)
        {
            LOP lop = db.LOPs.Find(id);
            if(lop==null)
                return Content("<script> alert(\"Không tồn tại lớp này\")</script>");
            ViewBag.Lop = lop;
            try
            {
                var list = db.NGUOIDUNGs.Where(x => x.Block == false && x.ChucVu == 1 && x.LOP1.IdLop == id);
                return View(list);
            }
            catch (Exception)
            {
                return Content("<script> alert(\"Không có danh sách hiển thị\")</script>");
            }
        }

    }
}