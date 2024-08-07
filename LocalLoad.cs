using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace eStock.Models
{
    public class LocalLoad
    {
        public LocalLoad() {
            

        }
       public static List<float> GetLocalData(string path)
        {
            List<float> closeValues = new List<float>();
            try
            {
                string JsonString = File.ReadAllText(path);
                JObject jsonObject = JObject.Parse(JsonString);

                JObject timeSeries = (JObject)jsonObject["Time Series (Daily)"];
                foreach (var entry in timeSeries)
                {
                   

                    JObject timeEntry = (JObject)entry.Value;
                    string closeString = ((string)timeEntry["4. close"]).Trim();


                    if(float.TryParse(closeString, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float closeValue))
                    {
                        closeValues.Add(closeValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid path or file structure");
            }

            return closeValues;
           

        }
    }
}
