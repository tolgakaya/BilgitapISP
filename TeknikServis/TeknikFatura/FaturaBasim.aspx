<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="FaturaBasim.aspx.cs" Inherits="TeknikServis.FaturaBasim" %>

<%@ Register Assembly="DevExpress.XtraReports.v15.1.Web, Version=15.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";

            if (confirm("Kaydı silmek istiyor musunuz?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }


    </script>
    <div class="kaydir">

        <div id="panelContents" runat="server" class="panel panel-info">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" class="collapsed">Arama Kriterleri</a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse" style="height: 0px;">
                <div class="panel-body">
                    <div class="row ">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label for="datetimepicker6">Şu Tarihten:</label>
                                <input type='text' runat="server" class="form-control" id="datetimepicker6" />

                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label for="datetimepicker7">Şu Tarihe:</label>
                                <input type='text' runat="server" class="form-control" id="datetimepicker7" />
                            </div>
                        </div>
                    </div>


                    <div class="form-group">

                        <asp:Button ID="btnBasilmis" Visible="false" CssClass="btn btn-info btn-lg btn-block" runat="server" Text="Ara..." OnClick="Button2_Click" />
                        <button id="btnGuncel" visible="false" runat="server" class="btn btn-info btn-lg btn-block" type="submit" onserverclick="MusteriAra">Ara...</button>
                    </div>
                    <!--body-->
                </div>
            </div>
        </div>

        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                <asp:Label Text="BASILMAMIŞ FATURALAR" ID="baslik" runat="server" />

                <div class="pull-right">
                    <div class="btn-group">
                        <button type="button" class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown">
                            Fatura Tipi Seçiniz
                                        <span class="caret"></span>
                        </button>
                        <ul class="dropdown-menu pull-right" role="menu">

                            <li><a id="linkCihaz" runat="server" href="/TeknikFatura/FaturaBasim.aspx?tip=cihaz">Ürün/Parça Faturası</a>
                            </li>
                            <li><a id="linkServis" runat="server" href="/TeknikFatura/FaturaBasim.aspx?tip=servis">Servis Faturası</a>
                            </li>
                            <li><a id="linkPesin" runat="server" href="/TeknikFatura/FaturaBasim.aspx?tip=pesin">Peşin Satışlar</a>
                            </li>
                            <li><a id="linkHepsi" runat="server" href="/TeknikFatura/FaturaBasim.aspx?tip=hepsi">Tümünü Göster</a>
                            </li>
                        </ul>

                    </div>
                 
                </div>

            </div>
            <%--<div class="panel-body">
               
            </div>--%>
            <div class="table-responsive ">

                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>

                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel ID="upCrudGrid" runat="server">
                    <ContentTemplate>
                        <div id="cariOzet" runat="server" class="pull-right">
                            <span id="txtAdet" runat="server" class="label label-success"></span>
                            <span id="txtTutar" runat="server" class="label label-warning"></span>
                            <span id="txtKDV" runat="server" class="label label-primary"></span>
                            <span id="txtOIV" runat="server" class="label label-info"></span>
                            <span id="txtYekun" runat="server" class="label label-danger"></span>

                        </div>
                        <%--  <script type="text/javascript">
                            Sys.Application.add_load(jScript);
                        </script>--%>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="ID"
                            EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand"
                            AllowPaging="true" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging">
                            <%-- <RowStyle BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("css").ToString())%>' />--%>

                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>

                                <asp:TemplateField HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <ItemTemplate>

                                        <div class="visible-lg">
                                            <asp:LinkButton ID="btnDel"
                                                runat="server"
                                                CssClass="btn btn-danger btn-xs"
                                                CommandName="silbak" CommandArgument='<%# Eval("tur") +";"+Eval("ID") %>'  OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />

                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-lg" />

                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <ItemTemplate>

                                        <div class="visible-lg">
                                            <asp:LinkButton ID="btnEdit"
                                                runat="server"
                                                CssClass="btn btn-success"
                                                CommandName="faturaBas" CommandArgument='<%# Eval("tur") +";"+Eval("ID") %>' Text="<i class='fa fa-print'></i>" />

                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-lg" />

                                </asp:TemplateField>

                                <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun">
                                    <%--     <HeaderStyle CssClass="visible-lg" />
                                <ItemStyle CssClass="visible-lg" />--%>
                                </asp:BoundField>
                                <asp:BoundField DataField="fat_seri" HeaderText="Seri">
                                    <%--     <HeaderStyle CssClass="visible-lg" />
                                <ItemStyle CssClass="visible-lg" />--%>
                                </asp:BoundField>
                                <asp:BoundField DataField="fat_no" HeaderText="No">
                                    <%--     <HeaderStyle CssClass="visible-lg" />
                                <ItemStyle CssClass="visible-lg" />--%>
                                </asp:BoundField>
                                <asp:BoundField DataField="isim" HeaderText="İsim">
                                    <%--    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />--%>
                                </asp:BoundField>
                                <asp:BoundField DataField="KDV" HeaderText="KDV" DataFormatString="{0:C}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <%-- <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />--%>
                                </asp:BoundField>
                                <asp:BoundField DataField="OIV" HeaderText="ÖİV" DataFormatString="{0:C}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <%--  <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />--%>
                                </asp:BoundField>
                                <asp:BoundField DataField="Yekun" HeaderText="Yekün" DataFormatString="{0:C}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <%--  <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />--%>
                                </asp:BoundField>
                                <asp:BoundField DataField="tarih" HeaderText="Tarih" HeaderStyle-CssClass="visible-lg " DataFormatString="{0:D}" ItemStyle-CssClass="visible-lg">
                                    <%--     <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />--%>
                                </asp:BoundField>
                                <asp:BoundField DataField="tur" HeaderText="Tür" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <%--  <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />--%>
                                </asp:BoundField>
                             


                            </Columns>

                        </asp:GridView>


                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuncel" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnBasilmis" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnKapat" EventName="Click" />

                    </Triggers>
                </asp:UpdatePanel>


            </div>
            <div class="panel-footer pull-right">
                <div class=" btn-group">

                    <asp:LinkButton ID="btnPrint"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnPrnt_Click"
                        Text="<i class='fa fa-print icon-2x'></i>" />


                    <asp:LinkButton ID="btnExportExcel"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnExportExcel_Click"
                        Text="<i class='fa fa-file-excel-o icon-2x'></i>" />

                    <asp:LinkButton ID="btnExportWord"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnExportWord_Click"
                        Text="<i class='fa fa-wikipedia-w icon-2x'></i>" />



                </div>

            </div>

            <div id="detailModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-content modal-lg">
                    <div class="modal-header modal-header-info">
                        <%--    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>--%>
                        <h3 id="myModalLabel" class="baslik">Fatura Bilgileri</h3>
                    </div>

                    <div class="modal-body">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">

                            <ContentTemplate>

                                <dx:ASPxDocumentViewer ID="faturaGoster" runat="server">
                                </dx:ASPxDocumentViewer>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />

                            </Triggers>
                        </asp:UpdatePanel>
                        <div class="modal-footer">
                            <%-- <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Kapat</button>--%>
                            <asp:Button ID="btnKapat" runat="server" CssClass="btn btn-info" Text="Kapat" OnClick="btnKapat_Click" />
                        </div>
                    </div>


                </div>

            </div>
            <!-- Add Record Modal Starts here-->
            <div id="addModal" class="modal  fade" tabindex="-1" role="dialog"
                aria-labelledby="addModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-content modal-md">
                    <div class="modal-header modal-header-info">
                        <button type="button" class="close" data-dismiss="modal"
                            aria-hidden="true">
                            ×</button>
                        <h3 id="addModalLabel" class="baslik">Pos Tanımlama</h3>
                    </div>
                    <asp:UpdatePanel ID="upAdd" runat="server">

                        <ContentTemplate>

                            <div class="modal-body">
                                <div class="form-horizontal">

                                    <div class="form-group">
                                        <asp:Label runat="server" AssociatedControlID="txtUnvan" CssClass="col-md-4 control-label">Ünvan</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtUnvan" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtUnvan" ErrorMessage="Ünvan giriniz."></asp:RequiredFieldValidator>

                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" AssociatedControlID="txtVD" CssClass="col-md-4 control-label">Vergi Dairesi</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtVD" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtVD" ErrorMessage="Vergi Dairesi giriniz"></asp:RequiredFieldValidator>

                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" AssociatedControlID="txtVN" CssClass="col-md-4 control-label">TC/Vergi No</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtVN" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtVN" ErrorMessage="VergiNo veya TC giriniz"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                     <div class="form-group">
                                        <asp:Label runat="server" AssociatedControlID="txtAdres" CssClass="col-md-4 control-label">Adres</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtAdres" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtAdres" ErrorMessage="Adres giriniz"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnPesinKaydet" runat="server" Text="Kaydet"
                                    CssClass="btn btn-info" ValidationGroup="musteriGrup2" OnClick="btnPesinKaydet_Click" />
                                <button class="btn btn-info" data-dismiss="modal"
                                    aria-hidden="true">
                                    Kapat</button>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnPesinKaydet" EventName="Click" />

                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!--Add Record Modal Ends here-->
        </div>

    </div>


    <script type="text/javascript">
        $(document).ready(function () {
            $('#detailModal').on('shown.bs.modal', function (e) {
                ContentPlaceHolder1_faturaGoster_Splitter.AdjustControl();
            })
        });

    </script>
    <script type="text/javascript">
        $(function () {
            $('#ContentPlaceHolder1_datetimepicker6').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
            $('#ContentPlaceHolder1_datetimepicker7').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
            $("#ContentPlaceHolder1_datetimepicker6").on("dp.change", function (e) {
                $('#ContentPlaceHolder1_datetimepicker7').data("DateTimePicker").minDate(e.date);
            });
            $("#ContentPlaceHolder1_datetimepicker7").on("dp.change", function (e) {
                $('#ContentPlaceHolder1_datetimepicker6').data("DateTimePicker").maxDate(e.date);
            });
        });

    </script>

</asp:Content>
