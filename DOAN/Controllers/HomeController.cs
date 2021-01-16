using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DOAN.Common;
using DOAN.Models;
using DOAN.ModelView;

namespace DOAN.Controllers
{
    public class HomeController : Controller
    {
        WEBDbContext db = new WEBDbContext();
        // GET: Home
        public ActionResult Index()
        {
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user != null)
            {
                ViewBag.Label = "";
                ViewBag.Value = "";
                List<CauHinh> list = new List<CauHinh>();
                foreach (var item in db.CAUHINHs.Where(x => x.Active == true))
                {
                    CauHinh ch = new CauHinh();
                    ch.IdCauHinh = item.IdCauHinh;
                    ch.TenCauHinh = item.LOAIDETAI.TenLoai + " " + item.NIENKHOA1.NamBD;
                    list.Add(ch);
                }
                ViewBag.GiaTri = 0;
                ViewBag.ThongBao = db.THONGBAOs.Where(x => x.IsDelete == false).Count();
                ViewBag.GiaoVien = db.NGUOIDUNGs.Where(x => x.ChucVu > 1 && x.Block == false).Count();
                ViewBag.SinhVien = db.NGUOIDUNGs.Where(x => x.ChucVu == 1 && x.Block == false).Count();
                if (user.IdUT == 1)
                    ViewBag.DeTai = db.SINHVIEN_DETAI.Where(x => x.SinhVien == user.IdUser && x.DETAI1.IsDuyet == true && x.DETAI1.IsDelete == false).Count();
                else if (user.IdUT > 1)
                    ViewBag.DeTai = db.DETAIs.Where(x => x.GVHuongDan == user.IdUser && x.IsDelete == false).Count();
                else
                    ViewBag.DeTai = 0;
                ViewBag.items = new SelectList(list, "IdCauHinh", "TenCauHinh");
                return View();
            }   
            else
                return RedirectToAction("DangNhap");
        }

        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if(user!=null)
            {
                var kq = f["ddlCauHinh"];
                List<CauHinh> list = new List<CauHinh>();
                foreach (var item in db.CAUHINHs.Where(x => x.Active == true))
                {
                    CauHinh ch = new CauHinh();
                    ch.IdCauHinh = item.IdCauHinh;
                    ch.TenCauHinh = item.LOAIDETAI.TenLoai + " " + item.NIENKHOA1.NamBD;
                    list.Add(ch);
                }
                ViewBag.ThongBao = db.THONGBAOs.Where(x => x.IsDelete == false).Count();
                ViewBag.GiaoVien = db.NGUOIDUNGs.Where(x => x.ChucVu > 1 && x.Block == false).Count();
                ViewBag.SinhVien = db.NGUOIDUNGs.Where(x => x.ChucVu == 1 && x.Block == false).Count();
                if (user.IdUT == 1)
                    ViewBag.DeTai = db.SINHVIEN_DETAI.Where(x => x.SinhVien == user.IdUser && x.DETAI1.IsDuyet == true && x.DETAI1.IsDelete == false).Count();
                else if (user.IdUT > 1)
                    ViewBag.DeTai = db.DETAIs.Where(x => x.GVHuongDan == user.IdUser && x.IsDelete == false).Count();
                else
                    ViewBag.DeTai = 0;
                if (kq != "")
                {
                    int giatri = int.Parse(kq);
                    ViewBag.items = new SelectList(list, "IdCauHinh", "TenCauHinh", giatri);
                    List<int> values = new List<int>();
                    List<string> labels = new List<string>();
                    foreach (var y in db.CHUYENNGANHs)
                    {
                        int k = db.DETAIs.Count(m => m.ChuyenNganh == y.IdCNganh && m.CauHinh == giatri && m.IsDelete == false && m.IsDuyet == true);
                        values.Add(k);
                        labels.Add(y.TenCNganh);
                    }

                    ViewBag.Label = labels;
                    ViewBag.Value = values;
                    ViewBag.GiaTri = giatri;
                }
                else
                {
                    ViewBag.items = new SelectList(list, "IdCauHinh", "TenCauHinh");
                    ViewBag.Label = "";
                    ViewBag.Value = "";
                    ViewBag.GiaTri = 0;
                }
                return View();
            }
            else
                return RedirectToAction("DangNhap");
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            ViewBag.ThongBao = 0;
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            ViewBag.ThongBao = 0;
            Session["Link"] = Request.Url.ToString();
            string username = f["username"].ToString();
            string password = Encryptor.MD5Hash(f["password"].ToString());
            var user = db.NGUOIDUNGs.SingleOrDefault(x => x.Username == username && x.Password == password &&x.Block==false);
            if (user != null)
            {
                IEnumerable<USERTYPE_QUYEN> lstQuyen = db.USERTYPE_QUYEN.Where(x => x.IdUT == user.IdUT);
                string Quyen = "";
                foreach(var item in lstQuyen)
                {
                    Quyen += item.QUYEN1.IdQuyen + ",";
                }
                Quyen = Quyen.Substring(0, Quyen.Length - 1);
                PhanQuyen(username, Quyen);
                Session["TaiKhoan"] = user;
                return RedirectToAction("Index");
            }
            ViewBag.ThongBao = 1;
            return View();
        }

        public ActionResult QuenMatKhau()
        {
            ViewBag.ThongBao = "";
            return View();
        }

        [HttpPost]
        public ActionResult QuenMatKhau(FormCollection f)
        {
            string email = f["email"];
            var nguoidung=db.NGUOIDUNGs.SingleOrDefault(x => x.Email.ToLower().Trim() == email.ToLower().Trim());
            if (nguoidung == null)
            {
                ViewBag.ThongBao = "Email không tồn tại. Vui lòng nhập lại";
                return View();
            }
            string password = Membership.GeneratePassword(6, 0);
            password = Regex.Replace(password, @"[^a-zA-Z0-9]", m => "9");
            Gmail gmail = new Gmail();
            gmail.To = email.Trim();
            gmail.From = "testgoog96@gmail.com";
            gmail.Subject = "Cấp lại mật khẩu đăng nhập";
            gmail.Body = "<p>Mật khẩu đăng nhập tạm thời của bạn l&agrave; <span style=\"color: #3598db;\">"+password+"</span>. Vui l&ograve;ng thay đổi lại mật khẩu khi đăng nhập th&agrave;nh c&ocirc;ng.</p>";

            
            try
            {
                MailMessage mail = new MailMessage(gmail.From, gmail.To);
                mail.Subject = gmail.Subject;
                mail.Body = gmail.Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                NetworkCredential nc = new NetworkCredential("testgoog96@gmail.com", "thuytien1234567890");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = nc;
                smtp.Send(mail);

                nguoidung.Password = Encryptor.MD5Hash(password);
                db.Entry(nguoidung).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ViewBag.ThongBao = "Email đã được gửi, vui lòng kiểm tra hộp thư để cập nhật thông tin.";
                return View();
            }
            catch (Exception)
            {
                ViewBag.ThongBao = "Quá trình thực hiện thất bại";
                return View();
            }
        }

        public void PhanQuyen(string username, string quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1, username, DateTime.Now, 
                                                        DateTime.Now.AddHours(3), //timeout
                                                        false,//remember me
                                                        quyen, 
                                                        FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent)
                cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
            
        }

        public ActionResult LoiPhanQuyen()
        {
            return View();
        }

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}