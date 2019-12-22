<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Deneme.aspx.cs" Inherits="TeknikServis.Deneme" %>

<%@ Register Assembly="DevExpress.XtraReports.v15.1.Web, Version=15.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    

    <dx:ASPxDocumentViewer ID="gosterge" runat="server"></dx:ASPxDocumentViewer>
    <%--<rsweb:ReportViewer ID="ReportViewer1" runat="server"></rsweb:ReportViewer>--%>
    <asp:GridView ID="GridView2" runat="server"></asp:GridView>
    <%--    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />--%>
    <h1 id="baslik" runat="server"></h1>
    <asp:GridView ID="GridView1" runat="server"></asp:GridView>
    <%-- <asp:Button ID="Button1" runat="server" Text="Kartla Öde"  OnClick="Button1_Click1"/>--%>
    <asp:Button Text="cache sil" ID="caching" OnClick="caching_Click" runat="server" />
       <asp:Button Text="cache goster" ID="btnCache" OnClick="btnCache_Click"  runat="server" />
       <asp:TextBox runat="server" ID="txtCache" CssClass="form-control" />
    <div class="form-horizontal">


        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtFirma" CssClass="col-md-4 control-label">Firma Kodu</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="txtFirma" CssClass="form-control" />

            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtTamFirma" CssClass="col-md-4 control-label">Tam Firma İsmi</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="txtTamFirma" CssClass="form-control" />

            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtResimyol" CssClass="col-md-4 control-label">Logo Yolu</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="txtResimyol" CssClass="form-control" />

            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txttelefon" CssClass="col-md-4 control-label">Telefon</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="txttelefon" CssClass="form-control" />

            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtWeb" CssClass="col-md-4 control-label">Web Adresi</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="txtWeb" CssClass="form-control" />

            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtAdmin" CssClass="col-md-4 control-label">Admin Kullanıcı Adı</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="txtAdmin" CssClass="form-control" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtAdres" CssClass="col-md-4 control-label">Firma Adresi</asp:Label>
            <div class="col-md-8">
                <asp:TextBox runat="server" ID="txtAdres" CssClass="form-control" />
            </div>
        </div>
        <asp:Button ID="Button2" runat="server" CssClass="btn btn-danger" Text="Temizle" OnClick="Button2_Click" />
    </div>
        <asp:Button ID="Button8" runat="server" CssClass="btn btn-primary" Text="Düzelt" OnClick="btnDuzelt_Click" />
    <asp:Button ID="btnDuzelt" runat="server" CssClass="btn btn-primary" Text="Düzelt" OnClick="btnDuzelt_Click" />
      <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Ödeme ve Tahsilat " OnClick="Button1_Click2" />
      <asp:Button ID="Button3" runat="server" CssClass="btn btn-primary" Text="Ödemeler " OnClick="Button1_Click3" />
      <asp:Button ID="Button4" runat="server" CssClass="btn btn-primary" Text="Tahsilatlar " OnClick="Button1_Click4" />
     <asp:Button ID="Button5" runat="server" CssClass="btn btn-primary" Text="gruplu " OnClick="Button1_Click5" />
      <asp:Button ID="Button6" runat="server" CssClass="btn btn-primary" Text="tahsilat_ödeme_satis" OnClick="Button1_Click6" />
      <asp:Button ID="Button7" runat="server" CssClass="btn btn-primary" Text="aylık rapor" OnClick="Button1_Click7" />
    <asp:Button ID="btnDeneme" runat="server" CssClass="btn btn-primary" Text="Kullanıcı Güncelle" OnClick="btnDeneme_Click" />
     <asp:Button ID="btnCariTel" runat="server" CssClass="btn btn-primary" Text="Cari Tel" OnClick="btnCariTel_Click"/>
      <asp:Button ID="btnSMS" runat="server" CssClass="btn btn-primary" Text="Cari Tel" OnClick="btnSMS_Click"/>
      <asp:Button ID="btnCihazGuncelle" runat="server" CssClass="btn btn-danger" Text="Cihaz Güncelle" OnClick="btnCihazGuncelle_Click"/>

      <asp:Button ID="btnMusteriOnline" runat="server" CssClass="btn btn-success" Text="OnlineMüsteri Sorgusu" OnClick="btnMusteriOnline_Click"/>

    <asp:Button ID="btnOdemeDeneme" runat="server" CssClass="btn btn-success" Text="Müşteri Kartı İle Öde" OnClick="btnOdemeDeneme_Click"/>
       <asp:Button ID="btnRadcheck" runat="server" CssClass="btn btn-success" Text="Radcheck Düzenle" OnClick="btnRadcheck_Click"/>
     <asp:Button ID="btnIslemTarih" runat="server" CssClass="btn btn-success" Text="İşlem Tarih Düzenle" OnClick="btnIslemTarih_Click"/>




      




</asp:Content>
