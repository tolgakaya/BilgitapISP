<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ServisPaketleri.aspx.cs" Inherits="TeknikServis.TeknikTeknik.ServisPaketleri" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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


    <div class="kaydir">
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
                SERVİS PAKETLERİ
            </div>
            <%--<div class="panel-body">
               
            </div>--%>
            <div class="table-responsive">

                <asp:UpdatePanel ID="upCrudGrid" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>

                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                            DataKeyNames="paket_id"
                            EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_RowCreated" 
                            OnPageIndexChanging="GridView1_PageIndexChanging" AllowPaging="true" PageSize="10">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>

                                <asp:TemplateField HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <ItemTemplate>


                                        <asp:LinkButton ID="delLink"
                                            runat="server"
                                            CssClass="btn btn-danger btn-xs"
                                            CommandName="del" CommandArgument='<%#Eval("paket_id") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                        <asp:LinkButton ID="btnDetay"
                                            runat="server"
                                            CssClass="btn btn-primary btn-xs"
                                            Text="<i class='fa fa-pencil'></i>" />

                                    </ItemTemplate>
                              
                                </asp:TemplateField>


                                <asp:BoundField DataField="paket_id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="paket_adi" HeaderText="Paket Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="tutar" HeaderText="Tutar" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                              
                            </Columns>

                        </asp:GridView>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
                        <%-- <asp:AsyncPostBackTrigger ControlID="btnPaket" EventName="Click" />--%>
                    </Triggers>
                </asp:UpdatePanel>

            </div>

            <div class="panel-footer pull-right">
                <div class=" btn-group">


                    <asp:Button ID="btnEkle" runat="server" Text="Yeni" CssClass="btn btn-info"
                        OnClick="btnEkle_Click" />


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
    </div>
</asp:Content>
