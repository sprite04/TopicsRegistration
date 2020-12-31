using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;

namespace DOAN.Controllers
{
    public class QLCauHinhController : Controller
    {
        WEBDbContext db = new WEBDbContext();
        // GET: QLCauHinh
        public ActionResult Index(int error=0)
        {
            var list = db.CAUHINHs.Where(x => x.Active == true);
            ViewBag.Error = error;
            return View(list);
        }

        public ActionResult Create()
        {
            ViewBag.LoaiDeTai =new SelectList( db.LOAIDETAIs,"IdLoai","TenLoai");
            ViewBag.NienKhoa = new SelectList(db.NIENKHOAs,"IdNK","TenNK");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CAUHINH cauhinh)
        {
            int error = 0;
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return HttpNotFound();

            CAUHINH ch = db.CAUHINHs.SingleOrDefault(x => x.LoaiDT == cauhinh.LoaiDT && x.NienKhoa == cauhinh.NienKhoa && x.HocKy == cauhinh.HocKy && x.NamHocBatDauHocKy == cauhinh.NamHocBatDauHocKy && x.NamHocKetThucHocKy == cauhinh.NamHocKetThucHocKy);
            if(ch!=null)
            {
                error = 2;//Đã tồn tại cấu hình này.
                return RedirectToAction("Index", "QLCauHinh", new { error = error });
            }    

            cauhinh.DateUpdate = DateTime.Now;
            cauhinh.NguoiTao = user.IdUser;
            cauhinh.Active = true;
           

            if(ModelState.IsValid)
            {
                db.CAUHINHs.Add(cauhinh);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại.");
                    ViewBag.LoaiDeTai = new SelectList(db.LOAIDETAIs, "IdLoai", "TenLoai", cauhinh.LoaiDT);
                    ViewBag.NienKhoa = new SelectList(db.NIENKHOAs, "IdNK", "TenNK", cauhinh.NienKhoa);
                    return View(cauhinh);
                }
            }
            ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin.");
            ViewBag.LoaiDeTai = new SelectList(db.LOAIDETAIs, "IdLoai", "TenLoai", cauhinh.LoaiDT);
            ViewBag.NienKhoa = new SelectList(db.NIENKHOAs, "IdNK", "TenNK", cauhinh.NienKhoa);
            return View(cauhinh);
        }


        public ActionResult Edit(int id)
        {
            CAUHINH cauhinh = db.CAUHINHs.Find(id);
            if (cauhinh == null)
                return HttpNotFound();

            ViewBag.LoaiDeTai = new SelectList(db.LOAIDETAIs, "IdLoai", "TenLoai", cauhinh.LoaiDT);
            ViewBag.NienKhoa = new SelectList(db.NIENKHOAs, "IdNK", "TenNK", cauhinh.NienKhoa);

            return View(cauhinh);
        }

        [HttpPost]
        [Route("Edit")]
        public ActionResult Edit(CAUHINH cauhinh)
        {
            cauhinh.DateUpdate = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(cauhinh).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại");
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập");
            }
            ViewBag.LoaiDeTai = new SelectList(db.LOAIDETAIs, "IdLoai", "TenLoai", cauhinh.LoaiDT);
            ViewBag.NienKhoa = new SelectList(db.NIENKHOAs, "IdNK", "TenNK", cauhinh.NienKhoa);

            return View(cauhinh);
        }


        public ActionResult Delete(int id)
        {
            CAUHINH cauhinh = db.CAUHINHs.Find(id);
            if (cauhinh == null)
                return HttpNotFound();
            int error = 0;
            try
            {
                cauhinh.Active = false;

                db.Entry(cauhinh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                error = 1;
                return RedirectToAction("Index", "QLCauHinh", new { error = error });
            }
        }

        public ActionResult ThemNienKhoa()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemNienKhoa(NIENKHOA nienkhoa)
        {
            NIENKHOA kq=db.NIENKHOAs.SingleOrDefault(x => x.TenNK.ToLower().Contains(nienkhoa.TenNK.ToLower())||nienkhoa.TenNK.ToLower().Contains(x.TenNK.ToLower()));
            if(kq!=null)
            {
                int error = 3;
                try
                {
                    kq.TenNK = nienkhoa.TenNK;
                    kq.NamBD = nienkhoa.NamBD;
                    kq.NamKT = nienkhoa.NamKT;
                    db.Entry(kq).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { error = error }); ;
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại");
                    return View(nienkhoa);
                }
            }    
            if (ModelState.IsValid)
            {
                db.NIENKHOAs.Add(nienkhoa);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại");
                    return View(nienkhoa);
                }
            }
            ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin");
            return View(nienkhoa);
        }
    }

    
}