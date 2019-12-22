<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="MusteriAnten.aspx.cs" Inherits="TeknikServis.TeknikHarita.MusteriAnten" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="kaydir">
        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                <h4 id="baslikkk" runat="server" class="panel-title">
                    <label id="baslik" runat="server">Müşteri Anten İlişkisi</label>

                </h4>
            </div>
            <%--<div class="panel-body">
               
            </div>--%>
            <div class="table-responsive ">
                <div class="input-group custom-search-form">
                    <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Ara..." />
                    <span class="input-group-btn">
                        <button id="btnARA" runat="server" class="btn btn-default" type="submit" onserverclick="MusteriAra">
                            <i class="fa fa-search"></i>
                        </button>
                    </span>
                </div>
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>

                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="upCrudGrid" runat="server">
                    <ContentTemplate>
                        <asp:HiddenField ID="hdnTeller" runat="server" />
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="CustID"
                            EmptyDataText="Kayıt girilmemiş" OnRowDataBound="GridView1_RowDataBound" AllowPaging="true" OnRowCreated="GridView1_RowCreated" OnPageIndexChanging="GridView1_PageIndexChanging"
                            PageSize="100">

                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField HeaderText="İşlem" ShowHeader="False">
                                    <ItemTemplate>
                                        <div class="visible-lg visible-xs visible-sm">
                                            <asp:LinkButton ID="btnTekAnten"
                                                runat="server"
                                                CssClass="btn btn-success"
                                                Text="<i class='fa fa-map'>Hasitada</i>" />
                                            
                                        </div>
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" SortExpression="Heading Title">
                                        <itemtemplate>
                                        <asp:Label ID="lblID" Text='<%# Bind("CustID") %>' runat="server" />
                                    </itemtemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="İsim" SortExpression="Heading Title">
                                        <itemtemplate>
                                        <asp:Label ID="lblAd" Text='<%# Bind("Ad") %>' runat="server" />
                                    </itemtemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Adres" SortExpression="Heading Title">
                                        <itemtemplate>
                                        <asp:Label ID="lblAdres" Text='<%# Bind("Adres") %>' runat="server" />
                                    </itemtemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Anten" ItemStyle-Width="150">
                                        <itemtemplate>
                                        <asp:DropDownList ID="ddlAnten" runat="server">
                                            <asp:ListItem Text="Antenseçiniz" Value="-1"></asp:ListItem>
                                            <%--  <asp:ListItem Text="ikinci Seçim" />--%>
                                        </asp:DropDownList>
                                    </itemtemplate>
                                    </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnARA" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-info btn-block" Text="Update" OnClick="btnUpdate_Click" />

            </div>
            <div id="onayModal" class="modal  fade" tabindex="-1" role="dialog"
                aria-labelledby="addModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-content modal-sm">

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">

                        <ContentTemplate>
                            <div class="modal-body">
                                <div class="row">

                                    <div class="col-md-12">
                                        <div class="alert alert-info text-center">
                                            <i class="fa fa-2x">Anten Seçimi</i>
                                            <asp:HiddenField ID="hdnEskiAnten" runat="server" />
                                            <asp:DropDownList ID="drdAntenler" CssClass="form-control" runat="server">
                                                <asp:ListItem Text="Anten seçiniz" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>


                                            <div class="btn-group pull-right">

                                                <asp:Button ID="btnAntenKaydet" runat="server" Text="Tamam"
                                                    CssClass="btn btn-info" OnClick="btnAntenKaydet_Click" />
                                                <button class="btn btn-info" data-dismiss="modal"
                                                    aria-hidden="true">
                                                    Kapat</button>

                                            </div>
                                        </div>
                                    </div>



                                </div>
                            </div>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAntenKaydet" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnHepsiniTasi" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>


            <div class="panel-footer pull-right">
                <div class=" btn-group">


                    <asp:LinkButton ID="btnPrint"
                        runat="server"
                        CssClass="btn btn-info visible-lg" OnClick="btnPrint_Click"
                        Text="<i class='fa fa-print icon-2x'></i>" />


                    <asp:LinkButton ID="btnExportExcel"
                        runat="server"
                        CssClass="btn btn-info visible-lg" OnClick="btnExportExcel_Click"
                        Text="<i class='fa fa-file-excel-o icon-2x'></i>" />



                    <asp:LinkButton ID="btnExportWord"
                        runat="server"
                        CssClass="btn btn-info visible-lg" OnClick="btnExportWord_Click"
                        Text="<i class='fa fa-wikipedia-w icon-2x'></i>" />

                    <asp:LinkButton ID="btnSms"
                        runat="server"
                        CssClass="btn btn-info" OnClick="btnSms_Click"
                        Text="<i class='fa fa-phone-square icon-2x'>SMS</i>" />

                    <asp:LinkButton ID="btnMail"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnMail_Click"
                        Text="<i class='fa fa-wifi icon-2x'>Antenler</i>" />
                    <asp:LinkButton ID="btnHepsiniTasi"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnHepsiniTasi_Click"
                        Text="<i class='fa fa-external-link icon-2x'>Taşı</i>" />
                </div>

            </div>
        </div>
    </div>
</asp:Content>
