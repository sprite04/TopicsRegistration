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
    [Authorize(Roles = "*,themxoasuadetai,duyetdetai,xemdanhsachdetai,dangkydetai,xemdanhsachdetaicuatunggiangvien")]
    public class QLDeTaiController : Controller
    {
        WEBDbContext db = new WEBDbContext();
        // GET: QLDeTai
        //Hiển thị danh sách các đề tài theo từng loại đề tài
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


        //Hiển thị chi tiết của từng đề tài
        public ActionResult ChiTiet(int id)
        {
            DETAI detai =db.DETAIs.SingleOrDefault(x=>x.IdDeTai==id);

            ViewBag.SVDT = db.SINHVIEN_DETAI;

            ViewBag.XinVaoNhom = db.XINVAONHOMs;

            if (detai==null)
                return HttpNotFound();
            return View(detai);
        }


        //Đăng ký đề tài: sinh viên đầu tiên đăng ký đề tài thì thực hiện đăng ký đề tài
        [Authorize(Roles = "quanlinhom")]
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

        [Authorize(Roles = "quanlinhom")]
        //Sinh viên đăng ký vào đề tài đã có người đăng ký làm trưởng nhóm và chưa đủ số lượng thì thực hiện action này
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
                xvn.ThoiGian = DateTime.Now;
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

        [Authorize(Roles = "*,xemdanhsachdetaicuatunggiangvien")]
        public ActionResult DanhSachDeTaiCuaTungGV()
        {
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return HttpNotFound();
            var list = db.DETAIs.Where(x => x.GVHuongDan == user.IdUser && user.IdUT>1&&x.IsDelete==false).OrderBy(k=>k.ChuyenNganh);
            return View(list);
        }

        [Authorize(Roles = "*,themxoasuadetai")]
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

        [Authorize(Roles = "*,themxoasuadetai")]
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
            string strURL = Session["Link"] as string;
            detai.TrangThai = null;
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
                    if (strURL != null)
                        return Redirect(strURL);
                    else
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


        [Authorize(Roles = "*,themxoasuadetai")]
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

        [Authorize(Roles = "*,duyetdetai")]
        public ActionResult DanhSachDeTaiCanDuocDuyet()
        {
            NGUOIDUNG nguoidung = Session["TaiKhoan"] as NGUOIDUNG;
            if (nguoidung == null)
                return HttpNotFound();
            ViewBag.ChuyenNganh = db.CHUYENNGANHs;
            var list = db.DETAIs.Where(x => x.TrangThai!=1 && x.IsDelete == false && x.IsDuyet == false && (DateTime.Compare(DateTime.Now, x.CAUHINH1.ThoiGianBatDauDuyet ?? DateTime.Now) >= 0 && DateTime.Compare(DateTime.Now, x.CAUHINH1.ThoiGianKetThucDuyet ?? DateTime.Now) <= 0)&&(nguoidung.IdUT > 3 || (nguoidung.IdUT == 3 && nguoidung.ChuyenNganh == x.ChuyenNganh)));
            return View(list);
        }

        [Authorize(Roles = "*,duyetdetai")]
        public ActionResult DuyetDeTai(int id)
        {
            NGUOIDUNG nguoidung = Session["TaiKhoan"] as NGUOIDUNG;

            DETAI detai = db.DETAIs.Find(id);
            if (detai == null||nguoidung==null)
                return HttpNotFound();
            if(nguoidung.IdUT>3||(nguoidung.IdUT==3&&nguoidung.ChuyenNganh==detai.ChuyenNganh))
            {
                try
                {
                    detai.TrangThai = 2;
                    detai.IsDuyet = true;
                    db.Entry(detai).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("DanhSachDeTaiCanDuocDuyet");

                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
                }
            }    
            else
            {
                return Content("<script> alert(\"Bạn không có quyền duyệt đề tài này\")</script>");
            }    
        }

        [Authorize(Roles = "*,duyetdetai")]
        public ActionResult BoQuaDeTai(int id)
        {
            NGUOIDUNG nguoidung = Session["TaiKhoan"] as NGUOIDUNG;
            DETAI detai = db.DETAIs.Find(id);
            if (detai == null || nguoidung == null)
                return HttpNotFound();
            if (nguoidung.IdUT > 3 || (nguoidung.IdUT == 3 && nguoidung.ChuyenNganh == detai.ChuyenNganh))
            {
                try
                {
                    detai.TrangThai = 1;
                    detai.IsDuyet = false;
                    db.Entry(detai).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("DanhSachDeTaiCanDuocDuyet");

                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
                }
            }
            else
            {
                return Content("<script> alert(\"Bạn không có quyền duyệt đề tài này\")</script>");
            }
        }

        [Authorize(Roles = "quanlinhom")]
        public ActionResult DanhSachXinVaoNhom()
        {
            Session["Link"] = Request.Url.ToString();
            NGUOIDUNG nguoidung = Session["TaiKhoan"] as NGUOIDUNG;
            if (nguoidung == null)
                return HttpNotFound();
            ViewBag.KQ = db.XINVAONHOMs.Count(x => x.DETAI1.TruongNhom == nguoidung.IdUser)>0;
            ViewBag.XinVaoNhom = db.XINVAONHOMs;
            ViewBag.SinhVienDeTai = db.SINHVIEN_DETAI;
            var list = db.DETAIs.Where(x => x.TruongNhom == nguoidung.IdUser && (DateTime.Compare(DateTime.Now, x.CAUHINH1.ThoiGianBatDauDK ?? DateTime.Now) >= 0 && DateTime.Compare(DateTime.Now, x.CAUHINH1.ThoiGianKetThucDK ?? DateTime.Now) <= 0) && nguoidung.IdUT == 1);
            return View(list);
        }

        [Authorize(Roles = "quanlinhom")]
        public ActionResult ChoVaoNhom(int idDT,int idND)
        {

            string strURL = Session["Link"] as string;
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return HttpNotFound();
            DETAI detai = db.DETAIs.Find(idDT);
            XINVAONHOM xvn = db.XINVAONHOMs.SingleOrDefault(x => x.DeTai == idDT && x.NguoiGui == idND);
            if(xvn==null||detai==null)
                return HttpNotFound();
            if (db.SINHVIEN_DETAI.Count(x => x.DeTai == idDT && x.SinhVien == idND) >= detai.SoLuongSV)
            {
                return Content("<script> alert(\"Đề tài này đã đủ số lượng người\")</script>");
            }
            SINHVIEN_DETAI sv_dt = new SINHVIEN_DETAI();
            sv_dt.DeTai = idDT;
            sv_dt.SinhVien = idND;
            try
            {
                db.SINHVIEN_DETAI.Add(sv_dt);
                db.SaveChanges();

                var ds=db.XINVAONHOMs.Where(x => x.DeTai == idDT && x.NguoiGui == idND);
                db.XINVAONHOMs.RemoveRange(ds);
                db.SaveChanges();
                if (strURL != null)
                    return Redirect(strURL);
                else
                    return RedirectToAction("DanhSachXinVaoNhom");
            }
            catch (Exception e)
            {
                return Content("<script> alert(\"Bạn không có quyền duyệt đề tài này\")</script>");
            }
        }

        [Authorize(Roles = "quanlinhom")]
        public ActionResult NhomDeTai()
        {
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return HttpNotFound();
            ViewBag.SinhVienDeTai = db.SINHVIEN_DETAI;
            ViewBag.KQ= db.SINHVIEN_DETAI.Any(x => x.SinhVien == user.IdUser && DateTime.Compare(DateTime.Now, x.DETAI1.ThoiGianKTBaoVe ?? DateTime.Now) <= 0);
            var list = db.SINHVIEN_DETAI.Where(x=> DateTime.Compare(DateTime.Now, x.DETAI1.ThoiGianKTBaoVe ?? DateTime.Now) <= 0);
            return View(list);
        }

        [Authorize(Roles = "quanlinhom")]
        public ActionResult RoiKhoiNhom(int idDT, int idND)
        {
            SINHVIEN_DETAI sv_dt = db.SINHVIEN_DETAI.SingleOrDefault(x => x.DeTai == idDT && x.SinhVien == idND);
            DETAI detai = db.DETAIs.SingleOrDefault(x => x.TruongNhom == idND && x.IdDeTai == idDT);
            if (sv_dt == null)
                return HttpNotFound();
            try
            {
                if(detai==null)
                {
                    db.SINHVIEN_DETAI.Remove(sv_dt);
                    db.SaveChanges();
                }    
                else
                {
                    SINHVIEN_DETAI truongnhom = db.SINHVIEN_DETAI.SingleOrDefault(x => x.DeTai == idDT && x.SinhVien != idND);
                    if(truongnhom!=null)
                    {
                        detai.TruongNhom = truongnhom.SinhVien;
                        db.Entry(detai).State = EntityState.Modified;
                        db.SaveChanges();
                        db.SINHVIEN_DETAI.Remove(sv_dt);
                        db.SaveChanges();
                    }
                    else
                    {
                        detai.TruongNhom = null;
                        db.Entry(detai).State = EntityState.Modified;
                        db.SaveChanges();
                        db.SINHVIEN_DETAI.Remove(sv_dt);
                        db.SaveChanges();
                    }    
                }    
                return RedirectToAction("NhomDeTai");
            }
            catch (Exception)
            {
                return Content("<script> alert(\"Bạn không có quyền duyệt đề tài này\")</script>");
            }
        }
    }
}