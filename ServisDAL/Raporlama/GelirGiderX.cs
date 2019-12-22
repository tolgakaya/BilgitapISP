using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace ServisDAL
{
    public partial class GelirGiderX : DevExpress.XtraReports.UI.XtraReport
    {
        public GelirGiderX()
        {
            InitializeComponent();
        }
        private void GelirGiderX_AfterPrint(object sender, EventArgs e)
        {
            //System.Web.HttpContext.Current.Session["gider_raporu"] = null;
            //System.Web.HttpContext.Current.Session["tahsilat_raporu"] = null;
        }

      
    }
}
