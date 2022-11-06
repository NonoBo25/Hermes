using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Hermes
{
    public class MediaManager
    {
        private RestClient client;
        private RestRequest request ;

        public MediaManager()
        {
            client = new RestClient("https://nsfw-images-detection-and-classification.p.rapidapi.com/adult-content");
            request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("content-type", "application/json");
            request.AddHeader("X-RapidAPI-Key", "05c6dc4978msh78a09e1f2745381p1f0e65jsnf4c85b44394f");
            request.AddHeader("X-RapidAPI-Host", "nsfw-images-detection-and-classification.p.rapidapi.com");

        }
        public bool IsSafe(string url)
        {
            request.AddOrUpdateParameter("url", url);
            RestResponse response = client.Execute(request);
            Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
            return bool.Parse(json["unsafe"].ToString());
        }

    }   
}