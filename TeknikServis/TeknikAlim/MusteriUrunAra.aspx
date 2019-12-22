<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="MusteriUrunAra.aspx.cs" Inherits="TeknikServis.TeknikAlim.MusteriUrunAra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="kaydir">
        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                <h4 id="baslik" runat="server" class="panel-title">MÜŞTERİYE VERİLEN ÜRÜN VE YEDEK PARÇALAR</h4>
            </div>
            <%--<div class="panel-body">
               
            </div>--%>
            <div class="table-responsive ">
                <div class="input-group custom-search-form">
                    <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Ürün/Parça ismi giriniz..." />
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
                        <div id="cariOzet" runat="server" class="pull-right">
                            <span id="txtAdet" runat="server" class="label label-success"></span>
                            <span id="txtTutar" runat="server" class="label label-warning"></span>
                        </div>

                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="urunID"
                            EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_OnRowCreated" OnRowDataBound="GridView1_RowDataBound"
                            OnPageIndexChanging="GridView1_PageIndexChanging" AllowPaging="true" PageSize="10">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" HeaderStyle-Width="100">

                                    <ItemTemplate>

                                        <asp:LinkButton ID="delLink"
                                            runat="server"
                                            CssClass="btn btn-danger btn-xs"
                                            CommandName="del" CommandArgument='<%#Eval("urunID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                        <asp:LinkButton ID="btnIade"
                                            runat="server"
                                            CssClass="btn btn-success btn-xs"
                                            CommandName="iade" CommandArgument='<%#Eval("urunID")+ ";" + Container.DisplayIndex  %>' Text="<i class='fa fa-check'></i>" />
                                        <asp:LinkButton ID="btnMusteriDetay"
                                            runat="server"
                                            CssClass="btn btn-warning btn-xs"
                                            Text="<i class='fa fa-user'></i>" />

                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-lg" />
                                    <HeaderStyle CssClass="visible-lg" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="urunID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>

                                <asp:TemplateField HeaderText="İşlemler" HeaderStyle-CssClass="visible-xs visible-sm" ItemStyle-CssClass="visible-xs visible-sm">

                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnMusteriDetayK"
                                            runat="server"
                                            CssClass="btn btn-info"
                                            Text="<i class='fa fa-user'></i>" />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-xs visible-sm" />
                                    <HeaderStyle CssClass="visible-xs visible-sm" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Müşteri Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">

                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnRandom"
                                            runat="server"
                                            CssClass="btn btn-primary"
                                            CommandName="detail" CommandArgument='<%#Eval("musteriID") %>' Text=' <%#Eval("musteriAdi") %> '>  </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Cinsi" HeaderText="Cihaz" />

                                <asp:BoundField DataField="garantiBaslangic" HeaderText="Garanti Başlangıcı" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:D}" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="garantiBitis" HeaderText="Garanti Bitişi" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:D}" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="garantiSuresi" HeaderText="Garanti Süresi" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="durum" HeaderText="Durum" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="satis_tutar" HeaderText="Satış Tutarı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="iade_tutar" HeaderText="İade Tutarı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="musteriID" HeaderText="Müşteri ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                            </Columns>

                        </asp:GridView>

                        <div class=" btn-group pull-right">


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

                            <asp:LinkButton ID="btnSms"
                                runat="server"
                                CssClass="btn btn-info" OnClick="btnSms_Click"
                                Text="<i class='fa fa-phone-square icon-2x'></i>" />

                            <asp:LinkButton ID="btnMail"
                                runat="server"
                                CssClass="btn btn-info " OnClick="btnMail_Click"
                                Text="<i class='fa fa-envelope icon-2x'></i>" />


                        </div>

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
                                            <asp:BoundField DataField="gecerlilik" HeaderText="Geçerlilik" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="Radius_UserName" HeaderText="UserName" />
                                            <asp:BoundField DataField="paket_adi" HeaderText="Paket" />

                                        </Fields>
                                    </asp:DetailsView>


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
                <!-- Detail Modal Ends here -->

                <!-- Add Record Modal Starts here-->
                <div id="onayModal" class="modal  fade" tabindex="-1" role="dialog"
                    aria-labelledby="onayModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-content modal-md">
                        <div class="modal-header modal-header-info">
                            <button type="button" class="close" data-dismiss="modal"
                                aria-hidden="true">
                                ×</button>
                            <h3 id="onayModalLabel" class="baslik">Cihaz İade Bilgileri</h3>
                        </div>
                        <asp:UpdatePanel ID="upAdd" runat="server">

                            <ContentTemplate>

                                <div class="modal-body">
                                    <div class="form-horizontal">

                                        <div class="form-group">
                                            <asp:Label runat="server" AssociatedControlID="txtIadeTutar" CssClass="col-md-4 control-label">İade Tutarı</asp:Label>
                                            <div class="col-md-8">
                                                <asp:TextBox runat="server" ID="txtIadeTutar" CssClass="form-control" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="iadeGrup" ControlToValidate="txtIadeTutar" ErrorMessage="Banka adı giriniz."></asp:RequiredFieldValidator>
                                                <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="txtIadeTutar" ValidationGroup="iadeGrup" Type="Currency" MinimumValue="0" MaximumValue="10000000" runat="server" />

                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <asp:Label runat="server" AssociatedControlID="txtIadeAciklama" CssClass="col-md-4 control-label">Açıklama</asp:Label>
                                            <div class="col-md-8">
                                                <asp:TextBox runat="server" ID="txtIadeAciklama" CssClass="form-control" />
                                                <asp:HiddenField ID="hdnGarantiID" runat="server" />
                                                <asp:HiddenField ID="hdnCustID" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <div class="btn-group pull-right">

                                        <asp:Button ID="btnIade" runat="server" Text="Tamam"
                                            CssClass="btn btn-info" OnClick="btnIade_Click" />
                                        <button class="btn btn-info" data-dismiss="modal"
                                            aria-hidden="true">
                                            Kapat</button>

                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />

                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <!--Add Record Modal Ends here-->

            </div>


        </div>
    </div>

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
</asp:Content>
