using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;
using PagedList;

namespace DOAN.Controllers
{
    public class QLThongBaoController : Controller
    {
        // GET: QLThongBao
        WEBDbContext db = new WEBDbContext();
        public ActionResult Index(int? page)
        {
            //So san pham tren 1 trang
            int PageSize = 1;
            //So trang hien tai
            int PageNumber = (page ?? 1);
            var list = db.THONGBAOs.Where(x => x.IsDelete == false);
            return View(list.OrderByDescending(x => x.NgayDang).ToPagedList(PageNumber, PageSize));
        }

        public ActionResult TaoThongBao()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult TaoThongBao(THONGBAO thongbao)
        {
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return HttpNotFound();
            thongbao.IsDelete = false;
            thongbao.CoTinMoi = true;
            thongbao.NguoiDang = user.IdUser;
            thongbao.NgayDang = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    db.THONGBAOs.Add(thongbao);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại.");
                }
            }
            else
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
            
            return View(thongbao);
        }

        public ActionResult ChiTietThongBao(int id)
        {
            var thongbao = db.THONGBAOs.Find(id);
            if (thongbao == null)
                return HttpNotFound();

            return View(thongbao);
        }

        public ActionResult ChinhSuaThongBao(int id)
        {
            var thongbao = db.THONGBAOs.Find(id);
            if (thongbao == null)
                return HttpNotFound();
            return View(thongbao);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSuaThongBao(THONGBAO thongbao)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    db.Entry(thongbao).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ChiTietThongBao", "QLThongBao", new { id = thongbao.IdTB });
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập");
            }    
            return View(thongbao);
        }
    }
}