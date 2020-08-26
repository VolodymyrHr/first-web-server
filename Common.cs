using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace first_web_server
{
    abstract class Common
    {
        public abstract Dictionary<string,string> doRequests(string[] urls, Dictionary<string, string[]>.KeyCollection keys);
        public string getIncampSentence(string[] urls)
        {
            Dictionary<string, string> requestList;

            Dictionary<string, string[]>.KeyCollection keyColl = DataBase.localMap.Keys;

            string sentence = null;
            string report = null;
            string runTime = null;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            string[] urlsForRequest = new string[keyColl.Count];
            for (int i=0; i<keyColl.Count; i++){
                urlsForRequest[i] = Sentence.randomStrValue(urls);
            }

            requestList = doRequests(urlsForRequest, keyColl);

            watch.Stop();
            TimeSpan ts = watch.Elapsed;
            string time = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            sentence = (requestList.Count > 0) ? Sentence.getSentence(requestList) : "Incamp18 не відповідає";
            report = (requestList.Count > 0) ? Sentence.getStrList(requestList, "Successful requests:") : "\nSuccessful requests: -";
            runTime = $"\nRuntime: {time}"; 
            

            return string.Concat(sentence, report, runTime);
        }
    }
}