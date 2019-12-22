<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="EmanetVerMusteri.aspx.cs" Inherits="TeknikServis.EmanetVerMusteri" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="kaydir">
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
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>

                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="upCrudGrid" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="CustID"
                            EmptyDataText="Kayıt girilmemiş" OnRowCreated="GridView1_OnRowCreated" OnRowCommand="GridView1_RowCommand">
                            <Columns>


                                <asp:TemplateField HeaderText="İşlem" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg ">

                                    <ItemTemplate>


                                        <asp:LinkButton ID="btnEmanetler"
                                            runat="server"
                                            CssClass="btn btn-warning btn-sm"
                                            Text="<i class='fa fa-map'>Emanetler</i>" />
                                        <asp:LinkButton ID="btnEmanetVer"
                                            runat="server"
                                            CssClass="btn btn-success btn-sm"
                                            Text="<i class='fa fa-hand-pointer-o'>Yeni Emanet</i>" />

                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-lg" />
                                    <HeaderStyle CssClass="visible-lg" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="İşlem" HeaderStyle-CssClass="visible-xs visible-sm" ItemStyle-CssClass="visible-xs visible-sm">

                                    <ItemTemplate>


                                        <asp:LinkButton ID="btnEmanetlerK"
                                            runat="server"
                                            CssClass="btn btn-warning"
                                            Text="<i class='fa fa-map'></i>" />
                                        <asp:LinkButton ID="btnEmanetVerK"
                                            runat="server"
                                            CssClass="btn btn-success"
                                            Text="<i class='fa fa-hand-pointer-o'></i>" />

                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-xs visible-sm" />
                                    <HeaderStyle CssClass="visible-xs visible-sm" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="CustID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
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
                                <asp:BoundField DataField="email" HeaderText="E-Posta" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                            </Columns>

                        </asp:GridView>

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
                            <h3 id="myModalLabel">Müşteri Detayları</h3>
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

                                </Triggers>
                            </asp:UpdatePanel>
                            <div class="modal-footer">
                                <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Kapat</button>
                            </div>
                        </div>


                    </div>

                </div>
                <!-- Detail Modal Ends here -->
            </div>
        </div>
    </div>
</asp:Content>
