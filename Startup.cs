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

                // endpoints.MapGet("/async", async context =>
                // {
                //     context.Response.Headers.Add("InCamp-Student", "VolodymyrHR");
                //     context.Response.ContentType = "text/html; charset=utf-8";
                //     await context.Response.WriteAsync(getWordsAsync());
                // });

            });
        }

        // public string getWordsAsync()
        // {
        //     Dictionary<string, string[]>.KeyCollection keyColl = DataBase.localMap.Keys;

        //     Word sentences;

        //     foreach (string key in keyColl)
        //     {
        //         sentences = doRequestAsync($"{Sentence.randomStrValue(incampUrls)}{key}").Result;
        //         Console.WriteLine(sentences);
        //     }
        //     return "";
        // }

        // public async Task<Word> doRequestAsync(string urlForRequest)
        // {
        //     string responseFromServer;
        //     string header;

        //     try
        //     {
        //         WebRequest request = (HttpWebRequest)WebRequest.Create(urlForRequest);

        //         using (WebResponse response = await request.GetResponseAsync())
        //         {
        //             using (Stream responseStream = response.GetResponseStream())
        //             {
        //                 Stream dataStream = response.GetResponseStream();

        //                 StreamReader reader = new StreamReader(dataStream);

        //                 responseFromServer = await reader.ReadToEndAsync();
        //                 header = response.Headers.Get("InCamp-Student");
        //             }
        //         }

        //         // WebResponse  response = (HttpWebRequest)request.GetResponseAsync();

        //         return new Word (urlForRequest, header, responseFromServer);
        //     }
        //     catch (WebException e)
        //     {
        //         Console.WriteLine("This program is expected to throw WebException on successful run." +
        //                             "\n\nException Message :" + e.Message);
        //         if (e.Status == WebExceptionStatus.ProtocolError)
        //         {
        //             Console.WriteLine("Status Code : {0}", ((HttpWebResponse)e.Response).StatusCode);
        //             Console.WriteLine("Status Description : {0}", ((HttpWebResponse)e.Response).StatusDescription);
        //         }

        //         return new Word (null, null, null);
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e.Message);

        //         return new Word (null, null, null);
        //     }
        // }

    }
}
