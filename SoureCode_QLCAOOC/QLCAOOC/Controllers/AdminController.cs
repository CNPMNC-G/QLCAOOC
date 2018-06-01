using CrystalDecisions.CrystalReports.Engine;
using Microsoft.AspNet.Identity.Owin;
using QLCAOOC.Models;
using QLCAOOC.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QLCAOOC.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        dbQLCAOOCDataContext data = new dbQLCAOOCDataContext();
        // GET: Admin

        public ActionResult Index()
        {
            return View();
            ViewBag.UserName = User.Identity.Name;
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(FormCollection collection, string ReturnUrl)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                ADMIN ad = data.ADMINs.SingleOrDefault(n => n.TenDN == tendn && n.MatKhau == matkhau);
                if (ad != null)
                {
                    FormsAuthentication.SetAuthCookie(ad.TenDN, true);
                    if (ReturnUrl != null)
                        return Redirect(ReturnUrl);
                    else
                        return RedirectToAction("index", "admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            ViewBag.UserName = null;
            return RedirectToAction("login", "admin");
        }

        //canho
        public ActionResult Canho()
        {
            return View(data.CANHOs.ToList().OrderBy(n => n.MaCanHo));
        }
        [HttpGet]
        public ActionResult Themcanho()
        {
            ViewBag.MaDienTich = new SelectList(data.DIENTICHes.ToList().OrderBy(n => n.MaDienTich), "MaDienTich", "TenDienTich");
            ViewBag.MaTang = new SelectList(data.TANGs.ToList().OrderBy(n => n.MaTang), "MaTang", "TenTang");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themcanho(CANHO canho, HttpPostedFileBase fileupload)
        {
            ViewBag.MaDienTich = new SelectList(data.DIENTICHes.ToList().OrderBy(n => n.MaDienTich), "MaDienTich", "TenDienTich");
            ViewBag.MaTang = new SelectList(data.TANGs.ToList().OrderBy(n => n.MaTang), "MaTang", "TenTang");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh căn hộ";
                return this.Themcanho();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/canho"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    canho.HinhAnh = fileName;
                    canho.TinhTrang = "Còn trống";
                    data.CANHOs.InsertOnSubmit(canho);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("canho", "admin");
        }
        [HttpGet]
        public ActionResult XoaCanHo(string id)
        {
            CANHO canho = data.CANHOs.SingleOrDefault(n => n.MaCanHo == id);
            if (canho == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(canho);
        }

        [HttpPost, ActionName("XoaCanHo")]
        public ActionResult Xacnhanxoa(string id)
        {
            CANHO canho = data.CANHOs.SingleOrDefault(n => n.MaCanHo == id);
            if (canho == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.CANHOs.DeleteOnSubmit(canho);
            try
            {
                data.SubmitChanges();
            }
            catch
            {
                ViewBag.Loi = "Không thể xóa căn hộ có dữ liệu được tham chiếu";
                return View(canho);
            }
            return RedirectToAction("canho", "admin");
        }
        public ActionResult Suacanho(string id)
        {
            CANHO canho = data.CANHOs.SingleOrDefault(n => n.MaCanHo == id);
            if (canho == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaDienTich = new SelectList(data.DIENTICHes.ToList().OrderBy(n => n.MaDienTich), "MaDienTich", "TenDienTich", canho.MaDienTich);
            ViewBag.MaTang = new SelectList(data.TANGs.ToList().OrderBy(n => n.MaTang), "MaTang", "TenTang", canho.MaTang);
            return View(canho);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suacanho(CANHO canho, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaDienTich = new SelectList(data.DIENTICHes.ToList().OrderBy(n => n.MaDienTich), "MaDienTich", "TenDienTich", canho.MaDienTich);
            ViewBag.MaTang = new SelectList(data.TANGs.ToList().OrderBy(n => n.MaTang), "MaTang", "TenTang", canho.MaTang);
            CANHO c = data.CANHOs.ToList().Find(n => n.MaCanHo == canho.MaCanHo);
            if (ModelState.IsValid)
            {
                if (fileUpload != null)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/canho/"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        return View(canho);
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                        c.HinhAnh = fileName;
                    }

                }
                c.GhiChu = canho.GhiChu;
                c.MaDienTich = canho.MaDienTich;
                c.MaTang = canho.MaTang;
                data.SubmitChanges();
            }
            return RedirectToAction("canho", "admin");
        }

        //nhankhau
        public ActionResult nhankhau()
        {
            return View(data.NHANKHAUs.ToList().OrderBy(n => n.MaNK));
        }
        [HttpGet]
        public ActionResult Themnhankhau()
        {
            HOPDONG hdap = new HOPDONG();
            hdap.MaHopDong = 0;
            hdap.MaCanHo = "Không có";
            List<HOPDONG> list = new List<HOPDONG>();
            list.Add(hdap);
            List<HOPDONG> list2 = data.HOPDONGs.OrderByDescending(s => s.MaHopDong).ToList();
            foreach (HOPDONG hd in list2)
                list.Add(hd);
            ViewBag.MaHopDong = new SelectList(list, "MaHopDong", "MaCanHo", hdap.MaHopDong);
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themnhankhau(NHANKHAU nhankhau, HttpPostedFileBase fileupload)
        {
            HOPDONG hdap = new HOPDONG();
            hdap.MaHopDong = 0;
            hdap.MaCanHo = "Không có";
            List<HOPDONG> list = new List<HOPDONG>();
            list.Add(hdap);
            List<HOPDONG> list2 = data.HOPDONGs.OrderByDescending(s => s.MaHopDong).ToList();
            foreach (HOPDONG hd in list2)
                list.Add(hd);
            if (nhankhau.MaHopDong != null)
                ViewBag.MaHopDong = new SelectList(list, "MaHopDong", "MaCanHo", nhankhau.MaHopDong);
            else
                ViewBag.MaHopDong = new SelectList(list, "MaHopDong", "MaCanHo", hdap.MaHopDong);
            int IdMax = data.NHANKHAUs.Max(w => w.MaNK) + 1;
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh nhân khẩu";
                return this.Themnhankhau();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/nhankhau"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    if (nhankhau.MaHopDong != 0)
                        nhankhau.MaHopDong = nhankhau.MaHopDong;
                    else
                        nhankhau.MaHopDong = null;
                    nhankhau.HinhAnh = fileName;
                    nhankhau.MaNK = IdMax;
                    data.NHANKHAUs.InsertOnSubmit(nhankhau);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("nhankhau", "admin");
        }
        public ActionResult Chitietnhankhau(int id)
        {
            NHANKHAU nhankhau = data.NHANKHAUs.SingleOrDefault(n => n.MaNK == id);
            if (nhankhau == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nhankhau);
        }
        [HttpGet]
        public ActionResult Xoanhankhau(int id)
        {
            NHANKHAU nhankhau = data.NHANKHAUs.SingleOrDefault(n => n.MaNK == id);
            if (nhankhau == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nhankhau);
        }

        [HttpPost, ActionName("Xoanhankhau")]
        public ActionResult Xacnhanxoa(int id)
        {
            NHANKHAU nhankhau = data.NHANKHAUs.SingleOrDefault(n => n.MaNK == id);
            if (nhankhau == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.NHANKHAUs.DeleteOnSubmit(nhankhau); try
            {
                data.SubmitChanges();
            }
            catch
            {
                ViewBag.Loi = "Không thể xóa nhân khẩu có dữ liệu được tham chiếu";
                return View(nhankhau);
            }
            return RedirectToAction("nhankhau", "admin");
        }
        [HttpGet]
        public ActionResult Suanhankhau(int id)
        {
            NHANKHAU nhankhau = data.NHANKHAUs.SingleOrDefault(n => n.MaNK == id);
            HOPDONG hdap = new HOPDONG();
            hdap.MaHopDong = 0;
            hdap.MaCanHo = "Không có";
            List<HOPDONG> list = new List<HOPDONG>();
            list.Add(hdap);
            List<HOPDONG> list2 = data.HOPDONGs.OrderByDescending(s => s.MaHopDong).ToList();
            foreach (HOPDONG hd in list2)
                list.Add(hd);
            if (nhankhau.MaHopDong != null)
                ViewBag.MaHopDong = new SelectList(list, "MaHopDong", "MaCanHo", nhankhau.MaHopDong);
            else
                ViewBag.MaHopDong = new SelectList(list, "MaHopDong", "MaCanHo", hdap.MaHopDong);
            if (nhankhau == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nhankhau);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suanhankhau(NHANKHAU nhankhau, HttpPostedFileBase fileUpload)
        {
            HOPDONG hdap = new HOPDONG();
            hdap.MaHopDong = 0;
            hdap.MaCanHo = "Không có";
            List<HOPDONG> list = new List<HOPDONG>();
            list.Add(hdap);
            List<HOPDONG> list2 = data.HOPDONGs.OrderByDescending(s => s.MaHopDong).ToList();
            foreach (HOPDONG hd in list2)
                list.Add(hd);
            ViewBag.MaHopDong = new SelectList(list, "MaHopDong", "MaCanHo", hdap.MaHopDong);
            NHANKHAU nk = data.NHANKHAUs.ToList().Find(n => n.MaNK == nhankhau.MaNK);
            if (ModelState.IsValid)
            {
                if (fileUpload != null)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/nhankhau/"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        return View(nhankhau);
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                        nk.HinhAnh = fileName;
                    }

                }
                if (nhankhau.MaHopDong != 0)
                    nk.MaHopDong = nhankhau.MaHopDong;
                else
                    nk.MaHopDong = null;
                nk.HoTen = nhankhau.HoTen;
                nk.CMND = nhankhau.CMND;
                nk.DienThoai = nhankhau.DienThoai;
                nk.DiaChi = nhankhau.DiaChi;
                nk.TenDN = nhankhau.TenDN;
                nk.MatKhau = nhankhau.MatKhau;
                data.SubmitChanges();
            }
            return RedirectToAction("nhankhau", "admin");
        }

        //hopdong
        public ActionResult Hopdong()
        {
            return View(data.HOPDONGs.ToList().OrderBy(n => n.MaHopDong));
        }
        [HttpGet]
        public ActionResult Themhopdong()
        {
            ViewBag.MaNK = new SelectList(data.NHANKHAUs.ToList().OrderBy(n => n.MaNK), "MaNK", "HoTen");
            ViewBag.MaCanHo = new SelectList(data.CANHOs.Where(s => s.TinhTrang == "Còn trống").ToList().OrderBy(n => n.MaCanHo), "MaCanHo", "MaCanHo");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themhopdong(HOPDONG hopdong)
        {
            ViewBag.MaNK = new SelectList(data.NHANKHAUs.ToList().OrderBy(n => n.MaNK), "MaNK", "HoTen", hopdong.MaNK);
            ViewBag.MaCanHo = new SelectList(data.CANHOs.Where(s => s.TinhTrang == "Còn trống").ToList().OrderBy(n => n.MaCanHo), "MaCanHo", "MaCanHo", hopdong.MaCanHo);
            int IdPT = data.PHIEUTHUTIENs.Max(w => w.MaPhieuThu) + 1;
            int IdHD = data.HOPDONGs.Max(w => w.MaHopDong) + 1;
            PHIEUTHUTIEN pt = new PHIEUTHUTIEN();
            CANHO ch = data.CANHOs.Where(s => s.MaCanHo == hopdong.MaCanHo).Single();
            if (ModelState.IsValid)
            {
                if (hopdong.ThoiGianThue < 6)
                {
                    ViewBag.Thongbao = "Thời gian thuê ít nhất là 6 tháng";
                    return this.Themnhankhau();
                }
                hopdong.MaHopDong = IdHD;
                hopdong.NgayHetHan = hopdong.NgayBatDauThue.AddMonths(6);
                hopdong.GiaPhong = ch.GIA.Gia1;
                hopdong.DaThanhToan = 6;
                data.HOPDONGs.InsertOnSubmit(hopdong);
                pt.MaPhieuThu = IdPT;
                pt.MaHopDong = hopdong.MaHopDong;
                pt.DotThu = 1;
                pt.NgayThu = DateTime.Now;
                pt.SoThangThu = 6;
                pt.SoTien = hopdong.GiaPhong * 6;
                data.PHIEUTHUTIENs.InsertOnSubmit(pt);
                ch.TinhTrang = "Đã thuê";
                data.SubmitChanges();
            }
            return RedirectToAction("hopdong", "admin");
        }
        public ActionResult Chitiethopdong(int id)
        {
            HOPDONG hd = data.HOPDONGs.SingleOrDefault(n => n.MaHopDong == id);
            if (hd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hd);
        }
        [HttpPost]
        public ActionResult Chitiethopdong(int id, int so)
        {
            ViewBag.Keyword = so;
            HOPDONG hd = data.HOPDONGs.SingleOrDefault(n => n.MaHopDong == id);
            if (so <= 0)
            {
                return View(hd);
            }
            hd.ThoiGianThue += so;
            data.SubmitChanges();
            return View(hd);
        }

        //gia
        public ActionResult Gia()
        {
            return View(data.GIAs.ToList().OrderBy(n => n.MaDienTich));
        }
        [HttpGet]
        public ActionResult Themgia()
        {
            ViewBag.Tang = new SelectList(data.TANGs.ToList().OrderBy(n => n.MaTang), "MaTang", "TenTang");
            ViewBag.DienTich = new SelectList(data.DIENTICHes.ToList().OrderBy(n => n.MaDienTich), "MaDienTich", "TenDienTich");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themgia(GIA gia)
        {
            ViewBag.Tang = new SelectList(data.TANGs.ToList().OrderBy(n => n.MaTang), "MaTang", "TenTang");
            ViewBag.DienTich = new SelectList(data.DIENTICHes.ToList().OrderBy(n => n.MaDienTich), "MaDienTich", "TenDienTich");
            if (ModelState.IsValid)
            {
                data.GIAs.InsertOnSubmit(gia);
                data.SubmitChanges();
            }
            return RedirectToAction("gia", "admin");
        }
        [HttpGet]
        public ActionResult Suagia(string id)
        {
            string[] arr = id.Split('-');
            string maDienTich = arr[0];
            string maTang = arr[1];
            GIA gia = data.GIAs.SingleOrDefault(n => n.MaDienTich == maDienTich && n.MaTang == maTang);
            if (gia == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.Tang = new SelectList(data.TANGs.ToList().OrderBy(n => n.MaTang), "MaTang", "TenTang", maTang);
            ViewBag.DienTich = new SelectList(data.DIENTICHes.ToList().OrderBy(n => n.MaDienTich), "MaDienTich", "TenDienTich", maDienTich);
            return View(gia);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suagia(GIA gia)
        {
            GIA g = data.GIAs.SingleOrDefault(n => n.MaDienTich == gia.MaDienTich && n.MaTang == gia.MaTang);
            ViewBag.Tang = new SelectList(data.TANGs.ToList().OrderBy(n => n.MaTang), "MaTang", "TenTang");
            ViewBag.DienTich = new SelectList(data.DIENTICHes.ToList().OrderBy(n => n.MaDienTich), "MaDienTich", "TenDienTich");
            if (ModelState.IsValid)
            {
                g.Gia1 = gia.Gia1;
                data.SubmitChanges();
            }
            return RedirectToAction("gia", "admin");
        }
        [HttpGet]
        public ActionResult Xoagia(string id)
        {
            string[] arr = id.Split('-');
            string maDienTich = arr[0];
            string maTang = arr[1];
            GIA gia = data.GIAs.SingleOrDefault(n => n.MaDienTich == maDienTich && n.MaTang == maTang);
            if (gia == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(gia);
        }

        [HttpPost, ActionName("Xoagia")]
        public ActionResult Xacnhanxoagia(string id)
        {
            string[] arr = id.Split('-');
            string maDienTich = arr[0];
            string maTang = arr[1];
            GIA gia = data.GIAs.SingleOrDefault(n => n.MaDienTich == maDienTich && n.MaTang == maTang);
            if (gia == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.GIAs.DeleteOnSubmit(gia);
            try
            {
                data.SubmitChanges();
            }
            catch
            {
                ViewBag.Loi = "Không thể xóa mức giá có dữ liệu được tham chiếu";
                return View(gia);
            }
            return RedirectToAction("gia", "admin");
        }

        //phieuthu
        public ActionResult Phieuthu()
        {
            return View(data.PHIEUTHUTIENs.ToList().OrderBy(n => n.MaPhieuThu));
        }
        [HttpGet]
        public ActionResult Themphieuthu()
        {
            ViewBag.MaHopDong = new SelectList(data.HOPDONGs.ToList().OrderBy(n => n.MaCanHo), "MaHopDong", "MaCanHo");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themphieuthu(PHIEUTHUTIEN phieuthu)
        {
            ViewBag.MaHopDong = new SelectList(data.HOPDONGs.ToList().OrderBy(n => n.MaCanHo), "MaHopDong", "MaCanHo", phieuthu.MaHopDong);
            int IdPT = data.PHIEUTHUTIENs.Max(w => w.MaPhieuThu) + 1;
            HOPDONG hd = data.HOPDONGs.Where(s => s.MaHopDong == phieuthu.MaHopDong).Single();
            if (ModelState.IsValid)
            {
                if (hd.ThoiGianThue - hd.DaThanhToan == 0)
                {
                    ViewBag.Thongbao = (string)("Hợp đồng đã được thanh toán hết, vui lòng gia hạn thêm thời gian thuê!");
                    return this.Themphieuthu();
                }
                if (0 >= phieuthu.SoThangThu || phieuthu.SoThangThu > hd.ThoiGianThue - hd.DaThanhToan)
                {
                    ViewBag.Thongbao = (string)("Số tháng thu phải nằm trong khoảng 1 đến " + (hd.ThoiGianThue - hd.DaThanhToan) + " tháng!");
                    return this.Themphieuthu();
                }
                phieuthu.MaPhieuThu = IdPT;
                phieuthu.DotThu = data.PHIEUTHUTIENs.Where(s => s.MaHopDong == phieuthu.MaHopDong).Max(s => s.DotThu) + 1;
                phieuthu.NgayThu = DateTime.Now;
                phieuthu.SoTien = phieuthu.SoThangThu * data.HOPDONGs.Where(s => s.MaHopDong == phieuthu.MaHopDong).Single().GiaPhong;
                data.PHIEUTHUTIENs.InsertOnSubmit(phieuthu);
                hd.DaThanhToan += phieuthu.SoThangThu;
                hd.NgayHetHan = hd.NgayHetHan.AddMonths(phieuthu.SoThangThu);
                data.SubmitChanges();
            }
            return RedirectToAction("phieuthu", "admin");
        }

        //tang
        public ActionResult Tang()
        {
            return View(data.TANGs.ToList().OrderBy(n => n.MaTang));
        }
        [HttpGet]
        public ActionResult Themtang()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themtang(TANG tang)
        {
            if (ModelState.IsValid)
            {
                data.TANGs.InsertOnSubmit(tang);
                data.SubmitChanges();
            }
            return RedirectToAction("tang", "admin");
        }
        [HttpGet]
        public ActionResult Suatang(string id)
        {
            TANG tang = data.TANGs.SingleOrDefault(n => n.MaTang == id);
            if (tang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tang);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suatang(TANG tang)
        {
            TANG t = data.TANGs.SingleOrDefault(n => n.MaTang == tang.MaTang);
            if (ModelState.IsValid)
            {
                t.TenTang = tang.TenTang;
                data.SubmitChanges();
            }
            return RedirectToAction("tang", "admin");
        }
        [HttpGet]
        public ActionResult Xoatang(string id)
        {
            TANG tang = data.TANGs.SingleOrDefault(n => n.MaTang == id);
            if (tang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tang);
        }

        [HttpPost, ActionName("Xoatang")]
        public ActionResult Xacnhanxoatang(string id)
        {
            TANG tang = data.TANGs.SingleOrDefault(n => n.MaTang == id);
            if (tang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.TANGs.DeleteOnSubmit(tang);
            try
            {
                data.SubmitChanges();
            }
            catch
            {
                ViewBag.Loi = "Không thể xóa tầng có dữ liệu được tham chiếu";
                return View(tang);
            }
            return RedirectToAction("tang", "admin");
        }

        //dientich
        public ActionResult Dientich()
        {
            return View(data.DIENTICHes.ToList().OrderBy(n => n.MaDienTich));
        }
        [HttpGet]
        public ActionResult Themdientich()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themdientich(DIENTICH dientich)
        {
            if (ModelState.IsValid)
            {
                data.DIENTICHes.InsertOnSubmit(dientich);
                data.SubmitChanges();
            }
            return RedirectToAction("dientich", "admin");
        }
        [HttpGet]
        public ActionResult Suadientich(string id)
        {
            DIENTICH dientich = data.DIENTICHes.SingleOrDefault(n => n.MaDienTich == id);
            if (dientich == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dientich);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suadientich(DIENTICH dientich)
        {
            DIENTICH dt = data.DIENTICHes.SingleOrDefault(n => n.MaDienTich == dientich.MaDienTich);
            if (ModelState.IsValid)
            {
                dt.TenDienTich = dientich.TenDienTich;
                data.SubmitChanges();
            }
            return RedirectToAction("dientich", "admin");
        }
        [HttpGet]
        public ActionResult Xoadientich(string id)
        {
            DIENTICH dientich = data.DIENTICHes.SingleOrDefault(n => n.MaDienTich == id);
            if (dientich == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dientich);
        }

        [HttpPost, ActionName("Xoadientich")]
        public ActionResult Xacnhanxoadientich(string id)
        {
            DIENTICH dientich = data.DIENTICHes.SingleOrDefault(n => n.MaDienTich == id);
            if (dientich == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.DIENTICHes.DeleteOnSubmit(dientich);
            try
            {
                data.SubmitChanges();
            }
            catch
            {
                ViewBag.Loi = "Không thể xóa diện tích có dữ liệu được tham chiếu";
                return View(dientich);
            }
            return RedirectToAction("dientich", "admin");
        }

        //report
        public ActionResult ExportHopDong(int id)
        {
            HOPDONG hd = data.HOPDONGs.Where(s => s.MaHopDong == id).Single();
            NHANKHAU nk = data.NHANKHAUs.Where(s => hd.MaNK == s.MaNK).Single();
            CANHO ch = data.CANHOs.Where(s => s.MaCanHo == hd.MaCanHo).Single();
            DIENTICH dt = data.DIENTICHes.Where(s => s.MaDienTich == ch.MaDienTich).Single();
            DataSetHopDong db = new DataSetHopDong();
            db.DataTable1.AddDataTable1Row(hd.MaHopDong, hd.NgayBatDauThue, hd.ThoiGianThue, hd.GiaPhong, ch.MaCanHo, ch.MaTang, dt.TenDienTich, nk.HoTen, nk.DiaChi, nk.CMND, nk.DienThoai);
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/CrystalReportHopDong.rpt")));
            rd.SetDataSource(db);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            str.Seek(0, SeekOrigin.Begin);
            return File(str, "application/pdf", "HopDong.pdf");
        }
        public ActionResult ExportPhieuThu(int id)
        {
            PHIEUTHUTIEN ptt = data.PHIEUTHUTIENs.Where(s => s.MaPhieuThu == id).Single();
            HOPDONG hd = data.HOPDONGs.Where(s => s.MaHopDong == ptt.MaHopDong).Single();
            NHANKHAU nk = data.NHANKHAUs.Where(s => hd.MaNK == s.MaNK).Single();
            DataSetPhieuThu db = new DataSetPhieuThu();
            db.DataTable1.AddDataTable1Row(hd.MaHopDong, hd.MaCanHo, hd.GiaPhong, nk.HoTen, nk.CMND, nk.DiaChi, nk.DienThoai, ptt.MaPhieuThu, ptt.DotThu, ptt.NgayThu, ptt.SoThangThu, ptt.SoTien);
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/CrystalReportPhieuThu.rpt")));
            rd.SetDataSource(db);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            str.Seek(0, SeekOrigin.Begin);
            return File(str, "application/pdf", "PhieuThuTien.pdf");
        }
    }
}