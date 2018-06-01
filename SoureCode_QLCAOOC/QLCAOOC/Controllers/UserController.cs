using QLCAOOC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLCAOOC.Controllers
{
    public class UserController : Controller
    {
        dbQLCAOOCDataContext data = new dbQLCAOOCDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            if (Session["Taikhoan"] != null)
                return RedirectToAction("index", "trangchu");
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                NHANKHAU kh = data.NHANKHAUs.SingleOrDefault(n => n.TenDN == tendn && n.MatKhau == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("index", "trangchu");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session["Taikhoan"] = null;
            return RedirectToAction("index", "trangchu");
        }
        public ActionResult Hopdong()
        {
            if (Session["Taikhoan"] == null)
                return RedirectToAction("index", "trangchu");
            NHANKHAU nk = (NHANKHAU)(Session["Taikhoan"]);
            return View(data.HOPDONGs.Where(s => s.MaNK == nk.MaNK).ToList());
        }
    }
}