<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="ServisDetay.aspx.cs" Inherits="TeknikServis.ServisDetay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--  <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap-theme.css" rel="stylesheet" />
    <link href="Content/font-awesome.css" rel="stylesheet" />--%>
    <div class="kaydir">
        <div id="Div1" runat="server" class="panel panel-info">
            <div class="panel-heading">
                <h4 runat="server" id="baslikDetay" class="panel-title">Detay Bilgileri

                </h4>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">Başlık-Konu</label>
                        <div class="col-sm-10">
                            <asp:TextBox ID="txtBaslik" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <asp:HiddenField   runat="server" ID="hdnKul" />
                    <div class="form-group">
                        <label class="col-sm-2 control-label">Açıklama</label>
                        <div class="col-sm-10">
                            <asp:TextBox ID="txtServisAciklama" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-2 control-label" for="drdDurum">Servis Durumu</label>
                        <div class="col-sm-10">
                            <asp:DropDownList ID="drdDurum" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div id="kullaniciSecim" visible="false" runat="server" class="form-group">
                        <label class="col-sm-2 control-label">Görevli</label>
                        <div class="col-sm-10">
                            <asp:DropDownList ID="drdKullanici" runat="server" CssClass="form-control">
                                <asp:ListItem Text="HERKESE AÇIK" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-2 control-label">Mesaj</label>
                        <div class="col-md-10">
                            <div class="checkbox-inline">
                                <label>
                                    <input id="chcSms" type="checkbox" />SMS</label>
                            </div>
                            <div class="checkbox-inline">
                                <label>
                                    <input id="chcMail" type="checkbox" />Mail</label>
                            </div>

                        </div>
                    </div>
                    <div id="Div2" runat="server" class="form-group">
                        <label class="col-sm-2 control-label">Foto</label>
                        <div class="col-sm-10 pull-right">
                            <input id="chcResim" type="checkbox" disabled="disabled" />
                            <span id="buttonStart" class="fa-stack fa-lg" onclick="start()">
                                <i class="fa fa-camera-retro fa-2x"></i>
                            </span>

                            <span id="buttonStop" class="fa-stack fa-lg" onclick="stop()">
                                <i class="fa fa-camera fa-stack-1x"></i>
                                <i class="fa fa-ban fa-stack-2x text-danger"></i>
                            </span>
                            <span id="buttonSnap" class="fa-stack fa-lg" onclick="snapshot()">
                                <i class="fa fa-cog fa-spin fa-2x"></i>
                            </span>

                        </div>

                    </div>


                    <div id="divCam" style="display: none" class="form-group">
                        <label class="col-sm-2 control-label">-</label>
                        <div class="col-sm-10">

                            <video id="video" class="img-responsive" autoplay="autoplay"></video>

                        </div>
                    </div>
                    <div id="divResim" style="display: none" class="form-group">
                        <label class="col-sm-2 control-label">-</label>
                        <div class="col-sm-10">


                            <canvas id="canvas" class="img-responsive"></canvas>
                        </div>
                    </div>
                    <style>
                        #ContentPlaceHolder1_islemGoster {
                            padding: 10px;
                            position: fixed;
                            top: 45%;
                            left: 50%;
                            display: none;
                        }
                    </style>
                    <img id="islemGoster" runat="server" src="~/img/ajax_loader_blue_64.gif" />

                    <div id="Div3" class="form-group">




                        <input id="Button1" type="button" class="btn btn-info btn-lg btn-block" value="Kaydet" onclick="resimKontrollu()" />
                    </div>


                    <input type="hidden" id="kullanici" runat="server" name="kullanici" value="" />
                    <input type="hidden" id="firma" runat="server" name="firma" value="" />
                    <input type="hidden" id="musteri" runat="server" name="musteri" value="" />

                </div>
            </div>

        </div>
    </div>
    <script type="text/javascript">
        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }
        function resimKontrollu() {
            var remember = document.getElementById('chcResim');
            if (remember.checked) {
                //alert("resimliymiş");
                var resimli = true;
                UploadPic(resimli);
            } else {
                //alert("resimsizmiş");
                var resimsiz = false;
                UploadPic(resimsiz);
            }
        }
        function UploadPic(ress) {

            // Generate the image data
            var Pic = document.getElementById("canvas").toDataURL("image/png");
            Pic = Pic.replace('data:image/png;base64,', '');
            var aciklama = $("#<%=txtServisAciklama.ClientID%>")[0].value;
            var kullanici = $("#<%=hdnKul.ClientID%>")[0].value;
            var baslik = $("#<%=txtBaslik.ClientID%>")[0].value;
            var gorevli = $('#<%= drdKullanici.ClientID %>').val();
            var durum = $('#<%= drdDurum.ClientID %>').val();
            var servis = getParameterByName('id');
            var kimlik = getParameterByName('kimlik');
            var eski_durum = getParameterByName('durum');

            // Sending the image data to ServerSS
            //var dataParam = '{' + '"imageData": "' + Pic + '", "durum": "' + durum + '", "ress": "' + ress + '", "servis": "' + servis + '", "aciklama": "' + aciklama + '", "baslik": "' + baslik + '"' + '}';
            var dataParam = '{' + '"imageData": "' + Pic + '", "durum": "' + durum + '", "ress": "' + ress + '", "servis": "' + servis + '", "aciklama": "' + aciklama + '", "baslik": "' + baslik + '", "kullanici": "' + kullanici + '"' + '}';

            if (baslik || aciklama) {

                $.ajax({
                    type: 'POST',
                    url: '<%= ResolveUrl("/TeknikTeknik/ResimKaydet.aspx/UploadPic") %>',
                    data: dataParam,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',

                    error: function (xhr) {
                        var readyState = {
                            1: "Loading",
                            2: "Loaded",
                            3: "Interactive",
                            4: "Complete"
                        };
                        if (xhr.readyState !== 0 && xhr.status !== 0 && xhr.responseText !== undefined) {

                            var url2 = "/Default.aspx?readyState=" + readyState[xhr.readyState] + "&status=" + xhr.status + "&responseText=" + xhr.responseText;
                            window.location.replace(url2);

                        }
                    },
                    beforeSend: function () {
                        $('#ContentPlaceHolder1_islemGoster').css("display", "block");
                    },
                    complete: function () {
                        $('#ContentPlaceHolder1_islemGoster').css("display", "none");
                    },
                    success: function (msg) {
                        //alert('Kayıt tamamlandı !');
                        //burada query string ile detaylistesine gönderme yapılacak 
                        var sms = "1";
                        var smsC = document.getElementById('chcSms');
                        if (smsC.checked) {

                            sms = "1";
                        } else {

                            sms = "0";

                        }

                        var mail = "1";
                        var mailC = document.getElementById('chcMail');
                        if (mailC.checked) {

                            mail = "1";
                        } else {

                            mail = "0";

                        }
                        var url = "/TeknikTeknik/ServisDetayList.aspx?durum=" + durum + "&servisid=" + servis + "&eski_durum=" + eski_durum + "&kimlik=" + kimlik + "&sms=" + sms + "&mail=" + mail + "&eleman=" + gorevli;
                        window.location.replace(url);
                    }
                });
            }
            else {
                alertify.error('Lütfen konu ve açıklama giriniz')
            }
        }
    </script>
    <%--<h2 id="demo">Demo</h2>
    <pre id="preLog">Loading…</pre>--%>



    <script type="text/javascript">
        //<![CDATA[
        "use strict";
        var video = document.getElementById('video');
        var canvas = document.getElementById('canvas');
        var videoStream = null;
        var preLog = document.getElementById('preLog');

        function log(text) {
            //if (preLog) preLog.textContent += ('\n' + text);
            //else alert(text);
        }

        //buraya resim çekildiğini ifade eden bir checkbox koyacağız
        function snapshot() {
            document.getElementById('divResim').style.display = "block";
            canvas.width = video.videoWidth;
            canvas.height = video.videoHeight;
            canvas.getContext('2d').drawImage(video, 0, 0);
            var remember = document.getElementById('chcResim');
            remember.checked = true;
            window.scrollTo(0, document.body.scrollHeight);
        }

        function noStream() {
            log('Access to camera was denied!');
        }

        function stop() {
            document.getElementById('divCam').style.display = "none";
            document.getElementById('divResim').style.display = "none";
            var remember = document.getElementById('chcResim');
            remember.checked = false;
            var myButton = document.getElementById('buttonStop');
            if (myButton) myButton.disabled = true;
            myButton = document.getElementById('buttonSnap');
            if (myButton) myButton.disabled = true;
            if (videoStream) {
                if (videoStream.stop) videoStream.stop();
                else if (videoStream.msStop) videoStream.msStop();
                videoStream.onended = null;
                videoStream = null;
            }
            if (video) {
                video.onerror = null;
                video.pause();
                if (video.mozSrcObject)
                    video.mozSrcObject = null;
                video.src = "";
            }
            myButton = document.getElementById('buttonStart');
            if (myButton) myButton.disabled = false;
        }

        function gotStream(stream) {
            var myButton = document.getElementById('buttonStart');
            if (myButton) myButton.disabled = true;
            videoStream = stream;
            log('Got stream.');
            video.onerror = function () {
                log('video.onerror');
                if (video) stop();
            };
            stream.onended = noStream;
            if (window.webkitURL) video.src = window.webkitURL.createObjectURL(stream);
            else if (video.mozSrcObject !== undefined) {//FF18a
                video.mozSrcObject = stream;
                video.play();
            }
            else if (navigator.mozGetUserMedia) {//FF16a, 17a
                video.src = stream;
                video.play();
            }
            else if (window.URL) video.src = window.URL.createObjectURL(stream);
            else video.src = stream;
            myButton = document.getElementById('buttonSnap');
            if (myButton) myButton.disabled = false;
            myButton = document.getElementById('buttonStop');
            if (myButton) myButton.disabled = false;
        }

        function start() {
            document.getElementById('divCam').style.display = "block";
            document.getElementById('divResim').style.display = "block";
            if ((typeof window === 'undefined') || (typeof navigator === 'undefined')) log('This page needs a Web browser with the objects window.* and navigator.*!');
            else if (!(video && canvas)) log('HTML context error!');
            else {
                log('Get user media…');
                if (navigator.getUserMedia) navigator.getUserMedia({ video: true }, gotStream, noStream);
                else if (navigator.oGetUserMedia) navigator.oGetUserMedia({ video: true }, gotStream, noStream);
                else if (navigator.mozGetUserMedia) navigator.mozGetUserMedia({ video: true }, gotStream, noStream);
                else if (navigator.webkitGetUserMedia) navigator.webkitGetUserMedia({ video: true }, gotStream, noStream);
                else if (navigator.msGetUserMedia) navigator.msGetUserMedia({ video: true, audio: false }, gotStream, noStream);
                else log('getUserMedia() not available from your Web browser!');
            }
        }

        //start();
        //]]></script>
    <%-- resim bölümü bitti --%>




    <!--servis bilgileri bitiyor-->

</asp:Content>
