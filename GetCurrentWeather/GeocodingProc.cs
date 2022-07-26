using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace GetCurrentWeather
{
    public class GeocodingProc
    {

        // GET GeoCode from API then proccess the data to extract "lon" and "lat" given a city name
        public static async Task<GeoCode?> GeoCoder(string City_name)
        {
            GeoCode Geo_info = new();
            if (string.IsNullOrWhiteSpace(City_name))
                return null;

            string Api_key = Convert.ToString(Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY"));
            string url = "http://api.openweathermap.org/geo/1.0/direct?q=" + City_name + "&limit=1&appid=" + Api_key;

            using (HttpResponseMessage response = await ApiClass.Api_user.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var Res = await response.Content.ReadAsStringAsync();
                    if (Res == "[]") // city not found (no info returned)
                    {
                        return null;
                    }

                    try
                    {
                        string Res_string = Convert.ToString(Res);
                        var Res_pbject = System.Text.Json.JsonDocument.Parse(Res_string.Substring(1, Res_string.Length - 2));         //Parsing the json we recieved
                        string Lon = Convert.ToString(Res_pbject.RootElement.GetProperty("lon"));
                        string Lat = Convert.ToString(Res_pbject.RootElement.GetProperty("lat"));
                        Geo_info.lon = Convert.ToDouble(Lon);
                        Geo_info.lat = Convert.ToDouble(Lat);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Error:" + ex.Message);
                    }
                    return Geo_info;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
