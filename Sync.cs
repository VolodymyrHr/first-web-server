using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace first_web_server
{
    class Sync : ITypeRequest
    {
        public string getIncampSentence(string[] urls)
        {
            Dictionary<string, string> requestList = new Dictionary<string, string>();
            Dictionary<string, string> erorsList = new Dictionary<string, string>();

            Dictionary<string, string[]>.KeyCollection keyColl = DataBase.localMap.Keys;

            string sentence = null;
            string report = null;
            string reportErrors = null;
            string runTime = null;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            foreach (string key in keyColl)
            {
                Word respons = new Word(null, null, null);
                while (respons.word == null)
                {
                    if (respons.url != null)
                    {
                        erorsList.TryAdd(respons.url, "404");
                    }

                    respons = Word.doRequest($"{Sentence.randomStrValue(urls)}{key}");
                }
                requestList.Add(respons.url + ": " + respons.header, respons.word);
            }

            watch.Stop();
            TimeSpan ts = watch.Elapsed;
            string time = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            sentence = (requestList.Count > 0) ? Sentence.getSentence(requestList) : "Incamp18 не відповідає";
            report = (requestList.Count > 0) ? Sentence.getStrList(requestList, "Successful requests:") : "\nSuccessful requests: -";
            reportErrors = (erorsList.Count > 0) ? Sentence.getStrList(erorsList, "Errors:") : "\nErrors: -";
            runTime = $"\nRuntime: {time}"; 
            

            return string.Concat(sentence, report, reportErrors, runTime);
        }
    }
}