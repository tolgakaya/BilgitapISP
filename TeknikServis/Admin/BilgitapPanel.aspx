<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="BilgitapPanel.aspx.cs" Inherits="TeknikServis.Admin.BilgitapPanel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="kaydir">
        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                BİLGİTAP PANEL
            </div>
            <%--<div class="panel-body">
               
            </div>--%>

            <div class="row">
                <!-- Welcome -->
                <div class="col-lg-12">
                    <div class="alert alert-info">
                        <%-- KULLANİCİ BAŞLIYOR --%>
                        <asp:LoginView runat="server" ViewStateMode="Disabled">
                            <AnonymousTemplate>
                            </AnonymousTemplate>
                            <LoggedInTemplate>
                                <i class="fa fa-folder-open"></i><b>&nbsp;Merhaba ! </b><b><a runat="server" href="~/Account/Manage" title="Manage your account"><%: Context.User.Identity.GetUserName()  %> !</a> </b>
                                <br />
                            </LoggedInTemplate>
                        </asp:LoginView>

                        <%-- KULLANICI BİTİYOR --%>
                    </div>
                </div>
                <!--end  Welcome -->
            </div>

            <div class="row">
                <div class="col-lg-4">
                    <div class="panel panel-primary text-center no-boder">
                        <div class="panel-body yellow">
                            <asp:LinkButton ID="btnYaklasan"
                                runat="server"
                                CssClass="btn btn-success" OnClick="btnYaklasan_Click"
                                Text="<i class='fa fa-line-chart fa-3x'></i>" />

                            <h3 id="yaklasanOdeme" class="baslik" runat="server"></h3>
                        </div>
                        <div class="panel-footer">
                            <span class="panel-eyecandy-title">Yaklaşan/Günü Geçen Ödeme
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="panel panel-primary text-center no-boder">
                        <div class="panel-body green">

                            <asp:LinkButton ID="btnKrediYaklas"
                                runat="server"
                                CssClass="btn btn-danger" OnClick="btnKrediYaklas_Click"
                                Text="<i class='fa fa-bell-slash-o fa-3x'></i>" />
                            <h3 id="yaklasanKredi" runat="server"></h3>
                        </div>
                        <div class="panel-footer">
                            <span class="panel-eyecandy-title">Yaklaşan Kredi Yenileme
                            </span>
                        </div>
                    </div>
                </div>

                <div class="col-lg-4">
                    <div class="panel panel-primary text-center no-boder">
                        <div class="panel-body blue">
                            <asp:LinkButton ID="btnMusteriler"
                                runat="server"
                                CssClass="btn btn-success" OnClick="btnMusteriler_Click"
                                Text="<i class='fa fa-user fa-3x'></i>" />

                            <h3 id="musteriler" class="baslik" runat="server"></h3>

                        </div>
                        <div class="panel-footer">
                            <span class="panel-eyecandy-title">Müşteri Sayısı
                            </span>
                        </div>
                    </div>
                </div>

            </div>

            <div class="row">
                <div class="col-lg-4">
                    <div class="panel panel-primary text-center no-boder">
                        <div class="panel-body yellow">
                            <asp:LinkButton ID="btnKasa"
                                runat="server"
                                CssClass="btn btn-success" OnClick="btnKasa_Click"
                                Text="<i class='fa fa-money fa-3x'></i>" />

                            <h3 id="kasa" class="baslik" runat="server"></h3>

                        </div>
                        <div class="panel-footer">
                            <span class="panel-eyecandy-title">Nakit Kasası
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="panel panel-primary text-center no-boder">
                        <div class="panel-body green">

                            <asp:LinkButton ID="btnBanka"
                                runat="server"
                                CssClass="btn btn-danger" OnClick="btnBanka_Click"
                                Text="<i class='fa fa-university fa-3x'></i>" />
                            <h3 id="bankahesap" runat="server"></h3>
                        </div>
                        <div class="panel-footer">
                            <span class="panel-eyecandy-title">Banka Hesapları
                            </span>
                        </div>
                    </div>
                </div>

                <div class="col-lg-4">
                    <div class="panel panel-primary text-center no-boder">
                        <div class="panel-body blue">
                            <asp:LinkButton ID="btnPos"
                                runat="server"
                                CssClass="btn btn-success" OnClick="btnPos_Click"
                                Text="<i class='fa fa-cc-mastercard fa-3x'></i>" />

                            <h3 id="poshesaplari" class="baslik" runat="server"></h3>

                        </div>
                        <div class="panel-footer">
                            <span class="panel-eyecandy-title">Pos Hesapları
                            </span>
                        </div>
                    </div>
                </div>

            </div>

            <div class="row">
                <div class="col-lg-4">
                    <div class="panel panel-primary text-center no-boder">
                        <div class="panel-body yellow">
                            <i class="fa fa-bar-chart-o fa-3x"></i>
                            <h3 id="netBakiye" runat="server"></h3>

                        </div>
                        <div class="panel-footer">
                            <span runat="server" id="Span1" class="panel-eyecandy-title">Bakiye
                            </span>

                        </div>
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="panel panel-primary text-center no-boder">
                        <div class="panel-body green">
                            <i class="fa fa-pencil-square-o fa-3x"></i>
                            <h3 id="netAlacak" runat="server"></h3>
                        </div>
                        <div class="panel-footer">
                            <span class="panel-eyecandy-title">Net Alacak
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="panel panel-primary text-center no-boder">
                        <div class="panel-body blue">
                            <i class="fa fa fa-floppy-o fa-3x"></i>
                            <h3 id="netBorc" runat="server"></h3>
                        </div>
                        <div class="panel-footer">
                            <span class="panel-eyecandy-title">Net Borç
                            </span>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <!--quick info section -->
                <div class="col-lg-4">
                    <div class="alert alert-info text-center">
                        <i class="fa fa-calendar fa-3x" id="servisSayisi" runat="server"></i>&nbsp;Aktif Servis
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="alert alert-success text-center">
                        <i class="fa  fa-thumbs-up fa-3x" id="onayBekleyen" runat="server"></i>&nbsp;Onay Bekleniyor  
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="alert alert-info text-center">
                        <i class="fa fa-rss fa-3x" id="emanetSayisi" runat="server"></i>&nbsp; Emanet Sayısı
                    </div>
                </div>

                <!--end quick info section -->
            </div>



        </div>
    </div>

</asp:Content>
