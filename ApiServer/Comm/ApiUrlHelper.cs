using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace ApiServer.Comm;

public class ApiUrlHelper
{
    public List<ApiUrlModel> GetApiUrls()
    {
        List<XmlModel> xmlurls = new List<XmlModel>();
        XElement.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "API.xml")).Elements().First(l => l.Name == "members").Elements().Where(l => l.Attribute("name").Value.Contains("M:ApiServer.Controllers.")).ToList().ForEach(l =>
        {
            xmlurls.Add(new XmlModel
            {
                Method = l.Attribute("name").Value.Split(':')[1].Split('(')[0],
                Tag = ((XElement)(l.Nodes().FirstOrDefault(k => k.NodeType == XmlNodeType.Element && ((XElement)k).Name.LocalName == "summary"))).Value.Trim(),
            });
        });

        List<ApiUrlModel> apiurls = new List<ApiUrlModel>();
        Assembly.GetExecutingAssembly().ExportedTypes.ForEach(type =>
        {
            string s = "";
            if (Attribute.IsDefined(type, typeof(RouteAttribute)))
            {
                RouteAttribute attribute = (RouteAttribute)Attribute.GetCustomAttribute(type, typeof(RouteAttribute));
                s = attribute.Template;
            }
            type.GetMethods().ForEach(methodInfo =>
            {
                if (Attribute.IsDefined(methodInfo, typeof(HttpPostAttribute)))
                {
                    HttpPostAttribute apiRouteAtt = (HttpPostAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(HttpPostAttribute));
                    string name = type.FullName + "." + methodInfo.Name;
                    apiurls.Add(new ApiUrlModel
                    {
                        Url = "/" + s.Trim('/') + "/" + apiRouteAtt.Template.Trim('/'),
                        Name = xmlurls.FirstOrDefault(l => l.Method == name)?.Tag,
                    });
                }
                else if (Attribute.IsDefined(methodInfo, typeof(RouteAttribute)))
                {
                    RouteAttribute apiRouteAtt = (RouteAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(RouteAttribute));
                    string name = type.FullName + "." + methodInfo.Name;
                    apiurls.Add(new ApiUrlModel
                    {
                        Url = "/" + s.Trim('/') + "/" + apiRouteAtt.Template.Trim('/'),
                        Name = xmlurls.FirstOrDefault(l => l.Method == name)?.Tag,
                    });
                }
                else  if (Attribute.IsDefined(methodInfo, typeof(HttpGetAttribute)))
                {
                    HttpGetAttribute apiRouteAtt = (HttpGetAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(HttpGetAttribute));
                    string name = type.FullName + "." + methodInfo.Name;
                    apiurls.Add(new ApiUrlModel
                    {
                        Url = "/" + s.Trim('/') + "/" + apiRouteAtt.Template.Trim('/'),
                        Name = xmlurls.FirstOrDefault(l => l.Method == name)?.Tag,
                    });
                }
                else if (Attribute.IsDefined(methodInfo, typeof(HttpHeadAttribute)))
                {
                    HttpHeadAttribute apiRouteAtt = (HttpHeadAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(HttpHeadAttribute));
                    string name = type.FullName + "." + methodInfo.Name;
                    apiurls.Add(new ApiUrlModel
                    {
                        Url = "/" + s.Trim('/') + "/" + apiRouteAtt.Template.Trim('/'),
                        Name = xmlurls.FirstOrDefault(l => l.Method == name)?.Tag,
                    });
                }
                else if (Attribute.IsDefined(methodInfo, typeof(HttpOptionsAttribute)))
                {
                    HttpOptionsAttribute apiRouteAtt = (HttpOptionsAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(HttpOptionsAttribute));
                    string name = type.FullName + "." + methodInfo.Name;
                    apiurls.Add(new ApiUrlModel
                    {
                        Url = "/" + s.Trim('/') + "/" + apiRouteAtt.Template.Trim('/'),
                        Name = xmlurls.FirstOrDefault(l => l.Method == name)?.Tag,
                    });
                }
                else if (Attribute.IsDefined(methodInfo, typeof(HttpPatchAttribute)))
                {
                    HttpPatchAttribute apiRouteAtt = (HttpPatchAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(HttpPatchAttribute));
                    string name = type.FullName + "." + methodInfo.Name;
                    apiurls.Add(new ApiUrlModel
                    {
                        Url = "/" + s.Trim('/') + "/" + apiRouteAtt.Template.Trim('/'),
                        Name = xmlurls.FirstOrDefault(l => l.Method == name)?.Tag,
                    });
                }
                else if (Attribute.IsDefined(methodInfo, typeof(HttpPutAttribute)))
                {
                    HttpPutAttribute apiRouteAtt = (HttpPutAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(HttpPutAttribute));
                    string name = type.FullName + "." + methodInfo.Name;
                    apiurls.Add(new ApiUrlModel
                    {
                        Url = "/" + s.Trim('/') + "/" + apiRouteAtt.Template.Trim('/'),
                        Name = xmlurls.FirstOrDefault(l => l.Method == name)?.Tag,
                    });
                }
                else if (Attribute.IsDefined(methodInfo, typeof(HttpDeleteAttribute)))
                {
                    HttpDeleteAttribute apiRouteAtt = (HttpDeleteAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(HttpDeleteAttribute));
                    string name = type.FullName + "." + methodInfo.Name;
                    apiurls.Add(new ApiUrlModel
                    {
                        Url = "/" + s.Trim('/') + "/" + apiRouteAtt.Template.Trim('/'),
                        Name = xmlurls.FirstOrDefault(l => l.Method == name)?.Tag,
                    });
                }
            });
        });
        return apiurls;
    }
}

public class XmlModel
{
    public string Method { get; set; }

    public string Tag { get; set; }
}

public class ApiUrlModel
{
    public string Url { get; set; }

    public string Name { get; set; }
}