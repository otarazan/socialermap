function saveLocation() {
    date = $("#datepicker").val();
    postWall = $("#postwall").is(':checked');
    desc = $("#descpost").val();
    isCommon = $("#common").is(':checked');
    window.location = "Default.aspx?action=add&date=" + date + "&latitude=" + latitude + "&longitude=" + longitude + "&postwall=" + postWall + "&description=" + desc + "&common=" + isCommon;
}

function addmarker() {
    var marker = new google.maps.Marker({
        position: map.getCenter(),
        map: map,
        draggable: true
    });
    var content = "<div><table><tr><td><img class='icon' src='https://graph.facebook.com/" + userFID + "/picture'/>:<input type='text' id='descpost' style='width:160px;' /></td></tr><tr><td>" +
                                 localizedStrings['string_postToWall'] +
                                  ":<input type='checkbox' id='postwall' />" +
                                   localizedStrings['string_isCommon'] +
                                   ":<input type='checkbox' id='common' /><a class='button' href='#' onclick='this.blur();saveLocation();'> <span>" +
                                    localizedStrings['string_btnSave'] + "</span> </a></tr></td></table></div>";

    var infowindow = new google.maps.InfoWindow({ content: content });
    infowindow.open(map, marker);
    latitude = marker.getPosition().lat();
    longitude = marker.getPosition().lng();
    google.maps.event.addListener(marker, 'click', function (e) {
        var infowindow = new google.maps.InfoWindow({
            content: content
        });
        infowindow.open(map, marker);
    });
    google.maps.event.addListener(marker, 'dragend', function () {
        latitude = marker.getPosition().lat();
        longitude = marker.getPosition().lng();
    });
}  

function showfriends() {
    date = $("#datepicker").val();
    fid = $("#cmbFriends").val();
    window.location = "default.aspx?date=" + date + "&fid=" + fid;
}

function deleteLocation(marker,locationID) {
    window.location = 'Default.aspx?action=delete&locationid=' + locationID;
}