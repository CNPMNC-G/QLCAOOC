using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLCAOOC.Models
{
    public class TTHopDong
    {
        public int MaHopDong { get; set; }
        public string MaCanHo { get; set; }
        public string HoTen { get; set; }
        public DateTime NgayBatDauThue { get; set; }
        public int ThoiGianThue { get; set; }
        public string Tang { get; set; }
        public string DienTich { get; set; }
        public int GiaPhong { get; set; }
        public string CMND { get; set; }
        public string DiaChi { get; set; }
        public int DienThoai { get; set; }
    }
}