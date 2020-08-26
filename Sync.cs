using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace first_web_server
{
    class Sync : Common, ITypeRequest
    {
        public override Dictionary<string, string> doRequests(string[] urls, Dictionary<string, string[]>.KeyCollection keys)
        {
            Dictionary<string, string> requestList = new Dictionary<string, string>();
            int i = 0;
            foreach (string key in keys)
            {
                Word response = new Word(null, null, null);
                response = Word.doRequest($"{urls[++i]}{key}");
                requestList.Add(response.url + ": " + response.header, response.word);
            }

            return requestList;
        }
    }
}