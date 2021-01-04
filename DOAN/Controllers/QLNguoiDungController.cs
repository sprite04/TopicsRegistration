using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DOAN.Common;
using DOAN.Models;
using Excel=Microsoft.Office.Interop.Excel;

namespace DOAN.Controllers
{
    [Authorize(Roles = "*,xemdsgiangvien,xemdssinhvien")]
    public class QLNguoiDungController : Controller
    {
        WEBDbContext db = new WEBDbContext();
        // GET: QLNguoiDung
        [HttpGet]
        public ActionResult Index(int error=0)
        {
            var list = db.NGUOIDUNGs.Where(x => x.Block == false && x.ChucVu ==1);
            ViewBag.Error = error;//1:vui lòng lựa chọn file excel, 2 quá trình thực hiện thất bại, 3 loại file không chính xác
            return View(list);
        }

        public ActionResult IndexGV()
        {
            var list = db.NGUOIDUNGs.Where(x => x.Block == false && x.ChucVu>1);
            return View(list);
        }

        [HttpPost]
        [Authorize(Roles = "*")]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            int error = 0;
            if(excelfile==null || excelfile.ContentLength==0)
            {
                error = 1;//Vui lòng lựa chọn 1 file excel
                return RedirectToAction("Index", new { error = error });
            }
            else
            {
                if(excelfile.FileName.EndsWith("xls")||excelfile.FileName.EndsWith("xlsx")||excelfile.FileName.EndsWith("xlsm"))
                {
                    string path = Server.MapPath("~/assets/excel/"+excelfile.FileName);
                    if (!System.IO.File.Exists(path))
                        excelfile.SaveAs(path);

                    //Doc du lieu tu file excel
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    for(int row=2;row<=range.Rows.Count;row++)
                    {
                        string mssv = ((Excel.Range)range.Cells[row, 1]).Text.Trim();
                        var kq = db.NGUOIDUNGs.SingleOrDefault(x=>x.Username==mssv);
                        if(kq==null)
                        {
                            bool hople = true;
                            NGUOIDUNG nd = new NGUOIDUNG();
                            nd.Username = mssv;
                            nd.Name = ((Excel.Range)range.Cells[row, 2]).Text.Trim() + " " + ((Excel.Range)range.Cells[row, 3]).Text.Trim();
                            nd.GioiTinh = int.Parse(((Excel.Range)range.Cells[row, 4]).Text.Trim()) == 0 ? false : true;
                            try
                            {
                                var ngaysinh = DateTime.FromOADate(Convert.ToDouble((range.Cells[row, 5] as Excel.Range).Value2));
                                nd.NgaySinh = ngaysinh;
                            }
                            catch (Exception)
                            {
                                nd.NgaySinh = null;
                            }
                            int lop;
                            hople= int.TryParse(((Excel.Range)range.Cells[row, 6]).Text.Trim(),out lop);
                            if (hople)
                                nd.Lop = lop;
                            nd.Phone = ((Excel.Range)range.Cells[row, 7]).Text.Trim();
                            nd.ChuyenNganh = int.Parse(((Excel.Range)range.Cells[row, 8]).Text.Trim());
                            float diem;
                            hople = float.TryParse(((Excel.Range)range.Cells[row, 9]).Text.Trim(), out diem);
                            if(hople)
                                nd.Diem = float.Parse(((Excel.Range)range.Cells[row, 9]).Text.Trim());
                            int tongtc;
                            hople= int.TryParse(((Excel.Range)range.Cells[row, 10]).Text.Trim(),out tongtc);
                            if (hople)
                                nd.TongTC = tongtc;
                            nd.Block = false;
                            nd.Password = Encryptor.MD5Hash(mssv);
                            nd.ChucVu = 1;
                            nd.IdUT = 1;
                            nd.RegisterDate = DateTime.Now;
                            try
                            {
                                db.NGUOIDUNGs.Add(nd);
                                db.SaveChanges();
                                
                            }
                            catch (Exception)
                            {
                                error = 2;//Quá trình thực hiện thất bại
                                return RedirectToAction("Index",new { error = error });
                            }
                            error = -1;
                        }
                        else
                        {
                            bool hople = true;
                            kq.Name = ((Excel.Range)range.Cells[row, 2]).Text.Trim() + " " + ((Excel.Range)range.Cells[row, 3]).Text.Trim();
                            kq.GioiTinh = int.Parse(((Excel.Range)range.Cells[row, 4]).Text.Trim()) == 0 ? false : true;
                            try
                            {
                                var ngaysinh = DateTime.FromOADate(Convert.ToDouble((range.Cells[row, 5] as Excel.Range).Value2));
                                kq.NgaySinh = ngaysinh;
                            }
                            catch (Exception)
                            {
                                
                                kq.NgaySinh = null;
                            }
                            int lop;
                            hople = int.TryParse(((Excel.Range)range.Cells[row, 6]).Text.Trim(), out lop);
                            if (hople)
                                kq.Lop = lop;
                            kq.Phone = ((Excel.Range)range.Cells[row, 7]).Text.Trim();
                            kq.ChuyenNganh = int.Parse(((Excel.Range)range.Cells[row, 8]).Text.Trim());
                            float diem;
                            hople = float.TryParse(((Excel.Range)range.Cells[row, 9]).Text.Trim(), out diem);
                            if (hople)
                                kq.Diem = float.Parse(((Excel.Range)range.Cells[row, 9]).Text.Trim());
                            int tongtc;
                            hople = int.TryParse(((Excel.Range)range.Cells[row, 10]).Text.Trim(), out tongtc);
                            if (hople)
                                kq.TongTC = tongtc;
                            kq.Block = false;
                            kq.ChucVu = 1;
                            kq.IdUT = 1;
                            try
                            {
                                db.Entry(kq).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            catch (Exception)
                            {
                                error = 2;//Quá trình thực hiện thất bại
                                return RedirectToAction("Index", new { error = error });
                            }
                            error = -1;
                        }
                    }
                    return RedirectToAction("Index", new { error = error });
                }
                else
                {
                    error = 3;//Loại file không chính xác
                    return RedirectToAction("Index", new { error = error });
                }    
            }    
        }

        [Authorize(Roles = "*")]
        public ActionResult Create()
        {
            ViewBag.ChuyenNganh =new SelectList(db.CHUYENNGANHs,"IdCNganh","TenCNganh");
            ViewBag.Lop=new SelectList(db.LOPs, "IdLop", "TenLop");
            
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        [Authorize(Roles = "*")]
        public ActionResult Create(NGUOIDUNG nd, HttpPostedFileBase Avatar)
        {
            if (Avatar != null)
            {
                if (Avatar.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Avatar.FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/avatar"), fileName);
                    nd.Avatar = fileName;
                    if (!System.IO.File.Exists(path))
                    {
                        Avatar.SaveAs(path);
                    }
                }
            }

            nd.Password = Encryptor.MD5Hash(nd.Username);
            nd.RegisterDate = DateTime.Now;
            nd.Block = false;
            nd.ChucVu = 1;
            nd.IdUT = 1;

            db.NGUOIDUNGs.Add(nd);
            try
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin.");
                ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh",nd.ChuyenNganh);
                ViewBag.Lop = new SelectList(db.LOPs, "IdLop", "TenLop",nd.Lop);
                return View(nd);
            }
        }

        [Authorize(Roles = "*")]
        public ActionResult CreateGV()
        {
            ViewBag.ChucVu = new SelectList(db.CHUCVUs.Where(x =>x.IdCVu>1), "IdCVu", "TenCVu");
            ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("CreateGV")]
        [Authorize(Roles = "*")]
        public ActionResult CreateGV(NGUOIDUNG nd, HttpPostedFileBase Avatar, FormCollection f)
        {
            if (Avatar != null)
            {
                if (Avatar.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Avatar.FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/avatar"), fileName);
                    nd.Avatar = fileName;
                    if (!System.IO.File.Exists(path))
                    {
                        Avatar.SaveAs(path);
                    }
                }
            }
            var kq = f["admin"];
            if (kq == null)
                nd.IdUT = nd.ChucVu;
            else
                nd.IdUT = 5;
            nd.Password = Encryptor.MD5Hash(nd.Username);
            nd.RegisterDate = DateTime.Now;
            nd.Block = false;

            db.NGUOIDUNGs.Add(nd);
            try
            {
                db.SaveChanges();
                return RedirectToAction("IndexGV");
            }

            catch (Exception)
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin.");
                ViewBag.ChucVu = new SelectList(db.CHUCVUs.Where(x => x.IdCVu > 1), "IdCVu", "TenCVu",nd.ChucVu);
                ViewBag.ChuyenNganh = new SelectList(db.CHUCVUs, "IdCNganh", "TenCNganh",nd.ChuyenNganh);
                return View(nd);
            }
        }


        [Authorize(Roles = "*")]
        public ActionResult Edit(int id)
        {
            NGUOIDUNG nd = db.NGUOIDUNGs.SingleOrDefault(x => x.IdUser == id);
            if (nd == null)
                return HttpNotFound();

            ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh", nd.ChuyenNganh);
            ViewBag.Lop = new SelectList(db.LOPs, "IdLop", "TenLop", nd.Lop);
            ViewBag.AnhCu = nd.Avatar;
            return View(nd);
        }

        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = "*")]
        public ActionResult Edit(NGUOIDUNG nd, HttpPostedFileBase Avatar, string AnhCu)
        {
            if (Avatar != null)
            {
                if (Avatar.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Avatar.FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/avatar"), fileName);
                    nd.Avatar = fileName;
                    if (!System.IO.File.Exists(path))
                    {
                        Avatar.SaveAs(path);
                    }
                }
            }
            else
                nd.Avatar = AnhCu;



            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(nd).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {
                        return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
                ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh", nd.ChuyenNganh);
                ViewBag.Lop = new SelectList(db.LOPs, "IdLop", "TenLop", nd.Lop);
            }
            return View(nd);
        }

        [Authorize(Roles = "*")]
        public ActionResult EditGV(int id)
        {
            NGUOIDUNG nd = db.NGUOIDUNGs.Find(id);
            if (nd == null)
                return HttpNotFound();

            ViewBag.ChucVu = new SelectList(db.CHUCVUs.Where(x => x.IdCVu > 1), "IdCVu", "TenCVu", nd.ChucVu);
            ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh", nd.ChuyenNganh);
            ViewBag.AnhCu = nd.Avatar;
            ViewBag.UserType = nd.IdUT;
            return View(nd);
        }

       
        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = "*")]
        public ActionResult EditGV(NGUOIDUNG nd, HttpPostedFileBase Avatar, string AnhCu, FormCollection f)
        {
            if (Avatar != null)
            {
                if (Avatar.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Avatar.FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/hinhnd"), fileName);
                    nd.Avatar = fileName;
                    if (!System.IO.File.Exists(path))
                    {
                        Avatar.SaveAs(path);
                    }
                }
            }
            else
                nd.Avatar = AnhCu;

            var admin = f["admin"];
            if (admin == null)
                nd.IdUT = nd.ChucVu;
            else
                nd.IdUT = 5;

            if (ModelState.IsValid)
            {
                db.Entry(nd).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("IndexGV");
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
                ViewBag.ChucVu = new SelectList(db.CHUCVUs.Where(x => x.IdCVu > 1), "IdCVu", "TenCVu", nd.ChucVu);
                ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh", nd.ChuyenNganh);
            }
            return View(nd);
        }



        [HttpPost]
        public ActionResult CapNhatThongTin(NGUOIDUNG nd, HttpPostedFileBase Avatar, string AnhCu)
        {
            var user = Session["TaiKhoan"] as NGUOIDUNG;
            if(user==null)
            {
                RedirectToAction("DangNhap","Home");
            }
            if(user.IdUT==5||(user.IdUser==nd.IdUser))
            {
                if (Avatar != null)
                {
                    if (Avatar.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(Avatar.FileName);
                        var path = Path.Combine(Server.MapPath("~/assets/avatar"), fileName);
                        nd.Avatar = fileName;
                        if (!System.IO.File.Exists(path))
                        {
                            Avatar.SaveAs(path);
                        }
                    }
                }
                else
                    nd.Avatar = AnhCu;

                int error = 0;

                if (ModelState.IsValid)
                {
                    try
                    {
                        db.Entry(nd).State = EntityState.Modified;
                        try
                        {
                            db.SaveChanges();

                            error = -1;
                            return RedirectToAction("ChiTietNguoiDung", new { error = error });
                        }
                        catch (Exception)
                        {
                            error = 1;
                            return RedirectToAction("ChiTietNguoiDung", new { error = error });
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        error = 1;
                        return RedirectToAction("ChiTietNguoiDung", new { error = error });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
                    ViewBag.ChuyenNganh = new SelectList(db.CHUYENNGANHs, "IdCNganh", "TenCNganh", nd.ChuyenNganh);
                    ViewBag.Lop = new SelectList(db.LOPs, "IdLop", "TenLop", nd.Lop);
                }
                return View(nd);
            }
            else
            {
                return View("LoiPhanQuyen", "Home");
            }
        }


        


        [Authorize(Roles = "*")]
        public ActionResult Delete(int id)
        {
            NGUOIDUNG nd = db.NGUOIDUNGs.Find(id);
            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            try
            {
                nd.Block = true;
                //nd.LOP1 = null;
                //nd.CHUYENNGANH1 = null;
                //nd.CHUCVU1 = null;
                //nd.USERTYPE = null;

                db.Entry(nd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
            }
        }

        public ActionResult ChiTietNguoiDung(int? id,int error=0)
        {
            if(id==null)
            {
                var user = Session["TaiKhoan"] as NGUOIDUNG;
                if (user == null)
                    return HttpNotFound();
                var nd = db.NGUOIDUNGs.Find(user.IdUser);
                Session["TaiKhoan"] = nd;
                ViewBag.AnhCu = nd.Avatar;
                ViewBag.Error = error;
                return View(nd);
            }    
            else
            {
                var nd = db.NGUOIDUNGs.Find(id);
                ViewBag.Error = error;
                return View(nd);
            }    
        }

        public ActionResult ThayDoiMatKhau (FormCollection f)
        {
            int error = 0;
            var user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return HttpNotFound();
            var nd = db.NGUOIDUNGs.Find(user.IdUser);
            string matkhaucu = f["matkhaucu"];
            string matkhaumoi = f["matkhaumoi"];
            string xacnhan = f["xacnhan"];
            if (matkhaumoi!=xacnhan)
            {
                error = 2;
                return RedirectToAction("ChiTietNguoiDung", new { error = error });
            }
            if(Encryptor.MD5Hash(matkhaucu)!=nd.Password)
            {
                error = 3;
                return RedirectToAction("ChiTietNguoiDung", new { error = error });
            }
            else
            {
                nd.Password =Encryptor.MD5Hash(matkhaumoi);
                db.Entry(nd).State = EntityState.Modified;
                db.SaveChanges();
                Session["TaiKhoan"] = null;
                FormsAuthentication.SignOut();
                return RedirectToAction("Index","Home");
            }    
        }
    }
}