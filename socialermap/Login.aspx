<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CSASPNETWebsite.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SocialerMap Login</title>
    <script type="text/javascript" src="http://connect.facebook.net/en_US/all.js"></script>
     <link type="text/css" href="Css/socialer.css" rel="stylesheet" />
     <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
     <script type="text/javascript">
         function initialize() {
             var myLatlng = new google.maps.LatLng(-34.397, 150.644);
             var myOptions = {
                 zoom: 8,
                 center: myLatlng,
                 mapTypeId: google.maps.MapTypeId.ROADMAP
             }
             var map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

             FB.init({ appId: '<%:Facebook.FacebookContext.Current.AppId%>', status: true, cookie: true, xfbml: true});
             FB.Event.subscribe('auth.sessionChange', function (response) {
                 if (response.session) {
                     // A user has logged in, and a new cookie has been saved
                     window.location.reload();
                 } else {
                     // The user has logged out, and the cookie has been cleared
                 }
             });
         }
</script>
</head>
<body onload="initialize()">
    <table>
        <tr>
            <td><img alt="logo" src="Resources/logo.png" /></td>
            <td>
                <div id="fb-root"></div>
                <fb:login-button perms="publish_stream"></fb:login-button>
            </td>
        </tr>
    </table>  
    <div id="map_canvas"></div>
</body>
</html>
