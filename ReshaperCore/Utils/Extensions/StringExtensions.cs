using System;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace ReshaperCore.Utils.Extensions
{
    public static class StringExtensions
    {
        public static bool StartsWith(this string text, string subString, int startIndex)
        {
            bool found = false;
            for (int charIndex = startIndex, subCharIndex = 0; subCharIndex < subString.Length && charIndex < text.Length; charIndex++, subCharIndex++)
            {
                if (text[charIndex] != subString[subCharIndex])
                {
                    break;
                }
                else if (subCharIndex == subString.Length - 1)
                {
                    found = true;
                }
            }
            return found;
        }

        public static bool StartsWith(this string text, IEnumerable<string> subStrings, out string foundString)
        {
            bool found = false;
            foundString = null;
            foreach (string subString in subStrings)
            {
                if (text.StartsWith(subString))
                {
                    found = true;
                    foundString = subString;
                    break;
                }
            }
            return found;
        }

        public static bool EndsWith(this string text, IEnumerable<string> subStrings, out string foundString)
        {
            bool found = false;
            foundString = null;
            foreach (string subString in subStrings)
            {
                if (text.EndsWith(subString))
                {
                    found = true;
                    foundString = subString;
                    break;
                }
            }
            return found;
        }

        public static List<Tuple<string, string>> SplitWithDelimiters(this string text, List<string> delimiters)
        {
            List<Tuple<string, string>> splitText = new List<Tuple<string, string>>();
            int beginningIndex = 0;

            for (int charIndex = beginningIndex; charIndex < text.Length; charIndex++)
            {
                foreach (string delimiter in delimiters)
                {
                    if (delimiter == string.Empty)
                    {
                        if (text.Length - beginningIndex >= 0)
                        {
                            splitText.Add(new Tuple<string, string>(text.Substring(beginningIndex, text.Length - beginningIndex), delimiter));
                        }
                        beginningIndex = text.Length;
                        break;
                    }
                    else if (text.StartsWith(delimiter, charIndex))
                    {
                        if (charIndex - beginningIndex >= 0)
                        {
                            splitText.Add(new Tuple<string, string>(text.Substring(beginningIndex, charIndex - beginningIndex), delimiter));
                        }
                        beginningIndex = charIndex + delimiter.Length;
                        break;
                    }
                }
                if (charIndex == text.Length - 1 && charIndex - beginningIndex >= 0)
                {
                    splitText.Add(new Tuple<string, string>(text.Substring(beginningIndex, charIndex - beginningIndex + 1), null));
                }
            }
            return splitText;
        }

        public static string GetJsonValue(this string json, string jsonPath)
        {
            try
            {
                JToken jToken = JToken.Parse(json);
                json = jToken.SelectToken(jsonPath).ToString();
            }
            catch
            {

            }
            return json;
        }

        public static string SetJsonValue(this string json, string jsonPath, string value)
        {
            try
            {
                JToken jToken = JToken.Parse(json);
                double temp;
                if (!(value.StartsWith("[") || value.StartsWith("{") || value.StartsWith("'") || value.StartsWith("\"") || value == "null" || double.TryParse(value, out temp)))
                {
                    value = $"'{value}'";
                }
                jToken.SelectToken(jsonPath).Replace(JToken.Parse(value));
                json = jToken.ToString();
            }
            catch
            {

            }
            return json;
        }

        public static string GetXmlValue(this string xml, string xpath)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                xml = document.SelectSingleNode(xpath).InnerXml;
            }
            catch
            {

            }
            return xml;
        }

        public static string SetXmlValue(this string xml, string xpath, string value)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                document.SelectSingleNode(xpath).Value = value;
                xml = document.OuterXml;
            }
            catch
            {

            }
            return xml;
        }

        public static string TrimEnd(this string str, string endingStr)
        {
            if (str.EndsWith(endingStr))
            {
                str = str.Substring(0, str.LastIndexOf(endingStr));
            }
            return str;
        }
    }
}
