
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthentication.Test.TestClasses
{
    public class CustomApiClient
    {
        private static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("https://localhost:7249/api"),
        };


        public static async Task<string> CallGetApiJsonResult(string apiPath)
        {
            HttpResponseMessage response = await sharedClient.GetAsync(apiPath);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }
    }
}
