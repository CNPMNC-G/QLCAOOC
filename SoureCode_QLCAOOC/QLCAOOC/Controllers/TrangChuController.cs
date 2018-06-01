using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLCAOOC.Models;

namespace QLCAOOC.Controllers
{
    public class TrangChuController : Controller
    {
        dbQLCAOOCDataContext data = new dbQLCAOOCDataContext();
        // GET: TrangChu
        [HttpGet]
        public ActionResult Index(string searchString)
        {
            ViewBag.Keyword = searchString;
            var ch = data.CANHOs.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                ch = ch.Where(s => s.MaCanHo.Contains(searchString) || s.GIA.TANG.TenTang.Contains(searchString) || s.GIA.DIENTICH.TenDienTich.Contains(searchString) || s.GIA.Gia1.ToString().Contains(searchString)).ToList();
                ch.OrderByDescending(v => v.MaCanHo);
                return View(ch);
            }
            return View(ch);
        }
        [HttpPost]
        public ActionResult Index(string drtang, string drdientich, int drgia)
        {
            var ch = data.CANHOs.ToList();
            if (drtang == "0" && drdientich == "0" && drgia == 0)
                return View(ch);
            if (drtang != "0" && drdientich == "0" && drgia == 0)
                return View(data.CANHOs.Where(s => s.MaTang == drtang).ToList());
            if (drtang == "0" && drdientich != "0" && drgia == 0)
                return View(data.CANHOs.Where(s => s.MaDienTich == drdientich).ToList());
            if (drtang == "0" && drdientich == "0" && drgia != 0)
            {
                if (drgia == 1)
                    return View(data.CANHOs.Where(s => s.GIA.Gia1 < 3000000).ToList());
                else if (drgia == 2)
                    return View(data.CANHOs.Where(s => s.GIA.Gia1 >= 3000000 && s.GIA.Gia1 <= 5000000).ToList());
                else
                    return View(data.CANHOs.Where(s => s.GIA.Gia1 > 5000000).ToList());
            }
            if (drtang != "0" && drdientich != "0" && drgia == 0)
                return View(data.CANHOs.Where(s => s.MaTang == drtang && s.MaDienTich == drdientich).ToList());
            if (drtang == "0" && drdientich != "0" && drgia != 0)
            {
                if (drgia == 1)
                    return View(data.CANHOs.Where(s => s.GIA.Gia1 < 3000000 && s.MaDienTich == drdientich).ToList());
                if (drgia == 2)
                    return View(data.CANHOs.Where(s => s.GIA.Gia1 >= 3000000 && s.GIA.Gia1 <= 5000000 && s.MaDienTich == drdientich).ToList());
                if (drgia == 3)
                    return View(data.CANHOs.Where(s => s.GIA.Gia1 > 5000000 && s.MaDienTich == drdientich).ToList());
            }
            if (drtang != "0" && drdientich == "0" && drgia != 0)
            {
                if (drgia == 1)
                    return View(data.CANHOs.Where(s => s.GIA.Gia1 < 3000000 && s.MaTang == drtang).ToList());
                if (drgia == 2)
                    return View(data.CANHOs.Where(s => s.GIA.Gia1 >= 3000000 && s.GIA.Gia1 <= 5000000 && s.MaTang == drtang).ToList());
                if (drgia == 3)
                    return View(data.CANHOs.Where(s => s.GIA.Gia1 > 5000000 && s.MaTang == drtang).ToList());
            }
            if (drtang != "0" && drdientich != "0" && drgia != 0)
            {
                if (drgia == 1)
                    return View(data.CANHOs.Where(s => s.GIA.Gia1 < 3000000 && s.MaTang == drtang && s.MaDienTich == drdientich).ToList());
                if (drgia == 2)
                    return View(data.CANHOs.Where(s => s.GIA.Gia1 >= 3000000 && s.GIA.Gia1 <= 5000000 && s.MaTang == drtang && s.MaDienTich == drdientich).ToList());
                if (drgia == 3)
                    return View(data.CANHOs.Where(s => s.GIA.Gia1 > 5000000 && s.MaTang == drtang && s.MaDienTich == drdientich).ToList());
            }
            return View(ch);
        }
        private List<CANHO> LayCanHo()
        {
            return data.CANHOs.ToList();
        }
        public ActionResult Details(string id)
        {
            return View(data.CANHOs.Where(s => s.MaCanHo == id).Single());
        }
        public ActionResult CanHoTuongTu(string id)
        {

            int gia = data.CANHOs.Where(s => s.MaCanHo == id).Single().GIA.Gia1;
            return PartialView(data.CANHOs.Where(s => s.GIA.Gia1 <= gia + 1000000 && s.GIA.Gia1 >= gia - 1000000 && s.MaCanHo != id).ToList());
        }
        public ActionResult LienHe()
        {
            return View();
        }
    }
}