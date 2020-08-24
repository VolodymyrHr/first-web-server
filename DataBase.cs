using System;
using System.Collections.Generic;

namespace first_web_server
{
    public class DataBase
    {
        public static string[] who = new string[] { "Аліса", "Вовк", "Степан", "Олег", "Товариш", "Гусь", "Дятел" };
        public static string[] how = new string[] { "гарно", "незабутньо", "жахливо", "неймовірно", "впевнено", "швидко", "задумливо", "проворно" };
        public static string[] does = { "плаває", "стрибає", "плаче", "співає", "водить", "заводить", "заповза", "заліта", "зносить", "розносить" };
        public static string[] what = new string[] { "машину", "в річкі", "міст", "класику", "за руку", "під стіл", "напої" };

        public static Dictionary<string, string[]> localMap = new Dictionary<string, string[]>() { { "who", who }, { "how", how }, { "does", does }, { "what", what } };
    }
}