<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="KartExtreOdemeler.aspx.cs" Inherits="TeknikServis.TeknikCari.KartExtreOdemeler" %>

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

                        <button id="btnGoster" runat="server" class="btn btn-info btn-lg btn-block" type="submit">>Göster...</button>
                    </div>
                    <!--body-->
                </div>
            </div>
        </div>

        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                <h4 id="baslikkk" runat="server" class="panel-title">
                    <label id="baslik" runat="server">Kart Extresi-Kart Adı-Görünüm Türü</label>

                </h4>
            </div>
            <%--<div class="panel-body">
               
            </div>--%>
            <div class="table-responsive">
                <div class="alert alert-info">
                    <%-- KULLANİCİ BAŞLIYOR --%>
                    <span class="label label-primary" id="spnBorc" runat="server">Borç Toplamı</span>
                    <span class="label label-success" id="spnOdenen" runat="server">Ödeme Toplamı</span>
                    <span class="label label-danger" id="spnBakiye" runat="server">Net Bakiye</span>

                    <%-- KULLANICI BİTİYOR --%>
                </div>

                <div id="cariOzet" runat="server" class="pull-right">
                    <span id="txtAdet" runat="server" class="label label-warning"></span>
                    <span id="txtTutar" runat="server" class="label label-primary"></span>
                </div>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                    DataKeyNames="id"
                    EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_RowCreated"
                    OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" AllowPaging="true" PageSize="30">
                    <PagerStyle CssClass="pagination-ys" />
                    <Columns>

                        <asp:BoundField DataField="kart_id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                            <HeaderStyle CssClass="visible-lg" />
                            <ItemStyle CssClass="visible-lg" />
                        </asp:BoundField>
                        <asp:BoundField DataField="kart_adi" HeaderText="Kart"></asp:BoundField>


                        <asp:BoundField DataField="tutar" HeaderText="Tutar" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                            <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                            <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                        </asp:BoundField>


                        <asp:BoundField DataField="tarih" HeaderText="Tarih" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:D}" ItemStyle-CssClass="visible-lg">
                            <HeaderStyle CssClass="visible-lg" />
                            <ItemStyle CssClass="visible-lg" />
                        </asp:BoundField>

                        <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                            <HeaderStyle CssClass="visible-lg" />
                            <ItemStyle CssClass="visible-lg" />
                        </asp:BoundField>
                        <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                            <HeaderStyle CssClass="visible-lg" />
                            <ItemStyle CssClass="visible-lg" />
                        </asp:BoundField>

                    </Columns>

                </asp:GridView>

            </div>
            <div class="panel-footer pull-right">
                <div class=" btn-group">

                    <asp:Button ID="btnYeniOdeme" runat="server" Text="Öde" CssClass="btn btn-info"
                        OnClick="btnYeniOdeme_Click" />

                    <asp:LinkButton ID="btnPrint"
                        runat="server"
                        CssClass="btn btn-info visible-lg" OnClick="btnPrnt_Click"
                        Text="<i class='fa fa-print icon-2x'></i>" />


                    <asp:LinkButton ID="btnExportExcel"
                        runat="server"
                        CssClass="btn btn-info visible-lg" OnClick="btnExportExcel_Click"
                        Text="<i class='fa fa-file-excel-o icon-2x'></i>" />

                    <asp:LinkButton ID="btnExportWord"
                        runat="server"
                        CssClass="btn btn-info visible-lg" OnClick="btnExportWord_Click"
                        Text="<i class='fa fa-wikipedia-w icon-2x'></i>" />

                    <asp:LinkButton ID="btnKartExtresi"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnKartExtresi_Click"
                        Text="Extre" />
                </div>

            </div>

        </div>
    </div>
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
