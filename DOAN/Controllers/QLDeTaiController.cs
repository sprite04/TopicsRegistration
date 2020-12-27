using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;
using DOAN.ModelView;

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

        public ActionResult DanhSachDeTaiCuaTungGV()
        {
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return HttpNotFound();
            var list = db.DETAIs.Where(x => x.GVHuongDan == user.IdUser && user.IdUT>1&&x.IsDelete==false).OrderBy(k=>k.ChuyenNganh);
            return View(list);
        }

        public ActionResult TaoDeTai()
        {
            
            ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh");
            List<CauHinh> list = new List<CauHinh>();
            foreach(var item in db.CAUHINHs.Where(x=>x.Active==true))
            {
                CauHinh ch = new CauHinh();
                ch.IdCauHinh = item.IdCauHinh;
                ch.TenCauHinh = item.LOAIDETAI.TenLoai + " | " + item.NIENKHOA1.TenNK + " (" + item.NIENKHOA1.NamBD + "-" + item.NIENKHOA1.NamKT + ") " + " | Học kỳ " + item.HocKy + " (" + item.NamHocBatDauHocKy + "-" + item.NamHocKetThucHocKy + ") ";
                list.Add(ch);
            }
            ViewBag.CauHinh = new SelectList(list, "IdCauHinh", "TenCauHinh");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult TaoDeTai(DETAI detai,FormCollection f)
        {
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return HttpNotFound();
            CAUHINH cauhinh = db.CAUHINHs.Find(detai.CauHinh);
            if (cauhinh == null)
                return HttpNotFound();
            if(!(DateTime.Compare(DateTime.Now, cauhinh.ThoiGianGVBatDauDK??DateTime.Now)>=0 && DateTime.Compare(DateTime.Now, cauhinh.ThoiGianGVKetThucDK ?? DateTime.Now) <= 0))
                return Content("<script> alert(\"Nằm ngoài thời gian tạo đề tài\")</script>");
            detai.IsDelete = false;
            detai.GVHuongDan = user.IdUser;
            detai.IsDuyet = false;
            var kq = f["dkkcn"];
            if (kq == null)
                detai.DuocDKKhacCN = false;
            else
                detai.DuocDKKhacCN = true;
            if (ModelState.IsValid)
            {
                try
                {
                    db.DETAIs.Add(detai);
                    db.SaveChanges();
                    return RedirectToAction("DanhSachDeTaiCuaTungGV");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại.");
                }
            }
            else
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
            ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh", detai.ChuyenNganh);
            List<CauHinh> list = new List<CauHinh>();
            foreach (var item in db.CAUHINHs.Where(x => x.Active == true))
            {
                CauHinh ch = new CauHinh();
                ch.IdCauHinh = item.IdCauHinh;
                ch.TenCauHinh = item.LOAIDETAI.TenLoai + " | " + item.NIENKHOA1.TenNK + " (" + item.NIENKHOA1.NamBD + "-" + item.NIENKHOA1.NamKT + ") " + " | Học kỳ " + item.HocKy + " (" + item.NamHocBatDauHocKy + "-" + item.NamHocKetThucHocKy + ") ";
                list.Add(ch);
            }
            ViewBag.CauHinh = new SelectList(list, "IdCauHinh", "TenCauHinh", detai.CauHinh);
            return View(detai);

        }

        public ActionResult SuaDeTai(int id)
        {
            DETAI detai = db.DETAIs.Find(id);
            if (detai == null)
                return HttpNotFound();
            ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh", detai.ChuyenNganh);
            List<CauHinh> list = new List<CauHinh>();
            foreach (var item in db.CAUHINHs.Where(x => x.Active == true))
            {
                CauHinh ch = new CauHinh();
                ch.IdCauHinh = item.IdCauHinh;
                ch.TenCauHinh = item.LOAIDETAI.TenLoai + " | " + item.NIENKHOA1.TenNK + " (" + item.NIENKHOA1.NamBD + "-" + item.NIENKHOA1.NamKT + ") " + " | Học kỳ " + item.HocKy + " (" + item.NamHocBatDauHocKy + "-" + item.NamHocKetThucHocKy + ") ";
                list.Add(ch);
            }
            ViewBag.CauHinh = new SelectList(list, "IdCauHinh", "TenCauHinh", detai.CauHinh);
            return View(detai);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SuaDeTai(DETAI detai,FormCollection f)
        {
            var kq = f["dkkcn"];
            if (kq == null)
                detai.DuocDKKhacCN = false;
            else
                detai.DuocDKKhacCN = true;
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(detai).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("DanhSachDeTaiCuaTungGV");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại.");
                }
            }
            else
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
            ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh", detai.ChuyenNganh);
            List<CauHinh> list = new List<CauHinh>();
            foreach (var item in db.CAUHINHs.Where(x => x.Active == true))
            {
                CauHinh ch = new CauHinh();
                ch.IdCauHinh = item.IdCauHinh;
                ch.TenCauHinh = item.LOAIDETAI.TenLoai + " | " + item.NIENKHOA1.TenNK + " (" + item.NIENKHOA1.NamBD + "-" + item.NIENKHOA1.NamKT + ") " + " | Học kỳ " + item.HocKy + " (" + item.NamHocBatDauHocKy + "-" + item.NamHocKetThucHocKy + ") ";
                list.Add(ch);
            }
            ViewBag.CauHinh = new SelectList(list, "IdCauHinh", "TenCauHinh", detai.CauHinh);
            return View(detai);
        }

      
        public ActionResult XoaDeTai(int id)
        {
            DETAI detai = db.DETAIs.Find(id);
            if (detai == null)
                return HttpNotFound();
            try
            {
                detai.IsDelete = true;
                db.Entry(detai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhSachDeTaiCuaTungGV");

            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
            }
        }
    }
}