<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="ServislerCanli.aspx.cs" Inherits="TeknikServis.ServislerCanli" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>

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
        function doPostBack(o) {
            __doPostBack(o.id, '');
        }
    </script>
    <div class="kaydir">

        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">

                <h4 id="baslikkk" runat="server" class="panel-title">
                    <label id="baslik" runat="server"></label>
                    <asp:DropDownList ID="drdTipSec" runat="server" CssClass="pull-right text-danger" AutoPostBack="true" OnSelectedIndexChanged="drdTipSec_SelectedIndexChanged">
                        <asp:ListItem Text="Tip Seçiniz" Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </h4>
            </div>
            <%--<div class="panel-body">
               
            </div>--%>
            <div class="table-responsive ">
                <div class="col-md-6 pull-right hidden-xs hidden-sm hidden-md ">
                    <asp:TextBox runat="server" ID="barkod" CssClass="form-control" OnTextChanged="barkod_TextChanged" AutoPostBack="true" onkeyup="doPostBack(this);" placeholder="barkod" />
                </div>
                <div class="input-group custom-search-form col-md-6">
                    <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Ara..." />
                    <span class="input-group-btn">
                        <button id="btnARA" runat="server" class="btn btn-default" type="submit" onserverclick="MusteriAra">
                            <i class="fa fa-search"></i>
                        </button>
                    </span>
                </div>
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upCrudGrid">
                    <ProgressTemplate>

                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel ID="upCrudGrid" runat="server">
                    <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(jScript);
                        </script>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="serviceID"
                            EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_OnRowCreated"
                            AllowPaging="true" PageSize="50" OnRowDataBound="GridView1_RowDataBound" OnPageIndexChanged="GridView1_PageIndexChanged" OnPageIndexChanging="GridView1_PageIndexChanging">
                            <%-- <RowStyle BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("css").ToString())%>' />--%>
                            <RowStyle ForeColor="White" />
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>

                                <%-- <%# Container.DataItemIndex %>' Text="<i class='fa fa-pencil'></i>" /> --%>
                                <asp:TemplateField HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" HeaderStyle-Width="100">
                                    <ItemTemplate>
                                        
                                        <div class="visible-lg">
                                            <asp:LinkButton ID="btnServis"
                                                runat="server"
                                                CssClass="btn btn-danger btn-xs"
                                                Text="<i class='fa fa-wrench'></i>" />
                                            <asp:LinkButton ID="btnEdit"
                                                runat="server"
                                                CssClass="btn btn-success btn-xs"
                                                CommandName="editRecord" CommandArgument='<%# Container.DisplayIndex %>' Text="<i class='fa fa-pencil'></i>" />

                                            <asp:LinkButton ID="delLink"
                                                runat="server"
                                                CssClass="btn btn-danger btn-xs"
                                                CommandName="del" CommandArgument='<%#Eval("serviceID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-lg" />

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Detay" HeaderStyle-CssClass="visible-xs visible-sm" ItemStyle-CssClass="visible-xs visible-sm">

                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnKucuk"
                                            runat="server"
                                            CssClass="btn btn-primary"
                                            Text="<i class='fa fa-refresh fa-spin'></i>" />

                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-xs visible-sm" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="serviceID" HeaderText="ID" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun"></asp:BoundField>
                                <asp:BoundField DataField="adres" HeaderText="Adres" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri Adı" />

                                <asp:BoundField DataField="baslik" HeaderText="Konu" />

                                <asp:BoundField DataField="aciklama" HeaderText="Açıklama"></asp:BoundField>
                                <asp:BoundField DataField="acilmaZamani" HeaderText="Tarih" DataFormatString="{0:d}"></asp:BoundField>
                                <asp:BoundField DataField="sonDurum" HeaderText="Durum" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                <asp:BoundField DataField="urunAdi" HeaderText="Ürün" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun"></asp:BoundField>
                                <asp:BoundField DataField="kimlikNo" HeaderText="Servis Kimlik" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun"></asp:BoundField>
                                <asp:BoundField DataField="servisTipi" HeaderText="Servis Tipi" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                <asp:BoundField DataField="tipID" HeaderText="TipID" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun"></asp:BoundField>
                                <asp:BoundField DataField="custID" HeaderText="custID" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun"></asp:BoundField>
                                <asp:BoundField DataField="css" HeaderText="css" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun"></asp:BoundField>
                                <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>

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
                      <h3 id="editModalLabel" class="baslik">Servis Formu</h3>
                  </div>
                  <asp:UpdatePanel ID="upEdit" runat="server">
                      <ContentTemplate>
                          <script type="text/javascript">
                              Sys.Application.add_load(jScript);
                          </script>
                          <div class="modal-body">
                              <div class="form-horizontal">
                                  <%-- <div class="form-group">

                                      <div class="col-sm-10">
                                          <asp:TextBox ID="lblID" CausesValidation="true" runat="server" Enabled="false" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                      </div>
                                  </div>--%>
                                  <asp:HiddenField ID="lblID" runat="server" />
                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtBaslik_2" CssClass="col-md-4 control-label">Konu/Başlık</asp:Label>
                                      <div class="col-sm-8">
                                          <asp:TextBox ID="txtBaslik_2" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                      </div>
                                  </div>

                                  <div class="form-group">

                                      <asp:Label runat="server" AssociatedControlID="txtmusteriAdi_3" Enabled="false" CssClass="col-md-4 control-label">Müşteri Adı</asp:Label>
                                      <div class="col-sm-8">

                                          <asp:TextBox ID="txtmusteriAdi_3" CausesValidation="true" Enabled="false" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                      </div>
                                  </div>

                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtAciklama_4" CssClass="col-md-4 control-label">Açıklama</asp:Label>
                                      <div class="col-sm-8">

                                          <asp:TextBox ID="txtAciklama_4" CausesValidation="true" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                      </div>
                                  </div>

                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtSonDurum_5" Enabled="false" CssClass="col-md-4 control-label">Servis Durumu</asp:Label>
                                      <div class="col-sm-8">

                                          <asp:TextBox ID="txtSonDurum_5" CausesValidation="true" Enabled="false" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                      </div>
                                  </div>

                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtUrun_6" Enabled="false" CssClass="col-md-4 control-label">Ürün/Hizmet</asp:Label>
                                      <div class="col-sm-8">
                                          <asp:HiddenField ID="custIDHdn" runat="server" Value="" />
                                          <asp:TextBox ID="txtUrun_6" CausesValidation="true" Enabled="false" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                      </div>
                                  </div>
                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="drdTip" CssClass="col-md-4 control-label">Servis Tipi</asp:Label>
                                      <div class="col-sm-8">
                                          <asp:DropDownList ID="drdTip" runat="server" CssClass="form-control"></asp:DropDownList>
                                      </div>
                                  </div>

                              </div>
                          </div>
                          <div class="modal-footer">
                              <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                              <asp:Button ID="btnSave" runat="server" Text="Güncelle" CssClass="btn btn-info" OnClick="btnSave_Click" />

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
            </div>
            <div class="panel-footer pull-right">
                <div class=" btn-group">

                    <asp:Button ID="btnAdd" runat="server" Text="Yeni" CssClass="btn btn-info"
                        OnClick="btnAdd_Click" />
                     <asp:Button ID="btnHarita" runat="server" Text="Servis Haritası" CssClass="btn btn-info"
                        OnClick="btnHarita_Click" />
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
</asp:Content>
