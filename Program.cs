using PractiCod2;
using System.Text.RegularExpressions;
var html = await Load("https://hebrewbooks.org/");
var html2 = "<!doctype html>\r\n<html id=\"blablabla\"  class=\"my-class-1 my-class-2\"  width=\"100%\" height=\"50%\" lang=\"en\">\r\n<head>\r\n  <meta charset=\"utf-8\">\r\n  <title>BuildingCurriculumClient</title>\r\n  <base href=\"/\">\r\n  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n  <link rel=\"icon\" type=\"image/x-icon\" href=\"favicon.ico\">\r\n</head>\r\n<body>\r\n  <app-root></app-root>\r\n</body>\r\n</html>\r\n";
var htmll = "<html id=\"myId\"  class=\"my-class-1 my-class-2\"  width=\"100%\" height=\"50%\" lang=\"en\">\r\n<head>\r\n  \r\n  <title>PracticodProject</title>\r\n\r\n \r\n</head>\r\n<body class=\"mat\">\r\n </body>\r\n</html>";
var cleanHtml1 = new Regex("//s").Replace(htmll, "");
var htmlLines11 = new Regex("<(.*?)>").Split(cleanHtml1).Where(s => s.Trim().Length > 0);
var htmlLinesInArray = htmlLines11.ToArray();
var htmlLines1 = new Regex("<[^>]+>").Matches(cleanHtml1).Cast<Match>().Select(m => m.Value);
var htmlElement = "<div id=\"my-id\" class=\"my-class-1 my-class-2\" width=\"100%\">text</div>";
var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlElement);
var cleanHtml = Regex.Replace(htmll, @"\s+", " "); // Replace multiple spaces with a single space
var htmlLines = Regex.Split(cleanHtml, @"(</?[^>]+>)").Where(s => !string.IsNullOrWhiteSpace(s));
HtmlElement currentElement = null;
HtmlElement root = new HtmlElement();
HtmlElement[] elements = new HtmlElement[htmlLinesInArray.Length];
List<HtmlElement> htmlElements = new List<HtmlElement>();
Stack<HtmlElement> elementStack = new Stack<HtmlElement>();
int count = -1;
for (int i = 0; i < htmlLinesInArray.Length; i++)
{
    var tagName = htmlLinesInArray[i].Split(" ")[0];
    if (HtmlHelper.Instance.HtmlTags.Contains(tagName))
    {
        currentElement = new HtmlElement();
        currentElement.Name = tagName;
        var elementAtrributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlLinesInArray[i]);
        foreach (Match match in elementAtrributes)
        {
            if (match.Success)
            {
                string attributeName = match.Groups[1].Value;
                if (attributeName == "id")
                    currentElement.Id = match.Groups[2].Value;
                else if (attributeName == "class")
                {
                    var classPattern = new Regex(@"class=""([^""]*)""").Matches(htmlLinesInArray[i]);
                    foreach (Match item in classPattern)
                    {
                        if (item.Success && item.Groups.Count >= 2)
                        {
                            string classesValue = item.Groups[1].Value;
                            currentElement.Classes = classesValue.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            //Console.WriteLine("Classes:");
                            //foreach (string className in currentElement.Classes)
                            //    Console.WriteLine(className);
                        }
                    }
                }
                else
                {
                    currentElement.Attributes.Add(match.Groups[0].Value);
                }
            }
        }
        elements[++count] = currentElement;
    }
    else if (tagName == "/html")
        break;
    else if (tagName.StartsWith("/"))
    {
        currentElement.Parent = elements[count - 1];
        elements[count - 1].Children.Add(currentElement);
        //currentElement.Parent.Children.Add(currentElement);
        currentElement = currentElement.Parent;
        count--;
    }
    else
    {
        if (i != 0)
            currentElement.InnerHtml = htmlLinesInArray[i];
    }
    if (tagName == "html")
    {
        root = elements[0];
    }
}

string query = "div #mydiv .class-name .class2 .class 3";



var query1 = "div #mydiv .class-name";
string s = "div #mydiv .class-name #arr .dd";

var selector = Selector.Parse(query1);

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
var s1 = "html#myId.my-class-1 body";
var s2 = "head title";
var s3 = "html.my-class-1.my-class-2";
var list = root.ElementsBySelectors(Selector.Parse(s1), true);
var hashList = new HashSet<HtmlElement>(list);
Console.WriteLine("count of elements : " + hashList.Count());
foreach (var item in hashList)
{
    Console.WriteLine("the element : " + item.Name);
}