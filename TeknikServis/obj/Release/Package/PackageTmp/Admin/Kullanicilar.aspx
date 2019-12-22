<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Kullanicilar.aspx.cs" Inherits="TeknikServis.Admin.Kullanicilar1" %>

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
        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                <h4 id="H1" runat="server" class="panel-title">KULLANICILAR</h4>
            </div>
            <%--<div class="panel-body">
               <
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
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="id"
                            EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand">
                            <Columns>

                                <asp:TemplateField HeaderText="işlemler" ShowHeader="False">
                                    <ItemTemplate>
                                        <div class="visible-lg visible-xs visible-sm">
                                            <asp:LinkButton ID="btnEdit"
                                                runat="server"
                                                CssClass="btn btn-success"
                                                CommandName="editRecord" CommandArgument='<%#Eval("id")+ ";" + Container.DisplayIndex + ";"+Eval("rolid") %>' Text="<i class='fa fa-pencil'></i>" />
                                            <asp:LinkButton ID="LinkButton1"
                                                runat="server"
                                                CssClass="btn btn-primary"
                                                CommandName="detail" CommandArgument='<%#Eval("id") %>' Text="<i class='fa fa-check'></i>" />
                                            <asp:LinkButton ID="delLink"
                                                runat="server"
                                                CssClass="btn btn-danger"
                                                CommandName="del" CommandArgument='<%#Eval("id") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />

                                </asp:TemplateField>


                                <asp:BoundField DataField="email" HeaderText="EPosta" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>

                                <asp:BoundField DataField="userName" HeaderText="Kullanıcı Adı"  >
                                 
                                </asp:BoundField>
                                <asp:BoundField DataField="rol" HeaderText="Rol Adı" >
                                 
                                </asp:BoundField>

                            </Columns>

                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnARA" EventName="ServerClick" />
                    </Triggers>
                </asp:UpdatePanel>

                <!-- Detail Modal Starts here-->
                <div id="detailModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-content modal-md">
                        <div class="modal-header modal-header-info">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3 id="myModalLabel" class="baslik">Detay</h3>
                        </div>

                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>

                                    <asp:DetailsView ID="DetailsView1" runat="server" CssClass="table table-bordered table-hover"
                                        BackColor="White" ForeColor="Black" FieldHeaderStyle-Wrap="false" FieldHeaderStyle-Font-Bold="true"
                                        FieldHeaderStyle-BackColor="LavenderBlush" FieldHeaderStyle-ForeColor="Black"
                                        BorderStyle="Groove" AutoGenerateRows="False">
                                        <Fields>
                                            <asp:BoundField DataField="id" HeaderText="ID" />
                                            <asp:BoundField DataField="email" HeaderText="E Posta" />
                                            <asp:BoundField DataField="userName" HeaderText="Kullanıcı Adı" />


                                        </Fields>
                                    </asp:DetailsView>


                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
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
                <div id="editModal" class="modal fade">
                    tabindex="-1" role="dialog" aria-labelledby="editModalLabel"
            aria-hidden="true">
              <div class="modal-dialog modal-content modal-md">
                  <div class="modal-header modal-header-info">
                      <button type="button" class="close"
                          data-dismiss="modal" aria-hidden="true">
                          ×</button>
                      <h3 id="editModalLabel" class="baslik">Kayıt Düzenleme</h3>
                  </div>
                  <asp:UpdatePanel ID="upEdit" runat="server">
                      <ContentTemplate>
                          <div class="modal-body">
                              <div class="form-horizontal">

                                  <asp:HiddenField ID="hdnID" runat="server" />
                                  <asp:HiddenField ID="hdnRolID" runat="server" />
                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtEposta" CssClass="col-md-2 control-label">EPosta</asp:Label>
                                      <div class="col-md-10">
                                          <asp:TextBox runat="server" ID="txtEposta" TextMode="Email" CssClass="form-control" />
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="musteriGrup" ControlToValidate="txtEposta" ErrorMessage="Geçerli bir eposta giriniz."></asp:RequiredFieldValidator>

                                      </div>
                                  </div>

                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtKullanici" CssClass="col-md-2 control-label">Kullanıcı Adı</asp:Label>
                                      <div class="col-md-10">
                                          <asp:TextBox runat="server" ID="txtKullanici" Enabled="false" CssClass="form-control" />
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="musteriGrup" ControlToValidate="txtKullanici" ErrorMessage="Geçerli bir kullanıcı adı giriniz."></asp:RequiredFieldValidator>
                                      </div>
                                  </div>

                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtYeniSifre" CssClass="col-md-2 control-label">Yeni Şifre</asp:Label>
                                      <div class="col-md-10">
                                          <asp:TextBox runat="server" ID="txtYeniSifre" CssClass="form-control" />
                                      </div>
                                  </div>

                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="drdRol" CssClass="col-md-2 control-label">Kullanıcı Rolü</asp:Label>
                                      <div class="col-md-10">
                                          <asp:DropDownList ID="drdRol" CssClass="form-control" runat="server">
                                              <%--<asp:ListItem Text="Pos/Banka seçiniz" Value="-1"></asp:ListItem>--%>
                                          </asp:DropDownList>
                                      </div>
                                  </div>

                              </div>
                          </div>
                          <div class="modal-footer">
                              <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                              <asp:Button ID="btnSave" runat="server" Text="Kaydet" CssClass="btn btn-info" ValidationGroup="musteriGroup" OnClick="btnSave_Click" />
                              <asp:Button ID="btnDel" runat="server" Text="Sil" CssClass="btn btn-danger" OnClick="btnDel_Click" />
                              <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">Kapat</button>
                          </div>
                      </ContentTemplate>
                      <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
                          <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                          <asp:AsyncPostBackTrigger ControlID="btnDel" EventName="Click" />
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
                            <h3 id="addModalLabel" class="baslik">Yeni Kayıt</h3>
                        </div>
                        <asp:UpdatePanel ID="upAdd" runat="server">
                            <ContentTemplate>
                                <div class="modal-body">


                                    <p class="text-danger">
                                        <asp:Literal runat="server" ID="ErrorMessage" />
                                    </p>
                                    <asp:ValidationSummary runat="server" CssClass="text-danger" ValidationGroup="grup" DisplayMode="BulletList" Enabled="true" ShowSummary="true" ShowValidationErrors="true" />


                                    <div class="form-horizontal">


                                        <div class="form-group">
                                            <asp:Label runat="server" AssociatedControlID="txtAddEposta" CssClass="col-md-2 control-label">EPosta</asp:Label>
                                            <div class="col-md-10">
                                                <asp:TextBox runat="server" ID="txtAddEposta" TextMode="Email" CssClass="form-control" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="grup" ControlToValidate="txtAddEposta" ErrorMessage="Geçerli bir eposta giriniz."></asp:RequiredFieldValidator>

                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <asp:Label runat="server" AssociatedControlID="txtAddKullanici" CssClass="col-md-2 control-label">Kullanıcı Adı</asp:Label>
                                            <div class="col-md-10">
                                                <asp:TextBox runat="server" ID="txtAddKullanici" CssClass="form-control" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="grup" ControlToValidate="txtAddKullanici" ErrorMessage="Geçerli bir kullanıcı adı giriniz."></asp:RequiredFieldValidator>

                                            </div>
                                        </div>


                                        <div class="form-group">
                                            <asp:Label runat="server" AssociatedControlID="txtTell" CssClass="col-md-2 control-label">Şifre</asp:Label>
                                            <div class="col-md-10">
                                                <asp:TextBox runat="server" ID="txtTell" CssClass="form-control" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="grup" ControlToValidate="txtTell" ErrorMessage="En az bir büyük bir küçük harf, harf dışında karakter ve sayı giriniz."></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label runat="server" AssociatedControlID="drdRoll" CssClass="col-md-2 control-label">Kullanıcı Rolü</asp:Label>
                                            <div class="col-md-10">
                                                <asp:DropDownList ID="drdRoll" CssClass="form-control" runat="server">
                                                    <%--<asp:ListItem Text="Pos/Banka seçiniz" Value="-1"></asp:ListItem>--%>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>


                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnAddRecord" runat="server" Text="Kaydet"
                                        CssClass="btn btn-info" OnClick="btnAddRecord_Click" />
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

            <div class="panel-footer pull-right">
                <asp:Button ID="btnAdd" runat="server" Text="Yeni" CssClass="btn btn-info btn-block"
                    OnClick="btnAdd_Click" />
            </div>

        </div>
    </div>

</asp:Content>
