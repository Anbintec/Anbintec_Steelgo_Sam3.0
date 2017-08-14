using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using System.Net;
using System;
using System.Web.UI;

namespace FrontEndSAM.Controllers.Reportes
{
    public class ReportesController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ObtenerReportes(string path)
        {
            try
            {
                if (path != null)
                {
                    ReportViewer reportViewer = new ReportViewer();
                    reportViewer.SizeToReportContent = true;
                    //reportViewer.Width = new System.Web.UI.WebControls.Unit("100%");
                    //reportViewer.Height = new System.Web.UI.WebControls.Unit("100%");
                    reportViewer.BackColor = System.Drawing.ColorTranslator.FromHtml("#305E8E");
                    reportViewer.ForeColor = System.Drawing.Color.White;
                    reportViewer.ID = "reportViewerReportes";

                    reportViewer.ProcessingMode = ProcessingMode.Remote;

                    IReportServerCredentials irsc = new CustomReportCredentials("Sam3", "Steelgo2016", "STEELGO-DB01");
                    reportViewer.ServerReport.ReportServerCredentials = irsc;
                    reportViewer.ServerReport.ReportServerUrl = new Uri("http://www.samaltamira.net/ReportServer");
                    //reportViewer.ServerReport.ReportPath = "/SAM 3.0/Materiales/Detalles/FotoIncidencia";
                    reportViewer.ServerReport.ReportPath = path.Replace("?leng=es-MX", "").Replace("?leng=en-US", "");
                    reportViewer.ServerReport.Refresh();
                    ViewBag.ReportViewer = reportViewer;
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
            return View();
        }

    }

    public class CustomReportCredentials : IReportServerCredentials
    {
        private string _UserName;
        private string _PassWord;
        private string _DomainName;

        public CustomReportCredentials(string UserName, string PassWord, string DomainName)
        {
            _UserName = UserName;
            _PassWord = PassWord;
            _DomainName = DomainName;
        }

        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {
            get { return null; }
        }

        public ICredentials NetworkCredentials
        {
            get { return new NetworkCredential(_UserName, _PassWord, _DomainName); }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string user,
         out string password, out string authority)
        {
            authCookie = null;
            user = password = authority = null;
            return false;
        }
    }
}