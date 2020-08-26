using System;
using System.Collections.Generic;


namespace first_web_server{
    public abstract class Sentence{
        public static string randomStrValue(string[] strValues)
        {
            Random r = new Random();
            return strValues[r.Next(strValues.Length)];
        }
        public static string getSentence(Dictionary<string, string> words)
        {
            Dictionary<string, string>.ValueCollection valueColl = words.Values;
            string sentence = null;

            foreach (string word in valueColl)
            {
                sentence += $"{word} ";
            }

            return "\n" + sentence;
        }

        public static string getStrList(Dictionary<string, string> dataList, string title)
        {
            string list = "\n" + title;

            foreach (KeyValuePair<string, string> kvp in dataList)
            {
                if(kvp.Value == null){
                    list += $"\nВід - {kvp.Key} отримали - 404";
                }else{
                    list += $"\nВід - {kvp.Key} отримали - {kvp.Value}";
                }
            }

            return list;
        }

        public static string generateSentence(Dictionary<string, string[]> map)
        {
            Dictionary<string, string> words = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string[]> item in map)
            {
                words.Add(item.Key, randomStrValue(item.Value));
            }
            return getSentence(words);
        }

    }
}