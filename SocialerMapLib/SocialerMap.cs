// ----------------------------------------------------------------------
// <copyright file="SocialerMap.cs" company="tarazan.net">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace SocialerMapLib
{
    using System;
    using System.Text;

    /// <summary>
    /// SocialerMap Map load methods
    /// </summary>
    public static class SocialerMap
    {
        /// <summary>
        /// Initializes SocialerMap
        /// </summary>
        /// <param name="sb">string which keeps javascript to load Map</param>
        /// <param name="latitude">initial value of latitude</param>
        /// <param name="longitude">initial value of longitude</param>
        /// <param name="zoomLevel">initial value of zoomLevel</param>
        /// <returns>javascript after modification</returns>
        public static StringBuilder InitializeMap(StringBuilder sb, string latitude, string longitude, int zoomLevel)
        {
            sb.Append("latlng = new google.maps.LatLng(" + latitude.Replace(',', '.').Trim(' ') + ", " + longitude.Replace(',', '.').Trim(' ') + ");");
            sb.Append("var myOptions = { zoom: " + zoomLevel +
                      ", center: latlng, mapTypeId: google.maps.MapTypeId.ROADMAP };");
            sb.Append("map = new google.maps.Map(document.getElementById('map_canvas'),myOptions);");
            sb.Append("geocoder = new google.maps.Geocoder();");
            return sb;
        }

        /// <summary>
        /// Adds Map Marker to the Map
        /// </summary>
        /// <param name="sb">string which keeps javascript to load Map</param>
        /// <param name="removable">Is this map can be removed by user</param>
        /// <param name="socialerMapObj">User's information</param>
        /// <param name="displayString">User's display information</param>
        /// <returns>javascript after modification</returns>
        public static StringBuilder AddMarker(StringBuilder sb, bool removable, SocialerMapDbObject socialerMapObj,string displayString)
        {
            sb.Append(String.Format(
                @"marker_{0} = new google.maps.Marker({{map: map,title: '{1}@{6}:{2}',icon:'https://graph.facebook.com/{3}/picture' }});
                              marker_{0}.setPosition(new google.maps.LatLng('{4}', '{5}'));                                                            ",
                socialerMapObj.LocationId,
                displayString,
                socialerMapObj.Date,
                socialerMapObj.UserId,
                socialerMapObj.Latitude,
                socialerMapObj.Longitude,
                socialerMapObj.Desc));

            if (!removable)
            {
                sb.Append("           google.maps.event.addListener(marker_" + socialerMapObj.LocationId +
                          @", 'click', function (e) {
                                                                            infowindow_" +
                          socialerMapObj.LocationId + " = new google.maps.InfoWindow({content: '" +
                          displayString + "@" + socialerMapObj.Date + ":" + socialerMapObj.Desc +
                          @"'});
                                                                            infowindow_" +
                          socialerMapObj.LocationId + ".open(map, marker_" + socialerMapObj.LocationId +
                          @");}
                                                                );");
            }
            else
            {
                sb.Append(String.Format(
                    @"var content_{0}='<div>{1}@{3}:</br>{2}</br><a class=\button\ href=\#\ onclick=\ this.blur();deleteLocation(marker_{0},{0});\> <center>Delete this location</center> </a></div>';
                                          google.maps.event.addListener(marker_{0}, 'click', function(e) {{
                                                           var infowindow = new google.maps.InfoWindow({{content: content_{0}}});
                                                           infowindow.open(map, marker_{0});}});",
                    socialerMapObj.LocationId,
                    displayString,
                    socialerMapObj.Desc,
                    socialerMapObj.Date));
            }

            return sb;
        }
    }
}