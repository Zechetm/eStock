using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace eStock.Models
{
    public class StockInt
    {
        private static string url = "https://www.alphavantage.co/query";
        private static readonly HttpClient client = new HttpClient();
        public StockInt()
        {

        }
        public static async Task<List<float>> Interpret(string Stock_name, string api_key = "")
        {
            string function = "TIME_SERIES_DAILY";
            string QUERY_URL = $"{url}?function={function}&symbol={Stock_name}&apikey={api_key}";
            List<float> closevals = new List<float>();
            try
            {
                string response = await client.GetStringAsync(QUERY_URL);

                JObject jsonObject = JObject.Parse(response);
                JObject series = (JObject)jsonObject[$"Time Series (Daily)"];

                foreach(var entry in series)
                {
                    JObject timeEntry = (JObject)entry.Value;
                    string closeString = ((string)timeEntry["4. close"]).Trim();

                    if (float.TryParse(closeString, System.Globalization.NumberStyles.Float, 
                        System.Globalization.CultureInfo.InvariantCulture, out float closeValue))
                    {
                        closevals.Add(closeValue);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to parse close value: {closeString}");
                    }

                }

            }
            catch (Exception ex) {
                Console.WriteLine("UnknownError upon making API call");
            }
            return closevals;

        }

    }
}