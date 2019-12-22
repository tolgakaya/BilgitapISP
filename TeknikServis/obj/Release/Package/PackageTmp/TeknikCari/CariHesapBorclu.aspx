<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="CariHesapBorclu.aspx.cs" Inherits="TeknikServis.CariHesapBorclu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="kaydir">
        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                <h4 id="baslikkk" runat="server" class="panel-title">
                    <label id="baslik" runat="server">BORÇLU CARİ LİSTESİ</label>
                    <asp:DropDownList ID="drdKritik" runat="server" CssClass="pull-right text-info" AutoPostBack="true" OnSelectedIndexChanged="drdKritik_SelectedIndexChanged">
                        <asp:ListItem Text="Sırala" Value="s"></asp:ListItem>
                        <asp:ListItem Text="Alfabetik göster" Value="a"></asp:ListItem>
                        <asp:ListItem Text="Son mesaja göre sırala" Value="son"></asp:ListItem>

                    </asp:DropDownList>
                </h4>
            </div>

            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>

                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                        <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                    </div>

                </ProgressTemplate>
            </asp:UpdateProgress>
            <div class="table-responsive">
                <div class="input-group custom-search-form">
                    <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Ara..." />
                    <span class="input-group-btn">
                        <button id="btnARA" runat="server" class="btn btn-default" type="submit" onserverclick="MusteriAra">
                            <i class="fa fa-search"></i>
                        </button>
                    </span>
                </div>
                <asp:UpdatePanel runat="server" ID="upGrd">
                    <ContentTemplate>
                        <div id="cariOzet" runat="server" class="pull-right">
                            <span id="smsSayi" runat="server" class="label label-warning label-lg"></span>
                        </div>

                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                            DataKeyNames="tel"
                            EmptyDataText="Kayıt girilmemiş" OnRowCreated="GridView1_OnRowCreated"
                            OnPageIndexChanged="GridView1_PageIndexChanged" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AllowPaging="true" PageSize="50">
                            <PagerStyle CssClass="pagination-ys" />
                            <SelectedRowStyle CssClass="danger" />
                            <Columns>


                                <asp:TemplateField ShowHeader="False" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">

                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnSms"
                                            runat="server"
                                            CssClass="btn btn-danger"
                                            CommandName="ekle" CommandArgument='<%#Eval("tel") %>' Text="Sms Ekle" />


                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">

                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnDetay"
                                            runat="server"
                                            CssClass="btn btn-info"
                                            Text="<i class='fa fa-refresh'> Detay</i>" />


                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-lg" />
                                    <HeaderStyle CssClass="visible-lg" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm" ItemStyle-CssClass="visible-xs visible-sm">

                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnDetayK"
                                            runat="server"
                                            CssClass="btn btn-primary"
                                            Text="<i class='fa fa-refresh fa-spin'></i>" />
                                        <asp:LinkButton ID="btnSmsK"
                                            runat="server"
                                            CssClass="btn btn-danger"
                                            CommandName="ekle" CommandArgument='<%#Eval("tel") %>' Text="<i class='fa fa-phone fa-spin'></i>" />
                                    </ItemTemplate>

                                </asp:TemplateField>

                                <asp:BoundField DataField="musteriID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="netBakiye" HeaderText="Bakiye" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="netBorclanma" HeaderText="Borç" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="netAlacak" HeaderText="Alacak" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="tel" HeaderText="Tel" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="son_mesaj" HeaderText="Son Mesaj" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>

                            </Columns>

                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSms" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnARA" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="drdKritik" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>


            </div>

            <div class="panel-footer pull-right">
                <div class=" btn-group">



                    <asp:LinkButton ID="btnSms"
                        runat="server"
                        CssClass="btn btn-info" OnClick="btnSms_Click"
                        Text="<i class='fa fa-phone-square icon-2x'></i>" />

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



                </div>

            </div>
        </div>
    </div>
</asp:Content>
