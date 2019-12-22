<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Baski.aspx.cs" Inherits="TeknikServis.Baski" %>

<%@ Register Assembly="DevExpress.XtraReports.v15.1.Web, Version=15.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxDocumentViewer ID="baskiGoster" runat="server"></dx:ASPxDocumentViewer>
</asp:Content>
