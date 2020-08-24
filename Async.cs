using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace first_web_server{
    class Async : ITypeRequest{

        public async Task<string> getIncampSentenceAsync(string[] urls)
        {
            Dictionary<string, string> requestList = new Dictionary<string, string>();

            string sentence = null;
            string runTime = null;

            Stopwatch watch = new Stopwatch();
            watch.Start();
            
            Task<Word> who = Task.Run(() =>
            {
                return Word.doRequest($"{Sentence.randomStrValue(urls)}who");
            });
            Task<Word> how = Task.Run(() =>
            {
                return Word.doRequest($"{Sentence.randomStrValue(urls)}how");
            });
            Task<Word> does = Task.Run(() =>
            {
                return Word.doRequest($"{Sentence.randomStrValue(urls)}does");
            });
            Task<Word> what = Task.Run(() =>
            {
                return Word.doRequest($"{Sentence.randomStrValue(urls)}what");
            });

            Word[] words = await Task.WhenAll(who, how, does, what);

            foreach (Word item in words)
            {
                requestList.Add(item.url + ": " + item.header, item.word);
            }

            watch.Stop();
            TimeSpan ts = watch.Elapsed;
            string time = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            
            sentence = (requestList.Count > 0) ? Sentence.getSentence(requestList) : "Incamp18 не відповідає";
            runTime = $"\nRuntime: {time}"; 
            
            return string.Concat(sentence, runTime);
        }
        public string getIncampSentence(string[] urls)
        {
            return getIncampSentenceAsync(urls).Result;
        }
    }
}