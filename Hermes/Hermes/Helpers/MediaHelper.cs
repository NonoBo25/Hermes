using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermes
{
    public static class MediaHelper
    {
        public const string api_link = "https://nsfw-images-detection-and-classification.p.rapidapi.com/adult-content";
        public static bool isSafe(string link)
        {
            
            var client = new RestClient("https://nsfw-images-detection-and-classification.p.rapidapi.com/adult-content");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("content-type", "application/json");
            request.AddHeader("X-RapidAPI-Key", "18aab05467msh0c14beaf82e5283p12df52jsn189c89c5c450");
            request.AddHeader("X-RapidAPI-Host", "nsfw-images-detection-and-classification.p.rapidapi.com");
            request.AddParameter("application/json", "{\"url\":\"" + link + "\"}", ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            Dictionary<string,object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content);

            return !(bool)dic["unsafe"];
        }
    }
}