<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" EnableViewStateMac="false" AutoEventWireup="true" CodeBehind="Ayarlar.aspx.cs" Inherits="TeknikServis.Admin.Ayarlar" %>



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
    <script type="text/javascript">
        function ConfirmM() {
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
    <link href="/Content/bootstrap-colorpicker.min.css" rel="stylesheet" />
    <script src="/Scripts/bootstrap-colorpicker.min.js"></script>


    <!-- /. ROW  -->
    <div class="kaydir">

        <div class="panel panel-info">
            <div class="panel-heading">
                SİSTEM AYARLARI
            </div>
            <div class="panel-body">
                <div class="panel-group" id="accordion">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" class="collapsed">Servis Durum Ayarları</a>
                            </h4>
                        </div>
                        <%-- Servis Durum başlıyor --%>
                        <div id="collapseOne" class="panel-collapse collapse" style="height: 0px;">
                            <div class="panel-body">

                                <div class="table-responsive">
                                    <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                                        <ProgressTemplate>

                                            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                                                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                                            </div>

                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <asp:UpdatePanel ID="upCrudGrid" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="Durum_ID" EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="İşlem" ShowHeader="False">
                                                        <ItemTemplate>

                                                            <asp:LinkButton ID="btnEdit"
                                                                runat="server"
                                                                CssClass="btn btn-success "
                                                                CommandName="editRecord" CommandArgument='<%# Container.DataItemIndex %>' Text="<i class='fa fa-pencil'></i>" />
                                                            <asp:LinkButton ID="LinkButton1"
                                                                runat="server"
                                                                CssClass="btn btn-primary "
                                                                CommandName="detail" CommandArgument='<%#Eval("Durum_ID") %>' Text="<i class='fa fa-check'></i>" />
                                                            <asp:LinkButton ID="delLink"
                                                                runat="server"
                                                                CssClass="btn btn-danger "
                                                                CommandName="del" CommandArgument='<%#Eval("Durum_ID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />

                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="Durum_ID" HeaderText="ID" >
                                                        
                                                       
                                                    </asp:BoundField>

                                                    <asp:TemplateField HeaderText="Durum" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Durum") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>

                                                            <asp:LinkButton ID="btnRandom"
                                                                runat="server"
                                                                CssClass="btn btn-primary"
                                                                CommandName="detaill" CommandArgument='<%#Eval("Durum_ID") %>' Text=' <%#Eval("Durum") %> '>
                          
                                                            </asp:LinkButton>

                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                    <asp:CheckBoxField DataField="SMS" HeaderText="SMS" />
                                                    
                                                    <asp:CheckBoxField DataField="Mail" HeaderText="Mail">
                                                       
                                                    </asp:CheckBoxField>
                                                    <asp:CheckBoxField DataField="Whatsapp" HeaderText="WhatsApp" >
                                                   
                                                    </asp:CheckBoxField>
                                                    <asp:CheckBoxField DataField="sonmu" HeaderText="Kapanma?" >
                                                        
                                                       
                                                    </asp:CheckBoxField>
                                                    <asp:CheckBoxField DataField="baslangicmi" HeaderText="Baslangıç?" >
                                                        
                                                       
                                                    </asp:CheckBoxField>
                                                    <asp:CheckBoxField DataField="kararmi" HeaderText="Karar?" >
                                                        
                                                       
                                                    </asp:CheckBoxField>
                                                    <asp:CheckBoxField DataField="onaymi" HeaderText="Onay?" >
                                                        
                                                       
                                                    </asp:CheckBoxField>
                                                </Columns>

                                            </asp:GridView>
                                            <asp:Button ID="btnAdd" runat="server" Text="Yeni Ekle" CssClass="btn btn-danger"
                                                OnClick="btnAdd_Click" />
                                        </ContentTemplate>

                                    </asp:UpdatePanel>


                                    <!-- Detail Modal Starts here-->
                                    <div id="detailModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                                        <div class="modal-dialog modal-content modal-sm">
                                            <div class="modal-header modal-header-info">
                                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                                <h3 id="myModalLabel" class="baslik">Durum Detayları</h3>
                                            </div>

                                            <div class="modal-body">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>

                                                        <asp:DetailsView ID="DetailsView1" runat="server" CssClass="table table-bordered table-hover"
                                                            BackColor="White" ForeColor="Black" FieldHeaderStyle-Wrap="false" FieldHeaderStyle-Font-Bold="true"
                                                            FieldHeaderStyle-BackColor="LavenderBlush" FieldHeaderStyle-ForeColor="Black"
                                                            BorderStyle="Groove" AutoGenerateRows="False">
                                                            <Fields>
                                                                <asp:BoundField DataField="Durum_ID" HeaderText="ID" />
                                                                <asp:BoundField DataField="Durum" HeaderText="Durum" />
                                                                <asp:CheckBoxField DataField="SMS" HeaderText="SMS" />
                                                                <asp:CheckBoxField DataField="Mail" HeaderText="Mail" />
                                                                <asp:CheckBoxField DataField="Whatsapp" HeaderText="WhatsApp" />
                                                                <asp:CheckBoxField DataField="baslangicmi" HeaderText="Başlangıç?" />
                                                                <asp:CheckBoxField DataField="sonmu" HeaderText="Son?" />
                                                                <asp:CheckBoxField DataField="kararmi" HeaderText="Karar?" />
                                                                <asp:CheckBoxField DataField="onaymi" HeaderText="Onay?" />
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
              <div class="modal-dialog modal-content modal-sm">
                  <div class="modal-header modal-header-info">
                      <button type="button" class="close"
                          data-dismiss="modal" aria-hidden="true">
                          ×</button>
                      <h3 id="editModalLabel" class="baslik">Durum Ayarı Güncelle</h3>
                  </div>
                  <asp:UpdatePanel ID="upEdit" runat="server">
                      <ContentTemplate>
                          <div class="modal-body">
                              <div class="form-horizontal">

                                  <%--      <div class="form-group">

                                      <div class="col-sm-10">
                                          <asp:TextBox ID="lblID" CausesValidation="true" runat="server" Enabled="false" CssClass="form-control" ValidationGroup="musteriGrup"></asp:TextBox>
                                      </div>
                                  </div>--%>
                                  <asp:HiddenField ID="lblID" runat="server" />
                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtDurum" CssClass="col-md-10 control-label">Durum</asp:Label>
                                      <div class="col-md-10">
                                          <asp:TextBox runat="server" ID="txtDurum" CssClass="form-control" />
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ValidationGroup="musteriGrup" ControlToValidate="txtDurum" ErrorMessage="Lütfen müşteri adını giriniz"></asp:RequiredFieldValidator>

                                      </div>
                                  </div>

                                  <div class="form-group">

                                      <div class="col-md-10">
                                          <asp:CheckBox ID="chcSMS" Text="SMS" runat="server" CssClass="form-control" />

                                      </div>
                                  </div>


                                  <div class="form-group">
                                      <div class="col-md-10">
                                          <asp:CheckBox ID="chcMail" runat="server" Text="Mail" CssClass="form-control" />
                                      </div>
                                  </div>

                                  <div class="form-group">
                                      <div class="col-md-10">
                                          <asp:CheckBox ID="chcWhat" runat="server" Text="WhatsApp" CssClass="form-control" />
                                      </div>
                                  </div>



                                  <div class="form-group">
                                      <%--"radio radiobuttonlist"--%>
                                      <div class=" col-md-10">
                                          <asp:RadioButtonList ID="rdDurum" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow">
                                              <asp:ListItem Text="Başlangıç" class="form-control" Value="baslangic"></asp:ListItem>
                                              <asp:ListItem Text="Servis sonu" class="form-control" Value="son"></asp:ListItem>
                                              <asp:ListItem Text="Karar bekleniyor" class="form-control" Value="karar"></asp:ListItem>
                                              <asp:ListItem Text="Karar onaylandı" class="form-control" Value="onay"></asp:ListItem>
                                          </asp:RadioButtonList>
                                      </div>
                                  </div>

                              </div>
                          </div>
                          <div class="modal-footer">
                              <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                              <asp:Button ID="btnSave" runat="server" Text="Kaydet" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                              <%--  <asp:Button ID="btnDel" runat="server" Text="Sil" CssClass="btn text-danger" OnClick="btnDel_Click" />--%>
                              <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">Kapat</button>
                          </div>
                      </ContentTemplate>
                      <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
                          <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                          <%--  <asp:AsyncPostBackTrigger ControlID="btnDel" EventName="Click" />--%>
                      </Triggers>
                  </asp:UpdatePanel>
              </div>
                                    </div>
                                    <!-- Edit Modal Ends here -->

                                    <!-- Add Record Modal Starts here-->
                                    <div id="addModal" class="modal  fade" tabindex="-1" role="dialog"
                                        aria-labelledby="addModalLabel" aria-hidden="true">
                                        <div class="modal-dialog modal-content modal-sm">
                                            <div class="modal-header modal-header-info">
                                                <button type="button" class="close" data-dismiss="modal"
                                                    aria-hidden="true">
                                                    ×</button>
                                                <h3 id="addModalLabel" class="baslik">Durum Tanımla</h3>
                                            </div>
                                            <asp:UpdatePanel ID="upAdd" runat="server">
                                                <ContentTemplate>
                                                    <div class="modal-body">

                                                        <div class="form-horizontal">

                                                            <%--    <div class="form-group">

                                                                <div class="col-sm-10">
                                                                    <asp:TextBox ID="lblAddDurumID" CausesValidation="true" runat="server" Enabled="false" CssClass="form-control" ValidationGroup="musteriGrup"></asp:TextBox>
                                                                </div>
                                                            </div>--%>

                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtAddDurum" CssClass="col-md-10 control-label">Durum</asp:Label>
                                                                <div class="col-md-10">
                                                                    <asp:TextBox runat="server" ID="txtAddDurum" CssClass="form-control" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ValidationGroup="grup" ControlToValidate="txtAddDurum" ErrorMessage="Lütfen müşteri adını giriniz"></asp:RequiredFieldValidator>

                                                                </div>
                                                            </div>

                                                            <div class="form-group">

                                                                <div class="col-md-10">
                                                                    <asp:CheckBox ID="chcAddSMS" Text="SMS" runat="server" CssClass="form-control" />

                                                                </div>
                                                            </div>


                                                            <div class="form-group">
                                                                <div class="col-md-10">
                                                                    <asp:CheckBox ID="chcAddMail" runat="server" Text="Mail" CssClass="form-control" />
                                                                </div>
                                                            </div>

                                                            <div class="form-group">
                                                                <div class="col-md-10">
                                                                    <asp:CheckBox ID="chcAddWhat" runat="server" Text="WhatsApp" CssClass="form-control" />
                                                                </div>
                                                            </div>



                                                            <div class="form-group">
                                                                <%--"radio radiobuttonlist"--%>
                                                                <div class=" col-md-10">
                                                                    <asp:RadioButtonList ID="rdDurumYeni" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow">
                                                                        <asp:ListItem Text="Başlangıç" class="form-control" Value="baslangic"></asp:ListItem>
                                                                        <asp:ListItem Text="Servis sonu" class="form-control" Value="son"></asp:ListItem>
                                                                        <asp:ListItem Text="Karar bekleniyor" class="form-control" Value="karar"></asp:ListItem>
                                                                        <asp:ListItem Text="Karar onaylandı" class="form-control" Value="onay"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <asp:Button ID="btnAddRecord" runat="server" Text="Kaydet"
                                                            CssClass="btn btn-primary" OnClick="btnAddRecord_Click" />
                                                        <button class="btn btn-danger" data-dismiss="modal"
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


                            </div>
                        </div>
                        <%-- servis durum bitiyor--%>
                    </div>
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo">Servis Tipleri</a>
                            </h4>
                        </div>
                        <%-- servis tipleri başlıyor --%>
                        <div id="collapseTwo" class="panel-collapse collapse" style="height: 0;">
                            <div class="panel-body">


                                <div class="table-responsive">

                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="grdTip" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="tip_id" EmptyDataText="Kayıt girilmemiş" OnRowCommand="grdTip_RowCommand">
                                                <Columns>

                                                    <asp:TemplateField HeaderText="İşlem" ShowHeader="False">
                                                        <ItemTemplate>

                                                            <asp:LinkButton ID="btnEditTip"
                                                                runat="server"
                                                                CssClass="btn btn-success "
                                                                CommandName="editRecord" CommandArgument='<%# Container.DataItemIndex %>' Text="<i class='fa fa-pencil'></i>" />
                                                            <asp:LinkButton ID="LinkButton1"
                                                                runat="server"
                                                                CssClass="btn btn-primary "
                                                                CommandName="detail" CommandArgument='<%#Eval("tip_id") %>' Text="<i class='fa fa-check'></i>" />
                                                            <asp:LinkButton ID="delLink"
                                                                runat="server"
                                                                CssClass="btn btn-danger "
                                                                CommandName="del" CommandArgument='<%#Eval("tip_id") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />

                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="tip_id" HeaderText="ID" >
                                                        
                                                       
                                                    </asp:BoundField>

                                                    <asp:TemplateField HeaderText="Tip" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">

                                                        <ItemTemplate>

                                                            <asp:LinkButton ID="btnRandom"
                                                                runat="server"
                                                                CssClass="btn btn-primary"
                                                                CommandName="detaill" CommandArgument='<%#Eval("tip_id") %>' Text=' <%#Eval("tip_ad") %> '>
                              
                                                            </asp:LinkButton>

                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                        <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="aciklama" HeaderText="Açıklama" >
                                                        
                                                       
                                                    </asp:BoundField>


                                                    <asp:TemplateField HeaderText="Renk" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">

                                                        <ItemTemplate>

                                                            <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("css") %>' BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("css").ToString())%>'> </asp:Label>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                </Columns>

                                            </asp:GridView>
                                            <asp:Button ID="btnAddTip" runat="server" Text="Yeni" CssClass="btn btn-danger"
                                                OnClick="btnAddTip_Click" />
                                        </ContentTemplate>
                                        <%--<Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnARATip" EventName="ServerClick" />
                                            </Triggers>--%>
                                    </asp:UpdatePanel>


                                    <!-- Detail Modal Starts here-->
                                    <div id="detailModalTip" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabelTip" aria-hidden="true">
                                        <div class="modal-dialog modal-content modal-sm">
                                            <div class="modal-header modal-header-info">
                                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                                <h3 id="myModalLabelTip" class="baslik">Tip Detayları</h3>
                                            </div>

                                            <div class="modal-body">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>

                                                        <asp:DetailsView ID="DetailsViewTip" runat="server" CssClass="table table-bordered table-hover"
                                                            BackColor="White" ForeColor="Black" FieldHeaderStyle-Wrap="false" FieldHeaderStyle-Font-Bold="true"
                                                            FieldHeaderStyle-BackColor="LavenderBlush" FieldHeaderStyle-ForeColor="Black"
                                                            BorderStyle="Groove" AutoGenerateRows="False">
                                                            <Fields>
                                                                <asp:BoundField DataField="tip_id" HeaderText="ID" />
                                                                <asp:BoundField DataField="tip_ad" HeaderText="Tip" />
                                                                <asp:BoundField DataField="Aciklama" HeaderText="Açıklama" />
                                                                <asp:TemplateField HeaderText="Renk" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">

                                                                    <ItemTemplate>

                                                                        <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("css") %>' BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("css").ToString())%>'> </asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>

                                                            </Fields>
                                                        </asp:DetailsView>


                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="grdTip" EventName="RowCommand" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddTip" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <div class="modal-footer">
                                                    <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">Kapat</button>
                                                </div>
                                            </div>


                                        </div>

                                    </div>
                                    <!-- Detail Modal Ends here -->

                                    <!-- Edit Modal Starts here -->
                                    <div id="editModalTip" class="modal fade">
                                        tabindex="-1" role="dialog" aria-labelledby="editModalLabelTip" aria-hidden="true">
              <div class="modal-dialog modal-content modal-sm">
                  <div class="modal-header modal-header-info">
                      <button type="button" class="close"
                          data-dismiss="modal" aria-hidden="true">
                          ×</button>
                      <h3 id="editModalLabelTip" class="baslik">Servis Tipi Güncelle</h3>
                  </div>
                  <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                      <ContentTemplate>
                          <script type="text/javascript">
                              Sys.Application.add_load(jScript);
                          </script>
                          <div class="modal-body">
                              <div class="form-horizontal">

                                  <asp:HiddenField ID="txtTipID" runat="server" />
                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtCssGuncelle" CssClass="col-md-10 control-label">Renk Seçimi</asp:Label>
                                      <div class="col-md-10">
                                          <input type="text" id="txtCssGuncelle" runat="server" cssclass="form-control" class="demo1 form-control" />
                                      </div>
                                  </div>

                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtTipAd" CssClass="col-md-10 control-label">Servis Tipi</asp:Label>
                                      <div class="col-md-10">
                                          <asp:TextBox runat="server" ID="txtTipAd" CssClass="form-control" />
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ValidationGroup="tipGrup" ControlToValidate="txtTipAd" ErrorMessage="Lütfen tip adı giriniz"></asp:RequiredFieldValidator>

                                      </div>
                                  </div>


                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtTipAciklama" CssClass="col-md-10 control-label">Tip Açıklaması</asp:Label>
                                      <div class="col-md-10">
                                          <asp:TextBox runat="server" ID="txtTipAciklama" CssClass="form-control" />

                                      </div>
                                  </div>

                              </div>
                          </div>
                          <div class="modal-footer">
                              <asp:Label ID="Label2" Visible="false" runat="server"></asp:Label>
                              <asp:Button ID="btnSaveTip" runat="server" Text="Kaydet" ValidationGroup="tipGrup" CssClass="btn btn-primary" OnClick="btnSaveTip_Click" />
                              <%--<asp:Button ID="btnDelTip" runat="server" Text="Sil" CssClass="btn text-danger" OnClick="btnDelTip_Click" />--%>
                              <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">Kapat</button>
                          </div>
                      </ContentTemplate>
                      <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="grdTip" EventName="RowCommand" />
                          <asp:AsyncPostBackTrigger ControlID="btnSaveTip" EventName="Click" />
                          <%--  <asp:AsyncPostBackTrigger ControlID="btnDelTip" EventName="Click" />--%>
                      </Triggers>
                  </asp:UpdatePanel>
              </div>
                                    </div>
                                    <!-- Edit Modal Ends here -->

                                    <!-- Add Record Modal Starts here-->
                                    <div id="addModalTip" class="modal  fade" tabindex="-1" role="dialog"
                                        aria-labelledby="addModalLabelTip" aria-hidden="true">
                                        <div class="modal-dialog modal-content modal-sm">
                                            <div class="modal-header modal-header-info">
                                                <button type="button" class="close" data-dismiss="modal"
                                                    aria-hidden="true">
                                                    ×</button>
                                                <h3 id="addModalLabelTip" class="baslik">Yeni Servis Tipi</h3>
                                            </div>
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>
                                                    <script type="text/javascript">
                                                        Sys.Application.add_load(jScript);
                                                    </script>
                                                    <div class="modal-body">
                                                        <table class="table table-bordered table-hover">


                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtCss" CssClass="col-md-10 control-label">Renk Seçimi</asp:Label>
                                                                <div class="col-md-10">
                                                                    <input type="text" id="txtCss" runat="server" class="demo1 form-control" />
                                                                </div>
                                                            </div>


                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtTipAdGoster" CssClass="col-md-10 control-label">Servis Tipi</asp:Label>
                                                                <div class="col-md-10">
                                                                    <asp:TextBox runat="server" ID="txtTipAdGoster" CssClass="form-control" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ValidationGroup="tipGrupAdd" ControlToValidate="txtTipAdGoster" ErrorMessage="Lütfen tip adı giriniz"></asp:RequiredFieldValidator>

                                                                </div>
                                                            </div>


                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtTipAciklamaGoster" CssClass="col-md-10 control-label">Tip Açıklaması</asp:Label>
                                                                <div class="col-md-10">
                                                                    <asp:TextBox runat="server" ID="txtTipAciklamaGoster" CssClass="form-control" />

                                                                </div>
                                                            </div>


                                                            <tr>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <asp:Button ID="btnAddRecordTip" runat="server" Text="Kaydet"
                                                            CssClass="btn btn-primary" OnClick="btnAddRecordTip_Click" ValidationGroup="tipGrupAdd" />
                                                        <button class="btn btn-danger" data-dismiss="modal"
                                                            aria-hidden="true">
                                                            Kapat</button>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnAddRecordTip" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <!--Add Record Modal Ends here-->
                                </div>

                            </div>
                        </div>
                        <%-- servis tipleri bitiyor --%>
                    </div>

                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapse3">Masraf Tipleri</a>
                            </h4>
                        </div>
                        <%-- servis tipleri başlıyor --%>
                        <div id="collapse3" class="panel-collapse in" style="height: auto;">
                            <div class="panel-body">


                                <div class="table-responsive">

                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="grdTipM" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="tip_id" EmptyDataText="Kayıt girilmemiş" OnRowCommand="grdTipM_RowCommand">
                                                <Columns>

                                                    <asp:TemplateField HeaderText="İşlem" ShowHeader="False">
                                                        <ItemTemplate>

                                                            <asp:LinkButton ID="btnEditTipM"
                                                                runat="server"
                                                                CssClass="btn btn-success "
                                                                CommandName="editRecord" CommandArgument='<%# Container.DataItemIndex %>' Text="<i class='fa fa-pencil'></i>" />
                                                            <asp:LinkButton ID="btnTipDetay"
                                                                runat="server"
                                                                CssClass="btn btn-primary "
                                                                CommandName="detail" CommandArgument='<%#Eval("tip_id") %>' Text="<i class='fa fa-check'></i>" />
                                                            <asp:LinkButton ID="delLinkM"
                                                                runat="server"
                                                                CssClass="btn btn-danger "
                                                                CommandName="del" CommandArgument='<%#Eval("tip_id") %>' OnClientClick="ConfirmM()" Text="<i class='fa fa-trash-o'></i>" />

                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="tip_id" HeaderText="ID" >
                                                        
                                                       
                                                    </asp:BoundField>

                                                    <asp:TemplateField HeaderText="Tip" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">

                                                        <ItemTemplate>

                                                            <asp:LinkButton ID="btnRandomM"
                                                                runat="server"
                                                                CssClass="btn btn-primary"
                                                                CommandName="detaill" CommandArgument='<%#Eval("tip_id") %>' Text=' <%#Eval("tip_adi") %> '>
                              
                                                            </asp:LinkButton>

                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                        <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="aciklama" HeaderText="Açıklama" >
                                                        
                                                       
                                                    </asp:BoundField>


                                                    <asp:TemplateField HeaderText="Renk" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">

                                                        <ItemTemplate>

                                                            <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("css") %>' BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("css").ToString())%>'> </asp:Label>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                </Columns>

                                            </asp:GridView>
                                            <asp:Button ID="btnAddTipM" runat="server" Text="Yeni" CssClass="btn btn-danger"
                                                OnClick="btnAddTipM_Click" />
                                        </ContentTemplate>
                                        <%--<Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnARATip" EventName="ServerClick" />
                                            </Triggers>--%>
                                    </asp:UpdatePanel>


                                    <!-- Detail Modal Starts here-->
                                    <div id="detailModalTipM" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabelTipM" aria-hidden="true">
                                        <div class="modal-dialog modal-content modal-sm">
                                            <div class="modal-header modal-header-info">
                                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                                <h3 id="myModalLabelTipM" class="baslik">Tip Detayları</h3>
                                            </div>

                                            <div class="modal-body">
                                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                    <ContentTemplate>

                                                        <asp:DetailsView ID="DetailsViewTipM" runat="server" CssClass="table table-bordered table-hover"
                                                            BackColor="White" ForeColor="Black" FieldHeaderStyle-Wrap="false" FieldHeaderStyle-Font-Bold="true"
                                                            FieldHeaderStyle-BackColor="LavenderBlush" FieldHeaderStyle-ForeColor="Black"
                                                            BorderStyle="Groove" AutoGenerateRows="False">
                                                            <Fields>
                                                                <asp:BoundField DataField="tip_id" HeaderText="ID" />
                                                                <asp:BoundField DataField="tip_adi" HeaderText="Tip" />
                                                                <asp:BoundField DataField="Aciklama" HeaderText="Açıklama" />
                                                                <asp:TemplateField HeaderText="Renk" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">

                                                                    <ItemTemplate>

                                                                        <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("css") %>' BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("css").ToString())%>'> </asp:Label>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>

                                                            </Fields>
                                                        </asp:DetailsView>


                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="grdTipM" EventName="RowCommand" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddTipM" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <div class="modal-footer">
                                                    <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">Kapat</button>
                                                </div>
                                            </div>


                                        </div>

                                    </div>
                                    <!-- Detail Modal Ends here -->

                                    <!-- Edit Modal Starts here -->
                                    <div id="editModalTipM" class="modal fade">
                                        tabindex="-1" role="dialog" aria-labelledby="editModalLabelTipM" aria-hidden="true">
              <div class="modal-dialog modal-content modal-sm">
                  <div class="modal-header modal-header-info">
                      <button type="button" class="close"
                          data-dismiss="modal" aria-hidden="true">
                          ×</button>
                      <h3 id="editModalLabelTipM" class="baslik">Tip güncelle</h3>
                  </div>
                  <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                      <ContentTemplate>
                          <script type="text/javascript">
                              Sys.Application.add_load(jScript);
                          </script>
                          <div class="modal-body">
                              <div class="form-horizontal">


                                  <asp:HiddenField ID="hdnTipIDM" runat="server" />
                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtCssGuncelle" CssClass="col-md-10 control-label">Renk Seçimi</asp:Label>
                                      <div class="col-md-10">
                                          <input type="text" id="txtCssGuncelleM" runat="server" cssclass="form-control" class="demo1 form-control" value="#5367ce" />
                                      </div>
                                  </div>

                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtTipAd" CssClass="col-md-10 control-label">Servis Tipi</asp:Label>
                                      <div class="col-md-10">
                                          <asp:TextBox runat="server" ID="txtTipAdM" CssClass="form-control" />
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ValidationGroup="tipGrupM" ControlToValidate="txtTipAdM" ErrorMessage="Lütfen tip adı giriniz"></asp:RequiredFieldValidator>

                                      </div>
                                  </div>


                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtTipAciklamaM" CssClass="col-md-10 control-label">Tip Açıklaması</asp:Label>
                                      <div class="col-md-10">
                                          <asp:TextBox runat="server" ID="txtTipAciklamaM" CssClass="form-control" />

                                      </div>
                                  </div>

                              </div>
                          </div>
                          <div class="modal-footer">
                              <%-- <asp:Label ID="Label1" Visible="false" runat="server"></asp:Label>--%>
                              <asp:Button ID="btnSaveTipM" runat="server" Text="Kaydet" ValidationGroup="tipGrup" CssClass="btn btn-primary" OnClick="btnSaveTipM_Click" />

                              <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">Kapat</button>
                          </div>
                      </ContentTemplate>
                      <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="grdTipM" EventName="RowCommand" />
                          <asp:AsyncPostBackTrigger ControlID="btnSaveTipM" EventName="Click" />

                      </Triggers>
                  </asp:UpdatePanel>
              </div>
                                    </div>
                                    <!-- Edit Modal Ends here -->

                                    <!-- Add Record Modal Starts here-->
                                    <div id="addModalTipM" class="modal  fade" tabindex="-1" role="dialog"
                                        aria-labelledby="addModalLabelTipM" aria-hidden="true">
                                        <div class="modal-dialog modal-content modal-sm">
                                            <div class="modal-header modal-header-info">
                                                <button type="button" class="close" data-dismiss="modal"
                                                    aria-hidden="true">
                                                    ×</button>
                                                <h3 id="addModalLabelTipM" class="baslik">Yeni Masraf Tipi</h3>
                                            </div>
                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                <ContentTemplate>
                                                    <script type="text/javascript">
                                                        Sys.Application.add_load(jScript);
                                                    </script>
                                                    <div class="modal-body">
                                                        <table class="table table-bordered table-hover">


                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtCssM" CssClass="col-md-10 control-label">Renk Seçimi</asp:Label>
                                                                <div class="col-md-10">
                                                                    <input type="text" id="txtCssM" runat="server" class="demo1 form-control" value="#5367ce" />
                                                                </div>
                                                            </div>


                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtTipAdGosterM" CssClass="col-md-10 control-label">Masraf Tipi</asp:Label>
                                                                <div class="col-md-10">
                                                                    <asp:TextBox runat="server" ID="txtTipAdGosterM" CssClass="form-control" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ValidationGroup="tipGrupAddM" ControlToValidate="txtTipAdGosterM" ErrorMessage="Lütfen tip adı giriniz"></asp:RequiredFieldValidator>

                                                                </div>
                                                            </div>


                                                            <div class="form-group">
                                                                <asp:Label runat="server" AssociatedControlID="txtTipAciklamaGosterM" CssClass="col-md-10 control-label">Tip Açıklaması</asp:Label>
                                                                <div class="col-md-10">
                                                                    <asp:TextBox runat="server" ID="txtTipAciklamaGosterM" CssClass="form-control" />

                                                                </div>
                                                            </div>


                                                            <tr>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <asp:Button ID="btnAddRecordTipM" runat="server" Text="Kaydet"
                                                            CssClass="btn btn-primary" OnClick="btnAddRecordTipM_Click" ValidationGroup="tipGrupAddM" />
                                                        <button class="btn btn-danger" data-dismiss="modal"
                                                            aria-hidden="true">
                                                            Kapat</button>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnAddRecordTipM" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <!--Add Record Modal Ends here-->
                                </div>

                            </div>
                        </div>
                        <%-- servis tipleri bitiyor --%>
                    </div>

                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapse4" class="collapsed">API Ayarları</a>
                            </h4>
                        </div>

                        <div id="collapse4" class="panel-collapse collapse">
                            <div class="panel-body">
                                <%-- mail ayarları başlıyor-mail --%>
                                <div class="col-md-6 pull-right">
                                    <h3>Mail Server Ayarları</h3>
                                    <div class="form-group">
                                        <label for="txtMailServer" class="control-label">SMTP Server</label>
                                        <input id="txtMailServer" runat="server" type="text" class="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" EnableClientScript="true" ControlToValidate="txtMailServer" ErrorMessage="Lütfen server adresi giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group">
                                        <label for="txtMailKimden" class="control-label">Mail kimden gelmiş görünecek</label>
                                        <asp:TextBox ID="txtMailKimden" runat="server" TextMode="Email" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" EnableClientScript="true" ControlToValidate="txtMailKimden" ErrorMessage="Lütfen gönderdiğiniz maillerin kimden gitmiş görüneceğini giriniz(mail)" ValidationGroup="valGrup"></asp:RequiredFieldValidator>

                                    </div>
                                    <div class="form-group">
                                        <label for="txtMailKullanici" class="control-label">Kullanıcı Adı</label>
                                        <asp:TextBox ID="txtMailKullanici" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" EnableClientScript="true" ControlToValidate="txtMailKullanici" ErrorMessage="Lütfen kullanıcı adını giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>

                                    </div>
                                    <div class="form-group">
                                        <label for="txtMailSifre" class="control-label">Şifre</label>
                                        <asp:TextBox ID="txtMailSifre" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" EnableClientScript="true" ControlToValidate="txtMailSifre" ErrorMessage="Lütfen mail server için şifrenizi giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>

                                    </div>
                                    <div class="form-group">
                                        <label for="txtMailPort" class="control-label">Port No</label>
                                        <asp:TextBox ID="txtMailPort" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" EnableClientScript="true" ControlToValidate="txtMailPort" ErrorMessage="Lütfen mail server için port giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>

                                    </div>
                                    <div class="form-group">
                                        <label for="txtMailAktif" class="control-label">Aktif Mail Adresi</label>
                                        <asp:TextBox ID="txtMailAktif" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" EnableClientScript="true" ControlToValidate="txtMailAktif" ErrorMessage="Lütfen size gelecek maillleri alacağınız adresi giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>

                                    </div>
                                    <div class="form-group">
                                        <asp:Button ID="btnMailKaydet" runat="server" Text="Kaydet" CssClass="btn btn-primary btn-block" OnClick="btnMailKaydet_Click" ValidationGroup="valGrup" />
                                        <%--            <asp:Button ID="btnGeri" runat="server" Text="Geri Dön" CssClass="btn btn-danger btn-block" OnClick="btnGeri_Click" />--%>
                                    </div>
                                </div>
                                <%-- mail ayarları bitiyor --%>
                                <%-- SMS ayarları başlıyor --%>
                                <div class="col-md-6 pull-left">
                                    <h3>SMS API Ayarları</h3>
                                    <div class="form-group">
                                        <label for="txtMailServer" class="control-label">Servis Sağlayıcı</label>
                                        <asp:DropDownList ID="drdSaglayici" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="NetGSM SMS Api" Value="NETGSM" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label for="txtMailKimden2" class="control-label">Gönderen İsmi</label>
                                        <asp:TextBox ID="txtMailKimden2" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" EnableClientScript="true" ControlToValidate="txtMailKimden2" ErrorMessage="Lütfen gönderdiğiniz maillerin kimden gitmiş görüneceğini giriniz" ValidationGroup="valGrup2"></asp:RequiredFieldValidator>

                                    </div>
                                    <div class="form-group">
                                        <label for="txtMailKullanici2" class="control-label">Kullanıcı Adı</label>
                                        <asp:TextBox ID="txtMailKullanici2" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" EnableClientScript="true" ControlToValidate="txtMailKullanici2" ErrorMessage="Lütfen kullanıcı adını giriniz" ValidationGroup="valGrup2"></asp:RequiredFieldValidator>

                                    </div>
                                    <div class="form-group">
                                        <label for="txtMailSifre2" class="control-label">Şifre</label>
                                        <asp:TextBox ID="txtMailSifre2" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" EnableClientScript="true" ControlToValidate="txtMailSifre2" ErrorMessage="Lütfen Sms api şifrenizi giriniz" ValidationGroup="valGrup2"></asp:RequiredFieldValidator>

                                    </div>
                                    <div class="form-group">
                                        <label for="txtSmsAktif" class="control-label">Aktif Cep No</label>
                                        <asp:TextBox ID="txtSmsAktif" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" EnableClientScript="true" ControlToValidate="txtSmsAktif" ErrorMessage="Lütfen size gelecek mesajları alacağınız cep noyu giriniz" ValidationGroup="valGrup2"></asp:RequiredFieldValidator>

                                    </div>
                                    <div class="form-group">
                                        <asp:Button ID="btnSmsKaydet" runat="server" Text="Sms Kaydet" CssClass="btn btn-primary btn-block" OnClick="btnSmsKaydet_Click" ValidationGroup="valGrup2" />

                                    </div>
                                </div>
                                <%-- SMS ayarları bitiyor --%>
                            </div>
                        </div>

                    </div>

                </div>
            </div>
        </div>

    </div>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $('.demo1').colorpicker();
        }

    </script>
</asp:Content>
