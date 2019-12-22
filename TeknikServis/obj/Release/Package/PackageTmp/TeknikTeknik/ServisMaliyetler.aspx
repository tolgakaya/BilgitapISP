<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ServisMaliyetler.aspx.cs" Inherits="TeknikServis.TeknikTeknik.ServisMaliyetler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--  --%>
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
                                <label for="datetimepicker6">Şu Tarihten:</label>
                                <input type='text' runat="server" class="form-control" id="datetimepicker6" />

                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label for="txtSayfalama">Sayfa başına gösterim:</label>
                                <asp:TextBox runat="server" ID="txtSayfalama" CssClass="form-control" TextMode="Number" />
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label for="drdKritik">Tamam/Açık</label>
                                <asp:DropDownList ID="drdKritik" runat="server" class="form-control">
                                    <asp:ListItem Text="Açık Servisler" Value="acik"></asp:ListItem>
                                    <asp:ListItem Text="Tamamlanan Servisler" Value="tamam"></asp:ListItem>
                                    <%--<asp:ListItem Text="Bütün Servisler" Value="hepsi"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-12">
                        </div>

                        <div class="col-sm-6">
                            <asp:Button ID="btnAra" CssClass="btn btn-info btn-block col-sm-5 pull-right" runat="server" Text="Ara..." OnClick="btnAra_Click" />

                        </div>
                        <div class="col-sm-6 pull-right">
                            <asp:Button ID="btnRapor" CssClass="btn btn-info btn-block col-sm-5 pull-left" runat="server" Text="Rapor..." OnClick="btnRapor_Click" />

                        </div>

                    </div>

                    <!--body-->
                </div>
            </div>
        </div>



        <div id="cariOzet" runat="server" class="pull-right">

            <span id="txtHesapAdet" runat="server" class="label label-success"></span>
            <span id="txtHesapMaliyet" runat="server" class="label label-warning"></span>
            <span id="txtHesapYekun" runat="server" class="label label-info"></span>
            <span id="txtHesapFark" runat="server" class="label label-default hidden-xs hidden-sm"></span>

        </div>

        <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" OnItemDataBound="Repeater1_ItemDataBound">
            <HeaderTemplate>
                <div class="panel panel-info">
                    <div class="panel-header">
                        <%--Tamirci Hesabı/burada hesap toplamları gösterilecek--%>
                    </div>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="panel panel-info">
                    <!-- Default panel contents -->
                    <div class="panel-heading">

                        <span runat="server" id="servis_id">ID: <%#Eval("serviceID") %></span>
                        <span runat="server" id="musteri">Müşteri: <%#Eval("musteriAdi") %></span>
                        <span runat="server" id="urun">Cihaz: <%#Eval("urunAdi") %></span>
                        <span runat="server" id="yekun">Tutar: <%#Eval("yekun") %></span>
                        <span runat="server" id="maliyet">Maliyet: <%#Eval("maliyet") %></span>
                        <span runat="server" id="kullanici">Kullanıcı: <%#Eval("kullanici") %></span>
                    </div>
                    <div class="panel-body">
                        <div class="table-responsive">
                            <div id="info" class="col-sm-12">
                                <div class="btn-group  pull-right">

                                    <asp:LinkButton ID="btnServis"
                                        runat="server"
                                        CommandName="gonder"
                                        CommandArgument='<%#Eval("serviceID") +";"+ Eval("kimlikNo") +";"+ Eval("custID") %>'
                                        CssClass="btn btn-info"
                                        Text="<i class='fa fa-wrench'>Servis</i>" />

                                    <asp:LinkButton ID="btnMusteri"
                                        runat="server"
                                        CssClass="btn btn-info"
                                        CommandName="musteri"
                                        CommandArgument='<%#Eval("custID") %>'
                                        Text="<i class='fa fa-user'>Müşteri</i>" />

                                    <asp:LinkButton ID="btnUsta"
                                        runat="server"
                                        CommandName="hobba"
                                        CommandArgument='<%#Eval("usta_id") %>'
                                        CssClass="btn btn-info"
                                        Text='<%#Eval("usta") %>' />

                                </div>
                            </div>

                            <%--  servis hesaplari --%>
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                DataKeyNames="hesapID" DataSource='<%#Eval("hesaplar") %>'
                                EmptyDataText="Servis hesabı bulunmuyor">
                                <PagerStyle CssClass="pagination-ys" />
                                <Columns>

                                    <asp:BoundField DataField="islemParca" HeaderText="İşlem" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                        <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                        <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="cihaz" HeaderText="Ürün/Parça" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
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
                                    <asp:BoundField DataField="disServis" HeaderText="Servis" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
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

            </ItemTemplate>
        </asp:Repeater>
        <div class="btn btn-group btn-block col-md-12">

            <div class="col-md-6 pull-left">
                <asp:LinkButton ID="btnGeri"
                    runat="server"
                    CssClass="btn btn-info btn-block "
                    Text="<i class='fa fa-arrow-left'>Geri</i>" OnClick="btnGeri_Click" />
            </div>
            <div class="col-md-6 pull-right">
                <asp:LinkButton ID="btnIleri"
                    runat="server"
                    CssClass="btn btn-info btn-block"
                    Text="<i class='fa fa-arrow-right '>İleri</i>" OnClick="btnIleri_Click" />
            </div>

        </div>
        <%--<asp:TextBox runat="server" ID="view_" />--%>
    </div>
    <script type="text/javascript">
        $(function () {
            $('#ContentPlaceHolder1_datetimepicker6').datetimepicker({
                format: 'L',

                locale: 'tr'
            });

        });

    </script>
</asp:Content>
