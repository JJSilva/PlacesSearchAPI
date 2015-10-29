using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PlacesSearchAPI
{
    [DataContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class PlacesResponseDataContract
    {

        [JsonProperty(PropertyName = "next_page_token")]
        public string nextPageToken { get; set; }


        [JsonProperty(PropertyName = "results")]
        public List<PlacesResponseResultsDataContract> results { get; set; }


        [JsonProperty(PropertyName = "status")]
        public string status { get; set; }



    }

    [DataContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class PlacesResponseResultsDataContract
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName ="formatted_address")]
        public string formatedAddress { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string name { get; set; }

        [JsonProperty(PropertyName = "geometry")]
        public geometry geometry { get; set; }

 

    }

    public class geometry
    {
        [JsonProperty(PropertyName = "location")]
        public location location { get; set; }
    }


    public class location
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }
}
