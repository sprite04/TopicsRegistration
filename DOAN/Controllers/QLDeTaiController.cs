﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using DOAN.Common;
using DOAN.Models;
using DOAN.ModelView;
using Excel = Microsoft.Office.Interop.Excel;

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
            Session["Link"] = Request.Url.ToString();



            var list = db.DETAIs.Where(x => x.IsDuyet == true && x.CauHinh == cauhinh.IdCauHinh);
            var listCN = db.CHUYENNGANHs;
            ViewBag.items = new SelectList(listCN, "IdCNganh", "TenCNganh");
            ViewBag.GiaTri = 0;
            ViewBag.DanhSach = list;
            ViewBag.CauHinh = cauhinh;
            return View(list);
        }

        [HttpPost]
        public ActionResult Index(int id,FormCollection f)
        {
            var kq = f["ddlChuyenNganh"];
            CAUHINH cauhinh = db.CAUHINHs.SingleOrDefault(x => x.IdCauHinh == id);
            if (cauhinh == null)
                return HttpNotFound();
            Session["Link"] = Request.Url.ToString();

            ViewBag.ChuyenNganh = db.CHUYENNGANHs;
            ViewBag.SVDT = db.SINHVIEN_DETAI;
            ViewBag.XinVaoNhom = db.XINVAONHOMs;

            var listCN = db.CHUYENNGANHs;

            if (kq!="")
            {
                int giatri = int.Parse(kq);
                var list = db.DETAIs.Where(x => x.IsDuyet == true && x.CauHinh == cauhinh.IdCauHinh && x.ChuyenNganh == giatri);
                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listCN, "IdCNganh", "TenCNganh",giatri);
                ViewBag.GiaTri = giatri;
                ViewBag.CauHinh = cauhinh;
                return View(list);
            }
            else
            {
                var list = db.DETAIs.Where(x => x.IsDuyet == true && x.CauHinh == cauhinh.IdCauHinh);
                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listCN, "IdCNganh", "TenCNganh");
                ViewBag.GiaTri = 0;
                ViewBag.CauHinh = cauhinh;
                return View(list);
            }    
        }

        public ActionResult NopBai(int id,int loai,HttpPostedFileBase file, string FolderId)
        {
            int error = 0;
            try
            {
                DETAI detai = db.DETAIs.Find(id);
                if (detai == null)
                    return HttpNotFound();
                if (file != null && file.ContentLength > 0)
                {
                    if (loai == 1)
                    {
                        detai.File_powerpoint = GoogleDriveFilesRepository.FileUploadInFolder(FolderId, file);
                    }
                    if (loai == 2)
                    {
                        detai.File_word = GoogleDriveFilesRepository.FileUploadInFolder(FolderId, file);
                    }
                    if (loai == 3)
                    {
                        detai.File_source = GoogleDriveFilesRepository.FileUploadInFolder(FolderId, file);
                    }
                    try
                    {
                        db.Entry(detai).State = EntityState.Modified;
                        db.SaveChanges();
                        error = -1;
                    }
                    catch (Exception)
                    {
                        error = 1;
                    }
                }
            }
            catch (Exception)
            {
                error = 1;
            }
            
            return RedirectToAction("DeTaiSinhVien", new { error=error});  
        }

        public ActionResult Download(int id,int loai)
        {
            int error = 0;
            try
            {
                DETAI detai = db.DETAIs.Find(id);
                if (detai == null)
                    return HttpNotFound();

                if (loai == 1)
                {
                    string FilePath = GoogleDriveFilesRepository.DownloadGoogleFile(detai.File_powerpoint);
                    Response.ContentType = "application/zip";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
                    Response.WriteFile(System.Web.HttpContext.Current.Server.MapPath("~/GoogleDriveFiles/" + Path.GetFileName(FilePath)));
                    Response.End();
                    Response.Flush();
                }
                if (loai == 2)
                {
                    string FilePath = GoogleDriveFilesRepository.DownloadGoogleFile(detai.File_word);
                    Response.ContentType = "application/zip";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
                    Response.WriteFile(System.Web.HttpContext.Current.Server.MapPath("~/GoogleDriveFiles/" + Path.GetFileName(FilePath)));
                    Response.End();
                    Response.Flush();
                }
                if (loai == 3)
                {
                    string FilePath = GoogleDriveFilesRepository.DownloadGoogleFile(detai.File_source);
                    Response.ContentType = "application/zip";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
                    Response.WriteFile(System.Web.HttpContext.Current.Server.MapPath("~/GoogleDriveFiles/" + Path.GetFileName(FilePath)));
                    Response.End();
                    Response.Flush();
                }
            }
            catch (Exception)
            {
                error = 2; //Khong the tai xuong tap tin
            }
            return RedirectToAction("DeTaiSinhVien", new { error = error });
        }

        public ActionResult XoaBaiNop(int id, int loai)
        {
            int error = 0;
            try
            {
                DETAI detai = db.DETAIs.Find(id);
                if (detai == null)
                    return HttpNotFound();
                var list = GoogleDriveFilesRepository.GetContainsInFolder(detai.CAUHINH1.folderDriveID);
                if (loai == 1)
                {
                    var file = list.SingleOrDefault(x => x.Id == detai.File_powerpoint);
                    if (file != null)
                        GoogleDriveFilesRepository.DeleteFile(file);
                    detai.File_powerpoint = null;
                }
                if (loai == 2)
                {
                    var file = list.SingleOrDefault(x => x.Id == detai.File_word);
                    if (file != null)
                        GoogleDriveFilesRepository.DeleteFile(file);
                    detai.File_word = null;
                }
                if (loai == 3)
                {
                    var file = list.SingleOrDefault(x => x.Id == detai.File_source);
                    if (file != null)
                        GoogleDriveFilesRepository.DeleteFile(file);
                    detai.File_source = null;
                }
                try
                {
                    db.Entry(detai).State = EntityState.Modified;
                    db.SaveChanges();
                    error = -1;
                }
                catch (Exception)
                {
                    error = 3; //Quá trình xoá thực hiện thất bại
                }
            }
            catch (Exception)
            {

                error = 3;
            }
            
            return RedirectToAction("DeTaiSinhVien", new { error=error});
        }

        public ActionResult Export(int id)
        {
            int error = 0;
            CAUHINH cauhinh = db.CAUHINHs.SingleOrDefault(x => x.IdCauHinh == id);
            if (cauhinh == null)
                return HttpNotFound();
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return HttpNotFound();
            var list = db.DETAIs.Where(x => x.GVHuongDan == user.IdUser && user.IdUT > 1 && x.IsDelete == false && x.CauHinh == cauhinh.IdCauHinh &&x.IsDuyet==true).OrderBy(k => k.ChuyenNganh);
            try
            {
                Excel.Application application = new Excel.Application();
                Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Excel.Worksheet worksheet = workbook.ActiveSheet;
                worksheet.Cells[1, 1] = "STT";
                worksheet.Cells[1, 2] = "MSSV";
                worksheet.Cells[1, 3] = "Họ và tên";
                worksheet.Cells[1, 4] = "Ngày sinh";
                worksheet.Cells[1, 5] = "Đề tài";
                worksheet.Cells[1, 6] = "Điểm";
                int row = 2;
                foreach (var detai in list)
                {
                    if(detai.SINHVIEN_DETAI.Count()>0)
                    {
                        foreach(var sv in db.SINHVIEN_DETAI.Where(x=>x.DeTai==detai.IdDeTai))
                        {
                            worksheet.Cells[row, 1] = row - 1;
                            worksheet.Cells[row, 2] = sv.NGUOIDUNG.Username;
                            worksheet.Cells[row, 3] = sv.NGUOIDUNG.Name;
                            string ngaysinh = sv.NGUOIDUNG.NgaySinh.ToString();
                            if (ngaysinh!="")
                            {

                                worksheet.Cells[row, 4] = DateTime.Parse(ngaysinh).ToString("dd/MM/yyyy");
                            }    
                                 

                            worksheet.Cells[row, 5] = sv.DETAI1.TenDeTai;
                            worksheet.Cells[row, 6] = sv.Diem;
                            row++;
                        }    
                    }
                    workbook.SaveAs("D:\\Export_Web\\DSSinhVienTungDeTai_"+user.Name.ToString()+"_"+DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss"));
                    workbook.Close();
                    Marshal.ReleaseComObject(workbook);
                    application.Quit();
                    Marshal.FinalReleaseComObject(application);
                    error = -1;
                    return RedirectToAction("DanhSachDeTaiCuaTungGV", new {id=cauhinh.IdCauHinh, error = error });

                }    
            }
            catch (Exception e)
            {
                error = 1; //Bao loi ben danh sach de tai cua tung gv
            }
            return RedirectToAction("DanhSachDeTaiCuaTungGV", new { id=cauhinh.IdCauHinh, error = error });
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
                detai.TruongNhom = user.IdUser;
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

                    var dsBo = db.XINVAONHOMs.Where(x => x.NguoiGui == user.IdUser);
                    db.XINVAONHOMs.RemoveRange(dsBo);
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
        public ActionResult LoaiDeTaiTheoNienKhoaCuaTungGV()
        {
            var list = db.CAUHINHs.Where(x => x.Active == true);
            return View(list);
        }

        [Authorize(Roles = "*,xemdanhsachdetaicuatunggiangvien")]
        public ActionResult DanhSachDeTaiCuaTungGV(int id, int error=0)
        {
            CAUHINH cauhinh = db.CAUHINHs.SingleOrDefault(x => x.IdCauHinh == id);
            if (cauhinh == null)
                return HttpNotFound();
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return HttpNotFound();
            ViewBag.Error = error;
            ViewBag.CauHinh = cauhinh;
            var list = db.DETAIs.Where(x => x.GVHuongDan == user.IdUser && user.IdUT>1&&x.IsDelete==false && x.CauHinh==cauhinh.IdCauHinh).OrderBy(k=>k.ChuyenNganh);
            return View(list);
        }

        [Authorize(Roles = "*,themxoasuadetai")]
        public ActionResult TaoDeTai()
        {
            
            ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh");
            List<CauHinh> list = new List<CauHinh>();
            foreach(var item in db.CAUHINHs.Where(x=>x.Active==true && (DateTime.Compare(DateTime.Now, x.ThoiGianGVBatDauDK ?? DateTime.Now) >= 0 && DateTime.Compare(DateTime.Now, x.ThoiGianGVKetThucDK ?? DateTime.Now) <= 0)))
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
        [Authorize(Roles = "*,themxoasuadetai")]
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
            if(detai.CauHinh==null)
            {
                ModelState.AddModelError("", "Chưa lựa chọn cấu hình");
                ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh", detai.ChuyenNganh);
                List<CauHinh> ds = new List<CauHinh>();
                foreach (var item in db.CAUHINHs.Where(x => x.Active == true && (DateTime.Compare(DateTime.Now, x.ThoiGianGVBatDauDK ?? DateTime.Now) >= 0 && DateTime.Compare(DateTime.Now, x.ThoiGianGVKetThucDK ?? DateTime.Now) <= 0)))
                {
                    CauHinh ch = new CauHinh();
                    ch.IdCauHinh = item.IdCauHinh;
                    ch.TenCauHinh = item.LOAIDETAI.TenLoai + " | " + item.NIENKHOA1.TenNK + " (" + item.NIENKHOA1.NamBD + "-" + item.NIENKHOA1.NamKT + ") " + " | Học kỳ " + item.HocKy + " (" + item.NamHocBatDauHocKy + "-" + item.NamHocKetThucHocKy + ") ";
                    ds.Add(ch);
                }
                ViewBag.CauHinh = new SelectList(ds, "IdCauHinh", "TenCauHinh");
                return View(detai);

            }    
            if (ModelState.IsValid)
            {
                try
                {
                    int error = -1;
                    db.DETAIs.Add(detai);
                    db.SaveChanges();
                    return RedirectToAction("DanhSachDeTaiCuaTungGV","QLDeTai",new { id=detai.CauHinh, error=error});
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
            foreach (var item in db.CAUHINHs.Where(x => x.Active == true && (DateTime.Compare(DateTime.Now, x.ThoiGianGVBatDauDK ?? DateTime.Now) >= 0 && DateTime.Compare(DateTime.Now, x.ThoiGianGVKetThucDK ?? DateTime.Now) <= 0)))
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
        [Authorize(Roles = "*,themxoasuadetai")]
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
                    int error = -1;
                    db.Entry(detai).State = EntityState.Modified;
                    db.SaveChanges();
                    if (strURL != null)
                        return Redirect(strURL);
                    else
                        return RedirectToAction("DanhSachDeTaiCuaTungGV","QLDeTai",new { id=detai.CauHinh, error=error});
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
                int error = -1;
                detai.IsDelete = true;
                db.Entry(detai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhSachDeTaiCuaTungGV","QLDeTai",new { id=detai.CauHinh, error=error});

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
            var listCN = db.CHUYENNGANHs;
            ViewBag.items = new SelectList(listCN, "IdCNganh", "TenCNganh");
            ViewBag.GiaTri = 0;
            ViewBag.DanhSach = list;

            return View(list);
        }


        [Authorize(Roles = "*,duyetdetai")]
        [HttpPost]
        public ActionResult DanhSachDeTaiCanDuocDuyet(FormCollection f)
        {
            var kq = f["ddlChuyenNganh"];

            NGUOIDUNG nguoidung = Session["TaiKhoan"] as NGUOIDUNG;
            if (nguoidung == null)
                return HttpNotFound();
            ViewBag.ChuyenNganh = db.CHUYENNGANHs;

            var listCN = db.CHUYENNGANHs;

            if (kq != "")
            {
                int giatri = int.Parse(kq);
                var list = db.DETAIs.Where(x => x.TrangThai != 1 && x.IsDelete == false && x.IsDuyet == false && (DateTime.Compare(DateTime.Now, x.CAUHINH1.ThoiGianBatDauDuyet ?? DateTime.Now) >= 0 && DateTime.Compare(DateTime.Now, x.CAUHINH1.ThoiGianKetThucDuyet ?? DateTime.Now) <= 0) && (nguoidung.IdUT > 3 || (nguoidung.IdUT == 3 && nguoidung.ChuyenNganh == x.ChuyenNganh)) && x.ChuyenNganh == giatri);

                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listCN, "IdCNganh", "TenCNganh", giatri);
                ViewBag.GiaTri = giatri;
                return View(list);
            }
            else
            {
                var list = db.DETAIs.Where(x => x.TrangThai != 1 && x.IsDelete == false && x.IsDuyet == false && (DateTime.Compare(DateTime.Now, x.CAUHINH1.ThoiGianBatDauDuyet ?? DateTime.Now) >= 0 && DateTime.Compare(DateTime.Now, x.CAUHINH1.ThoiGianKetThucDuyet ?? DateTime.Now) <= 0) && (nguoidung.IdUT > 3 || (nguoidung.IdUT == 3 && nguoidung.ChuyenNganh == x.ChuyenNganh)));
                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listCN, "IdCNganh", "TenCNganh");
                ViewBag.GiaTri = 0;
                return View(list);
            }
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
        public ActionResult DanhSachXinVaoNhom(int error=0)
        {
            NGUOIDUNG nguoidung = Session["TaiKhoan"] as NGUOIDUNG;
            if (nguoidung == null)
                return HttpNotFound();
            ViewBag.KQ = db.XINVAONHOMs.Count(x => x.DETAI1.TruongNhom == nguoidung.IdUser)>0;
            ViewBag.XinVaoNhom = db.XINVAONHOMs;
            ViewBag.SinhVienDeTai = db.SINHVIEN_DETAI;
            ViewBag.Error = error;
            var list = db.DETAIs.Where(x => x.TruongNhom == nguoidung.IdUser && (DateTime.Compare(DateTime.Now, x.CAUHINH1.ThoiGianBatDauDK ?? DateTime.Now) >= 0 && DateTime.Compare(DateTime.Now, x.CAUHINH1.ThoiGianKetThucDK ?? DateTime.Now) <= 0) && nguoidung.IdUT == 1);
            return View(list);
        }

        [Authorize(Roles = "quanlinhom")]
        public ActionResult ChoVaoNhom(int idDT,int idND)
        {
            int error = 0;
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return HttpNotFound();
            DETAI detai = db.DETAIs.Find(idDT);
            XINVAONHOM xvn = db.XINVAONHOMs.SingleOrDefault(x => x.DeTai == idDT && x.NguoiGui == idND);
            if(xvn==null||detai==null)
                return HttpNotFound();
            if (db.SINHVIEN_DETAI.Count(x => x.DeTai == idDT && x.SinhVien == idND) >= detai.SoLuongSV)
            {
                error = 1;
                return RedirectToAction("DanhSachXinVaoNhom", "QLDeTai", new { error = error });
            }
            SINHVIEN_DETAI sv_dt = new SINHVIEN_DETAI();
            sv_dt.DeTai = idDT;
            sv_dt.SinhVien = idND;
            try
            {
                db.SINHVIEN_DETAI.Add(sv_dt);
                db.SaveChanges();

                var ds=db.XINVAONHOMs.Where(x => x.NguoiGui == idND);
                db.XINVAONHOMs.RemoveRange(ds);
                db.SaveChanges();


                error = -1;
                return RedirectToAction("DanhSachXinVaoNhom", "QLDeTai", new { error = error });
            }
            catch (Exception e)
            {
                error = 2;
                return RedirectToAction("DanhSachXinVaoNhom", "QLDeTai", new { error = error });
            }
        }

        //Hiển thị các thành viên trong từng đề tài
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
                        var dsxinvn = db.XINVAONHOMs.Where(x => x.DeTai == detai.IdDeTai);
                        if(dsxinvn.Count()>0)
                        {
                            var date = dsxinvn.Min(x => x.ThoiGian);
                            XINVAONHOM loixvn = dsxinvn.SingleOrDefault(x => DateTime.Compare(x.ThoiGian, date) == 0);
                            if(loixvn!=null)
                            {
                                detai.TruongNhom = loixvn.NguoiGui;
                                db.Entry(detai).State = EntityState.Modified;
                                db.SaveChanges();
                                var ds_xvn = db.XINVAONHOMs.Where(x => x.NguoiGui == loixvn.NguoiGui);
                                db.XINVAONHOMs.RemoveRange(ds_xvn);
                                db.SaveChanges();
                                SINHVIEN_DETAI sinhvien_detai = new SINHVIEN_DETAI();
                                sinhvien_detai.DeTai = detai.IdDeTai;

                                sinhvien_detai.SinhVien = loixvn.NguoiGui;
                                db.SINHVIEN_DETAI.Add(sinhvien_detai);
                                db.SaveChanges();
                                db.SINHVIEN_DETAI.Remove(sv_dt);
                                db.SaveChanges();
                            }    
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
                }    
                return RedirectToAction("NhomDeTai");
            }
            catch (Exception)
            {
                return Content("<script> alert(\"Bạn không có quyền duyệt đề tài này\")</script>");
            }
        }


        //Nộp bài cho từng đề tài
        [Authorize(Roles = "quanlinhom")]
        public ActionResult DeTaiSinhVien(int error=0)
        {
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return HttpNotFound();

            var list = db.DETAIs.Where(x => DateTime.Compare(DateTime.Now, x.ThoiGianKTBaoVe ?? DateTime.Now) <= 0 && db.SINHVIEN_DETAI.Any(y=>y.DeTai==x.IdDeTai&&y.SinhVien==user.IdUser));
            ViewBag.KQ = list.Count()>0;
            ViewBag.Error = error;
            return View(list);
        }
    }
}