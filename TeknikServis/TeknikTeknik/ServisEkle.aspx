<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="ServisEkle.aspx.cs" Inherits="TeknikServis.ServisEkle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap-theme.css" rel="stylesheet" />--%>
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
                YENİ SERVİS GİRİŞİ
            </div>
            <div class="panel-body">
                <div class="panel-group" id="accordion">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" class="">Müşteri Seçimi</a>
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

                                                    <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-info" ButtonType="Button" Text="Seç" HeaderText="Seçim">
                                                        <ControlStyle CssClass="btn btn-primary"></ControlStyle>
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="CustID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <%-- <asp:BoundField DataField="Ad" HeaderText="Müşteri Adı" />--%>
                                                    <asp:TemplateField HeaderText="Müşteri Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
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
                                            <asp:Button ID="btnAdd" runat="server" Text="Yeni Müşteri" CssClass="btn btn-primary"
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
                                        <div class="modal-dialog modal-content modal-md">
                                            <div class="modal-header modal-header-info">
                                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                                <h3 id="myModalLabel" class="baslik">Müşteri Bilgileri</h3>
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
                                                                <asp:BoundField DataField="Ad" HeaderText="Müşteri Adı" />
                                                                <asp:BoundField DataField="Adres" HeaderText="Müşteri Adresi" />
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
                                                <h3 id="addModalLabel" class="baslik">Yeni Müşteri</h3>
                                            </div>
                                            <asp:UpdatePanel ID="upAdd" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="modal-body">
                                                        <div class="form-horizontal">

                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtAdi" CssClass="col-md-4 control-label">Kişi/Firma Adı</asp:Label>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox runat="server" ID="txtAdi" CssClass="form-control" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtAdi" ErrorMessage="Lütfen Kişi/Firma adını giriniz"></asp:RequiredFieldValidator>

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
                                                                <asp:Label runat="server" AssociatedControlID="txtTcAdd" CssClass="col-md-4 control-label">Kişi/Firma Tc</asp:Label>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox runat="server" ID="txtTcAdd" CssClass="form-control" />

                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtAdress" CssClass="col-md-4 control-label">Kişi/Firma Adres</asp:Label>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox runat="server" ID="txtAdress" CssClass="form-control" />

                                                                </div>
                                                            </div>


                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtKim" CssClass="col-md-4 control-label">Tanıtıcı Bilgi</asp:Label>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox runat="server" ID="txtKim" CssClass="form-control" />

                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="col-md-4 control-label">Kişi/Firma Email</asp:Label>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" />

                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtTell" CssClass="col-md-4 control-label">Kişi/Firma Telefon</asp:Label>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox runat="server" ID="txtTell" CssClass="form-control" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtTell" ErrorMessage="Lütfen Kişi/Firma telefonu giriniz"></asp:RequiredFieldValidator>

                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtPrimKar" CssClass="col-md-4 control-label">Kar Prim %</asp:Label>
                                                                <div class="col-md-2">
                                                                    <asp:TextBox runat="server" ID="txtPrimKar" TextMode="Number" CssClass="form-control" />

                                                                </div>
                                                                <asp:Label runat="server" AssociatedControlID="txtPrimYekun" CssClass="col-md-4 control-label">Satış Prim %</asp:Label>
                                                                <div class="col-md-2">
                                                                    <asp:TextBox runat="server" ID="txtPrimYekun" TextMode="Number" CssClass="form-control" />

                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-md-4 control-label">Kayıt Tipi</label>
                                                                <div class="col-md-8">
                                                                    <div class="checkbox-inline">
                                                                        <asp:CheckBox Text="Müşteri" ID="chcMusteri" runat="server" />
                                                                    </div>
                                                                    <div class="checkbox-inline">
                                                                        <asp:CheckBox Text="Tedarikçi" ID="chcTedarikci" runat="server" />
                                                                    </div>
                                                                    <div class="checkbox-inline">
                                                                        <asp:CheckBox Text="Usta" ID="chcUsta" runat="server" />
                                                                    </div>
                                                                    <div class="checkbox-inline">
                                                                        <asp:CheckBox Text="Dış servis" ID="chcDizServis" runat="server" />
                                                                    </div>

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
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" class="collapsed">Kayıtlı Müşteri Cihazları</a>
                            </h4>
                        </div>
                        <div id="collapseTwo" class="panel-collapse collapse" style="height: 0px;">
                            <div class="panel-body">

                                <!-- Ürün seçim alanı başlıyor -->

                                <div class="table-responsive">

                                    <asp:UpdatePanel ID="upCrudGrid2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                        <ContentTemplate>

                                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="urunID"
                                                EmptyDataText="Kayıt girilmemiş" EnablePersistedSelection="true" OnRowCommand="GridView2_RowCommand" OnSelectedIndexChanged="GridView2_SelectedIndexChanged">

                                                <SelectedRowStyle CssClass="danger" />
                                                <Columns>

                                                    <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-info" ButtonType="Button" Text="Seç" HeaderText="Seçim">
                                                        <ControlStyle CssClass="btn btn-primary"></ControlStyle>
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="urunID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Cinsi" HeaderText="Müşteri Cihazı" />

                                                    <%--      <asp:BoundField DataField="garantiBaslangic" HeaderText="Garanti Başlangıcı" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:D}" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>--%>
                                                    <asp:BoundField DataField="garantiBitis" HeaderText="Garanti Bitişi" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:D}" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <%-- <asp:BoundField DataField="garantiSuresi" HeaderText="Garanti Süresi" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>--%>
                                                    <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <%-- <asp:BoundField DataField="belgeYol" HeaderText="Belge" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>--%>
                                                    <asp:BoundField DataField="imei" HeaderText="IMEI" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="serino" HeaderText="serino" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="digerno" HeaderText="digerno" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                </Columns>

                                            </asp:GridView>
                                            <asp:Button ID="btnAdd2" runat="server" Text="Yeni Cihaz Ekle" CssClass="btn btn-primary"
                                                OnClick="btnAdd2_Click" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAddRecord2" EventName="Click" />

                                            <asp:AsyncPostBackTrigger ControlID="GridView2" EventName="RowCommand" />

                                        </Triggers>
                                    </asp:UpdatePanel>

                                    <!-- Add Record Modal Starts here-->
                                    <div id="addModal2" class="modal  fade" tabindex="-1" role="dialog"
                                        aria-labelledby="addModalLabel2" aria-hidden="true">
                                        <div class="modal-dialog modal-content modal-md">
                                            <div class="modal-header modal-header-info">
                                                <button type="button" class="close" data-dismiss="modal"
                                                    aria-hidden="true">
                                                    ×</button>
                                                <h3 id="addModalLabel2" class="baslik">Yeni Cihaz Ekle</h3>
                                            </div>
                                            <asp:UpdatePanel ID="upAdd2" runat="server">
                                                <ContentTemplate>
                                                    <script type="text/javascript">
                                                        Sys.Application.add_load(jScript);
                                                    </script>
                                                    <div class="modal-body">
                                                        <div class="form-horizontal">

                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtUrunCinsi" CssClass="col-md-4 control-label">Cihaz Cinsi</asp:Label>
                                                                <div class="col-md-8">
                                                                    <asp:TextBox runat="server" ID="txtUrunCinsi" CssClass="form-control" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtUrunCinsi" ValidationGroup="urunGrup" ErrorMessage="Lütfen cihaz cinsi giriniz"></asp:RequiredFieldValidator>

                                                                </div>
                                                            </div>

                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtUrunAciklama" CssClass="col-md-4 control-label">Cihaz Açıklama</asp:Label>
                                                                <div class="col-md-8">
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

                                                                <label for="datetimepicker2" class="col-md-4 control-label">Garanti Başlangıcı</label>
                                                                <div class="input-group date col-md-8" id='datetimepicker2'>


                                                                    <input type='text' id="tarih" runat="server" class="form-control col-md-8" />

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

                                <!-- Ürün seçimalanı bitiyor-->
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseThree" class="collapsed">Servis Bilgileri</a>
                            </h4>
                        </div>
                        <div id="collapseThree" class="panel-collapse collapse">
                            <div class="panel-body">


                                <!-- servis bilgileri başlıyor-->

                                <asp:UpdatePanel ID="upBilgi" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                    <ContentTemplate>
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <label class="col-sm-2 col-sm-2 control-label" for="txtKimlikNo">Servis Kimlik</label>
                                                <div class="col-sm-10">
                                                    <input class="form-control" id="txtKimlikNo" runat="server" type="text" placeholder="Disabled input here..." disabled="disabled" />
                                                </div>
                                            </div>


                                            <div class="form-group">
                                                <label class="col-sm-2 control-label" for="txtBaslik">Başlık/Konu</label>
                                                <div class="col-sm-10">
                                                    <%--<input id="txtBaslik" runat="server" type="text" class="form-control" />--%>
                                                    <asp:TextBox ID="txtBaslik" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="valGrup" ErrorMessage="Lütfen başlık giriniz" CssClass="text-danger" ControlToValidate="txtBaslik"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label" for="drdTip">Servis Tipi</label>
                                                <div class="col-sm-10">
                                                    <asp:DropDownList ID="drdTip" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label" for="txtServisAciklama">Açıklama</label>
                                                <div class="col-sm-10">
                                                    <asp:TextBox ID="txtServisAciklama" CausesValidation="true" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="5" ValidationGroup="valGrup"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" EnableClientScript="true" ControlToValidate="txtServisAciklama" CssClass="text-danger" ErrorMessage="Lütfen servis açıklaması giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div id="kullaniciSecim" visible="false" runat="server" class="form-group">
                                                <label class="col-sm-2 control-label" for="drdKullanici">Görevli</label>
                                                <div class="col-sm-10">
                                                    <asp:DropDownList ID="drdKullanici" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="HERKESE AÇIK" Value="0" Selected="True"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div id="Div1" class="form-group">
                                                <label class="col-sm-2 control-label" for="drdPaketler">Servis Paketi</label>
                                                <div class="col-sm-10">
                                                    <asp:DropDownList ID="drdPaketler" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="Servis paketi seçiniz" Value="-1" Selected="True"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="chcSms" CssClass="col-md-2 control-label">Seç</asp:Label>
                                                <div class="col-md-10">
                                                    <asp:CheckBox ID="chcSms" CssClass="col-md-3 checkbox" Text="SMS" runat="server" />
                                                    <asp:CheckBox ID="chcMail" CssClass="col-md-3 checkbox" Text="Mail" runat="server" />
                                                    <asp:CheckBox ID="cbYazdir" CssClass="col-md-3 checkbox" runat="server" Text="Yazdır" />
                                                </div>
                                            </div>
                                            <div id="Div2" visible="true" runat="server" class="form-group">
                                                <label class="col-sm-2 control-label"></label>
                                                <div class="col-sm-10">
                                                    <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="valGrup" CssClass="text-danger" runat="server" />

                                                    <asp:Label ID="lblVal" runat="server" Text="" CssClass="text-danger"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="form-group">

                                                <label for="tarih2" class="col-md-2 control-label">İşlem Tarihi</label>
                                                <div class="col-md-10">

                                                    <input type='text' id="tarih2" runat="server" class="form-control" />

                                                </div>
                                            </div>

                                            <div id="subeSecim" runat="server" visible="false" class="form-group">
                                                <label for="drdSubeler" class="col-md-2 control-label">Şube</label>
                                                <%--       <asp:Label runat="server" AssociatedControlID="drdPaketDuzen" CssClass="col-md-10 control-label">İnternet Paketleri</asp:Label>--%>
                                                <div class="col-md-10">
                                                    <asp:DropDownList ID="drdSubeler" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="Şube seçebilirsiniz" Value="-1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>

                                        <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />

                                    </Triggers>
                                </asp:UpdatePanel>


                                <asp:Button ID="Button1" CssClass="btn btn-info btn-lg btn-block" runat="server" CausesValidation="true" ValidationGroup="valGrup" Text="Kaydet" OnClick="Button1_Click" />

                                <!--servis bilgileri bitiyor-->
                            </div>
                        </div>
                    </div>


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

            $('#ContentPlaceHolder1_tarih2').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
        }
    </script>

</asp:Content>
