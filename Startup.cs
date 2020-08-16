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
        static string[] who = new string[] { "Kate", "Tom", "Mike", "Олег", "Товариш" };
        static string[] how = new string[] { "good", "beautiful", "awful", "incredibly", "впевнено" };
        static string[] does = new string[] { "swim", "jump", "walk", "співає", "водить" };
        static string[] what = new string[] { "car", "river", "bridge", "класику", "змагання" };
        static string[] incampUrls = new string[] { "http://localhost:5000", "http://localhost:8084" };

        Dictionary<string, string[]> localMap = new Dictionary<string, string[]>() { { "who", who }, { "how", how }, { "does", does }, { "what", what } };

        private string randomStrValue(string[] strValues)
        {
            Random r = new Random();
            return strValues[r.Next(strValues.Length)];
        }
        private string generateSentence()
        {
            return $"{getRandomWord("who")} {getRandomWord("how")} {getRandomWord("does")} {getRandomWord("what")}";
        }

        private string getSentence(Dictionary<string, string> words)
        {
            Dictionary<string, string>.ValueCollection valueColl = words.Values;
            string sentence = null;

            foreach (string word in valueColl)
            {
                sentence += $"{word} ";
            }

            return "\n" + sentence;
        }

        private string getStrList(Dictionary<string, string> dataList, string title)
        {
            string list = "\n" + title;

            foreach (KeyValuePair<string, string> kvp in dataList)
            {
                list += $"\nВід - {kvp.Key} отримали - {kvp.Value}";
            }

            return list;
        }

        private string getRandomWord(string kay)
        {
            string[] words;

            return (localMap.TryGetValue(kay, out words)) ? randomStrValue(words) : null;
        }

        private string getIncampSentence()
        {
            Dictionary<string, string> requestList = new Dictionary<string, string>();
            Dictionary<string, string> erorsList = new Dictionary<string, string>();

            Dictionary<string, string[]>.KeyCollection keyColl = localMap.Keys;

            string sentence = null;
            string report = null;
            string reportErrors = null;

            foreach (string key in keyColl)
            {
                Tuple<string, string> respons = new Tuple<string, string>(null, null);
                while (respons.Item2 == null)
                {
                    if (respons.Item1 != null)
                    {
                        erorsList.TryAdd(respons.Item1, "404");
                    }
                        respons = doRequest($"{randomStrValue(incampUrls)}/{key}");
                }
                requestList.Add(respons.Item1, respons.Item2);
            }

            sentence = (requestList.Count > 0)? getSentence(requestList) : "Incamp18 не відповідає";
            report = (requestList.Count > 0)? getStrList(requestList, "Successful requests:") : "\nSuccessful requests: -";
            reportErrors = (erorsList.Count > 0)? getStrList(erorsList, "Errors:") : "\nErrors: -";

            return string.Concat(sentence, report, reportErrors);
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
                    await context.Response.WriteAsync("Hello :)");
                });

                endpoints.MapGet("/{way:alpha}", async context => //*****************
                {
                    var way = context.Request.RouteValues["way"];
                    string word = getRandomWord(way.ToString());

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
                    await context.Response.WriteAsync(generateSentence());
                });

                endpoints.MapGet("/incamp18-quote", async context =>
                {
                    context.Response.Headers.Add("InCamp-Student", "VolodymyrHR");
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync(getIncampSentence());
                });

            });
        }


        public Tuple<string, string> doRequest(string urlForRequest)
        {
            try
            {
                WebRequest request = WebRequest.Create(urlForRequest);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();
                response.Close();
                return new Tuple<string, string>(urlForRequest, responseFromServer);
            }
            catch (WebException e)
            {
                Console.WriteLine("This program is expected to throw WebException on successful run." +
                                    "\n\nException Message :" + e.Message);
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    Console.WriteLine("Status Code : {0}", ((HttpWebResponse)e.Response).StatusCode);
                    Console.WriteLine("Status Description : {0}", ((HttpWebResponse)e.Response).StatusDescription);
                }

                return new Tuple<string, string>(urlForRequest, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return new Tuple<string, string>(urlForRequest, null);
            }
        }
    }
}
