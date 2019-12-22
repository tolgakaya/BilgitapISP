<%@ Page Title="" Language="C#" MasterPageFile="Super.master" AutoEventWireup="true" CodeBehind="Firmalar.aspx.cs" Inherits="TeknikServis.AdminSuper.Firmalar" %>

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
        function pageLoad(sender, args) {

            $('#ContentPlaceHolder1_tarih2').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
        }
    </script>
    <div class="kaydir">
        <div class="panel panel-info">

            <div class="panel-heading">
                <h4 id="H1" runat="server" class="panel-title">Kayıtlı Firmalar</h4>
            </div>

            <div class="table-responsive">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>

                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div class="input-group custom-search-form">
                    <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Ara..." />
                    <span class="input-group-btn">
                        <button id="btnARA" runat="server" class="btn btn-default" type="submit" onserverclick="MusteriAra">
                            <i class="fa fa-search"></i>
                        </button>
                    </span>
                </div>
                <asp:UpdatePanel ID="upCrudGrid" runat="server">
                    <ContentTemplate>

                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="id"
                            EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound">
                            <Columns>

                                <asp:TemplateField HeaderText="işlemler" ShowHeader="False">
                                    <ItemTemplate>
                                        <div class="visible-lg visible-xs visible-sm">
                                            <asp:LinkButton ID="btnEdit"
                                                runat="server"
                                                CssClass="btn btn-success"
                                                CommandName="lisansla" CommandArgument='<%#Eval("id")+ ";" +Eval("config") %>' Text="<i class='fa fa-pencil'></i>" />

                                            <asp:LinkButton ID="delLink"
                                                runat="server"
                                                CssClass="btn btn-danger"
                                                CommandName="sil" CommandArgument='<%#Eval("id")+ ";" +Eval("config") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />

                                </asp:TemplateField>


                                <asp:BoundField DataField="config" HeaderText="Config" />
                                <asp:BoundField DataField="firma_kod" HeaderText="Firma Kodu" />
                                <asp:BoundField DataField="firma_tam" HeaderText="Firma" />
                                <asp:BoundField DataField="adres" HeaderText="Adres" />
                                <asp:BoundField DataField="tel" HeaderText="Telefon" />
                                <asp:BoundField DataField="web" HeaderText="Web" />
                                <asp:BoundField DataField="email" HeaderText="Email" />
                                <asp:BoundField DataField="katilma_tarihi" HeaderText="Katılma" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="yenileme_tarihi" HeaderText="Yenileme" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="expiration" HeaderText="Geçerlilik" DataFormatString="{0:d}" />

                            </Columns>

                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnARA" EventName="ServerClick" />
                    </Triggers>
                </asp:UpdatePanel>

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
                                  <asp:HiddenField ID="hdnConfig" runat="server" />

                                  <div class="form-group">

                                      <label for="txtAy" class="col-md-2 control-label">Lisans Süresi</label>
                                      <div class="col-md-10">

                                          <asp:TextBox runat="server" ID="txtAy" TextMode="Number" CssClass="form-control" />

                                      </div>
                                  </div>

                                  <%--       <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="drdRol" CssClass="col-md-2 control-label">Kullanıcı Rolü</asp:Label>
                                      <div class="col-md-10">
                                          <asp:DropDownList ID="drdRol" CssClass="form-control" runat="server">
                                            
                                          </asp:DropDownList>
                                      </div>
                                  </div>--%>
                              </div>
                          </div>
                          <div class="modal-footer">
                              <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                              <asp:Button ID="btnLisansKaydet" runat="server" Text="Kaydet" CssClass="btn btn-info" OnClick="btnLisansKaydet_Click" />

                              <button class="btn btn-danger" data-dismiss="modal" aria-hidden="true">Kapat</button>
                          </div>
                      </ContentTemplate>
                      <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
                          <%--<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />--%>
                      </Triggers>
                  </asp:UpdatePanel>
              </div>
                </div>
                <!-- Edit Modal Ends here -->
            </div>
            <div class="panel-footer pull-right">
                  <asp:Button ID="btnYeni" runat="server" Text="Yeni" CssClass="btn btn-info btn-block"
                    OnClick="btnYeni_Click" />
            </div>
        </div>

    </div>
</asp:Content>
