<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Bankalar.aspx.cs" Inherits="TeknikServis.TeknikCari.Bankalar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="kaydir">
        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                <h4 id="baslikkk" runat="server" class="panel-title">
                    <label id="baslik" runat="server">BANKA HESAPLARI</label>

                </h4>
            </div>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>

                    <div class="table-responsive">

                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                            DataKeyNames="banka_id"
                            EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_RowCreated"
                            OnPageIndexChanging="GridView1_PageIndexChanging" AllowPaging="true" PageSize="10">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="delLink"
                                            runat="server"
                                            CssClass="btn btn-danger"
                                            CommandName="del" CommandArgument='<%#Eval("banka_id") %>' Text="<i class='fa fa-money'></i>" />
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:TemplateField>


                                <asp:BoundField DataField="banka_id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>

                                <asp:BoundField DataField="banka_adi" HeaderText="Banka" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="giris" HeaderText="Giriş" DataFormatString="{0:C}">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="cikis" HeaderText="Çıkış" DataFormatString="{0:C}">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="bakiye" HeaderText="Bakiye" DataFormatString="{0:C}"></asp:BoundField>

                            </Columns>

                        </asp:GridView>

                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnTransferKaydet" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnKaydet" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <div class="panel-footer pull-right">
                <div class=" btn-group">

                    <asp:LinkButton ID="btnEkle"
                        runat="server"
                        CssClass="btn btn-info" OnClick="btnEkle_Click"
                        Text="Yeni Banka" />
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
                        CssClass="btn btn-info" OnClick="btnExportWord_Click"
                        Text="<i class='fa fa-wikipedia-w icon-2x'></i>" />

                </div>

            </div>

        </div>

        <div id="onayModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="onayModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">

                <asp:UpdatePanel ID="UpdatePanel4" runat="server">

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">

                                <div class="col-md-12">
                                    <div class="alert alert-info text-center">


                                        <i class="fa fa-2x">Kasaya aktaracağınız tutar</i>

                                        <div class="form-vertical">
                                            <div class="form-group">
                                               
                                                <div class="col-md-12">
                                                    <asp:TextBox runat="server" ID="txtTutar" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="transferGrup" ControlToValidate="txtTutar" ErrorMessage="Tahsilatta yapılacak kesintiyi giriniz"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtTutar" ValidationGroup="transferGrup" Type="Currency" MinimumValue="1" MaximumValue="1000000" ErrorMessage="Küsurat için virgül kullanınız"></asp:RangeValidator>
                                                     <asp:HiddenField ID="hdnCekID" runat="server" />
                                                </div>
                                            </div>


                                            <div class="btn-group pull-right">

                                                   <asp:Button ID="btnTransferKaydet" runat="server" Text="Tamam" ValidationGroup="transferGrup"
                                            CssClass="btn btn-info" OnClick="btnTransferKaydet_Click" />
                                        <button class="btn btn-info" data-dismiss="modal"
                                            aria-hidden="true">
                                            Kapat</button>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />

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
                    <h3 id="addModalLabel" class="baslik">Banka Tanımı</h3>
                </div>
                <asp:UpdatePanel ID="upAdd" runat="server">

                    <ContentTemplate>

                        <div class="modal-body">
                            <div class="form-horizontal">

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtBankaAdi" CssClass="col-md-4 control-label">Banka Adı</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtBankaAdi" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtBankaAdi" ErrorMessage="Banka adı giriniz."></asp:RequiredFieldValidator>

                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtBankaAciklama" CssClass="col-md-4 control-label">Açıklama</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtBankaAciklama" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtBankaAciklama" ErrorMessage="Hesap açıklaması giriniz"></asp:RequiredFieldValidator>

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtBankaBakiye" CssClass="col-md-4 control-label">Bakiye</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtBankaBakiye" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtBankaBakiye" ErrorMessage="Lütfen bakiye giriniz"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="txtBankaBakiye" Type="Currency" MinimumValue="0" MaximumValue="10000000" runat="server" />
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
