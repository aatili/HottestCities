// See https://aka.ms/new-console-template for more information

using System;
using System.Data;


/*
 * Adnan Atili
 * Requesting data from OpenWeatherMap as JSON
 */


namespace GetCurrentWeather // Note: actual namespace depends on the project name.
{
    public class Program
    {
        static async Task<int> Main(string[] args)
        {
            /*Checking args correctness*/
            if (args.Length < 2)
            {
                return -1;
            }
            if (args[0] != "-c" && args[0] != "--city")
            {
                return -2;
            }
            if (args.Length > 2)
            {
                if (args.Length != 4)
                {
                    return -3;
                }
                if (args[2] != "-u" && args[2] != "--units")
                {
                    return -4;
                }
                if (args[3] != "imperial" && args[3] != "metric")
                {
                    return -4;
                }
            }

            string city_name = args[1];
            string weather_units = args.Length == 4 ? args[3] : "metric";

            Environment.SetEnvironmentVariable("OPENWEATHER_API_KEY", "46b5748ea126e994721c3af5040ea098"); // setting enivroment var

            ApiClass.start_user(); //initializing client

            var Res = await GeocodingProc.GeoCoder(city_name);
            if (Res == null) //checking if city was found (did we recieved GeoCode?)
            {
                return -5;
            }

            var Res2 = await WeatherProc.WeatherSummarise(Res.lat, Res.lon, weather_units);
            if (Res2 == null) //checking if city was found (did we recieved GeoCode?)
            {
                return -6;
            }

            Console.WriteLine("{0}|{1}|{2}|{3}|{4}", city_name, Res2.Weather_temp, Res2.Weather_wind_speed, Res2.Weather_humidity, Res2.Weather_pressure);

            return 0;
        }
    }
}
