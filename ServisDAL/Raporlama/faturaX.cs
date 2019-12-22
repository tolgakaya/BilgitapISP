using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using TeknikServis.Radius;
using System.Linq;
using System.Web;

namespace ServisDAL
{
    public partial class faturaX : DevExpress.XtraReports.UI.XtraReport
    {
        public faturaX()
        {
            InitializeComponent();
        }

        private void faturaX_AfterPrint(object sender, EventArgs e)
        {
            //HttpContext.Current.Response.Redirect("/TeknikFatura/FaturaBasim");
            //radiusEntities dc = MyContext.Context(this.firma.Text);
            //string bayi = this.bayi.Text;

            //adminliste liste = dc.adminlistes.FirstOrDefault(x => x.username == bayi);
            //if (liste!=null)
            //{
            //    int id = Int32.Parse(this.id.Text);
            //    fatura i = dc.faturas.FirstOrDefault(x => x.ID == id);
            //    liste.fat_no = Int32.Parse(no.Text);
            //    liste.fat_seri = seri.Text;
            //    i.fat_no = Int32.Parse(no.Text);
            //    i.fat_seri = seri.Text;
            //    i.basim_tarih = DateTime.Now;
            //    KaydetmeIslemleri.kaydetR(dc);
            //}

        }

    }
}
