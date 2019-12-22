<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="TekRaporView.aspx.cs" Inherits="TeknikServis.Raporlar.TekRaporView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="kaydir">

        <div id="panelContents" runat="server" class="panel panel-info">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseOnee" class="collapsed">Arama Kriterleri</a>
                </h4>
            </div>

            <div id="collapseOnee" class="panel-collapse collapse" style="height: 0px;">
                <div class="panel-body">
                    <div class="row ">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label for="datetimepicker6">Şu Tarihten:</label>
                                <input type='text' runat="server" class="form-control" id="datetimepicker6" />

                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label for="datetimepicker7">Şu Tarihe:</label>
                                <input type='text' runat="server" class="form-control" id="datetimepicker7" />
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

        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
            <ProgressTemplate>

                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                </div>

            </ProgressTemplate>
        </asp:UpdateProgress>
        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                <asp:Label Text="DURUM ÖZETİ" ID="baslik" runat="server" />
            </div>
            <%--<div class="panel-body">
               
            </div>--%>
            <div class="table-responsive ">

                <asp:UpdatePanel ID="upCrudGrid" runat="server">
                    <ContentTemplate>

                        <div class="table-responsive">
                            <table class="table">

                                <tbody>
                                    <tr>
                                        <td>
                                            <h4>
                                                <asp:Label ID="kasalbl" CssClass="label label-warning" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="bankalbl" CssClass="label label-warning" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="poslbl" CssClass="label label-warning" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="kartlbl" CssClass="label label-warning" runat="server" /></h4>
                                        </td>
                                    </tr>
                                    <tr class="info">
                                        <td>
                                            <h4>
                                                <asp:Label ID="cariborclbl" CssClass="label label-danger" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="carialacaklbl" CssClass="label label-danger" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="caribakiyelbl" CssClass="label label-danger" runat="server" /></h4>
                                        </td>
                                        <td>-</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <h4>
                                                <asp:Label ID="acilanservissayisi" CssClass="label label-info" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="kapananservissayisi" CssClass="label label-info" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="kapananservistutari" CssClass="label label-info" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="kapananservismaliyeti" CssClass="label label-info" runat="server" /></h4>
                                        </td>
                                    </tr>
                                    <tr class="info">
                                        <td>
                                            <h4>
                                                <asp:Label ID="acilandisservissayisi" CssClass="label label-default" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="acilandisservisMaliyeti" CssClass="label label-default" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="tamamlanandisservisSayisi" CssClass="label label-default" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="tamamlanandisservisMaliyeti" CssClass="label label-default" runat="server" /></h4>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <h4>
                                                <asp:Label ID="odemesayisi" CssClass="label label-success" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="odemetoplami" CssClass="label label-success" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="tahsilatsayisi" CssClass="label label-success" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="tahsilattoplami" CssClass="label label-success" runat="server" /></h4>
                                        </td>
                                    </tr>
                                    <tr class="info">
                                        <td>
                                            <h4>
                                                <asp:Label ID="satinalmasayisi" CssClass="label label-warning" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="satinalmaToplami" CssClass="label label-warning" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="iadeSayisi" CssClass="label label-warning" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="iadeTutari" CssClass="label label-warning" runat="server" /></h4>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <h4>
                                                <asp:Label ID="pesinsatisSayisi" CssClass="label label-danger" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="pesinSatisTutari" CssClass="label label-danger" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="emanetVerilenSayisi" CssClass="label label-default" runat="server" /></h4>
                                        </td>
                                        <td>
                                            <h4>
                                                <asp:Label ID="emanettenDonenlerSayisi" CssClass="label label-default" runat="server" /></h4>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>

                        </div>

                        <div class="panel panel-info">
                            <div class="panel-heading">
                                Rapor Detayları
                            </div>
                            <div class="panel-body">
                                <div class="panel-group" id="accordion">

                                    <div class="panel panel-info" id="acilanPanel" runat="server">
                                        <div class="panel-heading">
                                            <h4 class="panel-title">
                                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" class="collapsed">Açılan Servisler</a>
                                            </h4>
                                        </div>
                                        <div id="collapseOne" class="panel-collapse collapse" style="height: 0px;">
                                            <div class="panel-body">
                                                <div class="table-responsive">
                                                    <asp:GridView ID="grdAcilanServis" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="serviceID"
                                                        EmptyDataText="Kayıt girilmemiş" OnRowCreated="grdAcilanServis_RowCreated"
                                                        AllowPaging="true" PageSize="50" OnRowDataBound="grdAcilanServis_RowDataBound" OnPageIndexChanging="grdAcilanServis_PageIndexChanging">
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
                                                            <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri" />
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
                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                    <div class="panel panel-info" id="kapananPanel" runat="server">
                                        <div class="panel-heading">
                                            <h4 class="panel-title">
                                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" class="collapsed">Kapanan Servisler</a>
                                            </h4>
                                        </div>
                                        <div id="collapseTwo" class="panel-collapse collapse" style="height: 0px;">
                                            <div class="panel-body">

                                                <div class="table-responsive">
                                                    <asp:GridView ID="grdKapananServis" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="serviceID"
                                                        EmptyDataText="Kayıt girilmemiş" OnRowCreated="grdKapananServis_RowCreated"
                                                        AllowPaging="true" PageSize="50" OnRowDataBound="grdKapananServis_RowDataBound" OnPageIndexChanging="grdKapananServis_PageIndexChanging">
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
                                                            <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri" />
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
                                                </div>

                                            </div>


                                        </div>
                                    </div>


                                    <div class="panel panel-info" id="yeniDisPanel" runat="server" visible="false">
                                        <div class="panel-heading">
                                            <h4 class="panel-title">
                                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseThree" class="collapsed">Yeni Dış servisler</a>
                                            </h4>
                                        </div>
                                        <div id="collapseThree" class="panel-collapse collapse">
                                            <div class="panel-body">
                                                <div class="table-responsive">
                                                    <asp:GridView ID="grdDisAcilan" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                                        DataKeyNames="hesapID"
                                                        EmptyDataText="Kayıt girilmemiş" OnRowCreated="grdDisAcilan_RowCreated"
                                                        OnPageIndexChanging="grdDisAcilan_PageIndexChanging" AllowPaging="true" PageSize="10">
                                                        <PagerStyle CssClass="pagination-ys" />
                                                        <Columns>

                                                            <asp:TemplateField>
                                                                <ItemTemplate>

                                                                    <asp:LinkButton ID="btnServis"
                                                                        runat="server"
                                                                        CssClass="btn btn-success btn-xs visible-lg"
                                                                        CommandName="gonder" CommandArgument='<%#Eval("servisID")+ ";" + Container.DisplayIndex +";"+ Eval("musteriID")+";"+ Eval("kimlik")  %>' Text="<i class='fa fa-wrench'></i>" />

                                                                    <asp:LinkButton ID="btnServisK"
                                                                        runat="server"
                                                                        CssClass="btn btn-success visible-xs visible-sm"
                                                                        CommandName="gonder" CommandArgument='<%#Eval("servisID")+ ";" + Container.DisplayIndex +";"+ Eval("musteriID")+";"+ Eval("kimlik")  %>' Text="<i class='fa fa-wrench'></i>" />

                                                                </ItemTemplate>

                                                            </asp:TemplateField>


                                                            <asp:BoundField DataField="hesapID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="islemParca" HeaderText="İşlem" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="cihaz" HeaderText="Ürün/Parça" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                                            <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                                            <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="kdv" HeaderText="KDV" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="tutar" HeaderText="Tutar" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="yekun" HeaderText="Yekün" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="birim_maliyet" HeaderText="Birim Maliyet" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="toplam_maliyet" HeaderText="Toplam Maliyet" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="onayDurumu" HeaderText="Onay" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>


                                                            <asp:BoundField DataField="tarihZaman" HeaderText="Hesap Tarihi" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="onayTarih" HeaderText="Onay Tarihi" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="servisID" HeaderText="SID" HeaderStyle-CssClass="hidden-xs  hidden-lg" ItemStyle-CssClass="hidden-xs hidden-lg"></asp:BoundField>
                                                            <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                        </Columns>

                                                    </asp:GridView>
                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                    <div class="panel panel-info" id="tamamDisPanel" runat="server" visible="false">
                                        <div class="panel-heading">
                                            <h4 class="panel-title">
                                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseFour" class="collapsed">Tamamlanan Dış Servisler</a>
                                            </h4>
                                        </div>
                                        <div id="collapseFour" class="panel-collapse collapse">
                                            <div class="panel-body">
                                                <div class="table-responsive">
                                                    <asp:GridView ID="grdDisTamam" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                                        DataKeyNames="hesapID"
                                                        EmptyDataText="Kayıt girilmemiş" OnRowCreated="grdDisTamam_RowCreated"
                                                        OnPageIndexChanging="grdDisTamam_PageIndexChanging" AllowPaging="true" PageSize="10">
                                                        <PagerStyle CssClass="pagination-ys" />
                                                        <Columns>

                                                            <asp:TemplateField>
                                                                <ItemTemplate>

                                                                    <asp:LinkButton ID="btnServis"
                                                                        runat="server"
                                                                        CssClass="btn btn-success btn-xs visible-lg"
                                                                        CommandName="gonder" CommandArgument='<%#Eval("servisID")+ ";" + Container.DisplayIndex +";"+ Eval("musteriID")+";"+ Eval("kimlik")  %>' Text="<i class='fa fa-wrench'></i>" />

                                                                    <asp:LinkButton ID="btnServisK"
                                                                        runat="server"
                                                                        CssClass="btn btn-success visible-xs visible-sm"
                                                                        CommandName="gonder" CommandArgument='<%#Eval("servisID")+ ";" + Container.DisplayIndex +";"+ Eval("musteriID")+";"+ Eval("kimlik")  %>' Text="<i class='fa fa-wrench'></i>" />

                                                                </ItemTemplate>

                                                            </asp:TemplateField>


                                                            <asp:BoundField DataField="hesapID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="islemParca" HeaderText="İşlem" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="cihaz" HeaderText="Ürün/Parça" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                                            <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                                            <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="kdv" HeaderText="KDV" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="tutar" HeaderText="Tutar" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="yekun" HeaderText="Yekün" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="birim_maliyet" HeaderText="Birim Maliyet" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="toplam_maliyet" HeaderText="Toplam Maliyet" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="onayDurumu" HeaderText="Onay" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>


                                                            <asp:BoundField DataField="tarihZaman" HeaderText="Hesap Tarihi" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="onayTarih" HeaderText="Onay Tarihi" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="servisID" HeaderText="SID" HeaderStyle-CssClass="hidden-xs  hidden-lg" ItemStyle-CssClass="hidden-xs hidden-lg"></asp:BoundField>
                                                            <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                        </Columns>

                                                    </asp:GridView>
                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                    <div class="panel panel-info" id="odemePanel" runat="server" visible="false">
                                        <div class="panel-heading">
                                            <h4 class="panel-title">
                                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseFive" class="collapsed">Ödeme/Tahsilat ve İadeler</a>
                                            </h4>
                                        </div>
                                        <div id="collapseFive" class="panel-collapse collapse">
                                            <div class="panel-body">
                                                <div class="table-responsive">

                                                    <asp:GridView ID="grdOdeme" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                                        DataKeyNames="odemeID"
                                                        EmptyDataText="Kayıt girilmemiş"
                                                        OnPageIndexChanging="grdOdeme_PageIndexChanging" OnRowDataBound="grdOdeme_RowDataBound" AllowPaging="true" PageSize="10">
                                                        <PagerStyle CssClass="pagination-ys" />
                                                        <Columns>


                                                            <asp:BoundField DataField="odemeID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="kullaniciID" HeaderText="kullaniciID" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun">
                                                                <HeaderStyle CssClass="gizlisutun" />
                                                                <ItemStyle CssClass="gizlisutun" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="musteriID" HeaderText="musteriID" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun">
                                                                <HeaderStyle CssClass="gizlisutun" />
                                                                <ItemStyle CssClass="gizlisutun" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="odemeMiktari" HeaderText="Miktar" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="odemeTarih" HeaderText="Tarih" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" DataFormatString="{0:d}" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="tahsilatOdeme_turu" HeaderText="Tür" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="tahsilat_odeme" HeaderText="İşlem" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="islem_adres" HeaderText="İşlem Yeri" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="islem_tarihi" HeaderText="İşlem Tarihi" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:d}" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:d}" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>

                                                        </Columns>

                                                    </asp:GridView>

                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                    <div class="panel panel-info" id="alimPanel" runat="server" visible="false">
                                        <div class="panel-heading">
                                            <h4 class="panel-title">
                                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseSix" class="collapsed">Satınalmalar</a>
                                            </h4>
                                        </div>
                                        <div id="collapseSix" class="panel-collapse collapse">
                                            <div class="panel-body">
                                                <div class="table-responsive">
                                                    <asp:GridView ID="grdAlimlar" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="alim_id"
                                                        EmptyDataText="Satın alma kaydı bulunmuyor" AllowPaging="true" PageSize="10" OnPageIndexChanging="grdAlimlar_PageIndexChanging" OnRowCreated="grdAlimlar_RowCreated">

                                                        <PagerStyle CssClass="pagination-ys" />
                                                        <Columns>

                                                            <%-- <%# Container.DataItemIndex %>' Text="<i class='fa fa-pencil'></i>" /> --%>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>

                                                                    <asp:LinkButton ID="btnDetay"
                                                                        runat="server"
                                                                        CssClass="btn btn-success"
                                                                        CommandName="detay" CommandArgument='<%# Eval("alim_id") %>' Text="<i class='fa fa-pencil'></i>" />


                                                                    </div>
                                                                </ItemTemplate>


                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("musteri_adi") %>'></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>

                                                                    <asp:LinkButton ID="btnTedarikci"
                                                                        runat="server"
                                                                        CssClass="btn btn-primary"
                                                                        CommandName="detail" CommandArgument='<%#Eval("CustID") %>' Text=' <%#Eval("musteri_adi") %> '>
                           
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>

                                                            </asp:TemplateField>

                                                            <%--<asp:BoundField DataField="musteri_adi" HeaderText="Kişi/Firma"></asp:BoundField>--%>
                                                            <asp:BoundField DataField="konu" HeaderText="Konu" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>

                                                            <asp:BoundField DataField="aciklama" HeaderText="Açıklama"></asp:BoundField>
                                                            <asp:BoundField DataField="belge_no" HeaderText="Belge No" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                                            <asp:BoundField DataField="toplam_tutar" HeaderText="Tutar" DataFormatString="{0:C}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                            <asp:BoundField DataField="toplam_kdv" HeaderText="KDV" DataFormatString="{0:C}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                            <asp:BoundField DataField="toplam_yekun" HeaderText="Yekün" DataFormatString="{0:C}" />
                                                            <asp:BoundField DataField="alim_tarih" HeaderText="Tarih" DataFormatString="{0:d}"></asp:BoundField>
                                                            <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg"></asp:BoundField>


                                                        </Columns>

                                                    </asp:GridView>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel panel-info" id="emanetVerilenPanel" runat="server" visible="false">
                                        <div class="panel-heading">
                                            <h4 class="panel-title">
                                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseseven" class="collapsed">Emanet Verilenler</a>
                                            </h4>
                                        </div>
                                        <div id="collapseseven" class="panel-collapse collapse">
                                            <div class="panel-body">
                                                <div class="table-responsive">
                                                    <asp:GridView ID="grdEmanetYeni" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                                        DataKeyNames="yedekID"
                                                        EmptyDataText="Kayıt girilmemiş">
                                                        <Columns>


                                                            <asp:BoundField DataField="yedekID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="musteriID" HeaderText="musteriID" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun">
                                                                <HeaderStyle CssClass="gizlisutun" />
                                                                <ItemStyle CssClass="gizlisutun" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="urunAciklama" HeaderText="Emanet" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="verilmeTarihi" HeaderText="Verilme" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="donusTarih" HeaderText="İade" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="donmeDurumu" HeaderText="Durum" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>

                                                        </Columns>

                                                    </asp:GridView>
                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                    <div class="panel panel-info" id="emanetDonenPanel" runat="server" visible="false">
                                        <div class="panel-heading">
                                            <h4 class="panel-title">
                                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseeight" class="collapsed">Dönen Emanetler</a>
                                            </h4>
                                        </div>
                                        <div id="collapseeight" class="panel-collapse collapse">
                                            <div class="panel-body">
                                                <div class="table-responsive">
                                                    <asp:GridView ID="grdEmanetDonen" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                                        DataKeyNames="yedekID"
                                                        EmptyDataText="Kayıt girilmemiş">
                                                        <Columns>


                                                            <asp:BoundField DataField="yedekID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="musteriID" HeaderText="musteriID" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun">
                                                                <HeaderStyle CssClass="gizlisutun" />
                                                                <ItemStyle CssClass="gizlisutun" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="urunAciklama" HeaderText="Emanet" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="verilmeTarihi" HeaderText="Verilme" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="donusTarih" HeaderText="İade" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="donmeDurumu" HeaderText="Durum" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>

                                                        </Columns>

                                                    </asp:GridView>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <%-- gridview yeri --%>
                    </ContentTemplate>
                    <Triggers>

                        <%--<asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />--%>
                        <asp:AsyncPostBackTrigger ControlID="btnAra" EventName="Click" />


                    </Triggers>
                </asp:UpdatePanel>


            </div>
            <div class="panel-footer pull-right">
                <div class=" btn-group">

                    <%--      <asp:LinkButton ID="btnPrint"
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
                        Text="<i class='fa fa-wikipedia-w icon-2x'></i>" />--%>
                </div>

            </div>


        </div>

    </div>

    <script type="text/javascript">
        $(function () {
            $('#ContentPlaceHolder1_datetimepicker6').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
            $('#ContentPlaceHolder1_datetimepicker7').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
            $("#ContentPlaceHolder1_datetimepicker6").on("dp.change", function (e) {
                $('#ContentPlaceHolder1_datetimepicker7').data("DateTimePicker").minDate(e.date);
            });
            $("#ContentPlaceHolder1_datetimepicker7").on("dp.change", function (e) {
                $('#ContentPlaceHolder1_datetimepicker6').data("DateTimePicker").maxDate(e.date);
            });
        });

    </script>

</asp:Content>
