using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Lab6.Serialization;

namespace Lab6.WindowsForms
{
    public static class AttributeHandler
    {
        public static bool CheckIfHasAttributes(string nodeText)
        {
            if (nodeText.Split(' ').Length > 1)
            {
                return true;
            }

            return false;
        }

        public static List<AttributeItem> ParseStringOnAttributes(string nodeText)
        {
            var attributes = new List<AttributeItem>();
            var parts = nodeText.Substring(1, nodeText.Length - 2).Split(' ');
            var list = new List<string>(parts);

            list.RemoveAt(0);
            if (list[list.Count - 1] == "/")
            {
                list.RemoveAt(list.Count - 1);
            }

            var tempList = "";
            for (var i = 0; i < list.Count; i++)
            {
                var count = list[i].ToCharArray().Where(x => x == '\"').Count();
                if (count < 2)
                {

                    if (tempList == "")
                    {
                        tempList = list[i];
                    }
                    else
                    {
                        list[i - 1] += $" {list[i]}";
                        list.RemoveAt(i);
                        i--;
                        if (count != 0)
                        {
                            tempList = "";
                        }
                    }
                }
            }
            foreach (var attributeString in list)
            {
                var attribute = ParseAttributeFromString(attributeString);
                attributes.Add(attribute);
            }

            return attributes;
        }

        public static string TransformAttributesListToString(List<AttributeItem> attributes)
        {
            var resultLine = String.Empty;
            foreach (var attribute in attributes)
            {
                resultLine = resultLine + attribute.Key + "=\"" + attribute.Value + "\" ";
            }

            return resultLine.TrimEnd(' ');
        }


        private static AttributeItem ParseAttributeFromString(string attributeString)
        {
            var delimiter = attributeString.IndexOfAny(new []{'='});
            var key = attributeString.Substring(0, delimiter);
            var value = attributeString.Substring(delimiter + 2, attributeString.Length - delimiter - 3);
            var attribute = new AttributeItem(key, value);

            return attribute;
        }
    }



    public sealed class AttributeItem
    {
        public string Key { get; }

        public string Value { get; }


        public AttributeItem(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}