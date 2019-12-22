<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Poslar.aspx.cs" Inherits="TeknikServis.TeknikCari.Poslar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="kaydir">

        <div id="panelContents" runat="server" class="panel panel-info">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" class="collapsed">Arama Kriterleri</a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse" style="height: 0px;">
                <div class="panel-body">
                    <div class="row ">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label for="chcCekilen">Sadece Çekilenler:</label>
                                <asp:CheckBox ID="chcCekilen" CssClass="form-control" Text="Sadece çekilenler" runat="server" />
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label for="drdAktif">Çekilebilir Bakiye:</label>
                                <asp:DropDownList ID="drdAktif" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Bütün Hesaplar" Value="hepsi"></asp:ListItem>
                                    <asp:ListItem Text="Çekilebilir Hesaplar" Value="cek"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-sm-4">
                            <div class="form-group">
                                <label for="drdKart">Pos Seçimi:</label>
                                <asp:DropDownList ID="drdKart" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Pos Seçiniz" Value="sec"></asp:ListItem>
                                    <asp:ListItem Text="Bütün Poslar" Value="hepsi"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>


                    <div class="form-group">

                        <asp:Button ID="btnAra" CssClass="btn btn-info btn-lg btn-block" runat="server" Text="Ara..." OnClick="btnAra_Click" />

                    </div>
                    <!--body-->
                </div>
            </div>
        </div>

        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                <h4 id="baslikkk" runat="server" class="panel-title">
                    <label id="baslikk" runat="server">POS HESAPLARI</label>
                </h4>
            </div>
            <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                <ProgressTemplate>
                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                        <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="grdUp" runat="server">
                <ContentTemplate>

                    <div class="table-responsive">
                        <div id="cariOzet" runat="server" class="pull-right">
                            <span id="txtAdet" runat="server" class="label label-warning"></span>
                            <span id="txtTutar" runat="server" class="label label-primary"></span>
                            <span id="txtKomisyon" runat="server" class="label label-primary"></span>
                            <span id="txtNet" runat="server" class="label label-primary"></span>
                            <asp:Button ID="btnTransfer" runat="server" Text="Transfer" Visible="false" CssClass="btn btn-sm btn-danger" OnClick="btnTransfer_Click" />
                        </div>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                            DataKeyNames="pos_id"
                            EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand"
                            OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" AllowPaging="true" PageSize="10">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>

                                <asp:BoundField DataField="pos_id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="posadi" HeaderText="POS" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>

                                <asp:BoundField DataField="extre_tarihi" HeaderText="Extre Tarihi" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="tahsilat_tarih" HeaderText="Tarih" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm" DataFormatString="{0:D}">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Musteri_Adi" HeaderText="Kişi" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="komisyon_tutar" HeaderText="Tür" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="komsiyon_oran" HeaderText="Kom %" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="net_tutar" HeaderText="Net Tutar" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:C}" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Aciklama" HeaderText="İşlem Yeri" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>


                            </Columns>

                        </asp:GridView>

                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAra" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <div class="panel-footer pull-right">
                <div class=" btn-group">

                    <asp:LinkButton ID="btnEkle"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnEkle_Click"
                        Text="Yeni Pos" />
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
        <div id="onayModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="addModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-sm">

                <asp:UpdatePanel ID="UpdatePanel2" runat="server">

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">

                                <div class="col-md-12">
                                    <div class="alert alert-info text-center">
                                        <i class="fa fa-2x">Seçilen pos hesabını bankaya transfer edecek misiniz?</i>

                                        <div class="btn-group pull-right">

                                            <asp:Button ID="btnTransferKaydet" runat="server" Text="Tamam"
                                                CssClass="btn btn-success" OnClick="btnTransferKaydet_Click" />
                                            <button class="btn btn-warning" data-dismiss="modal"
                                                aria-hidden="true">
                                                Kapat</button>

                                        </div>
                                    </div>
                                </div>

                                <asp:HiddenField ID="hdnCekID" runat="server" />

                            </div>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnTransfer" EventName="Click" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <!-- Add Record Modal Starts here-->
        <div id="addModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="addModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">
                <div class="modal-header modal-header-info">
                    <button type="button" class="close" data-dismiss="modal"
                        aria-hidden="true">
                        ×</button>
                    <h3 id="addModalLabel" class="baslik">Pos Tanımlama</h3>
                </div>
                <asp:UpdatePanel ID="upAdd" runat="server">

                    <ContentTemplate>

                        <div class="modal-body">
                            <div class="form-horizontal">

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtPosAdi" CssClass="col-md-4 control-label">Pos Adı</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtPosAdi" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtPosAdi" ErrorMessage="Pos adı giriniz."></asp:RequiredFieldValidator>

                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtPosSure" CssClass="col-md-4 control-label">Hesaba Geçiş Süresi(gün)</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtPosSure" TextMode="Number" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtPosSure" ErrorMessage="Gün olarak süre giriniz"></asp:RequiredFieldValidator>

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtPosKomisyon" CssClass="col-md-4 control-label">Komisyon %</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtPosKomisyon" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtPosKomisyon" ErrorMessage="Komisyon oranı giriniz"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="txtPosKomisyon" Type="Currency" MinimumValue="0" MaximumValue="100" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="drdPosBanka" CssClass="col-md-4 control-label">Banka</asp:Label>
                                    <div class="col-md-8">
                                        <asp:DropDownList ID="drdPosBanka" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Banka seçiniz" Value="-1"></asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnKaydet" runat="server" Text="Kaydet"
                                CssClass="btn btn-info" ValidationGroup="musteriGrup2" OnClick="btnKaydet_Click" />
                            <button class="btn btn-info" data-dismiss="modal"
                                aria-hidden="true">
                                Kapat</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnEkle" EventName="Click" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <!--Add Record Modal Ends here-->
    </div>

</asp:Content>
