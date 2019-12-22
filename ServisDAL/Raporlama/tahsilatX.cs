using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Web;

namespace ServisDAL
{
    public partial class tahsilatX : DevExpress.XtraReports.UI.XtraReport
    {
        public tahsilatX()
        {
            InitializeComponent();
        }

        private void Detail_AfterPrint(object sender, EventArgs e)
        {
            //HttpContext.Current.Session["ozet"] = null;
        }

    }
}
