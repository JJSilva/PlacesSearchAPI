using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PlacesSearchAPI
{
    class Program
    {

        public const string searrchText = "fire station";

        static void Main(string[] args)
        {

            string apiKey = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(File.ReadAllText("key.json")).key;

            List<PlacesResponseResultsDataContract> placesResults = new List<PlacesResponseResultsDataContract>();
            List<PlacesResponseResultsDataContract> totalPlacesResults = new List<PlacesResponseResultsDataContract>();

            double lat;
            double lng;


            for (lat = 32.0; lat < 42.0; lat += .1)
            {
                for (lng = -124; lng < -114; lng += .1)
                {

                    Console.WriteLine(lat + "," + lng);
                    placesResults = GetPlaceResponse(lat.ToString() + "," + lng.ToString(), apiKey);

                    foreach (var place in placesResults)
                    {
                        totalPlacesResults.Add(place);
                        string output = place.Id + "," + place.name.Replace(",", "") + "," + place.formatedAddress.Replace(",", "") + System.Environment.NewLine;
                        Console.WriteLine(output);
                    }
                }
            }

            totalPlacesResults = totalPlacesResults.GroupBy(x => x.Id).Select(y => y.First()).OrderBy(x => x.Id).ToList();

            foreach (var place in totalPlacesResults)
            {
                string output = place.Id + "," + place.name.Replace(",", "") + "," + place.formatedAddress.Replace(",", "") + System.Environment.NewLine;
                File.AppendAllText("out.csv", output);

            }
            Console.WriteLine(totalPlacesResults.Count());
            Console.ReadLine();
        }

        public static List<PlacesResponseResultsDataContract> GetPlaceResponse(string location = "", string apiKey = "")
        {
            List<PlacesResponseResultsDataContract> results = new List<PlacesResponseResultsDataContract>();
            PlacesResponseDataContract mainResponse = new PlacesResponseDataContract();
            int backofftimer = 10;

            do
            {
                string url = "https://maps.googleapis.com/maps/api/place/textsearch/json?key=" + apiKey;
                if (mainResponse.nextPageToken != null)
                {
                    url += "&pagetoken=" + mainResponse.nextPageToken;
                }
                url += "&location=" + location;
                url += "&radius=50";
                url += "&query=" + System.Web.HttpUtility.UrlEncode(searrchText);
                url += "&types=fire_station";

            YO:
                WebRequest wr = WebRequest.Create(url);

                wr.ContentLength = 0;
                wr.Method = "GET";

                string json;
                using (var sr = new StreamReader(wr.GetResponse().GetResponseStream()))
                {
                    json = sr.ReadToEnd();
                }

                mainResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<PlacesResponseDataContract>(json);

                if (mainResponse.status != null)
                {
                    Console.WriteLine(mainResponse.status);
                }
                if (mainResponse.status == "INVALID_REQUEST")
                {
                    backofftimer += backofftimer;
                    Console.WriteLine("BORKEN! backing off for " + backofftimer);

                    System.Threading.Thread.Sleep(backofftimer);
                    goto YO;

                }

                mainResponse.results.ForEach(x => results.Add(x));
            }
            while (mainResponse.nextPageToken != null);

            return results;
        }


    }
}
