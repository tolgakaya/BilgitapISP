<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ServisTamirci.aspx.cs" Inherits="TeknikServis.TeknikTeknik.ServisTamirci" %>

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
                                <label for="drdKritik">Tamam/Açık</label>
                                <asp:DropDownList ID="drdKritik" runat="server" class="form-control">
                                    <asp:ListItem Text="Açık Servisler" Value="acik"></asp:ListItem>
                                    <asp:ListItem Text="Tamamlanan Servisler" Value="tamam"></asp:ListItem>
                                    <%--<asp:ListItem Text="Bütün Servisler" Value="hepsi"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <asp:Button ID="btnAra" CssClass="btn btn-info btn-lg btn-block" runat="server" Text="Ara..." OnClick="btnAra_Click" />
                            <%--<asp:Button ID="btnRapor" CssClass="btn btn-primary btn-lg btn-block" runat="server" Text="Rapor..." OnClick="btnRapor_Click" />--%>
                        </div>
                    </div>

                    <!--body-->
                </div>
            </div>
        </div>

        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
            <ProgressTemplate>

                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                </div>

            </ProgressTemplate>
        </asp:UpdateProgress>
        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                SERVİS KARAR VE HESAPLARI
            </div>
            <div class="table-responsive">

                <asp:UpdatePanel ID="upCrudGrid" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div id="cariOzet" runat="server" class="pull-right">
                            <span id="txtHesapAdet" runat="server" class="label label-success"></span>
                            <span id="txtHesapMaliyet" runat="server" class="label label-warning"></span>
                            <span id="txtHesapTutar" runat="server" class="label label-info"></span>
                        </div>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                            DataKeyNames="hesapID"
                            EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound"
                            OnPageIndexChanging="GridView1_PageIndexChanging" AllowPaging="true" PageSize="10">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>

                                <asp:TemplateField>
                                    <ItemTemplate>


                                        <asp:LinkButton ID="delLink"
                                            runat="server"
                                            CssClass="btn btn-danger btn-xs visible-lg"
                                            CommandName="del" CommandArgument='<%#Eval("hesapID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />

                                        <asp:LinkButton ID="btnOnay"
                                            runat="server"
                                            CssClass="btn btn-success btn-xs visible-lg"
                                            CommandName="onay" CommandArgument='<%#Eval("hesapID")+ ";" + Container.DisplayIndex +";"+ Eval("musteriID")  %>' Text="<i class='fa fa-check'></i>" />

                                        <asp:LinkButton ID="btnServis"
                                            runat="server"
                                            CssClass="btn btn-success btn-xs visible-lg"
                                            CommandName="gonder" CommandArgument='<%#Eval("servisID")+ ";" + Container.DisplayIndex +";"+ Eval("musteriID")+";"+ Eval("kimlik")  %>' Text="<i class='fa fa-wrench'></i>" />

                                        <asp:LinkButton ID="delLinkK"
                                            runat="server"
                                            CssClass="btn btn-danger visible-xs visible-sm"
                                            CommandName="del" CommandArgument='<%#Eval("hesapID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />

                                        <asp:LinkButton ID="btnOnayK"
                                            runat="server"
                                            CssClass="btn btn-success visible-xs visible-sm"
                                            CommandName="onay" CommandArgument='<%#Eval("hesapID")+ ";" + Container.DisplayIndex +";"+ Eval("musteriID")  %>' Text="<i class='fa fa-check'></i>" />

                                        <asp:LinkButton ID="btnServisK"
                                            runat="server"
                                            CssClass="btn btn-success visible-xs visible-sm"
                                            CommandName="gonder" CommandArgument='<%#Eval("servisID")+ ";" + Container.DisplayIndex +";"+ Eval("musteriID")+";"+ Eval("kimlik")  %>' Text="<i class='fa fa-wrench'></i>" />

                                    </ItemTemplate>

                                </asp:TemplateField>


                                <asp:BoundField DataField="hesapID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="islemParca" HeaderText="İşlem" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="cihaz" HeaderText="Ürün/Parça" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="kdv" HeaderText="KDV" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="tutar" HeaderText="Tutar" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="yekun" HeaderText="Yekün" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="birim_maliyet" HeaderText="Birim Maliyet" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="toplam_maliyet" HeaderText="Toplam Maliyet" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="onayDurumu" HeaderText="Onay" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>


                                <asp:BoundField DataField="tarihZaman" HeaderText="Hesap Tarihi" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="onayTarih" HeaderText="Onay Tarihi" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="servisID" HeaderText="SID" HeaderStyle-CssClass="hidden-xs  hidden-lg" ItemStyle-CssClass="hidden-xs hidden-lg"></asp:BoundField>
                                <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                            </Columns>

                        </asp:GridView>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnAra" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>

                <div id="onayModal" class="modal  fade" tabindex="-1" role="dialog"
                    aria-labelledby="addModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-content modal-sm">

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">

                            <ContentTemplate>
                                <div class="modal-body">
                                    <div class="row">

                                        <div class="col-md-12">
                                            <div class="alert alert-info text-center">
                                                <i class="fa fa-2x">Servis hesabını onaylıyor musunuz?</i>
                                                <div class="checkbox-inline">

                                                    <asp:CheckBox ID="chcSms" Text="SMS" runat="server" />
                                                </div>
                                                <div class="checkbox-inline">

                                                    <asp:CheckBox ID="chcMail" Text="Mail" runat="server" />
                                                </div>

                                                <div class="btn-group pull-right">

                                                    <asp:Button ID="btnOnay" runat="server" Text="Tamam"
                                                        CssClass="btn btn-success" OnClick="btnOnay_Click" />
                                                    <button class="btn btn-warning" data-dismiss="modal"
                                                        aria-hidden="true">
                                                        Kapat</button>

                                                </div>
                                            </div>
                                        </div>

                                        <asp:HiddenField ID="hdnHesapID" runat="server" />
                                        <asp:HiddenField ID="hdnMusteriID" runat="server" />
                                        <asp:HiddenField ID="hdnServisIDD" runat="server" />
                                        <asp:HiddenField ID="hdnIslemm" runat="server" />
                                        <asp:HiddenField ID="hdnYekunn" runat="server" />
                                    </div>
                                </div>

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnOnay" EventName="Click" />

                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
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

        });

    </script>
</asp:Content>
