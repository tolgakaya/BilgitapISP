<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CihazGruplar.aspx.cs" Inherits="TeknikServis.TeknikAlim.CihazGruplar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="kaydir">


        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                Ürün ve Yedek Parça Stokları

            </div>
            <%--<div class="panel-body">
               
            </div>--%>
            <div class="table-responsive ">
                <div class="input-group custom-search-form">
                    <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Ara..." />
                    <span class="input-group-btn">
                        <button id="btnARA" runat="server" class="btn btn-default" type="submit" onserverclick="CihazAra">
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
                        <asp:GridView ID="grdAlimlar" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="grupid"
                            EmptyDataText="Ürün grubu tanımlanmamış" OnRowCommand="grdAlimlar_RowCommand"
                            AllowPaging="true" PageSize="10" OnPageIndexChanging="grdAlimlar_PageIndexChanging" OnRowCreated="grdAlimlar_RowCreated" OnRowDataBound="grdAlimlar_RowDataBound">

                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnGuncelle"
                                            runat="server"
                                            CssClass="btn btn-danger"
                                            CommandName="guncelle" CommandArgument='<%#Eval("grupid")+ ";" + Container.DisplayIndex  %>' Text="<i class='fa fa-pencil'></i>" />

                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="grupid" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                <asp:BoundField DataField="grupadi" HeaderText="Grup Adı"></asp:BoundField>
                                <asp:BoundField DataField="kdv" HeaderText="KDV %"></asp:BoundField>
                                <asp:BoundField DataField="otv" HeaderText="ÖTV %" />
                                <asp:BoundField DataField="oiv" HeaderText="ÖİV %" />



                            </Columns>

                        </asp:GridView>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAra" EventName="ServerClick" />
                    </Triggers>
                </asp:UpdatePanel>


            </div>
            <div class="panel-footer pull-right">
                <div class=" btn-group">

                    <asp:LinkButton ID="btnYeni"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnYeni_Click"
                        Text="Yeni" />
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
        <!-- Add Record Modal Starts here-->
        <div id="cihazModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="cihazModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">
                <div class="modal-header modal-header-info">
                    <button type="button" class="close" data-dismiss="modal"
                        aria-hidden="true">
                        ×</button>
                    <h3 id="cihazModalLabel" class="baslik">Yeni Cihaz/Malzeme Grubu</h3>
                </div>
                <asp:UpdatePanel ID="upAdd2" runat="server">
                    <ContentTemplate>
                        <%--   <script type="text/javascript">
                                        Sys.Application.add_load(jScript);
                                    </script>--%>
                        <div class="modal-body">
                            <div class="form-horizontal">


                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="grupadi" CssClass="col-md-2 control-label">Grup İsmi</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="grupadi" ValidationGroup="cihazGrup" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="grupadi" ValidationGroup="cihazGrup" ErrorMessage="Lütfen grup adı giriniz"></asp:RequiredFieldValidator>

                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="kdv" CssClass="col-md-2 control-label">Kdv Oranı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="kdv" ValidationGroup="cihazGrup" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="kdv" ValidationGroup="cihazGrup" ErrorMessage="Lütfen kdv oranı giriniz"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="kdv" runat="server" Type="Currency" ValidationGroup="cihazGrup" MinimumValue="0" MaximumValue="100" />

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="otv" CssClass="col-md-2 control-label">Ötv Oranı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="otv" ValidationGroup="cihazGrup" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="otv" Type="Currency" ValidationGroup="cihazGrup" ErrorMessage="Lütfen ötv oranı giriniz"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="otv" runat="server" Type="Currency" ValidationGroup="cihazGrup" MinimumValue="0" MaximumValue="100" />

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="oiv" CssClass="col-md-2 control-label">Öiv Oranı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="oiv" ValidationGroup="cihazGrup" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="oiv" ValidationGroup="cihazGrup" ErrorMessage="Lütfen öiv oranı giriniz"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="oiv" runat="server" Type="Currency" ValidationGroup="cihazGrup" MinimumValue="0" MaximumValue="100" />
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnCihazKaydet" runat="server" Text="Kaydet"
                                CssClass="btn btn-info" OnClick="btnCihazKaydet_Click" ValidationGroup="cihazGrup" />
                            <button class="btn btn-info" data-dismiss="modal"
                                aria-hidden="true">
                                Kapat</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnYeni" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <div id="updateModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="updateModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">
                <div class="modal-header modal-header-info">
                    <button type="button" class="close" data-dismiss="modal"
                        aria-hidden="true">
                        ×</button>
                    <h3 id="updateModalLabel" class="baslik">Cihaz/Malzeme Grubu</h3>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>

                        <div class="modal-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="hdnGrupID" runat="server" />

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="grupadid" CssClass="col-md-2 control-label">Grup İsmi</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="grupadid" ValidationGroup="cihazGrup" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="grupadid" ValidationGroup="cihazGrupd" ErrorMessage="Lütfen grup adı giriniz"></asp:RequiredFieldValidator>

                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="kdvd" CssClass="col-md-2 control-label">Kdv Oranı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="kdvd" ValidationGroup="cihazGrup" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="kdvd" ValidationGroup="cihazGrupd" ErrorMessage="Lütfen kdv oranı giriniz"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="kdvd" Type="Currency" runat="server" ValidationGroup="cihazGrupd" MinimumValue="0" MaximumValue="100" />

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="otvd" CssClass="col-md-2 control-label">Ötv Oranı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="otvd" ValidationGroup="cihazGrup" TextMode="Number" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="otvd" ValidationGroup="cihazGrupd" ErrorMessage="Lütfen ötv oranı giriniz"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="otvd" runat="server" Type="Currency" ValidationGroup="cihazGrupd" MinimumValue="0" MaximumValue="100" />

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="oivd" CssClass="col-md-2 control-label">Öiv Oranı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="oivd" ValidationGroup="cihazGrup" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="oivd" ValidationGroup="cihazGrupd" ErrorMessage="Lütfen öiv oranı giriniz"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="oivd" Type="Currency" runat="server" ValidationGroup="cihazGrupd" MinimumValue="0" MaximumValue="100" />

                                    </div>
                                </div>


                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnCihazUpdate" runat="server" Text="Kaydet"
                                CssClass="btn btn-info" OnClick="btnCihazUpdate_Click" CausesValidation="true" ValidationGroup="cihazGrupd" />
                            <button class="btn btn-info" data-dismiss="modal"
                                aria-hidden="true">
                                Kapat</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdAlimlar" EventName="RowCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

    </div>


</asp:Content>
