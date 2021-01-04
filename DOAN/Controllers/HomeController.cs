using System;
using System.Collections.Generic;
using System.Linq;
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
                ViewBag.DeTai = db.DETAIs.Where(x => x.IsDelete == false && x.IsDuyet == true).Count();
                ViewBag.items = new SelectList(list, "IdCauHinh", "TenCauHinh");
                return View();
            }   
            else
                return RedirectToAction("DangNhap");
        }

        [HttpPost]
        public ActionResult Index(FormCollection f)
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
            if (kq != "")
            {
                int giatri = int.Parse(kq);
                ViewBag.items = new SelectList(list, "IdCauHinh", "TenCauHinh", giatri);
                List<int> values = new List<int>();
                List<string> labels = new List<string>();
                foreach (var y in db.CHUYENNGANHs)
                {
                    int k = db.DETAIs.Count(m => m.ChuyenNganh == y.IdCNganh && m.CauHinh == giatri);
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