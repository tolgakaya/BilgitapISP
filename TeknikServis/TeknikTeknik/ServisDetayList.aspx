<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="ServisDetayList.aspx.cs" Inherits="TeknikServis.ServisDetayList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--   <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap-theme.css" rel="stylesheet" />--%>
    <div class="kaydir">
        <div id="panelContents" runat="server" class="panel panel-info">
            <div class="panel-heading">
                <h2 class="panel-title">
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" class="collapsed">Servis Bilgileri</a>

                </h2>

            </div>
            <div id="collapseOne" class="panel-collapse collapse" style="height: 0px;">
                <div class="panel-body">

                    <div class="btn-group visible-lg pull-right">
                        <asp:Button ID="btnEkle" runat="server" CssClass="btn btn-info" Text="Detay" OnClick="btnEkle_Click" />
                        <asp:Button ID="btnHesaplar" runat="server" CssClass="btn btn-info" Text="Kararlar" OnClick="btnHesaplar_Click" />
                        <asp:Button ID="btnServis" runat="server" CssClass="btn btn-info" Text="Servis" OnClick="btnServis_Click" />
                        <asp:Button ID="btnSonlandir" runat="server" CssClass="btn btn-info" Text="Sonladır" OnClick="btnSonlandir_Click" />
                        <asp:Button ID="btnYol" runat="server" CssClass="btn btn-info" Text="Pusula" OnClick="btnYol_Click" />
                        <asp:Button ID="btnBelge" runat="server" CssClass="btn btn-info" Text="Belge" OnClick="btnBelge_Click" />
                    </div>

                    <div class="btn-group visible-sm visible-xs pull-right">
                        <asp:LinkButton ID="btnEkleK"
                            runat="server"
                            CssClass="btn btn-info " OnClick="btnEkle_Click"
                            Text="<i class='fa fa-cog icon-2x'></i>" />

                        <asp:LinkButton ID="btnHesaplarK"
                            runat="server"
                            CssClass="btn btn-info " OnClick="btnHesaplar_Click"
                            Text="<i class='fa fa-calendar icon-2x'></i>" />
                           <asp:LinkButton ID="btnServisK"
                            runat="server"
                            CssClass="btn btn-danger " OnClick="btnServis_Click"
                            Text="<i class='fa fa-wrench icon-2x'></i>" />
                        <asp:LinkButton ID="btnSonlandirK"
                            runat="server"
                            CssClass="btn btn-info " OnClick="btnSonlandir_Click"
                            Text="<i class='fa fa-hourglass-end icon-2x'></i>" />
                        <asp:LinkButton ID="btnYolK"
                            runat="server"
                            CssClass="btn btn-info " OnClick="btnYol_Click"
                            Text="<i class='fa fa-car icon-2x'></i>" />

                    </div>

                    <h3>
                        <label id="txtMusteri" runat="server" class="label label-danger"></label>
                    </h3>
                    <h3 id="txtKonu" runat="server"></h3>


                    <p id="txtServisAciklama" runat="server" class="lead"></p>

                    <p id="txtServisAdresi" runat="server" class="lead"></p>

                    <input class="form-control alert-info" id="txtKimlikNo" runat="server" type="text" disabled="disabled" />

                    <%-- <input class="form-control alert-danger" id="txtMusteri" runat="server" type="text" />--%>
                    <%-- <input class="form-control alert-success" id="txtKonu" runat="server" />--%>

                    <%--                    <asp:TextBox ID="txtServisAciklama" runat="server" TextMode="MultiLine" Wrap="true" CssClass="form-control alert-info"></asp:TextBox>--%>


                    <input class="form-control alert-warning" id="txtTarih" runat="server" type="datetime" />
                    <input class="form-control alert-success" id="txtDurum" runat="server" />

                    <input type="hidden" name="txtServisID" value="" runat="server" id="txtServisID" />
                    <input type="hidden" name="hdnDurumID" value="" runat="server" id="hdnDurumID" />
                    <input type="hidden" name="txtAtanan" value="" runat="server" id="hdnAtananID" />
                    <input type="hidden" name="txtCust" value="" runat="server" id="hdnCustID" />
                    <%-- <input type="hidden" name="txtCape" value="" runat="server" id="hdnCape" />--%>
                </div>
            </div>
        </div>
    </div>
    <div id="onayModal" class="modal  fade" tabindex="-1" role="dialog"
        aria-labelledby="addModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-content modal-sm">

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">

                <ContentTemplate>
                    <div class="modal-body">
                        <div class="row">

                            <div class="col-md-12">
                                <div class="alert alert-info text-center">
                                    <i class="fa fa-2x">Servis sonlandırmak istiyor musunuz?</i>
                                    <div class="checkbox-inline">

                                        <asp:CheckBox ID="chcSms" Text="SMS" runat="server" />
                                    </div>
                                    <div class="checkbox-inline">

                                        <asp:CheckBox ID="chcMail" Text="Mail" runat="server" />
                                    </div>

                                    <div class="btn-group pull-right">

                                        <asp:Button ID="btnOnay" runat="server" Text="Tamam"
                                            CssClass="btn btn-success" OnClick="btnOnay_Click" />
                                        <button class="btn btn-warning" data-dismiss="modal"
                                            aria-hidden="true">
                                            Kapat</button>

                                    </div>
                                </div>
                            </div>

                            <asp:HiddenField ID="hdnHesapID" runat="server" />
                            <asp:HiddenField ID="hdnMusteriID" runat="server" />
                            <asp:HiddenField ID="hdnServisIDD" runat="server" />

                        </div>
                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSonlandir" EventName="Click" />

                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="pager" runat="server">
        <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="5">

            <Fields>
                <asp:NextPreviousPagerField ShowLastPageButton="False" ShowNextPageButton="False" ButtonType="Button" ButtonCssClass="btn btn-danger" RenderNonBreakingSpacesBetweenControls="false" />
                <asp:NumericPagerField ButtonType="Button" RenderNonBreakingSpacesBetweenControls="false" NumericButtonCssClass="btn btn-primary" CurrentPageLabelCssClass="btn btn-primary disabled" NextPreviousButtonCssClass="btn" />
                <asp:NextPreviousPagerField ShowFirstPageButton="False" ShowPreviousPageButton="False" ButtonType="Button" ButtonCssClass="btn btn-danger" RenderNonBreakingSpacesBetweenControls="false" />
            </Fields>
        </asp:DataPager>

    </div>
    <asp:ListView ID="ListView1" runat="server" OnItemDataBound="ListView1_ItemDataBound" OnDataBound="ListView1_DataBound" OnPagePropertiesChanging="ListView1_PagePropertiesChanging">
        <ItemTemplate>
            <div class="row">
                <div id="Div1" runat="server" class="panel panel-info">
                    <div class="panel-heading">
                        <h4 class="panel-title"><%#DataBinder.Eval(Container.DataItem,"tarihZaman")%> - <%#DataBinder.Eval(Container.DataItem,"kullanici")%>
                        </h4>
                    </div>
                    <div class="panel-body">

                        <h3><span class="label label-danger "><%#DataBinder.Eval(Container.DataItem,"durumAdi")%></span></h3>

                        <h3><%#DataBinder.Eval(Container.DataItem,"baslik")%></h3>


                        <p class="lead"><%#DataBinder.Eval(Container.DataItem,"aciklama")%> </p>


                        <div runat="server" id="resimCerceve">

                            <img id="resHTML" class="img-responsive img-rounded" runat="server" src='<%# Eval("belgeYol") %>' />

                            <asp:TextBox ID="txtYol" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"belgeYol")%>'></asp:TextBox>

                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>

    </asp:ListView>





</asp:Content>
