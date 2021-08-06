﻿using Business.Implements;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLWeb.Areas.Admin.Controllers
{
    public class BaoCaoDatHangController : BaseController
    {
        static DateTime _dateTo;
        static DateTime _dateFrom;

        readonly BaoCaoDatHangBusiness _baoCaoDatHangBUS = new BaoCaoDatHangBusiness();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DanhSachBaoCaoDatHang(string dateFrom, string dateTo)
        {
            _dateFrom = Convert.ToDateTime(dateFrom);
            if (_dateFrom == default(DateTime))
            {
                _dateFrom = Convert.ToDateTime("1/" + DateTime.Now.Month + "/" + DateTime.Now.Year);
            }
            _dateTo = Convert.ToDateTime(dateTo);
            if (_dateTo == default(DateTime))
            {
                _dateTo = DateTime.Now;
            }
            return View(_baoCaoDatHangBUS.ListView(HomeController.userName, _dateFrom, _dateTo).ToList());
        }

        public ActionResult XuatFilePDF()
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Report/BaoCaoDatHangRP.rpt")));
                rd.SetDataSource(_baoCaoDatHangBUS.ListView(HomeController.userName, _dateFrom, _dateTo).ToList());
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                rd.SetParameterValue("txtDateFrom", _dateFrom.ToString("dd/MM/yyyy"));
                rd.SetParameterValue("txtDateTo", _dateTo.ToString("dd/MM/yyyy"));
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "BaoCaoDatHangRP.pdf");
            }
            catch
            {
                SetAlert("Dữ liệu không có! Bạn hãy lọc lại dữ liệu", "error");
                return RedirectToAction("Index");
            }

        }

        public ActionResult XuatFileEXE()
        {
            try
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Report/BaoCaoDatHangRP.rpt")));
                rd.SetDataSource(_baoCaoDatHangBUS.ListView(HomeController.userName, _dateFrom, _dateTo).ToList());
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                rd.SetParameterValue("txtDateFrom", _dateFrom.ToString("dd/MM/yyyy"));
                rd.SetParameterValue("txtDateTo", _dateTo.ToString("dd/MM/yyyy"));
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/xls", "BaoCaoDatHangRP.xls");
            }
            catch
            {
                SetAlert("Dữ liệu không có! Bạn hãy lọc lại dữ liệu", "error");
                return RedirectToAction("Index");
            }

        }
    }
}