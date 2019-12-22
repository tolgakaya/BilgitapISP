﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="SatinAl.aspx.cs" Inherits="TeknikServis.TeknikAlim.SatinAl" %>

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

    <div class="row kaydir">
        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div class="panel panel-info">
            <div class="panel-heading">
                Satın Alma Kaydı
            </div>
            <div class="panel-body">
                <div class="panel-group" id="accordion">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" class="collapsed">Firma/Kişi Seçimi</a>
                            </h4>
                        </div>
                        <div id="collapseOne" class="panel-collapse in" style="height: auto;">
                            <div class="panel-body">
                                <!-- Müşteri seçim alanı başlıyor -->

                                <div class="table-responsive">
                                    <div class="input-group custom-search-form ">
                                        <span class="input-group-btn">
                                            <button id="btnARA" runat="server" class="btn btn-default" type="submit" onserverclick="MusteriAra">
                                                <i class="fa fa-search"></i>
                                            </button>
                                        </span>
                                        <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Ara..." />

                                    </div>

                                    <asp:UpdatePanel ID="upCrudGrid" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                                        <ContentTemplate>

                                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" EnablePersistedSelection="false" DataKeyNames="CustID" EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                                                <SelectedRowStyle CssClass="danger" />
                                                <Columns>

                                                    <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Seç" HeaderText="Seçim">
                                                      
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="CustID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <%-- <asp:BoundField DataField="Ad" HeaderText="Müşteri Adı" />--%>
                                                    <asp:TemplateField HeaderText="Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Ad") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>

                                                            <asp:LinkButton ID="btnRandom"
                                                                runat="server"
                                                                CssClass="btn btn-primary"
                                                                CommandName="detail" CommandArgument='<%#Eval("CustID") %>' Text=' <%#Eval("Ad") %> '>
                           
                                                            </asp:LinkButton>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Adres" HeaderText="Adres" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Telefon" HeaderText="Telefon" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="email" HeaderText="E Posta" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                </Columns>

                                            </asp:GridView>
                                            <asp:Button ID="btnAdd" runat="server" Text="Yeni Firma" CssClass="btn btn-primary"
                                                OnClick="btnAdd_Click" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnARA" EventName="ServerClick" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAddRecord" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="Gridview1" EventName="RowCommand" />
                                        </Triggers>
                                    </asp:UpdatePanel>


                                    <!-- Detail Modal Starts here-->
                                    <div id="detailModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                                        <div class="modal-dialog modal-content modal-sm">
                                            <div class="modal-header modal-header-info">
                                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                                <h3 id="myModalLabel" class="baslik">Cari Bilgileri</h3>
                                            </div>

                                            <div class="modal-body">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>

                                                        <asp:DetailsView ID="DetailsView1" runat="server" CssClass="table table-bordered table-hover"
                                                            BackColor="White" ForeColor="Black" FieldHeaderStyle-Wrap="false" FieldHeaderStyle-Font-Bold="true"
                                                            FieldHeaderStyle-BackColor="LavenderBlush" FieldHeaderStyle-ForeColor="Black"
                                                            BorderStyle="Groove" AutoGenerateRows="False">
                                                            <Fields>
                                                                <asp:BoundField DataField="CustID" HeaderText="ID" />
                                                                <asp:BoundField DataField="Ad" HeaderText="Adı" />
                                                                <asp:BoundField DataField="Adres" HeaderText=" Adresi" />
                                                                <asp:BoundField DataField="Telefon" HeaderText="Telefon" />

                                                            </Fields>
                                                        </asp:DetailsView>


                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <div class="modal-footer">
                                                    <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Kapat</button>
                                                </div>
                                            </div>


                                        </div>

                                    </div>
                                    <!-- Detail Modal Ends here -->



                                    <!-- Add Record Modal Starts here-->
                                    <div id="addModal" class="modal  fade" tabindex="-1" role="dialog"
                                        aria-labelledby="addModalLabel" aria-hidden="true">
                                        <div class="modal-dialog modal-content modal-md">
                                            <div class="modal-header modal-header-info">
                                                <button type="button" class="close" data-dismiss="modal"
                                                    aria-hidden="true">
                                                    ×</button>
                                                <h3 id="addModalLabel" class="baslik">Yeni Firma Kaydı</h3>
                                            </div>
                                            <asp:UpdatePanel ID="upAdd" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="modal-body">

                                                        <div class="form-horizontal">

                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtAdi" CssClass="col-md-4 control-label"> Adı</asp:Label>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox runat="server" ID="txtAdi" CssClass="form-control" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtAdi" ErrorMessage="Lütfen firma adı giriniz"></asp:RequiredFieldValidator>

                                                                </div>
                                                            </div>

                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtSoyAdi" CssClass="col-md-4 control-label">Soyadı</asp:Label>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox runat="server" ID="txtSoyAdi" CssClass="form-control" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtSoyAdi" ErrorMessage="Lütfen  soyadını giriniz"></asp:RequiredFieldValidator>

                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtDuzenUnvan" CssClass="col-md-4 control-label">Ünvan</asp:Label>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox runat="server" ID="txtDuzenUnvan" CssClass="form-control" />

                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtTcAdd" CssClass="col-md-4 control-label">Tc/VD</asp:Label>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox runat="server" ID="txtTcAdd" CssClass="form-control" />

                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtAdress" CssClass="col-md-4 control-label"> Adres</asp:Label>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox runat="server" ID="txtAdress" CssClass="form-control" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtAdress" ErrorMessage="Lütfen  adres giriniz"></asp:RequiredFieldValidator>

                                                                </div>
                                                            </div>


                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtKim" CssClass="col-md-4 control-label">Tanıtıcı Bilgi</asp:Label>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox runat="server" ID="txtKim" CssClass="form-control" />

                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="col-md-4 control-label">Email</asp:Label>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" />

                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtTell" CssClass="col-md-4 control-label">Telefon</asp:Label>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox runat="server" ID="txtTell" CssClass="form-control" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtTell" ErrorMessage="Lütfen telefon giriniz"></asp:RequiredFieldValidator>

                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>


                                                    <div class="modal-footer">
                                                        <asp:Button ID="btnAddRecord" runat="server" Text="Kaydet"
                                                            CssClass="btn btn-info" OnClick="btnAddRecord_Click" ValidationGroup="musteriGrup2" />
                                                        <button class="btn btn-info" data-dismiss="modal"
                                                            aria-hidden="true">
                                                            Kapat</button>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnAddRecord" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <!--Add Record Modal Ends here-->
                                </div>

                                <!-- Müşteri seç,malanı bitiyor-->
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo">Satın Alma Kalemleri</a>
                            </h4>
                        </div>
                        <div id="collapseTwo" class="panel-collapse in" style="height: auto;">
                            <div class="panel-body">

                                <!-- Ürün seçim alanı başlıyor -->

                                <div class="table-responsive">

                                    <asp:UpdatePanel ID="upCrudGrid2" runat="server">
                                        <ContentTemplate>

                                            <asp:GridView ID="grdDetay" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="cihaz_id"
                                                EmptyDataText="Detay bilgileri" EnablePersistedSelection="true" OnRowCommand="grdDetay_RowCommand" OnSelectedIndexChanged="grdDetay_SelectedIndexChanged">

                                                <SelectedRowStyle CssClass="danger" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>


                                                            <asp:LinkButton ID="delLink"
                                                                runat="server"
                                                                CssClass="btn btn-danger btn-xs"
                                                                CommandName="del" CommandArgument='<%#Eval("cihaz_id") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />


                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                        <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="cihaz_id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="cihaz_adi" HeaderText="Ürün/Parça/Malzeme" />
                                                    <asp:BoundField DataField="adet" HeaderText="Adet/Miktar" />
                                                    <asp:BoundField DataField="tutar" HeaderText="Tutar"  />
                                                    <asp:BoundField DataField="kdv" HeaderText="Kdv" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                    <asp:BoundField DataField="yekun" HeaderText="Yekün"  />
                                                    <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                       
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:Button Text="Detay Ekle" ID="btnDetayEkle" OnClick="btnDetayEkle_Click" CssClass="btn btn-primary" runat="server" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnDetayKaydet" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>


                                </div>

                                <!-- Ürün seçimalanı bitiyor-->
                            </div>


                        </div>
                    </div>

                    <!-- detay Modal Starts here-->
                    <div id="detayModal" class="modal  fade" tabindex="-1" role="dialog"
                        aria-labelledby="detayModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-content modal-lg">
                            <div class="modal-header modal-header-info">
                                <button type="button" class="close" data-dismiss="modal"
                                    aria-hidden="true">
                                    ×</button>
                                <h3 id="detayModalLabel" class="baslik">Fatura Kalemi</h3>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>

                                    <div class="modal-body">
                                        <div class="input-group custom-search-form col-md-12">
                                            <input runat="server" type="text" id="txtCihazAra" class="form-control" placeholder="Ara..." />
                                            <span class="input-group-btn">
                                                <button id="btnCihazAra" runat="server" class="btn btn-default" type="submit" onserverclick="CihazAra">
                                                    <i class="fa fa-search"></i>
                                                </button>
                                            </span>
                                        </div>


                                        <div class="form-horizontal" id="cihaz">
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="grdCihaz" CssClass="col-md-12 control-label">Ürün/Parça/Hizmet Seçimi</asp:Label>
                                                <div class="col-md-12">
                                                    <asp:GridView ID="grdCihaz" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="ID"
                                                        EmptyDataText="Cihaz seç" EnablePersistedSelection="true" OnRowCommand="grdCihaz_RowCommand" OnSelectedIndexChanged="grdCihaz_SelectedIndexChanged">

                                                        <SelectedRowStyle CssClass="danger" />
                                                        <Columns>

                                                            <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-info" ButtonType="Button" Text="Seç" HeaderText="Seçim">
                                                                <ControlStyle CssClass="btn btn-primary"></ControlStyle>
                                                            </asp:ButtonField>
                                                            <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="cihaz_adi" HeaderText="Ürün/Parça/Hizmet" />

                                                            <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="giris" HeaderText="Toplam Giriş" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:D}" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="cikis" HeaderText="Toplam Çıkış" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="bakiye" HeaderText="Stok Miktarı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>

                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:Button ID="btnYeniCihaz" runat="server" Text="Yeni Tanımla"
                                                        CssClass="btn btn-info" OnClick="btnYeniCihaz_Click" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtAdet" CssClass="col-md-12 control-label">Adet/Miktar</asp:Label>
                                                <div class="col-md-12">
                                                    <asp:TextBox runat="server" ID="txtAdet" TextMode="Number" ValidationGroup="detayGrup" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtAdet" ValidationGroup="detayGrup" ErrorMessage="Lütfen ürün cinsi giriniz"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtTutar" CssClass="col-md-12 control-label">Tutar</asp:Label>
                                                <div class="col-md-12">
                                                    <asp:TextBox runat="server" ID="txtTutar" ValidationGroup="detayGrup" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtTutar" ValidationGroup="detayGrup" ErrorMessage="Lütfen tutar giriniz"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="txtTutar" runat="server" ValidationGroup="detayGrup" MinimumValue="0" MaximumValue="9999999" />

                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <%-- buradaki kdv sadece oran olacak --%>
                                                <asp:Label runat="server" AssociatedControlID="txtKdv" CssClass="col-md-12 control-label">Kdv Oranı</asp:Label>
                                                <div class="col-md-12">
                                                    <asp:TextBox runat="server" ID="txtKdv" ValidationGroup="detayGrup" Text="18" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtKdv" ValidationGroup="detayGrup" ErrorMessage="Lütfen oran giriniz"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="txtKDV" runat="server" ValidationGroup="detayGrup" MinimumValue="0" MaximumValue="18" />

                                                </div>
                                            </div>
                                            <%--     <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtYekun" CssClass="col-md-12 control-label">Yekün</asp:Label>
                                                <div class="col-md-12">
                                                    <asp:TextBox runat="server" ID="txtYekun" ValidationGroup="detayGrup" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtYekun" ValidationGroup="detayGrup" ErrorMessage="Lütfen toplam giriniz"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="txtYekun" runat="server" ValidationGroup="detayGrup" MinimumValue="0" MaximumValue="9999999" />
                                                </div>
                                            </div>--%>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtDetayAciklama" CssClass="col-md-12 control-label">Açıklama</asp:Label>
                                                <div class="col-md-12">
                                                    <asp:TextBox runat="server" ID="txtDetayAciklama" ValidationGroup="detayGrup" CssClass="form-control" />

                                                </div>
                                            </div>

                                        </div>

                                    </div>

                                    <div class="modal-footer">
                                        <asp:Button ID="btnDetayKaydet" runat="server" Text="Detay Kaydet"
                                            CssClass="btn btn-info" OnClick="btnDetayKaydet_Click" ValidationGroup="detayGrup" />
                                        <button class="btn btn-info" data-dismiss="modal"
                                            aria-hidden="true">
                                            Kapat</button>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnDetayEkle" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCihazKaydet" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCihazAra" EventName="ServerClick" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <!-- Add Record Modal Starts here-->
                    <div id="cihazModal" class="modal  fade" tabindex="-1" role="dialog"
                        aria-labelledby="cihazModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-content modal-sm">
                            <div class="modal-header modal-header-info">
                                <button type="button" class="close" data-dismiss="modal"
                                    aria-hidden="true">
                                    ×</button>
                                <h3 id="cihazModalLabel" class="baslik">Yeni Ürün/Parça/Malzeme Tanımla</h3>
                            </div>
                            <asp:UpdatePanel ID="upAdd2" runat="server">
                                <ContentTemplate>
                                    <%--   <script type="text/javascript">
                                        Sys.Application.add_load(jScript);
                                    </script>--%>
                                    <div class="modal-body">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="drdGrup" CssClass="col-md-10 control-label">Ürün Grubu</asp:Label>
                                                <div class="col-md-10">
                                                    <asp:DropDownList ID="drdGrup" CssClass="form-control" runat="server">
                                                        <%--<asp:ListItem Text="Pos/Banka seçiniz" Value="-1"></asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="cihaz_adi" CssClass="col-md-10 control-label">Ürün/Parça Tanımı</asp:Label>
                                                <div class="col-md-10">
                                                    <asp:TextBox runat="server" ID="cihaz_adi" ValidationGroup="cihazGrup" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="cihaz_adi" ValidationGroup="cihazGrup" ErrorMessage="Lütfen ürün cinsi giriniz"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="aciklama" CssClass="col-md-10 control-label">Ürün/Parça Açıklama</asp:Label>
                                                <div class="col-md-10">
                                                    <asp:TextBox runat="server" ID="aciklama" ValidationGroup="cihazGrup" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="aciklama" ValidationGroup="cihazGrup" ErrorMessage="Lütfen açıklama giriniz"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="barkod" CssClass="col-md-10 control-label">Barkod</asp:Label>
                                                <div class="col-md-10">
                                                    <asp:TextBox runat="server" ID="barkod" ValidationGroup="cihazGrup" CssClass="form-control" />

                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="garanti_suresi" CssClass="col-md-10 control-label">Garanti Süresi(Ay)</asp:Label>
                                                <div class="col-md-10">
                                                    <asp:TextBox runat="server" ID="garanti_suresi" ValidationGroup="cihazGrup" TextMode="Number" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="garanti_suresi" ValidationGroup="cihazGrup" ErrorMessage="Lütfen süre giriniz"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnCihazKaydet" runat="server" Text="Kaydet"
                                            CssClass="btn btn-info" OnClick="btnCihazKaydet_Click" ValidationGroup="cihazGrup" />
                                        <button class="btn btn-info" data-dismiss="modal"
                                            aria-hidden="true">
                                            Kapat</button>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnYeniCihaz" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <!--Add Record Modal Ends here-->
                    <!--detay Modal Ends here-->
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseThree" class="collapsed">Fatura Bilgileri</a>
                            </h4>
                        </div>
                        <div id="collapseThree" class="panel-collapse collapse">
                            <div class="panel-body">


                                <!-- servis bilgileri başlıyor-->

                                <asp:UpdatePanel ID="upBilgi" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <label class="col-sm-2 col-sm-2 control-label" for="txtKonu">Konu</label>
                                                <div class="col-sm-10">
                                                    <asp:TextBox ID="txtKonu" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label class="col-sm-2 control-label" for="txtAciklama">Açıklama</label>
                                                <div class="col-sm-10">
                                                    <asp:TextBox ID="txtAciklama" CausesValidation="true" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="5" ValidationGroup="valGrup"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label" for="txtBelgeNo">Belge No</label>
                                                <div class="col-sm-10">
                                                    <asp:TextBox ID="txtBelgeNo" CausesValidation="true" runat="server" CssClass="form-control" Rows="5" ValidationGroup="valGrup"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label" for="toplam_tutar">Tutar</label>
                                                <div class="col-sm-10">
                                                    <div class="col-sm-3">
                                                        <asp:TextBox ID="toplam_tutar" CausesValidation="true" Enabled="false" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="toplam_tutar2" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>

                                                    </div>
                                                </div>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" EnableClientScript="true" ControlToValidate="toplam_tutar" CssClass="text-danger" ErrorMessage="Lütfen tutar giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                                <asp:RangeValidator ErrorMessage="Ondalıklar için virgül kullanınız" ControlToValidate="toplam_tutar" ValidationGroup="valGrup" MinimumValue="0" MaximumValue="1000000" Type="Currency" runat="server" />

                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label" for="toplam_kdv">KDV</label>
                                                <div class="col-sm-10">
                                                    <div class="col-sm-3">
                                                        <asp:TextBox ID="toplam_kdv" CausesValidation="true" Enabled="false" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>

                                                    </div>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="toplam_kdv2" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>

                                                    </div>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" EnableClientScript="true" ControlToValidate="toplam_kdv" CssClass="text-danger" ErrorMessage="Lütfen kdv giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator ErrorMessage="Ondalıklar için virgül kullanınız" ControlToValidate="toplam_kdv" ValidationGroup="valGrup" MinimumValue="0" MaximumValue="1000000" Type="Currency" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label" for="toplam_yekun">Yekün</label>
                                                <div class="col-sm-10">
                                                    <div class="col-sm-3">
                                                        <asp:TextBox ID="toplam_yekun" CausesValidation="true" Enabled="false" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>

                                                    </div>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="toplam_yekun2" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>

                                                    </div>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" EnableClientScript="true" ControlToValidate="toplam_yekun" CssClass="text-danger" ErrorMessage="Lütfen vergi dahil tutar giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator ErrorMessage="Ondalıklar için virgül kullanınız" ControlToValidate="toplam_yekun" ValidationGroup="valGrup" MinimumValue="0" MaximumValue="1000000" Type="Currency" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group">

                                                <label for="tarih2" class="col-sm-2 control-label">Alım Tarihi</label>
                                                <div class="col-sm-10">
                                                    <input type='text' id="tarih2" runat="server" class="form-control" />
                                                </div>

                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>

                                        <asp:AsyncPostBackTrigger ControlID="btnAlimKaydet" EventName="Click" />

                                    </Triggers>
                                </asp:UpdatePanel>


                                <asp:Button ID="btnAlimKaydet" CssClass="btn btn-info btn-lg btn-block" runat="server" CausesValidation="true" ValidationGroup="valGrup" Text="Kaydet" OnClick="btnAlimKaydet_Click" />


                            </div>
                        </div>
                    </div>


                </div>
            </div>
        </div>

    </div>
    <script type="text/javascript">
        function pageLoad(sender, args) {

            $('#ContentPlaceHolder1_tarih2').datetimepicker({
                format: 'L',

                locale: 'tr'
            });

        }
    </script>
</asp:Content>
