<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tasarimci.aspx.cs" Inherits="TeknikServis.Admin.Tasarimci" %>

<%@ Register Assembly="DevExpress.XtraReports.v15.1.Web, Version=15.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/bootstrap-theme.min.css" rel="stylesheet" />
  
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel-footer pull-right">
            <div class="btn-group pull-right hidden-xs hidden-sm">

                <asp:Button ID="Button1" runat="server" Text="Giriş" CssClass="btn btn-primary" OnClick="btnAna_Click" />

                <asp:Button ID="Button3" runat="server" Text="Geri Dön" CssClass="btn btn-danger" OnClick="btnGeri_Click" />
                <input type="button" value="Yenile" class="btn btn-success" onclick="document.location.reload(true)" />

            </div>
            <div class="btn-group pull-right visible-xs visible-sm">

                <asp:Button ID="Button7" runat="server" Text="Giriş" CssClass="btn btn-primary btn-sm" OnClick="btnAna_Click" />
                <asp:Button ID="Button9" runat="server" Text="Geri Dön" CssClass="btn btn-danger btn-sm" OnClick="btnGeri_Click" />
                <input type="button" value="Yenile" class="btn btn-success btn-sm" onclick="document.location.reload(true)" />

            </div>

        </div>
        <div>

            <dx:ASPxReportDesigner ID="ASPxReportDesigner1" runat="server" OnSaveReportLayout="ASPxReportDesigner1_SaveReportLayout"></dx:ASPxReportDesigner>
        </div>

    </form>
</body>
</html>
