<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="CariHesaplar.aspx.cs" Inherits="TeknikServis.CariHesaplar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="kaydir">
        <div class="panel-group">
            <div class="panel panel-info">
                <!-- Default panel contents -->
                <div class="panel-heading">
                    MÜŞTERİ SEÇİMİ
                </div>
                <%--<div class="panel-body">
               
            </div>--%>
                <div class="table-responsive">
                    <div class="input-group custom-search-form">
                        <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Ara..." />
                        <span class="input-group-btn">
                            <button id="btnARA" runat="server" class="btn btn-default" type="submit" onserverclick="MusteriAra">
                                <i class="fa fa-search"></i>
                            </button>
                        </span>
                    </div>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                        <ProgressTemplate>

                            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                                <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                            </div>

                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:UpdatePanel ID="upCrudGrid" runat="server">
                        <ContentTemplate>

                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="CustID"
                                EmptyDataText="Kayıt girilmemiş" OnRowCreated="GridView1_RowCreated" OnRowCommand="GridView1_RowCommand" EnablePersistedSelection="false"
                                OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnPageIndexChanged="GridView1_PageIndexChanged" OnPageIndexChanging="GridView1_PageIndexChanging" AllowPaging="true" PageSize="10">
                                <SelectedRowStyle CssClass="danger" />
                                <PagerStyle CssClass="pagination-ys" />
                                <Columns>

                                    <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Seç">
                                        <ControlStyle CssClass="btn btn-danger"></ControlStyle>
                                        <ItemStyle CssClass="visible-lg" />
                                        <HeaderStyle CssClass="visible-lg" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="CustID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                        <HeaderStyle CssClass="visible-lg" />
                                        <ItemStyle CssClass="visible-lg" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="İşlemler" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg ">

                                        <ItemTemplate>

                                            <asp:LinkButton ID="btnOdemeler"
                                                runat="server"
                                                CssClass="btn btn-success btn-xs"
                                                Text="<i class='fa fa-bitcoin'>Ödemeler</i>" />
                                            <asp:LinkButton ID="btnDetay"
                                                runat="server"
                                                CssClass="btn btn-danger btn-xs"
                                                Text="<i class='fa fa-area-chart'>Detay</i>" />
                                            <asp:LinkButton ID="btnTahsilat"
                                                runat="server"
                                                CssClass="btn btn-warning btn-xs"
                                                Text="<i class='fa fa-money'>Al</i>" />
                                            <asp:LinkButton ID="btnOde"
                                                runat="server"
                                                CssClass="btn btn-warning btn-xs"
                                                Text="<i class='fa fa-info'>Öde</i>" />
                                            <asp:LinkButton ID="btnCariGuncel"
                                                runat="server"
                                                CssClass="btn btn-primary  btn-xs"
                                                CommandName="cari" CommandArgument='<%#Eval("CustID") %>' Text='cari'>  </asp:LinkButton>


                                        </ItemTemplate>
                                        <ItemStyle CssClass="visible-lg" />
                                        <HeaderStyle CssClass="visible-lg" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="İşlemler" HeaderStyle-CssClass="visible-xs visible-sm" ItemStyle-CssClass="visible-xs visible-sm">

                                        <ItemTemplate>
                                            <asp:LinkButton ID="_UIPCCodeLinkButton"
                                                runat="server"
                                                CssClass="btn btn-danger"
                                                Text="<i class='fa fa-refresh fa-spin'></i>"
                                                CommandName="Select"
                                                CommandArgument='<%# ((GridViewRow) Container).RowIndex  %>' />
                                            <asp:LinkButton ID="btnCariGuncelK"
                                                runat="server"
                                                CssClass="btn btn-primary  btn-xs"
                                                CommandName="cari" CommandArgument='<%#Eval("CustID") %>' Text='cari'>  </asp:LinkButton>
                                            <asp:LinkButton ID="btnOdeK"
                                                runat="server"
                                                CssClass="btn btn-warning"
                                                Text="<i class='fa fa-money'></i>" />

                                        </ItemTemplate>
                                        <ItemStyle CssClass="visible-xs visible-sm" />
                                        <HeaderStyle CssClass="visible-xs visible-sm" />
                                    </asp:TemplateField>
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
                                    <asp:BoundField DataField="email" HeaderText="E Posta" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                        <HeaderStyle CssClass="visible-lg" />
                                        <ItemStyle CssClass="visible-lg" />
                                    </asp:BoundField>
                                </Columns>

                            </asp:GridView>
                            <%-- <asp:Button ID="btnAdd" runat="server" Text="Add New Record" CssClass="btn btn-info"
                    OnClick="btnAdd_Click" />--%>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnARA" EventName="ServerClick" />
                        </Triggers>

                    </asp:UpdatePanel>

                    <!-- Detail Modal Starts here-->
                    <div id="detailModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-content modal-sm">
                            <div class="modal-header modal-header-info">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <h3 id="myModalLabel" class="baslik">Müşteri Detayları</h3>
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
                                        <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
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

                    <div id="cariModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="cariModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-content modal-md">
                            <div class="modal-header modal-header-info">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <h3 id="cariModalLabel" class="baslik">Cari Güncelleme</h3>
                            </div>

                            <div class="modal-body">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <div class="form-horizontal">

                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtCariBakiye" CssClass="col-md-4 control-label">Cari Bakiye</asp:Label>
                                                <div class="col-md-8">
                                                    <asp:TextBox runat="server" ID="txtCariBakiye" CssClass="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="chcBorclu" CssClass="col-md-4 control-label">Borçlu?</asp:Label>
                                                <div class="col-md-8">
                                                    <asp:CheckBox ID="chcBorclu" CssClass="form-control" Checked="true" runat="server" />
                                                    <asp:HiddenField ID="hdnCariCustID" runat="server" />
                                                </div>
                                            </div>
                                            <asp:Button ID="btnCariKaydet" runat="server" CssClass="btn btn-danger" Text="Kaydet" OnClick="btnCariKaydet_Click" />
                                        </div>


                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />

                                    </Triggers>
                                </asp:UpdatePanel>
                                <div class="modal-footer">
                                    <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Kapat</button>
                                </div>
                            </div>


                        </div>

                    </div>
                </div>
                <div class="panel panel-info">
                    <div class="panel-heading">
                        MÜŞTERİ EKSTRESİ
                   
                    </div>
                    <%--<div class="panel-body">
               
            </div>--%>
                    <div class="table-responsive">

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div id="cariOzet" runat="server" visible="false" class="pull-right">
                                    <span id="txtOdenen" runat="server" class="label label-warning"></span>
                                    <span id="txtBorc" runat="server" class="label label-primary"></span>
                                    <span id="txtBakiye" runat="server" class="label label-danger"></span>

                                </div>

                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="musteriAdi"
                                    EmptyDataText="Kayıt girilmemiş" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowCreated="GridView2_RowCreated" OnRowDataBound="GridView2_RowDataBound" AllowPaging="true" PageSize="10">
                                    <PagerStyle CssClass="pagination-ys" />
                                    <SelectedRowStyle CssClass="danger" />
                                    <Columns>

                                        <asp:BoundField DataField="musteriAdi" HeaderText="Kişi" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                            <HeaderStyle CssClass="visible-lg" />
                                            <ItemStyle CssClass="visible-lg" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="borc" HeaderText="Borç" HeaderStyle-CssClass="visible-lg visible-sm visible-xs" ItemStyle-CssClass="visible-lg visible-sm visible-xs">
                                            <HeaderStyle CssClass="visible-lg visible-sm visible-xs" />
                                            <ItemStyle CssClass="visible-lg visible-sm visible-xs" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="alacak" HeaderText="Alacak" HeaderStyle-CssClass="visible-lg visible-sm visible-xs" ItemStyle-CssClass="visible-lg visible-sm visible-xs">
                                            <HeaderStyle CssClass="visible-lg visible-sm visible-xs" />
                                            <ItemStyle CssClass="visible-lg visible-sm visible-xs" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="tarih" HeaderText="Tarih" HeaderStyle-CssClass="visible-lg visible-sm visible-xs" DataFormatString="{0:d}" ItemStyle-CssClass="visible-lg">
                                            <HeaderStyle CssClass="visible-lg visible-sm visible-xs" />
                                            <ItemStyle CssClass="visible-lg visible-sm visible-xs" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="islem" HeaderText="İşlem" HeaderStyle-CssClass="visible-lg visible-sm visible-xs" ItemStyle-CssClass="visible-lg visible-sm visible-xs">
                                            <HeaderStyle CssClass="visible-lg visible-sm visible-xs" />
                                            <ItemStyle CssClass="visible-lg visible-sm visible-xs" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="Konu" HeaderText="Konu" HeaderStyle-CssClass="visible-lg visible-sm visible-xs" ItemStyle-CssClass="visible-lg visible-sm visible-xs">
                                            <HeaderStyle CssClass="visible-lg visible-sm visible-xs" />
                                            <ItemStyle CssClass="visible-lg visible-sm visible-xs" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg visible-sm visible-xs" ItemStyle-CssClass="visible-lg visible-sm visible-xs">
                                            <HeaderStyle CssClass="visible-lg visible-sm visible-xs" />
                                            <ItemStyle CssClass="visible-lg visible-sm visible-xs" />
                                        </asp:BoundField>

                                          <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg visible-sm visible-xs" ItemStyle-CssClass="visible-lg visible-sm visible-xs">
                                            <HeaderStyle CssClass="visible-lg visible-sm visible-xs" />
                                            <ItemStyle CssClass="visible-lg visible-sm visible-xs" />
                                        </asp:BoundField>
                                    </Columns>

                                </asp:GridView>

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnARA" EventName="ServerClick" />
                                <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </div>

        </div>
    </div>
</asp:Content>
