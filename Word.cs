using System;
using System.Net;
using System.IO;


namespace first_web_server
{
    public class Word : Sentence
    {

        public string url { get; set; }
        public string header { get; set; }
        public string word { get; set; }

        public Word(string url, string header, string word)
        {
            this.url = url;
            this.header = header;
            this.word = word;
        }

        public static string getRandomWord(string kay)
        {
            string[] words;

            return (DataBase.localMap.TryGetValue(kay, out words)) ? randomStrValue(words) : null;
        }

        public static Word doRequest(string urlForRequest)
        {
            try
            {
                WebRequest request = WebRequest.Create(urlForRequest);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                string header = response.Headers.Get("InCamp-Student");

                reader.Close();
                dataStream.Close();
                response.Close();

                return new Word(urlForRequest, header, responseFromServer);
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

                return new Word(urlForRequest, null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return new Word(urlForRequest, null, null);
            }
        }
    }
}