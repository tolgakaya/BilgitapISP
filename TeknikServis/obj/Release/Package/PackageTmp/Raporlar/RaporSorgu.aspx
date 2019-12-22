<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="RaporSorgu.aspx.cs" Inherits="TeknikServis.Raporlar.RaporSorgu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="kaydir">

        <div class="panel panel-info">
            <div class="panel-heading">
                <h3 id="baslik" runat="server" class="panel-title baslik">Rapor Sorgusu</h3>
            </div>
            <div class="panel-body">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>
                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
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
                        <div id="periyot" runat="server" visible="false" class="row ">
                            <div class="col-sm-12">
                                <div class="form-group">
                                    <label for="txtPeriyot">Periyot (Gün)</label>
                                    <asp:TextBox ID="txtPeriyot" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>

                                </div>
                            </div>

                        </div>
                        <div class="form-group">

                            <asp:Button ID="btnRapor" CssClass="btn btn-info btn-lg btn-block" runat="server" Text="Rapor..." OnClick="btnRapor_Click" />
                        </div>
                    </ContentTemplate>

                </asp:UpdatePanel>

            </div>
            <div class="panel-footer">
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
