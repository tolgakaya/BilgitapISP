using ServisDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class Musteri : System.Web.UI.Page
    {
        private string firma;
        public Musteri()
        {
            firma = KullaniciIslem.firma();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }
            //this.Master.kisiarama = false;
            //this.Master.servisarama = false;
            //string firma = KullaniciIslem.firma();
            using (radiusEntities dc = MyContext.Context(firma))
            {
                MusteriIslemleri m = new MusteriIslemleri(dc);

                if (!IsPostBack)
                {
                    AyarCurrent ay = new AyarCurrent(dc);
                    if (ay.lisansKontrol() == false)
                    {
                        Response.Redirect("/LisansError");
                    }
                    List<anten> antenler = m.antenler();
                    if (antenler != null)
                    {
                        drdAnten.AppendDataBoundItems = true;
                        drdAnten.DataSource = antenler;
                        drdAnten.DataValueField = "anten_id";
                        drdAnten.DataTextField = "anten_adi";

                        drdAnten.DataBind();

                        drdAntenDuzen.AppendDataBoundItems = true;
                        drdAntenDuzen.DataSource = antenler;
                        drdAntenDuzen.DataValueField = "anten_id";
                        drdAntenDuzen.DataTextField = "anten_adi";
                        drdAntenDuzen.DataBind();

                    }
                    toplu(m);
                }

                //gosterHepsiSession(m);
            }

        }

        private void toplu(MusteriIslemleri m)
        {
            string antenid = Request.QueryString["antenid"];
            string tip = Request.QueryString["tip"];
            string s = txtAra.Value;

            //seçimli sms listesi
            if (Session["teller2"] != null)
            {
                smsSayi.InnerHtml = Session["teller2"].ToString().Split((new char[] { ',' }), StringSplitOptions.RemoveEmptyEntries).Length.ToString() + " Adet";
                cariOzet.Visible = true;

            }

            //müşteri arama
            if (!String.IsNullOrEmpty(s))
            {
                baslik.InnerHtml = "Kişi ve Firmalar";
                musteri_tel_mail musteri_bilgileri = m.musteriAraR(s);
                hdnTeller.Value = musteri_bilgileri.teller;
                GridView1.DataSource = musteri_bilgileri.musteriler;
                GridView1.DataBind();
            }

            else if (!String.IsNullOrEmpty(antenid))
            {
                //anten listesinden gelenler
                int id = Convert.ToInt32(antenid);

                musteri_tel_mail musteri_bilgileri = m.antenMusterileri(id);
                baslik.InnerHtml = musteri_bilgileri.anten_adi + " Kayıtlı Müşteriler";
                hdnTeller.Value = musteri_bilgileri.teller;
                GridView1.DataSource = musteri_bilgileri.musteriler;
                GridView1.DataBind();
            }
            else if (!String.IsNullOrEmpty(tip))
            {
                //bütün müşteriler
                baslik.InnerHtml = "Kişi ve Firmalar";
                musteri_tel_mail musteri_bilgileri = m.musteriListesiR();
                hdnTeller.Value = musteri_bilgileri.teller;
                GridView1.DataSource = musteri_bilgileri.musteriler;

                GridView1.DataBind();

            }
            //haritadan gelenler
            else if (Session["antendekiler"] != null)
            {
                string antens = Session["antendekiler"].ToString();
                if (!string.IsNullOrEmpty(antens))
                {

                    musteri_tel_mail musteri_bilgileri = m.antenMusterileri(antens);
                    hdnTeller.Value = musteri_bilgileri.teller;
                    GridView1.DataSource = musteri_bilgileri.musteriler;
                    GridView1.DataBind();
                }
            }
            //masterpageden gelenler
            else if (Session["kriter"] != null)
            {
                string ss = Session["kriter"].ToString();
                musteri_tel_mail musteri_bilgileri = m.musteriAraR(ss);
                hdnTeller.Value = musteri_bilgileri.teller;
                GridView1.DataSource = musteri_bilgileri.musteriler;
                GridView1.DataBind();
            }



        }


        public void MusteriAra(object sender, EventArgs e)
        {
            baslik.InnerHtml = "Kişi ve Firmalar";
            string s = txtAra.Value;
            using (radiusEntities dc = MyContext.Context(firma))
            {
                MusteriIslemleri m = new MusteriIslemleri(dc);
                baslik.InnerHtml = "Kişi ve Firmalar";
                musteri_tel_mail musteri_bilgileri = m.musteriAraR(s);
                hdnTeller.Value = musteri_bilgileri.teller;
                GridView1.DataSource = musteri_bilgileri.musteriler;
                GridView1.DataBind();
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            using (radiusEntities dc = MyContext.Context(firma))
            {
                MusteriIslemleri m = new MusteriIslemleri(dc);
                int id = Int32.Parse(lblID.Value.Trim());
                string ad = txtDuzenAd.Text;

                string adres = txtDuzenAdres.Text;
                string tel = txtDuzenTelefon.Text;
                string tc = txtDuzenTc.Text;
                string prim_yekun = txtPrimYekunDuzen.Text;
                string prim_kar = txtPrimKarDuzen.Text;

                string tanim = txtKimDuzen.Text;
                string unvan = txtDuzenUnvan.Text;
                int? antenid = null;
                if (drdAntenDuzen.SelectedValue != "-1")
                {
                    antenid = Convert.ToInt32(drdAntenDuzen.SelectedValue);
                }
                if (String.IsNullOrEmpty(unvan))
                {
                    unvan = ad;
                }


                m.musteriGuncelleR(id, ad, unvan, adres, tel, tc, tanim, prim_yekun, prim_kar, chcDuzenMust.Checked, chcDuzenTedarikci.Checked, chcDuzenUsta.Checked, chcDuzenDisServis.Checked, antenid);
                toplu(m);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            sb.Append(" alertify.success('Kayıt Güncellendi!');");
            sb.Append("$('#editModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);

        }

        //Handles Add button click in add modal popup
        protected void btnAddRecord_Click(object sender, EventArgs e)
        {
            //string id = lblMusID.Text;
            string ad = txtAdi.Text;
            string soyad = txtSoyAdi.Text;
            string adres = txtAdress.Text;
            string email = txtEmail.Text;
            string tel = txtTell.Text;
            string tc = txtTcAdd.Text;

            if (String.IsNullOrEmpty(tc))
            {
                tc = Araclar.KimlikUret(11);
            }

            string unvan = txtUnvan.Text;
            if (string.IsNullOrEmpty(unvan))
            {
                unvan = ad + " " + soyad;
            }

            string kim = txtKim.Text;
            string prim_kar = txtPrimKar.Text.Trim();
            string prim_yekun = txtPrimYekun.Text.Trim();
            int? antenid = null;
            if (drdAnten.SelectedValue != "-1")
            {
                antenid = Convert.ToInt32(drdAnten.SelectedValue);
            }
            using (radiusEntities dc = MyContext.Context(firma))
            {
                MusteriIslemleri m = new MusteriIslemleri(dc);
                m.musteriEkleR(ad, soyad, unvan, adres, tel, tel, email, kim, tc, prim_kar, prim_yekun, chcMusteri.Checked, chcTedarikci.Checked, chcUsta.Checked, chcDizServis.Checked, antenid);
                toplu(m);
            }


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            //sb.Append("alert('Record Added Successfully');");
            sb.Append(" alertify.success('Kayıt Eklendi!');");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        protected void GridView1_OnRowCreated(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string s = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CustID"));
                bool? mu = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "musteri"));
                if (mu == true)
                {
                    (e.Row.FindControl("btnServisler") as LinkButton).PostBackUrl = "~/TeknikTeknik/ServislerCanli.aspx?custid=" + s;

                }
                else
                {
                    bool? tam = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "disservis"));
                    if (tam == true)
                    {
                        (e.Row.FindControl("btnServisler") as LinkButton).PostBackUrl = "~/TeknikTeknik/ServisTamirci.aspx?tamirid=" + s;

                    }
                    else
                    {
                        if (User.IsInRole("Admin") || User.IsInRole("mudur"))
                        {
                            bool? ust = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "usta"));
                            if (ust == true)
                            {
                                (e.Row.FindControl("btnServisler") as LinkButton).PostBackUrl = "~/TeknikTeknik/ServisMaliyetler.aspx?tamirci=" + s;

                            }
                        }

                    }
                }
                //(e.Row.FindControl("btnServisler") as LinkButton).PostBackUrl = "~/TeknikTeknik/ServislerCanli.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");

                (e.Row.FindControl("btnMusteriDetay") as LinkButton).PostBackUrl = "~/MusteriDetayBilgileri.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");
                //(e.Row.FindControl("btnMusteriDetayK") as LinkButton).PostBackUrl = "~/MusteriDetayBilgileri.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");
            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Equals("detail"))
            {

                int code = Convert.ToInt32(e.CommandArgument);
                using (radiusEntities dc = MyContext.Context(firma))
                {
                    MusteriIslemleri m = new MusteriIslemleri(dc);
                    DetailsView1.DataSource = m.musteriTekListeR(code);
                    DetailsView1.DataBind();
                }


                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#detailModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetailModalScript", sb.ToString(), false);
            }
            else if (e.CommandName.Equals("smsEkle"))
            {
                string no = e.CommandArgument.ToString();

                if (Session["teller2"] != null)
                {

                    string teller = Session["teller2"].ToString();
                    if (!teller.Contains(no))
                    {
                        teller += no + ",";
                        Session["teller2"] = teller;
                    }

                }
                else
                {
                    string teller = "";

                    teller += no + ",";
                    Session["teller2"] = teller;

                }
                using (radiusEntities dc = MyContext.Context(firma))
                {
                    MusteriIslemleri m = new MusteriIslemleri(dc);
                    toplu(m);
                }

            }

            else if (e.CommandName.Equals("editRecord"))
            {


                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');

                int id = Convert.ToInt32(arg[0]);

                int index = Convert.ToInt32(arg[1]);

                using (radiusEntities dc = MyContext.Context(firma))
                {
                    MusteriIslemleri m = new MusteriIslemleri(dc);

                    customer must = m.musteriTekR(Convert.ToInt32(id));

                    lblID.Value = id.ToString();
                    txtDuzenAd.Text = must.Ad; //HttpUtility.HtmlDecode(link.Text);
                    txtDuzenAdres.Text = must.Adres; //HttpUtility.HtmlDecode(gvrow.Cells[5].Text);
                    txtDuzenTelefon.Text = must.telefon; //HttpUtility.HtmlDecode(gvrow.Cells[6].Text);
                    //string tcc = HttpUtility.HtmlDecode(gvrow.Cells[7].Text);

                    txtKimDuzen.Text = must.tanimlayici; //HttpUtility.HtmlDecode(gvrow.Cells[8].Text);
                    txtPrimYekunDuzen.Text = must.prim_yekun.ToString(); //HttpUtility.HtmlDecode(gvrow.Cells[9].Text);
                    txtPrimKarDuzen.Text = must.prim_kar.ToString();//HttpUtility.HtmlDecode(gvrow.Cells[10].Text);
                    txtDuzenTc.Text = must.TC;
                    txtDuzenUnvan.Text = must.unvan;
                    if (must.antenid != null)
                    {
                        drdAntenDuzen.SelectedValue = must.antenid.ToString();
                    }
                    else
                    {
                        drdAntenDuzen.SelectedIndex = 0;
                    }

                    if (must.musteri == true)
                    {
                        chcDuzenMust.Checked = true;
                    }
                    else
                    {
                        chcDuzenMust.Checked = false;
                    }
                    if (must.tedarikci == true)
                    {
                        chcDuzenTedarikci.Checked = true;
                    }
                    else
                    {
                        chcDuzenTedarikci.Checked = false;
                    }
                    if (must.usta == true)
                    {
                        chcDuzenUsta.Checked = true;
                    }
                    else
                    {
                        chcDuzenUsta.Checked = false;
                    }
                    if (must.disservis == true)
                    {
                        chcDuzenDisServis.Checked = true;
                    }
                    else
                    {
                        chcDuzenDisServis.Checked = false;
                    }

                    lblResult.Visible = false;

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#editModal').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditModalScript", sb.ToString(), false);



                }


            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string isim = "Kişi ve Firmalar-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(GridView1, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Kişi ve Firmalar-" + DateTime.Now.ToString();
            ExportHelper.ToWord(GridView1, isim);
        }

        protected void btnPrnt_Click(object sender, EventArgs e)
        {
            //Session["ctrl"] = GridView1;
            //ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script language=javascript>window.open('Print.aspx','PrintMe','height=300px,width=300px,scrollbars=1');</script>");

            GridView1.AllowPaging = false;
            GridView1.RowStyle.ForeColor = System.Drawing.Color.Black;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.RenderControl(hw);
            string gridHTML = sw.ToString().Replace("\"", "'")
                .Replace(System.Environment.NewLine, "");
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload = new function(){");
            sb.Append("var printWin = window.open('', '', 'left=0");
            sb.Append(",top=0,width=1000,height=600,status=0');");
            sb.Append("printWin.document.write(\"");
            sb.Append(gridHTML);
            sb.Append("\");");
            sb.Append("printWin.document.close();");
            sb.Append("printWin.focus();");
            sb.Append("printWin.print();");
            sb.Append("printWin.close();};");
            sb.Append("</script>");

            ScriptManager.RegisterStartupScript(GridView1, this.GetType(), "GridPrint", sb.ToString(), false);
            GridView1.AllowPaging = true;



        }
        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
        {
            //gosterHepsi();
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(firma))
            {
                MusteriIslemleri m = new MusteriIslemleri(dc);
                toplu(m);
            }

        }

        protected void btnSms_Click(object sender, EventArgs e)
        {
            //MesajGönder.aspxe yönlendir
            string tur = Request.QueryString["tur"];

            if (Session["teller2"] == null)
            {
                Session["teller"] = hdnTeller.Value;
                Response.Redirect("/MesajGonder.aspx?tur=sms&tip=gnltp");
            }
            else
            {
                Session["teller"] = Session["teller2"];
                Response.Redirect("/MesajGonder.aspx?tur=sms&tip=gnltp");
            }



        }

        protected void btnMail_Click(object sender, EventArgs e)
        {
            string tur = Request.QueryString["tur"];
            if (!String.IsNullOrEmpty(tur))
            {

                if (tur == "borc")
                {
                    //gunugelenler gösterilecek

                    string kritikS = Request.QueryString["kritik"];
                    //yaklaşan kredi
                    if (String.IsNullOrEmpty(kritikS))
                    {
                        Response.Redirect("/MesajGonder.aspx?tur=mail&tip=ggl");
                    }
                    else
                    {
                        Response.Redirect("/MesajGonder.aspx?tur=mail&tip=ggl&kritik=" + kritikS);


                    }
                }

            }
            else
            {
                Response.Redirect("/MesajGonder.aspx?tur=mail&tip=gnltp");
            }
        }



        protected void btnTemizle_Click(object sender, EventArgs e)
        {
            if (Session["teller"] != null)
            {
                Session["teller"] = null;
            }
            if (Session["teller2"] != null)
            {
                Session["teller2"] = null;
            }
            cariOzet.Visible = false;
            using (radiusEntities dc = MyContext.Context(firma))
            {
                MusteriIslemleri m = new MusteriIslemleri(dc);
                toplu(m);
            }

        }



    }
}