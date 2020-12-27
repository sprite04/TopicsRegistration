using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;

namespace DOAN.Controllers
{
    public class QLDeTaiController : Controller
    {
        WEBDbContext db = new WEBDbContext();
        // GET: QLDeTai
        public ActionResult Index(int id)
        {
            CAUHINH cauhinh = db.CAUHINHs.SingleOrDefault(x=>x.IdCauHinh==id);
            if (cauhinh == null)
                return HttpNotFound();

            ViewBag.ChuyenNganh = db.CHUYENNGANHs;

            ViewBag.SVDT = db.SINHVIEN_DETAI;

            ViewBag.XinVaoNhom = db.XINVAONHOMs;


            var list = db.DETAIs.Where(x => x.IsDuyet == true && x.CauHinh==cauhinh.IdCauHinh);
            return View(list);
        }

        public ActionResult LoaiDeTaiTheoNienKhoa()
        {
            var list = db.CAUHINHs.Where(x => x.Active == true);
            return View(list);
        }

        public ActionResult ChiTiet(int id)
        {
            DETAI detai =db.DETAIs.SingleOrDefault(x=>x.IdDeTai==id);

            ViewBag.SVDT = db.SINHVIEN_DETAI;

            ViewBag.XinVaoNhom = db.XINVAONHOMs;

            if (detai==null)
                return HttpNotFound();
            return View(detai);
        }

        public ActionResult DangKyDeTai(int id, string strURL)
        {
            DETAI detai = db.DETAIs.SingleOrDefault(x => x.IdDeTai == id);
            if (detai == null)
                return HttpNotFound();
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return RedirectToAction("DangNhap", "Home");


            if(detai.TruongNhom==null)
            {
                //detai.TruongNhom = user.IdUT;
                //detai.CAUHINH1 = null;
                //detai.CHUYENNGANH1 = null;
                //detai.NGUOIDUNG = null;
                //detai.NGUOIDUNG1 = null;
                //detai.SINHVIEN_DETAI = null;
                //detai.TRANGTHAI1 = null;
                //detai.XINVAONHOMs = null;
                db.Entry(detai).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    SINHVIEN_DETAI sv_dt = new SINHVIEN_DETAI();
                    sv_dt.DeTai = detai.IdDeTai;
                    sv_dt.SinhVien = user.IdUser;
                    db.SINHVIEN_DETAI.Add(sv_dt);
                    db.SaveChanges();
                    return Redirect(strURL);
                }
                catch (Exception)
                {
                    return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
                }
            }
            return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
        }

        public ActionResult XinVaoNhom(int id,string strURL)
        {
            DETAI detai = db.DETAIs.SingleOrDefault(x => x.IdDeTai == id);
            if (detai == null)
                return HttpNotFound();
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return RedirectToAction("DangNhap", "Home");

            


            if (db.XINVAONHOMs.Count(x=>x.DeTai==detai.IdDeTai)<detai.SoLuongSV && db.XINVAONHOMs.Count(x=>x.DeTai==detai.IdDeTai&&x.NguoiGui==user.IdUser)==0)
            {
                XINVAONHOM xvn = new XINVAONHOM();
                xvn.NguoiGui = user.IdUser;
                xvn.DeTai = detai.IdDeTai;
                db.XINVAONHOMs.Add(xvn);
                try
                {
                    db.SaveChanges();
                    return Redirect(strURL);
                }
                catch (Exception)
                {
                    return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
                }
            }
            return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
        }
    }
}