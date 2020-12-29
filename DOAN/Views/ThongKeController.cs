using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.ModelView;
using DOAN.Models;

namespace DOAN.Views
{
    public class ThongKeController : Controller
    {
        WEBDbContext db = new WEBDbContext();
        // GET: ThongKe
        public ActionResult Index()
        {
            ViewBag.Label = "";
            ViewBag.Value = "";
            List<CauHinh> list = new List<CauHinh>();
            foreach (var item in db.CAUHINHs.Where(x => x.Active == true))
            {
                CauHinh ch = new CauHinh();
                ch.IdCauHinh = item.IdCauHinh;
                ch.TenCauHinh = item.LOAIDETAI.TenLoai + " "+ item.NIENKHOA1.NamBD;
                list.Add(ch);
            }
            ViewBag.GiaTri = 0;
            ViewBag.items = new SelectList(list, "IdCauHinh", "TenCauHinh");
            return View();
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
    }
}