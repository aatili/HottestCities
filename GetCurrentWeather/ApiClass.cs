using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

// Class to initiate a client


namespace GetCurrentWeather
{

    public static class ApiClass
    {
        public static HttpClient Api_user { get; set; }
        public static void start_user()
        {
            Api_user = new HttpClient();
            Api_user.DefaultRequestHeaders.Accept.Clear();
            Api_user.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // expecting response in JSON format
        }
    }
}
