<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Sonuc.aspx.cs" Inherits="TeknikServis.Sonuc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="kaydir">

        <div class="panel panel-info">
            <div class="panel-heading">
                <h3 class="panel-title baslik">İşlem Sonucu: </h3>
            </div>
            <div class="panel-body">
                <div class="alert alert-info">
                    <h1 id="baslik" runat="server"></h1>
                    <asp:GridView ID="GridView1" runat="server"></asp:GridView>
                </div>

            </div>
            <div class="panel-footer">
                <div class="btn-group">
                    <asp:Button ID="Button5" runat="server" Text="Giriş" CssClass="btn btn-info" OnClick="btnAna_Click" />
                    <asp:Button ID="Button6" runat="server" Text="Geri" CssClass="btn btn-info" OnClick="btnGeri_Click" />
                </div>

            </div>
        </div>

    </div>
</asp:Content>
