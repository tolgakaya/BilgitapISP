<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="SatisEkle.aspx.cs" Inherits="TeknikServis.TeknikTeknik.SatisEkle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="kaydir">

        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>


        <div class="panel panel-info">
            <div class="panel-heading">
                HIZLI SERVİS KAYDI
            </div>
            <div class="panel-body">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse" data-parent="#accordion" href="#collapseThere" class="collapsed">Ürün/Parça Seçimi</a>
                        </h4>
                    </div>
                    <div id="collapseThere" class="panel-collapse collapse" style="height: 0px;">
                        <div class="panel-body">
                            <!-- Müşteri seçim alanı başlıyor -->

                            <div class="table-responsive">
                                <div class="input-group custom-search-form ">
                                    <span class="input-group-btn">
                                        <button id="btnARA" runat="server" class="btn btn-default" type="submit" onserverclick="CihazAra">
                                            <i class="fa fa-search"></i>
                                        </button>
                                    </span>
                                    <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Ara..." />

                                </div>

                                <asp:UpdatePanel ID="upCrudGrid" runat="server">
                                    <ContentTemplate>

                                        <asp:GridView ID="grdCihaz" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="ID"
                                            EmptyDataText="Ürün/Parça girilmemiş" OnSelectedIndexChanged="grdCihaz_SelectedIndexChanged">
                                            <SelectedRowStyle CssClass="danger" />
                                            <Columns>

                                                <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Seç" HeaderText="Seçim">
                                                    <ControlStyle CssClass="btn btn-primary"></ControlStyle>
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                    <HeaderStyle CssClass="visible-lg" />
                                                    <ItemStyle CssClass="visible-lg" />
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderText="Ürün/Parça Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">

                                                    <ItemTemplate>


                                                        <asp:LinkButton ID="btnRandom"
                                                            runat="server"
                                                            CssClass="btn btn-primary"
                                                            CommandName="detail" CommandArgument='<%#Eval("ID") %>' Text=' <%#Eval("cihaz_adi") %> '>
                           
                                                        </asp:LinkButton>
                                                    </ItemTemplate>

                                                </asp:TemplateField>
                                                <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                    <HeaderStyle CssClass="visible-lg" />
                                                    <ItemStyle CssClass="visible-lg" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="garanti_suresi" HeaderText="Garanti(AY)" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                    <HeaderStyle CssClass="visible-lg" />
                                                    <ItemStyle CssClass="visible-lg" />
                                                </asp:BoundField>

                                            </Columns>

                                        </asp:GridView>
                                        <asp:Button ID="btnAddCihaz" runat="server" Text="Yeni Ürün/Parça" CssClass="btn btn-primary"
                                            OnClick="btnAddCihaz_Click" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnARA" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="btnAddCihazRecord" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="grdCihaz" EventName="RowCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>

                                <!-- Add Record Modal Starts here-->
                                <div id="addModal" class="modal  fade" tabindex="-1" role="dialog"
                                    aria-labelledby="addModalLabel" aria-hidden="true">
                                    <div class="modal-dialog modal-content modal-sm">
                                        <div class="modal-header modal-header-info">
                                            <button type="button" class="close" data-dismiss="modal"
                                                aria-hidden="true">
                                                ×</button>
                                            <h3 id="addModalLabel" class="baslik">Yeni Ürün/Parça</h3>
                                        </div>
                                        <asp:UpdatePanel ID="upAdd" runat="server">
                                            <ContentTemplate>
                                                <div class="modal-body">

                                                    <div class="form-horizontal">

                                                        <div class="form-group">
                                                            <asp:Label runat="server" AssociatedControlID="txtCihazAdi" CssClass="col-md-10 control-label">Ürün/Parça Adı</asp:Label>
                                                            <div class="col-md-10">
                                                                <asp:TextBox runat="server" ID="txtCihazAdi" CssClass="form-control" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtCihazAdi" ErrorMessage="Lütfen Ürün/Parça adını giriniz"></asp:RequiredFieldValidator>
                                                                <asp:HiddenField ID="hdnGarantiSure" runat="server" />
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <asp:Label runat="server" AssociatedControlID="txtCihazAciklama" CssClass="col-md-10 control-label">Açıklama</asp:Label>
                                                            <div class="col-md-10">
                                                                <asp:TextBox runat="server" ID="txtCihazAciklama" CssClass="form-control" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtCihazAciklama" ErrorMessage="Lütfen  açıklama giriniz"></asp:RequiredFieldValidator>

                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <asp:Label runat="server" AssociatedControlID="txtGarantiSuresi" CssClass="col-md-10 control-label">Garanti(AY)</asp:Label>
                                                            <div class="col-md-10">
                                                                <asp:TextBox runat="server" ID="txtGarantiSuresi" CssClass="form-control" TextMode="Number" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtGarantiSuresi" ErrorMessage="Lütfen  süre giriniz"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>


                                                <div class="modal-footer">
                                                    <asp:Button ID="btnAddCihazRecord" runat="server" Text="Kaydet"
                                                        CssClass="btn btn-info" OnClick="btnAddCihazRecord_Click" ValidationGroup="musteriGrup2" />
                                                    <button class="btn btn-info" data-dismiss="modal"
                                                        aria-hidden="true">
                                                        Kapat</button>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnAddCihazRecord" EventName="Click" />
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="panel panel-info">
                            <div class="panel-heading">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo">Satış Bilgileri</a>
                                </h4>
                            </div>
                            <div id="collapseTwo" class="panel-collapse in" style="height: auto;">
                                <div class="panel-body">
                                    <div class="form-horizontal">

                                        <div class="form-group">
                                            <label class="col-sm-2 control-label" for="txtAdet">Ürün/Parça Adet</label>
                                            <div class="col-sm-10">

                                                <asp:TextBox ID="txtAdet" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label" for="txtCihazAdiGoster">Ürün/Parça</label>
                                            <div class="col-sm-10">
                                                <input id="txtCihazAdiGoster" runat="server" type="text" class="form-control" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label" for="txtIslemParca">Yapılacak İşlem</label>
                                            <div class="col-sm-10">
                                                <input id="txtIslemParca" runat="server" type="text" class="form-control" />
                                            </div>
                                        </div>

                                        <div class="form-group">

                                            <label class="col-sm-2 control-label" for="txtKDV">KDV Oranı</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="txtKDV" runat="server" Text="18" CssClass="form-control"></asp:TextBox>
                                                <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtKDV" Type="Currency" ValidationGroup="valGrup" CssClass="text-danger" MaximumValue="50" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">

                                            <label class="col-sm-2 control-label" for="txtYekun">Tutar(KDV Dahil)</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="txtYekun" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtYekun" Type="Currency" ValidationGroup="valGrup" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">

                                            <label class="col-sm-2 control-label" for="txtAciklama">Açıklama</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="txtAciklama" runat="server" TextMode="MultiLine" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
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

                                            <label for="tarih2" class="col-md-2 control-label">İşlem Tarihi</label>
                                            <div class="col-md-10">

                                                <input type='text' id="tarih2" runat="server" class="form-control" />

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnKaydet" runat="server" Text="Kaydet" CausesValidation="true" ValidationGroup="valGrup" CssClass="btn btn-info btn-block" OnClick="btnKaydet_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdCihaz" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
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
