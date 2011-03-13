 
var geocoder;
var map;
var marker;
var latlng;
var userFID;
var latitude;
var longitude;
var locale='en';
var localizedStrings = {
    string_postToWall: 'Post to Wall',
    string_isCommon  :'Common',
    string_btnSave   : 'Save'
}

$(document).ready(function () {
    loadMap();
    $(function () {
        var language = window.navigator.userLanguage || window.navigator.language;
        locale = language.substring(0, 2);
        $("#datepicker").datetimepicker({ dateFormat: "yy-mm-dd" });
        if (locale != 'en') {
            $.getScript('Js/socialerLang-' + locale + '.js');
        }
        $("#dialog").dialog({ autoOpen: false })
        $("#dialog").dialog({ buttons: { "Ok": function () { $(this).dialog("close"); } } });
        $("#address").autocomplete({
            //This bit uses the geocoder to fetch address values
            source: function (request, response) {
                geocoder.geocode({ 'address': request.term }, function (results, status) {
                    response($.map(results, function (item) {
                        return {
                            label: item.formatted_address,
                            value: item.formatted_address,
                            latitude: item.geometry.location.lat(),
                            longitude: item.geometry.location.lng()
                        }
                    }));
                })
            },
            //This bit is executed upon selection of an address
            select: function (event, ui) {
                latitude = ui.item.latitude;
                longitude = ui.item.longitude;
                var location = new google.maps.LatLng(ui.item.latitude, ui.item.longitude);
                var marker = new google.maps.Marker({
                    position: latlng,
                    map: map,
                    draggable: true,
                    title: $("#desc").val()
                });
                marker.setPosition(location);
                map.setZoom(15);
                map.setCenter(location);
                google.maps.event.addListener(marker, 'dragend', function () {
                    latitude = marker.getPosition().lat();
                    longitude = marker.getPosition().lng();
                });
                var content = "<div><table><tr><td><img class='icon' src='https://graph.facebook.com/" + userFID + "/picture'/>:<input type='text' id='descpost' style='width:160px;' /></td></tr><tr><td>" +
                                 localizedStrings['string_postToWall']+
                                  ":<input type='checkbox' id='postwall' />" +
                                   localizedStrings['string_isCommon'] +
                                   ":<input type='checkbox' id='common' /><a class='button' href='#' onclick='this.blur();saveLocation();'> <span>" +
                                    localizedStrings['string_btnSave'] + "</span> </a></tr></td></table></div>";

                var infowindow = new google.maps.InfoWindow({ content: content });
                infowindow.open(map, marker);
                google.maps.event.addListener(marker, 'click', function (e) {
                    var infowindow = new google.maps.InfoWindow({
                        content: content
                    });
                    infowindow.open(map, marker);
                });
            }
        });
    });
});
