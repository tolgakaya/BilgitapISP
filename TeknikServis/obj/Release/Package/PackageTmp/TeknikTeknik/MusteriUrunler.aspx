<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="MusteriUrunler.aspx.cs" Inherits="TeknikServis.TeknikTeknik.MusteriUrunler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="kaydir">
        <div class="panel-group">

            <%-- müşteri seçimi başlıyor --%>
            <div class="panel panel-info">
                <!-- Default panel contents -->
                <div class="panel-heading">
                    MÜŞTERİ SEÇİMİ
                </div>
                <%--<div class="panel-body">
               
            </div>--%>
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>

                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div class="table-responsive">
                    <div class="input-group custom-search-form">
                        <input runat="server" type="text" id="txtMusteriSorgu" class="form-control" placeholder="Ara..." />
                        <span class="input-group-btn">
                            <button id="btnMusteriAra" runat="server" class="btn btn-default" type="submit" onserverclick="MusteriAra">
                                <i class="fa fa-search"></i>
                            </button>
                        </span>
                    </div>

                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>

                            <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="CustID"
                                EmptyDataText="Kayıt girilmemiş" EnablePersistedSelection="false"
                                OnSelectedIndexChanged="GridView3_SelectedIndexChanged" OnRowCommand="GridView3_RowCommand" SelectedIndex="0">
                                <SelectedRowStyle CssClass="danger" />

                                <Columns>
                                    <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Seç">
                                        <ControlStyle CssClass="btn btn-danger"></ControlStyle>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="CustID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                        <HeaderStyle CssClass="visible-lg" />
                                        <ItemStyle CssClass="visible-lg" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Müşteri Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnRandom"
                                                runat="server"
                                                CssClass="btn btn-primary"
                                                CommandName="detail" CommandArgument='<%#Eval("CustID") %>' Text=' <%#Eval("Ad") %> '>  </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                        <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Adres" HeaderText="Adres" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                        <HeaderStyle CssClass="visible-lg" />
                                        <ItemStyle CssClass="visible-lg" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Telefon" HeaderText="Telefon" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                        <HeaderStyle CssClass="visible-lg" />
                                        <ItemStyle CssClass="visible-lg" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Tc" HeaderText="Tc" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                        <HeaderStyle CssClass="visible-lg" />
                                        <ItemStyle CssClass="visible-lg" />
                                    </asp:BoundField>
                                </Columns>

                            </asp:GridView>
                            <%-- <asp:Button ID="btnAdd" runat="server" Text="Add New Record" CssClass="btn btn-info"
                    OnClick="btnAdd_Click" />--%>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnMusteriAra" EventName="ServerClick" />
                        </Triggers>

                    </asp:UpdatePanel>
                    <!-- Detail Modal Starts here-->
                    <div id="detailModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-content modal-sm">
                            <div class="modal-header modal-header-info">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <h3 id="myModalLabel">Müşteri Detayları</h3>
                            </div>

                            <div class="modal-body">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>

                                        <asp:DetailsView ID="DetailsView1" runat="server" CssClass="table table-bordered table-hover"
                                            BackColor="White" ForeColor="Black" FieldHeaderStyle-Wrap="false" FieldHeaderStyle-Font-Bold="true"
                                            FieldHeaderStyle-BackColor="LavenderBlush" FieldHeaderStyle-ForeColor="Black"
                                            BorderStyle="Groove" AutoGenerateRows="False">
                                            <Fields>
                                                <asp:BoundField DataField="CustID" HeaderText="ID" />
                                                <asp:BoundField DataField="Ad" HeaderText="Müşteri Adı" />
                                                <asp:BoundField DataField="Adres" HeaderText="Müşteri Adresi" />
                                                <asp:BoundField DataField="Telefon" HeaderText="Telefon" />

                                            </Fields>
                                        </asp:DetailsView>


                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="GridView3" EventName="RowCommand" />
                                        <%-- <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />--%>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <div class="modal-footer">
                                    <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Kapat</button>
                                </div>
                            </div>


                        </div>

                    </div>
                    <!-- Detail Modal Ends here -->
                </div>

            </div>
            <%-- müşteri seçimiitiyor --%>

            <div class="panel panel-info">
                <div class="panel-heading">
                    MÜŞTERİ CİHAZLARI
                   
                </div>
                <%--<div class="panel-body">
               
            </div>--%>
                <div class="table-responsive">

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="urunID"
                                EmptyDataText="Kayıt girilmemiş" EnablePersistedSelection="true" OnRowCommand="GridView2_RowCommand">

                                <SelectedRowStyle CssClass="danger" />
                                <Columns>

                                    <asp:TemplateField HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                        <ItemTemplate>


                                            <asp:LinkButton ID="delLink"
                                                runat="server"
                                                CssClass="btn btn-danger btn-xs"
                                                CommandName="del" CommandArgument='<%#Eval("urunID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />


                                        </ItemTemplate>
                                        <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                        <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="urunID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                        <HeaderStyle CssClass="visible-lg" />
                                        <ItemStyle CssClass="visible-lg" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Cinsi" HeaderText="Müşteri Ürünü" />

                                    <asp:BoundField DataField="garantiBaslangic" HeaderText="Garanti Başlangıcı" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:D}" ItemStyle-CssClass="visible-lg">
                                        <HeaderStyle CssClass="visible-lg" />
                                        <ItemStyle CssClass="visible-lg" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="garantiBitis" HeaderText="Garanti Bitişi" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:D}" ItemStyle-CssClass="visible-lg">
                                        <HeaderStyle CssClass="visible-lg" />
                                        <ItemStyle CssClass="visible-lg" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="garantiSuresi" HeaderText="Garanti Süresi" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                        <HeaderStyle CssClass="visible-lg" />
                                        <ItemStyle CssClass="visible-lg" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                        <HeaderStyle CssClass="visible-lg" />
                                        <ItemStyle CssClass="visible-lg" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="belgeYol" HeaderText="Belge" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                        <HeaderStyle CssClass="visible-lg" />
                                        <ItemStyle CssClass="visible-lg" />
                                    </asp:BoundField>
                                </Columns>

                            </asp:GridView>
                            <asp:Button ID="btnAdd2" runat="server" Text="Yeni Ürün Ekle" CssClass="btn btn-primary"
                                OnClick="btnAdd2_Click" />
                            <asp:Label ID="lblMesaj" runat="server" Text=""></asp:Label>
                            <%--                            <asp:Button ID="btnTahsilEt" runat="server" Text="Tahsil Et" CssClass="btn btn-primary btn-lg btn-block" OnClick="btnTahsilEt_Click" />
                            <asp:Button ID="btnIptal" runat="server" Text="Son Faturayı İptal" CssClass="btn btn-danger btn-lg btn-block" OnClick="btnIptal_Click" />--%>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnMusteriARA" EventName="ServerClick" />
                            <asp:AsyncPostBackTrigger ControlID="GridView2" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="GridView3" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <!-- Add Record Modal Starts here-->
                    <div id="addModal2" class="modal  fade" tabindex="-1" role="dialog"
                        aria-labelledby="addModalLabel2" aria-hidden="true">
                        <div class="modal-dialog modal-content modal-sm">
                            <div class="modal-header modal-header-info">
                                <button type="button" class="close" data-dismiss="modal"
                                    aria-hidden="true">
                                    ×</button>
                                <h3 id="addModalLabel2">Yeni Ürün Ekle</h3>
                            </div>
                            <asp:UpdatePanel ID="upAdd2" runat="server">
                                <ContentTemplate>
                                    <script type="text/javascript">
                                        Sys.Application.add_load(jScript);
                                    </script>
                                    <div class="modal-body">
                                        <div class="form-horizontal">

                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtUrunCinsi" CssClass="col-md-10 control-label">Ürün Cinsi</asp:Label>
                                                <div class="col-md-10">
                                                    <asp:TextBox runat="server" ID="txtUrunCinsi" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtUrunCinsi" ValidationGroup="urunGrup" ErrorMessage="Lütfen ürün cinsi giriniz"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>



                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtUrunAciklama" CssClass="col-md-10 control-label">Ürün Açıklama</asp:Label>
                                                <div class="col-md-10">
                                                    <asp:TextBox runat="server" ID="txtUrunAciklama" CssClass="form-control" />

                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtUrunImei" CssClass="col-md-4 control-label">Cihaz Imei</asp:Label>
                                                <div class="col-md-8">
                                                    <asp:TextBox runat="server" ID="txtUrunImei" CssClass="form-control" />

                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtUrunSeriNo" CssClass="col-md-4 control-label">Seri No</asp:Label>
                                                <div class="col-md-8">
                                                    <asp:TextBox runat="server" ID="txtUrunSeriNo" CssClass="form-control" />

                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtUrunDiger" CssClass="col-md-4 control-label">Diğer Kod(Örn plaka)</asp:Label>
                                                <div class="col-md-8">
                                                    <asp:TextBox runat="server" ID="txtUrunDiger" CssClass="form-control" />

                                                </div>
                                            </div>

                                            <div class="form-group">

                                                <label for="datetimepicker2" class="col-md-10 control-label">Garanti Başlangıcı</label>
                                                <div class="input-group date col-md-10" id='datetimepicker2'>


                                                    <input type='text' id="tarih" runat="server" class="form-control col-md-10" />

                                                    <span class="input-group-addon">
                                                        <span class="glyphicon glyphicon-calendar"></span>
                                                    </span>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnAddRecord2" runat="server" Text="Kaydet" ValidationGroup="urunGrup"
                                            CssClass="btn btn-info" OnClick="btnAddRecord2_Click" />
                                        <button class="btn btn-info" data-dismiss="modal"
                                            aria-hidden="true">
                                            Kapat</button>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAddRecord2" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <!--Add Record Modal Ends here-->
                </div>
            </div>

        </div>


    </div>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $('#datetimepicker2').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
        }
    </script>
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
</asp:Content>
