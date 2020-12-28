using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DOAN.Common;
using DOAN.Models;

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
                return View();
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