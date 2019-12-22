<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Alimlar.aspx.cs" Inherits="TeknikServis.TeknikAlim.Alimlar" %>

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

                        <button id="btnAra" runat="server" class="btn btn-info btn-lg btn-block" type="submit" onserverclick="AlimAra">Ara...</button>
                    </div>
                    <!--body-->
                </div>
            </div>
        </div>

        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                Tarihe Göre Satın Almalar

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

                            <span id="txtYekun" runat="server" class="label label-danger"></span>

                        </div>

                        <asp:GridView ID="grdAlimlar" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="alim_id"
                            EmptyDataText="Satın alma kaydı bulunmuyor" OnRowCommand="grdAlimlar_RowCommand"
                            AllowPaging="true" PageSize="10" OnPageIndexChanging="grdAlimlar_PageIndexChanging" OnRowCreated="grdAlimlar_RowCreated">

                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>

                                <%-- <%# Container.DataItemIndex %>' Text="<i class='fa fa-pencil'></i>" /> --%>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        
                                            <asp:LinkButton ID="btnDetay"
                                                runat="server"
                                                CssClass="btn btn-success"
                                                CommandName="detay" CommandArgument='<%# Eval("alim_id") %>' Text="<i class='fa fa-pencil'></i>" />

                                            <asp:LinkButton ID="btnDel"
                                                runat="server"
                                                CssClass="btn btn-danger"
                                                CommandName="del" CommandArgument='<%# Eval("alim_id") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                        </div>
                                    </ItemTemplate>
                                 

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("musteri_adi") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnTedarikci"
                                            runat="server"
                                            CssClass="btn btn-primary"
                                            CommandName="detail" CommandArgument='<%#Eval("CustID") %>' Text=' <%#Eval("musteri_adi") %> '>
                           
                                        </asp:LinkButton>
                                    </ItemTemplate>

                                </asp:TemplateField>

                                <%--<asp:BoundField DataField="musteri_adi" HeaderText="Kişi/Firma"></asp:BoundField>--%>
                                <asp:BoundField DataField="konu" HeaderText="Konu" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>

                                <asp:BoundField DataField="aciklama" HeaderText="Açıklama" ></asp:BoundField>
                                <asp:BoundField DataField="belge_no" HeaderText="Belge No" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                <asp:BoundField DataField="toplam_tutar" HeaderText="Tutar" DataFormatString="{0:C}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                <asp:BoundField DataField="toplam_kdv" HeaderText="KDV" DataFormatString="{0:C}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                <asp:BoundField DataField="toplam_yekun" HeaderText="Yekün" DataFormatString="{0:C}"   />
                                <asp:BoundField DataField="alim_tarih" HeaderText="Tarih"  DataFormatString="{0:d}" ></asp:BoundField>
                                <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg "  ItemStyle-CssClass="visible-lg"></asp:BoundField>


                            </Columns>

                        </asp:GridView>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAra" EventName="ServerClick" />

                    </Triggers>
                </asp:UpdatePanel>


            </div>
            <div class="panel-footer pull-right">
                <div class=" btn-group">

                    <asp:LinkButton ID="btnYeni"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnYeni_Click"
                        Text="Yeni" />
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
                    <asp:LinkButton ID="btnMusteriDetayim"
                        runat="server" Visible="false"
                        CssClass="btn btn-info " OnClick="btnMusteriDetayim_Click"
                        Text="<i class='fa fa-user icon-2x'></i>" />

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
