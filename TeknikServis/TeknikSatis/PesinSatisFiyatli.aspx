<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="PesinSatisFiyatli.aspx.cs" Inherits="TeknikServis.TeknikSatis.PesinSatisFiyatli" %>

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

    <div class="row kaydir">
        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
     
           
                   
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo">Ürün/Parça Seçimi</a>
                            </h4>
                        </div>
                        <div id="collapseTwo" class="panel-collapse in" style="height: auto;">
                            <div class="panel-body">

                                <!-- liste  alanı başlıyor -->

                                <div class="table-responsive">

                                    <asp:UpdatePanel ID="upCrudGrid2" runat="server">
                                        <ContentTemplate>
                                              <div class="input-group custom-search-form col-md-12">
                                            <input runat="server" type="text" id="txtCihazAra" class="form-control" placeholder="Ara..." />
                                            <span class="input-group-btn">
                                                <button id="btnCihazAra" runat="server" class="btn btn-default" type="submit" onserverclick="CihazAra">
                                                    <i class="fa fa-search"></i>
                                                </button>
                                            </span>
                                        </div>


                                        <div class="form-horizontal" id="cihaz">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <asp:GridView ID="grdCihaz" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="ID"
                                                        EmptyDataText="Cihaz seç" EnablePersistedSelection="true" OnRowCommand="grdCihaz_RowCommand" OnSelectedIndexChanged="grdCihaz_SelectedIndexChanged">

                                                        <SelectedRowStyle CssClass="danger" />
                                                        <Columns>
                                                                         <asp:TemplateField>
                                                        <ItemTemplate>


                                                            <asp:LinkButton ID="ekleLink"
                                                                runat="server"
                                                                CssClass="btn btn-success btn-xs"
                                                                CommandName="ekle" CommandArgument='<%#Eval("ID")+ ";" + Container.DisplayIndex  %>'  Text="<i class='fa fa-plus'></i>" />


                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                        <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                    </asp:TemplateField>
 
                                                            <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="cihaz_adi" HeaderText="Ürün/Parça/Hizmet" />

                                                              <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                        <HeaderStyle CssClass="visible-lg" />
                                                                        <ItemStyle CssClass="visible-lg" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="bakiye" HeaderText="Stok" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                        <HeaderStyle CssClass="visible-lg" />
                                                                        <ItemStyle CssClass="visible-lg" />
                                                                    </asp:BoundField>
                                                                      <asp:BoundField DataField="satis" HeaderText="Satış Fiyat" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                        <HeaderStyle CssClass="visible-lg" />
                                                                        <ItemStyle CssClass="visible-lg" />
                                                                    </asp:BoundField>

                                                        </Columns>
                                                    </asp:GridView>
                                           
                                                </div>
                                            </div>
                                           <div class="form-group">
                                                <label class="col-sm-2 control-label" for="urun">Ürün</label>
                                                <div class="col-sm-10">
                                                    <div class="col-sm-3">
                                                        <asp:TextBox ID="urun" CausesValidation="true" Enabled="false" runat="server" CssClass="form-control" ValidationGroup="cihazGrup"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hdnCihazID" Value="" />
                                                    </div>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="fiyat" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="cihazGrup"></asp:TextBox>

                                                    </div>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" EnableClientScript="true" ControlToValidate="fiyat" CssClass="text-danger" ErrorMessage="Lütfen vergi dahil tutar giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
<%--                                                    <asp:RangeValidator ErrorMessage="Ondalıklar için virgül kullanınız" ControlToValidate="fiyat" ValidationGroup="cihazGrup" MinimumValue="0" MaximumValue="1000000" Type="Currency" runat="server" />--%>
                                                </div>
                                            </div>
                                           <div class="col-sm-6 pull-right">
                                <asp:Button ID="btnSepete" CssClass="btn btn-success btn-block col-sm-5 pull-left" runat="server" CausesValidation="false" ValidationGroup="cihazGrup" Text="Sepete Ekle" OnClick="btnSepete_Click" />

                                               </div> 

                                        </div>
                                            <div class="panel panel-info">
                                                    <div class="panel-heading">
                                                        <h4 class="panel-title">
                                                            Alışveriş Sepeti
                                                        </h4>
                                                    </div>
                                                <div class="panel-body">
                                               

                                            <asp:GridView ID="grdDetay" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="cihaz_id"
                                                EmptyDataText="Detay bilgileri" EnablePersistedSelection="true" OnRowCommand="grdDetay_RowCommand" OnSelectedIndexChanged="grdDetay_SelectedIndexChanged">

                                                <SelectedRowStyle CssClass="danger" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>


                                                            <asp:LinkButton ID="delLink"
                                                                runat="server"
                                                                CssClass="btn btn-danger btn-xs"
                                                                CommandName="del" CommandArgument='<%#Eval("cihaz_id") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />


                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                        <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="cihaz_id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="cihaz_adi" HeaderText="Ürün/Parça/Malzeme" />
                                                    <asp:BoundField DataField="adet" HeaderText="Adet/Miktar" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                    <asp:BoundField DataField="tutar" HeaderText="Tutar" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                    <asp:BoundField DataField="vergi_toplami" HeaderText="Vergiler" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                    <asp:BoundField DataField="yekun" HeaderText="Yekün" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                   
                                                </Columns>
                                            </asp:GridView>
                                           
                                           <div class="form-horizontal">
                                          
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label" for="toplam_yekun">Yekün</label>
                                                <div class="col-sm-10">
                                                    <div class="col-sm-3">
                                                        <asp:TextBox ID="toplam_yekun" CausesValidation="true" Enabled="false" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>

                                                    </div>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="toplam_yekun2" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>

                                                    </div>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" EnableClientScript="true" ControlToValidate="toplam_yekun" CssClass="text-danger" ErrorMessage="Lütfen vergi dahil tutar giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator ErrorMessage="Ondalıklar için virgül kullanınız" ControlToValidate="toplam_yekun" ValidationGroup="valGrup" MinimumValue="0" MaximumValue="1000000" Type="Currency" runat="server" />
                                                </div>
                                            </div>
                                   
                                           <div class="col-sm-6">    
                                <asp:Button ID="btnAlimKaydet" CssClass="btn btn-primary btn-block col-sm-5 pull-right" runat="server" CausesValidation="true" ValidationGroup="valGrup" Text="Nakit" OnClick="btnAlimKaydet_Click" />
                                           </div>   
                                               <div class="col-sm-6 pull-right">
                                <asp:Button ID="btnKart" CssClass="btn btn-warning btn-block col-sm-5 pull-left" runat="server" CausesValidation="true" ValidationGroup="valGrup" Text="Kredi Kartı" OnClick="btnKart_Click" />

                                               </div>  
                                        </div>
                                                     </div>
                                                </div>
                                             </ContentTemplate>
                                     
                                        
                                    </asp:UpdatePanel>


                                </div>

                                <!-- Liste alanı bitiyor-->
                            </div>


                        </div>
                    </div>

                       <div id="onayModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="addModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-sm">

                <asp:UpdatePanel ID="UpdatePanel2" runat="server">

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">

                                <div class="col-md-12">
                                    <div class="alert alert-info text-center">
                                        <i class="fa fa-2x">Kart Tahsilatı</i>

                                        <asp:DropDownList ID="drdPos" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Pos/Banka seçiniz" Value="-1"></asp:ListItem>
                                        </asp:DropDownList>


                                        <div class="btn-group pull-right">

                                            <asp:Button ID="btnKartKaydet" runat="server" Text="Tamam"
                                                CssClass="btn btn-success" OnClick="btnKartKaydet_Click" />
                                            <button class="btn btn-warning" data-dismiss="modal"
                                                aria-hidden="true">
                                                Kapat</button>

                                        </div>
                                    </div>
                                </div>



                            </div>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnKartKaydet" EventName="Click" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
                    <!-- Add Record Modal Starts here-->
                    <div id="cihazModal" class="modal  fade" tabindex="-1" role="dialog"
                        aria-labelledby="cihazModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-content modal-sm">
                            <div class="modal-header modal-header-info">
                                <button type="button" class="close" data-dismiss="modal"
                                    aria-hidden="true">
                                    ×</button>
                                <h3 id="cihazModalLabel">Yeni Ürün/Parça/Malzeme Tanımla</h3>
                            </div>
                            <asp:UpdatePanel ID="upAdd2" runat="server">
                                <ContentTemplate>
                                    <%--   <script type="text/javascript">
                                        Sys.Application.add_load(jScript);
                                    </script>--%>
                                    <div class="modal-body">
                                        <div class="form-horizontal">

                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="cihaz_adi" CssClass="col-md-10 control-label">Ürün/Parça Tanımı</asp:Label>
                                                <div class="col-md-10">
                                                    <asp:TextBox runat="server" ID="cihaz_adi" ValidationGroup="cihazGrup" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="cihaz_adi" ValidationGroup="cihazGrup" ErrorMessage="Lütfen ürün cinsi giriniz"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="aciklama" CssClass="col-md-10 control-label">Ürün/Parça Açıklama</asp:Label>
                                                <div class="col-md-10">
                                                    <asp:TextBox runat="server" ID="aciklama" ValidationGroup="cihazGrup" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="aciklama" ValidationGroup="cihazGrup" ErrorMessage="Lütfen açıklama giriniz"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="garanti_suresi" CssClass="col-md-10 control-label">Garanti Süresi(Ay)</asp:Label>
                                                <div class="col-md-10">
                                                    <asp:TextBox runat="server" ID="garanti_suresi" ValidationGroup="cihazGrup" TextMode="Number" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="garanti_suresi" ValidationGroup="cihazGrup" ErrorMessage="Lütfen süre giriniz"></asp:RequiredFieldValidator>

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
                               <%-- <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnYeniCihaz" EventName="Click" />
                                </Triggers>--%>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <!--Add Record Modal Ends here-->
                    <!--detay Modal Ends here-->
       

    </div>
   
</asp:Content>
