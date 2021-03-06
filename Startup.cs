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


namespace first_web_server
{
    public class Startup
    {
        private char[] delimiterChars = {',', ' ', ';', '\t' };
        private static string[] incampUrls = new string[] { "http://service1:3030/",
                                                            "http://service2:3030/",
                                                            "http://service3:3030/" };

        private string[] getEnviromentVariables(){
            return Environment.GetEnvironmentVariable("myurls").Split(delimiterChars);
        }


        public string ChooseTypeRequest(string[] urls)
        {
            ITypeRequest strategy;
            if (Program.keys.Length > 0 && Program.keys[0] == "async")
            {
                strategy = new Async();
            }
            else
            {
                strategy = new Sync();
            }
            return strategy.getIncampSentence(urls);

        }

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
                    await context.Response.WriteAsync($"Hello, dear, friend :)");
                });

                endpoints.MapGet("/{way}", async context =>
                {
                    var way = context.Request.RouteValues["way"];
                    string word = Word.getRandomWord(way.ToString());

                    context.Response.Headers.Add("InCamp-Student", "VolodymyrHR");
                    context.Response.ContentType = "text/html; charset=utf-8";

                    if (word == null)
                    {
                        context.Response.StatusCode = 404;
                    }
                    else
                    {
                        await context.Response.WriteAsync(word);
                    }

                });

                endpoints.MapGet("/quote", async context =>
                {
                    context.Response.Headers.Add("InCamp-Student", "VolodymyrHR");
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync(Sentence.generateSentence(DataBase.localMap));
                });

                endpoints.MapGet("/incamp18-quote", async context =>
                {
                    context.Response.Headers.Add("InCamp-Student", "VolodymyrHR");
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync(ChooseTypeRequest((getEnviromentVariables().Length > 0)?getEnviromentVariables():incampUrls));
                });

            });
        }

    }
}
