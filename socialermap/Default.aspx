<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CSASPNETWebsite.Default" Culture="auto:en-US" UICulture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SocialerMap.com</title>
    <meta name="description" content="Social application which helps people to know where they are." />
    <meta name="keywords" content="Googlemap,facebook app,show on map,my friends" /> 
	<link rel="socialermap icon" href="Resources/icon.ico" />
	<link type="text/css" href="Css/socialer.css" rel="stylesheet" />
	<link type="text/css" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="stylesheet" />
	<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=true"></script>
	<script type="text/javascript" src="Js/jquery.js"></script>
	<script type="text/javascript" src="http://ui-dev.jquery.com/ui/jquery.ui.core.js"></script> 
	<script type="text/javascript" src="http://ui-dev.jquery.com/ui/jquery.ui.widget.js"></script> 
	<script type="text/javascript" src="http://ui-dev.jquery.com/ui/jquery.ui.position.js"></script> 
	<script type="text/javascript" src="http://ui-dev.jquery.com/ui/jquery.ui.dialog.js"></script> 
	<script type="text/javascript" src="Js/jquery-ui.js"></script>
	<script type="text/javascript" src="Js/socialer.js"></script>
	<script type="text/javascript" src="Js/location.js"></script>
	<script type="text/javascript" src="Js/jquery-ui-timepicker-addon.js"></script>
	<script type="text/javascript" src="Js/googleanalitics.js"></script>
</head>
<body>                 
    <div id="info">
        <table summary="layout table">
				<tr>
					<td rowspan="2"><img alt="Logo" src="Resources/logo.png" /> </td>
					<td><asp:Label ID="lblDate" runat="server" meta:resourceKey="lblDate"></asp:Label>:</td>
					<td><input type="text" id="datepicker" /></td>
					<td><asp:Label ID="lblAddress" runat="server" meta:resourceKey="lblAddress"></asp:Label>:</td>
					<td><input id="address" type="text" /></td>
					<td><a class="button" href="#" onclick="this.blur();addmarker();"> <asp:Label ID="lblAddButton" runat="server" meta:resourceKey="lblAddButton"></asp:Label> </a></td>
					<td rowspan="2">
						<table id="rounded-table" summary="layout table">
							<thead>
								<tr>
									<th scope="col" class="rounded-headerLeft">
										<asp:Label ID="lblStatistics" runat="server" meta:resourceKey="lblStatistics"></asp:Label>
									</th>
									<th scope="col" class="rounded-headerRight"></th>
								</tr>
							</thead>
							<tr>
								<td class="rounded-foot-left">
									<asp:Label ID="lblFriendCount" runat="server" Text="0/0"></asp:Label>
								</td>
								<td>
									<asp:Label ID="lblSocialerMapFriendCount" runat="server" meta:resourceKey="lblSocialerMapFriendCount"></asp:Label>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:Label ID="lblRankingHeader" runat="server" meta:resourceKey="lblRankingHeader"></asp:Label>
									<table summary="layout table">
										<tr>
											<td>Top List </td>
											<td><asp:Label ID="Label1" runat="server" meta:resourceKey="lblCheckinCounter"></asp:Label></td>
										</tr>
										<asp:Label ID="lblRanking" runat="server" meta:resourceKey="lblRanking"></asp:Label>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td><asp:Label ID="lblFindFriend" runat="server" meta:resourceKey="lblFindFriend"></asp:Label>:</td>
					<td>
						<form id="form1" runat="server">
							<asp:DropDownList ID="cmbFriends" runat="server">
							</asp:DropDownList>
						</form>
					</td>
					<td colspan="3">
						<a class="button" href="#" onclick="this.blur();showfriends();"> <asp:Label ID="lblShowRefesh" runat="server" meta:resourceKey="lblShowRefesh"></asp:Label> </a>
					</td>
				</tr>
			</table>
    </div>
    <div id="map_canvas"></div>
    <div id="footer">   
        <div id="copyright">Copyright @ socialermap.com Design & Development - All rights Reserved</div> 
    </div>

</body>
</html>
