<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Musteri.aspx.cs" Inherits="TeknikServis.Musteri" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="kaydir">
        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                <h4 id="baslikkk" runat="server" class="panel-title">
                    <label id="baslik" runat="server"></label>

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
                        <div id="cariOzet" runat="server" visible="false" class="pull-right">
                            <span id="smsSayi" runat="server" class="label label-warning label-lg"></span>
                            <asp:Button ID="btnTemizle" runat="server" Text="Temizle" CssClass="btn btn-info btn-xs" OnClick="btnTemizle_Click" />
                        </div>

                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="CustID"
                            EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_OnRowCreated"
                            OnPageIndexChanged="GridView1_PageIndexChanged" OnPageIndexChanging="GridView1_PageIndexChanging" AllowPaging="true" PageSize="10">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>

                                <asp:TemplateField>

                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnServisler"
                                            runat="server"
                                            CssClass="btn btn-success"
                                            Text="<i class='fa fa-wrench'></i>" />

                                        <asp:LinkButton ID="btnSmsEkle"
                                            runat="server"
                                            CssClass="btn btn-danger"
                                            CommandName="smsEkle" CommandArgument='<%# Eval("Telefon") %>' Text="<i class='fa fa-phone-square'></i>" />

                                    </ItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField>

                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnEdit"
                                            runat="server"
                                            CssClass="btn btn-danger"
                                            CommandName="editRecord" CommandArgument='<%#Eval("CustID")+ ";" + Container.DisplayIndex  %>' Text="<i class='fa fa-pencil'></i>" />
                                        <asp:LinkButton ID="btnMusteriDetay"
                                            runat="server"
                                            CssClass="btn btn-warning"
                                            CommandName="MusteriDetay" CommandArgument='<%# Eval("CustID")%>' Text="<i class='fa fa-user'></i>" />

                                    </ItemTemplate>

                                </asp:TemplateField>

                                <asp:BoundField DataField="CustID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>


                                <asp:TemplateField HeaderText="Kişi/Firma Adı">

                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnRandom"
                                            runat="server"
                                            CssClass="btn btn-primary"
                                            CommandName="detail" CommandArgument='<%#Eval("CustID") %>' Text=' <%#Eval("Ad") %> '>  </asp:LinkButton>

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
                                <asp:BoundField DataField="TC" HeaderText="TC" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="tanimlayici" HeaderText="Tanıtıcı" />
                                <asp:BoundField DataField="anten.anten_adi" HeaderText="anten" />

                            </Columns>

                        </asp:GridView>



                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnARA" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnTemizle" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>


                <!-- Detail Modal Starts here-->
                <div id="detailModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-content modal-md">
                        <div class="modal-header modal-header-info">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3 id="myModalLabel" class="baslik">Kişi/Firma Detayları</h3>
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
                                            <asp:BoundField DataField="Ad" HeaderText="Kişi/Firma Adı" />
                                            <asp:BoundField DataField="Adres" HeaderText="Kişi/Firma Adresi" />
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

                <!-- Edit Modal Starts here -->
                <div id="editModal" class="modal fade">
                    tabindex="-1" role="dialog" aria-labelledby="editModalLabel"
            aria-hidden="true">
              <div class="modal-dialog modal-content modal-md">
                  <div class="modal-header modal-header-info">
                      <button type="button" class="close"
                          data-dismiss="modal" aria-hidden="true">
                          ×</button>
                      <h3 id="editModalLabel" class="baslik">Kişi/Firma Güncelle</h3>
                  </div>
                  <asp:UpdatePanel ID="upEdit" runat="server">

                      <ContentTemplate>

                          <div class="modal-body">
                              <div class="form-horizontal">

                                  <asp:HiddenField ID="lblID" runat="server" />
                                  <%--<input type="text" id="lblID" runat="server" />--%>
                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtDuzenAd" CssClass="col-md-4 control-label">Kişi/Firma Adı</asp:Label>
                                      <div class="col-md-8">
                                          <asp:TextBox runat="server" ID="txtDuzenAd" CssClass="form-control" />
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="musteriGrup" ControlToValidate="txtDuzenAd" ErrorMessage="Lütfen Kişi/Firma adını giriniz"></asp:RequiredFieldValidator>

                                      </div>
                                  </div>
                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtDuzenUnvan" CssClass="col-md-4 control-label">Ünvan</asp:Label>
                                      <div class="col-md-8">
                                          <asp:TextBox runat="server" ID="txtDuzenUnvan" CssClass="form-control" />

                                      </div>
                                  </div>
                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtDuzenTc" CssClass="col-md-4 control-label">Kişi/Firma TC</asp:Label>
                                      <div class="col-md-8">
                                          <asp:TextBox runat="server" ID="txtDuzenTc" CssClass="form-control" />

                                      </div>
                                  </div>

                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtDuzenAdres" CssClass="col-md-4 control-label">Adres</asp:Label>
                                      <div class="col-md-8">
                                          <asp:TextBox runat="server" ID="txtDuzenAdres" CssClass="form-control" />

                                      </div>
                                  </div>

                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtDuzenTelefon" CssClass="col-md-4 control-label">Telefon</asp:Label>
                                      <div class="col-md-8">
                                          <asp:TextBox runat="server" ID="txtDuzenTelefon" CssClass="form-control" />
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="musteriGrup" ControlToValidate="txtDuzenTelefon" ErrorMessage="Lütfen telefon giriniz"></asp:RequiredFieldValidator>

                                      </div>
                                  </div>



                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtKimDuzen" CssClass="col-md-4 control-label">Tanıtıcı Bilgi</asp:Label>
                                      <div class="col-md-8">
                                          <asp:TextBox runat="server" ID="txtKimDuzen" CssClass="form-control" />

                                      </div>
                                  </div>
                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtPrimKarDuzen" CssClass="col-md-4 control-label">Kar Prim %</asp:Label>
                                      <div class="col-md-2">
                                          <asp:TextBox runat="server" ID="txtPrimKarDuzen" TextMode="Number" CssClass="form-control" />

                                      </div>
                                      <asp:Label runat="server" AssociatedControlID="txtPrimYekun" CssClass="col-md-4 control-label">Satış Prim %</asp:Label>
                                      <div class="col-md-2">
                                          <asp:TextBox runat="server" ID="txtPrimYekunDuzen" TextMode="Number" CssClass="form-control" />

                                      </div>
                                  </div>
                                  <div class="form-group">
                                      <label class="col-md-4 control-label">Kayıt Tipi</label>
                                      <div class="col-md-8">
                                          <div class="checkbox-inline">
                                              <asp:CheckBox Text="Müşteri" ID="chcDuzenMust" runat="server" />
                                          </div>
                                          <div class="checkbox-inline">
                                              <asp:CheckBox Text="Tedarikçi" ID="chcDuzenTedarikci" runat="server" />
                                          </div>
                                          <div class="checkbox-inline">
                                              <asp:CheckBox Text="Usta" ID="chcDuzenUsta" runat="server" />
                                          </div>
                                          <div class="checkbox-inline">
                                              <asp:CheckBox Text="Dış servis" ID="chcDuzenDisServis" runat="server" />
                                          </div>

                                      </div>

                                  </div>
                                  <div id="antenSecimDuzen" runat="server" class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="drdAntenDuzen" CssClass="col-md-4 control-label">Anten: </asp:Label>
                                      <div class="col-md-8">
                                          <asp:DropDownList ID="drdAntenDuzen" runat="server" CssClass="form-control" ValidationGroup="musteriGrup">
                                              <asp:ListItem Text="Anten seçiniz" Value="-1" Selected="True"></asp:ListItem>
                                          </asp:DropDownList>
                                          <asp:HiddenField ID="hdnAntenDuzen" runat="server" />
                                      </div>
                                  </div>
                              </div>
                          </div>
                          <div class="modal-footer">
                              <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                              <asp:Button ID="btnSave" runat="server" Text="Kaydet" CssClass="btn btn-info" ValidationGroup="musteriGrup" OnClick="btnSave_Click" />
                              <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Kapat</button>
                          </div>
                      </ContentTemplate>
                      <Triggers>
                          <%--<asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />--%>
                          <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                      </Triggers>
                  </asp:UpdatePanel>
              </div>
                </div>
                <!-- Edit Modal Ends here -->



                <!-- Add Record Modal Starts here-->
                <div id="addModal" class="modal  fade" tabindex="-1" role="dialog"
                    aria-labelledby="addModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-content modal-md">
                        <div class="modal-header modal-header-info">
                            <button type="button" class="close" data-dismiss="modal"
                                aria-hidden="true">
                                ×</button>
                            <h3 id="addModalLabel" class="baslik">Kişi/Firma Kaydı</h3>
                        </div>
                        <asp:UpdatePanel ID="upAdd" runat="server">

                            <ContentTemplate>
                                <script type="text/javascript">
                                    Sys.Application.add_load(jScript);
                                </script>
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
                                            <asp:Label runat="server" AssociatedControlID="txtUnvan" CssClass="col-md-4 control-label">Ünvan</asp:Label>
                                            <div class="col-md-8">
                                                <asp:TextBox runat="server" ID="txtUnvan" CssClass="form-control" />

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
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtTell" ErrorMessage="Lütfen Kişi/Firma telefonu giriniz"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <div id="antenSecim" runat="server" class="form-group">
                                            <asp:Label runat="server" AssociatedControlID="drdAnten" CssClass="col-md-4 control-label">Anten: </asp:Label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="drdAnten" runat="server" CssClass="form-control" ValidationGroup="musteriGrup">
                                                    <asp:ListItem Text="Anten seçiniz" Value="-1" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>

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
                                        CssClass="btn btn-info" ValidationGroup="musteriGrup2" OnClick="btnAddRecord_Click" />
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

            <div class="panel-footer pull-right">
                <div class=" btn-group">

                    <asp:Button ID="btnAdd" runat="server" Text="Yeni" CssClass="btn btn-info"
                        OnClick="btnAdd_Click" />
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

                    <asp:LinkButton ID="btnSms"
                        runat="server"
                        CssClass="btn btn-info" OnClick="btnSms_Click"
                        Text="<i class='fa fa-phone-square icon-2x'></i>" />

                    <asp:LinkButton ID="btnMail"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnMail_Click"
                        Text="<i class='fa fa-envelope icon-2x'></i>" />


                </div>

            </div>

        </div>
    </div>

</asp:Content>

