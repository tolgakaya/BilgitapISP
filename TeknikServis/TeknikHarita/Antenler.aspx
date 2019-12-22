<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Antenler.aspx.cs" Inherits="BilgitapServis.Antenler" %>

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
        <div class="panel panel-danger">
            <!-- Default panel contents -->
            <div class="panel-heading">
                ANTENLER
            </div>
            <%--<div class="panel-body">
               
            </div>--%>
            <div class="input-group custom-search-form">
                <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Ara..." />
                <span class="input-group-btn">
                    <button id="btnARA" runat="server" class="btn btn-default" type="submit" onserverclick="MusteriAra">
                        <i class="fa fa-search"></i>
                    </button>
                </span>
            </div>
            <div class="table-responsive">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>

                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="upCrudGrid" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(jScript);
                        </script>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                            DataKeyNames="anten_id" EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_OnRowCreated">
                            <Columns>


                                <asp:TemplateField HeaderText="İşlem" ShowHeader="False">
                                    <ItemTemplate>
                                        <div class="visible-lg visible-xs visible-sm">
                                            <asp:LinkButton ID="btnTekAnten"
                                                runat="server"
                                                CssClass="btn btn-success"
                                                ToolTip="Antene bağlı olmasada kapsamı içerisindekileri gösterir"
                                                Text="<i class='fa fa-map'>Kapsam</i>" />
                                            <asp:LinkButton ID="btnKayitlilar"
                                                runat="server"
                                                CssClass="btn btn-success"
                                                ToolTip="Antene kayıtlı müşterileri haritada gösterir"
                                                Text="<i class='fa fa-map'>Kullanıcı Haritası</i>" />

                                            <asp:LinkButton ID="btnMusteriler"
                                                runat="server"
                                                CssClass="btn btn-warning"
                                                ToolTip="Antene kayıtlı müşterilerin listesini"
                                                Text="<i class='fa fa-user'>Kullanıcılar</i>" />
                                            <%--<asp:LinkButton ID="btnDuzenle"
                                                runat="server"
                                                CssClass="btn btn-danger"
                                                ToolTip="Antene harita üzerinde değişiklik yapmak için"
                                                Text="<i class='fa fa-pencil'></i>" />--%>
                                            <asp:LinkButton ID="btnDuzenle2"
                                                runat="server"
                                                CssClass="btn btn-info"
                                                ToolTip="(Daha Pratik)Antene harita üzerinde değişiklik yapmak için"
                                                Text="<i class='fa fa-pencil'></i>" />
                                            <asp:LinkButton ID="delLink"
                                                runat="server"
                                                CssClass="btn btn-danger"
                                                ToolTip="Anteni silmek için"
                                                CommandName="del" CommandArgument='<%#Eval("anten_id") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />

                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="anten_id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="anten_adi" HeaderText="Anten" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="center_Lat" HeaderText="Merkez Lat" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="center_Long" HeaderText="Merkez Long" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="start_Lat" HeaderText="B Ucu Lat" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun">
                                    <HeaderStyle CssClass="gizlisutun" />
                                    <ItemStyle CssClass="gizlisutun" />
                                </asp:BoundField>
                                <asp:BoundField DataField="start_Long" HeaderText="B Ucu Long" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun">
                                    <HeaderStyle CssClass="gizlisutun" />
                                    <ItemStyle CssClass="gizlisutun" />
                                </asp:BoundField>
                                <asp:BoundField DataField="end_Long" HeaderText="C Ucu Lat" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun">
                                    <HeaderStyle CssClass="gizlisutun" />
                                    <ItemStyle CssClass="gizlisutun" />
                                </asp:BoundField>
                                <asp:BoundField DataField="end_Lat" HeaderText="C Ucu Long" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun">
                                    <HeaderStyle CssClass="gizlisutun" />
                                    <ItemStyle CssClass="gizlisutun" />
                                </asp:BoundField>
                            </Columns>

                        </asp:GridView>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnARA" EventName="ServerClick" />
                        <asp:PostBackTrigger ControlID="GridView1" />
                    </Triggers>
                </asp:UpdatePanel>

            </div>
            <div class="panel-footer pull-right">
                <div class=" btn-group">

                    <asp:LinkButton ID="btnHaritadaGoster"
                        runat="server"
                        CssClass="btn btn-info "
                        ToolTip="Antenleri müşterisiz haritada gösterir"
                        OnClick="btnHaritadaGoster_Click"
                        Text="<i class='fa fa-cog  icon-2x'>Bütün Antenler</i>" />
                    <asp:LinkButton ID="btnButunAntenMusteriKapsam"
                        runat="server"
                        CssClass="btn btn-info "
                        ToolTip="Antenleri kendisine bağlı olsun yada olmasın kapsamı alanı içerindeki müşterilerle gösterir"
                        OnClick="btnButunAntenMusteriKapsam_Click"
                        Text="<i class='fa fa-cog  icon-2x'>Anten Kapsam/Kullanıcı</i>" />
                    <asp:LinkButton ID="btnButunAntenMusteriKayitli"
                        runat="server"
                        CssClass="btn btn-info "
                        ToolTip="Bütün Antenleri bağlı müşterilerle gösterir"
                        OnClick="btnButunAntenMusteriKayitli_Click"
                        Text="<i class='fa fa-cog icon-2x'>Anten Bağlı/Kullanıcı</i>" />

                   <%-- <asp:Button ID="btnAdd" runat="server" Text="Yeni" CssClass="btn btn-danger"
                        OnClick="btnAdd_Click" />--%>
                    <asp:Button ID="btnAdd2" runat="server" Text="Yeni" CssClass="btn btn-info"
                        OnClick="btnAdd2_Click" />
                    <asp:LinkButton ID="btnPrint"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnPrnt_Click"
                        Text="<i class='fa fa-print icon-2x'></i>" />


                    <asp:LinkButton ID="btnExportExcel"
                        runat="server"
                        CssClass="btn btn-warning " OnClick="btnExportExcel_Click"
                        Text="<i class='fa fa-file-excel-o icon-2x'></i>" />



                    <asp:LinkButton ID="btnExportWord"
                        runat="server"
                        CssClass="btn btn-primary " OnClick="btnExportWord_Click"
                        Text="<i class='fa fa-wikipedia-w icon-2x'></i>" />


                </div>

            </div>
        </div>
    </div>
</asp:Content>
