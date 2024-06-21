using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis.Models.NextCloudClient.Scraping
{
    public class HtmlEtiq
    {
        public string Name { get; protected set; }
        public string Content { get; set; }
        public Dictionary<string, string> Attribs { get; set; }

        public HtmlEtiq(string name)
        {
            Name = name;
            Attribs = new Dictionary<string, string>();
        }

        public void AddAttrib(string key, string val)
        {
            try
            {
                Attribs.Add(key, val);
            }
            catch { }
        }
        public string GetAttrib(string key)
        {
            if (Attribs.TryGetValue(key, out string val))
                return val;
            return "";
        }
    }
}
