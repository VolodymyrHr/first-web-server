using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace first_web_server{
    class Async : Common, ITypeRequest{

        public async Task<Dictionary<string, string>> getIncampSentenceAsync(string[] urls, Dictionary<string, string[]>.KeyCollection keys)
        {
            Dictionary<string, string> requestList = new Dictionary<string, string>();
           
            Task<Word> who = Task.Run(() =>
            {
                return Word.doRequest($"{urls[0]}who");
            });
            Task<Word> how = Task.Run(() =>
            {
                return Word.doRequest($"{urls[1]}how");
            });
            Task<Word> does = Task.Run(() =>
            {
                return Word.doRequest($"{urls[2]}does");
            });
            Task<Word> what = Task.Run(() =>
            {
                return Word.doRequest($"{urls[3]}what");
            });

            Word[] words = await Task.WhenAll(who, how, does, what);

            foreach (Word item in words)
            {
                requestList.Add(item.url + ": " + item.header, item.word);
            }

            return requestList;
        }

        public override Dictionary<string, string> doRequests(string[] urls, Dictionary<string, string[]>.KeyCollection keys)
        {
            return getIncampSentenceAsync(urls, keys).Result;
        }
    }
}