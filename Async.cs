using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace first_web_server
{
    class Async : Common, ITypeRequest
    {

        public async Task<Dictionary<string, string>> getIncampSentenceAsync(string[] urls, Dictionary<string, string[]>.KeyCollection keys)
        {
            Dictionary<string, string> requestList = new Dictionary<string, string>();
            List<Task<Word>> tasks = new List<Task<Word>>();

            int i = 0;
            foreach (string key in keys)
            {
                tasks.Add(Task.Run(() =>
                {
                    return Word.doRequest($"{urls[i++]}{key}");
                }
                ));
            }

            Word[] words = await Task.WhenAll(tasks);

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