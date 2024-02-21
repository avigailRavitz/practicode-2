using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PractiCod2
{
    public class Selector
    {
        public string Id { get; set; }

        public string TagName { get; set; }

        public List<string> Classes { get; set; }

        public Selector Parent { get; set; }

        public Selector Child { get; set; }

        public Selector()
        {
            Classes = new List<string>();
        }

        public static List<string> ConvertStr(string s)
        {
            List<string> arr = new List<string>();
            bool flag = false;
            int startIndex = -1;
            for (int i = 0; i < s.Length; i++)
            {
                if (i == s.Length - 1 && i != 1)
                {
                    if (startIndex == -1)
                    {
                        startIndex = 0;
                        arr.Add(s);
                    }
                    else
                    {
                        arr.Add(s.Substring(startIndex, i - startIndex + 1));
                    }
                }

                if ((s[i] == '.' || s[i] == '#') && startIndex != -1 && i >= startIndex)
                {
                    arr.Add(s.Substring(startIndex, i - startIndex));
                    startIndex = i;
                }
                else if (s[i] == '#' || s[i] == '.')
                {
                    startIndex = i;
                    if (flag == false)
                    {
                        arr.Add(s.Substring(0, i));
                    }
                    flag = true;
                }

            }
            return arr;
        }
        public static Selector Parse(string query)
        {
            string s = "div#myDiv.class-name p .class-1";
            string[] arrStr = query.Split(' ');
            Selector rootSelector = new Selector();
            Selector currentSelector = new Selector();
            currentSelector = rootSelector;
            for (int i = 0; i < arrStr.Length; i++)
            {
                List<string> arrSplit = ConvertStr(arrStr[i]);
                foreach (string item in arrSplit)
                {
                    if (item.StartsWith("#"))
                    {
                        currentSelector.Id = item.Substring(1).Trim();
                    }
                    else if (item.StartsWith("."))
                    {
                        currentSelector.Classes.Add(item.Substring(1).Trim());
                    }

                    else if (HtmlHelper.Instance.HtmlTags.Contains(item.Trim()))
                    {
                        currentSelector.TagName = item;
                    }
                }
                if (arrStr.Length > 1)
                {
                    Selector newSelector = new Selector();
                    newSelector = currentSelector;
                    currentSelector = new Selector();
                    currentSelector.Parent = newSelector;
                    newSelector.Child = currentSelector;

                }
            }
            return rootSelector;

        }

        public override bool Equals(object? obj)
        {
            if (obj is HtmlElement)
            {
                HtmlElement element = obj as HtmlElement;
                if (element != null)
                {
                    bool isClasses = true;
                    foreach (var c in Classes)
                    {
                        if (!element.Classes.Contains(c))
                            isClasses = false;
                    }
                    return (element.Name.Equals(TagName) || TagName == null) && ((element.Id != null && element.Id.Equals(Id)) || Id == null) && isClasses;
                }
            }
            return false;
        }
    }
}
