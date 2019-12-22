namespace ServisDAL
{
    partial class faturaTekX
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cinsi = new DevExpress.XtraReports.UI.XRTableCell();
            this.miktar = new DevExpress.XtraReports.UI.XRTableCell();
            this.fiyat = new DevExpress.XtraReports.UI.XRTableCell();
            this.tutar = new DevExpress.XtraReports.UI.XRTableCell();
            this.yazi = new DevExpress.XtraReports.UI.XRLabel();
            this.gtoplam = new DevExpress.XtraReports.UI.XRLabel();
            this.oiv = new DevExpress.XtraReports.UI.XRLabel();
            this.kdv = new DevExpress.XtraReports.UI.XRLabel();
            this.toplam = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.tarih = new DevExpress.XtraReports.UI.XRLabel();
            this.vd = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTc = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAdi = new DevExpress.XtraReports.UI.XRLabel();
            this.adres = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.Detail.HeightF = 25.0001F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(4.791641F, 9.536743E-05F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(426.0417F, 25F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cinsi,
            this.miktar,
            this.fiyat,
            this.tutar});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // cinsi
            // 
            this.cinsi.Name = "cinsi";
            this.cinsi.Text = "cinsi";
            this.cinsi.Weight = 1.8470347197994608D;
            // 
            // miktar
            // 
            this.miktar.Name = "miktar";
            this.miktar.Text = "miktar";
            this.miktar.Weight = 0.83473750640227562D;
            // 
            // fiyat
            // 
            this.fiyat.Name = "fiyat";
            this.fiyat.Text = "fiyat";
            this.fiyat.Weight = 0.56721651801387607D;
            // 
            // tutar
            // 
            this.tutar.Name = "tutar";
            this.tutar.Text = "tutar";
            this.tutar.Weight = 0.678403552056198D;
            // 
            // yazi
            // 
            this.yazi.LocationFloat = new DevExpress.Utils.PointFloat(93.75003F, 91.99995F);
            this.yazi.Name = "yazi";
            this.yazi.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.yazi.SizeF = new System.Drawing.SizeF(278.75F, 23F);
            this.yazi.Text = "yazı ile";
            // 
            // gtoplam
            // 
            this.gtoplam.LocationFloat = new DevExpress.Utils.PointFloat(272.5F, 68.99995F);
            this.gtoplam.Name = "gtoplam";
            this.gtoplam.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.gtoplam.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.gtoplam.Text = "geneltoplam";
            // 
            // oiv
            // 
            this.oiv.LocationFloat = new DevExpress.Utils.PointFloat(272.5F, 45.99997F);
            this.oiv.Name = "oiv";
            this.oiv.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.oiv.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.oiv.Text = "oiv";
            // 
            // kdv
            // 
            this.kdv.LocationFloat = new DevExpress.Utils.PointFloat(272.5F, 22.99999F);
            this.kdv.Name = "kdv";
            this.kdv.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.kdv.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.kdv.Text = "kdv";
            // 
            // toplam
            // 
            this.toplam.LocationFloat = new DevExpress.Utils.PointFloat(272.5F, 0F);
            this.toplam.Name = "toplam";
            this.toplam.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.toplam.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.toplam.Text = "toplam";
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 98.95834F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 100F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.tarih,
            this.vd,
            this.lblTc,
            this.lblAdi,
            this.adres});
            this.PageHeader.HeightF = 96.875F;
            this.PageHeader.Name = "PageHeader";
            // 
            // tarih
            // 
            this.tarih.LocationFloat = new DevExpress.Utils.PointFloat(222.8655F, 46.35417F);
            this.tarih.Multiline = true;
            this.tarih.Name = "tarih";
            this.tarih.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.tarih.SizeF = new System.Drawing.SizeF(155.2083F, 23F);
            this.tarih.Text = "tarih";
            // 
            // vd
            // 
            this.vd.LocationFloat = new DevExpress.Utils.PointFloat(14.53211F, 69.35416F);
            this.vd.Multiline = true;
            this.vd.Name = "vd";
            this.vd.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.vd.SizeF = new System.Drawing.SizeF(191.6667F, 23F);
            this.vd.Text = "vergi dairesi";
            // 
            // lblTc
            // 
            this.lblTc.LocationFloat = new DevExpress.Utils.PointFloat(14.53211F, 46.35413F);
            this.lblTc.Multiline = true;
            this.lblTc.Name = "lblTc";
            this.lblTc.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTc.SizeF = new System.Drawing.SizeF(191.6667F, 23F);
            this.lblTc.Text = "TC/VergiNo";
            // 
            // lblAdi
            // 
            this.lblAdi.LocationFloat = new DevExpress.Utils.PointFloat(14.53211F, 0.3541311F);
            this.lblAdi.Multiline = true;
            this.lblAdi.Name = "lblAdi";
            this.lblAdi.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblAdi.SizeF = new System.Drawing.SizeF(191.6667F, 23F);
            this.lblAdi.Text = "Adi";
            // 
            // adres
            // 
            this.adres.LocationFloat = new DevExpress.Utils.PointFloat(14.53211F, 23.35412F);
            this.adres.Multiline = true;
            this.adres.Name = "adres";
            this.adres.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.adres.SizeF = new System.Drawing.SizeF(190.625F, 23F);
            this.adres.Text = "Adres";
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.toplam,
            this.kdv,
            this.oiv,
            this.gtoplam,
            this.yazi});
            this.ReportFooter.HeightF = 114.9999F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // faturaX
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.ReportFooter});
            this.DataSource = this.bindingSource1;
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 99, 100);
            this.PageHeight = 827;
            this.PageWidth = 1169;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4Rotated;
            this.Version = "15.1";
            this.AfterPrint += new System.EventHandler(this.faturaX_AfterPrint);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        public DevExpress.XtraReports.UI.DetailBand Detail;
        public DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        public DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        public System.Windows.Forms.BindingSource bindingSource1;
        public DevExpress.XtraReports.UI.XRLabel gtoplam;
        public DevExpress.XtraReports.UI.XRLabel oiv;
        public DevExpress.XtraReports.UI.XRLabel kdv;
        public DevExpress.XtraReports.UI.XRLabel toplam;
        public DevExpress.XtraReports.UI.XRTable xrTable1;
        public DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        public DevExpress.XtraReports.UI.XRTableCell cinsi;
        public DevExpress.XtraReports.UI.XRTableCell miktar;
        public DevExpress.XtraReports.UI.XRTableCell fiyat;
        public DevExpress.XtraReports.UI.XRTableCell tutar;
        public DevExpress.XtraReports.UI.XRLabel yazi;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        public DevExpress.XtraReports.UI.XRLabel tarih;
        public DevExpress.XtraReports.UI.XRLabel vd;
        public DevExpress.XtraReports.UI.XRLabel lblTc;
        public DevExpress.XtraReports.UI.XRLabel lblAdi;
        public DevExpress.XtraReports.UI.XRLabel adres;
    }
}
