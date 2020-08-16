using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Web;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using System.Text;

namespace first_web_server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        static string[] who = new string[] { "Kate", "Tom", "Mike", "Олег", "Товариш" };
        static string[] how = new string[] { "good", "beautiful", "awful", "incredibly", "впевнено" };
        static string[] does = new string[] { "swim", "jump", "walk", "співає", "водить" };
        static string[] what = new string[] { "car", "river", "bridge", "класику", "змагання" };

        Dictionary<string, string[]> map = new Dictionary<string, string[]>() { { "who", who }, { "how", how }, { "does", does }, { "what", what } };

        private string randomWord(string kay)
        {
            string[] words;
            if (map.TryGetValue(kay, out words))
            {
                Random r = new Random();
                int index = r.Next(words.Length);
                return words[index];
            }
            else
            {
                return $"error 404 стоінка /{kay} незнайдена";
            }// else 404
        }
        private string generateSentenses()
        {
            return $"{randomWord("who")} {randomWord("how")} {randomWord("does")} {randomWord("what")}";
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    context.Response.Headers.Add("InCamp-Student", "VolodymyrHR");
                    await context.Response.WriteAsync("Hello :)", Encoding.UTF8);
                });

                endpoints.MapGet("/{way:alpha}", async context => //*****************
                {
                    var way = context.Request.RouteValues["way"];
                    context.Response.Headers.Add("InCamp-Student", "VolodymyrHR");
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync(randomWord(way.ToString()));
                });

                endpoints.MapGet("/quote", async context =>
                {
                    context.Response.Headers.Add("InCamp-Student", "VolodymyrHR");
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync(generateSentenses());
                });

                endpoints.MapGet("/incamp18-quote", async context =>
                {
                    context.Response.Headers.Add("InCamp-Student", "VolodymyrHR");
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync(generateSentenses());
                });

                endpoints.MapGet("/test", async context =>
                {
                    await context.Response.WriteAsync(getRequest());
                });
            });
        }


        public string getRequest()
        {
            string responseFromServer;

            WebRequest request = WebRequest.Create("http://972f647d75a1.ngrok.io/who");
            request.Credentials = CredentialCache.DefaultCredentials;

            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {

                StreamReader reader = new StreamReader(dataStream, Encoding.UTF8);

                responseFromServer = reader.ReadToEnd();

                Console.WriteLine(responseFromServer);
            }


            response.Close();
            return $"{responseFromServer}";
        }
    }
}
