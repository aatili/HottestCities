using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCurrentWeather
{
    public class WeatherProc
    {

        // GET weather information from API then proccess the data to extract "temp" ,"wind_speed","humidity","pressure" given the "lat" and "lon" values
        // returning info by units of choice {metric,imperial} 
        public static async Task<WeatherInfo?> WeatherSummarise(double Lat, double Lon, string Measure_units)
        {
            WeatherInfo info = new();

            string Api_key = Convert.ToString(Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY"));

            string url_units = Measure_units == "metric" ? "&units=metric" : "&units=imperial"; // metric or imperial units

            string url = "https://api.openweathermap.org/data/2.5/weather?lat=" + Lat + "&lon=" + Lon + "&appid=" + Api_key + url_units; // building url

            using (HttpResponseMessage response = await ApiClass.Api_user.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var Res = await response.Content.ReadAsStringAsync();
                    if (Res == "[]") // no info returned
                    {
                        return null;
                    }

                    try
                    {
                        string Res_string = Convert.ToString(Res);
                        var Res_object = System.Text.Json.JsonDocument.Parse(Res_string);

                        //extracting temp,humidity,pressure from 'main' root
                        var Res_main = Res_object.RootElement.GetProperty("main");
                        var Res_temp = Convert.ToString(Res_main.GetProperty("temp"));
                        var Res_humidity = Convert.ToString(Res_main.GetProperty("humidity"));
                        var Res_pressure = Convert.ToString(Res_main.GetProperty("pressure"));

                        //extracting temp,humidity,pressure from 'wind' root
                        var Res_wind = Res_object.RootElement.GetProperty("wind");
                        var Res_wind_speed = Convert.ToString(Res_wind.GetProperty("speed"));


                        info.Weather_temp = Convert.ToDouble(Res_temp);
                        info.Weather_wind_speed = Convert.ToDouble(Res_wind_speed);
                        info.Weather_humidity = Convert.ToDouble(Res_humidity);
                        info.Weather_pressure = Convert.ToDouble(Res_pressure);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error:" + ex.Message);
                    }
                    

                    return info;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
