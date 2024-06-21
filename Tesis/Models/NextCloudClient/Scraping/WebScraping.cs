using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesis.Models.NextCloudClient.Scraping;

namespace NexCloudClient.Scraping
{


    public class WebScraping
    {
        private string _html;
        private Dictionary<string, HtmlEtiq> _etiqs = new Dictionary<string, HtmlEtiq>();

        public WebScraping(string html)
        {
            _html = html;
            ParseEtiqs();
        }

        private void ParseEtiqs()
        {

        }


        public HtmlEtiq Fin(string etiq, Dictionary<string, string> attribs = null)
        {
            string[] etiqs = _html.Split('>');
            for (int j = 0; j < etiqs.Length; j++)
            {
                string etq = etiqs[j];
                try
                {
                    string etiqueta = etq.Replace("\r", "").Replace("\n", "").Replace("\"", "").Replace("\t", "");
                    if (etiqueta[0] == '<')
                    {
                        //Etiqueta Valida
                        etiqueta = etiqueta.Replace("<", "");
                        string[] spliter = etiqueta.Split(' ');
                        string name = spliter[0];
                        if (name == etiq)
                        {

                            if (attribs != null)
                            {
                                bool containAttrib = false;
                                foreach (var attr in attribs)
                                {
                                    if (etiqueta.Contains(attr.Value))
                                    {
                                        containAttrib = true;
                                        break;
                                    }
                                }
                                if (!containAttrib) continue;
                            }
                            HtmlEtiq newetq = new HtmlEtiq(name);
                            for (int i = 1; i < spliter.Length; i++)
                            {
                                string[] values = spliter[i].Split(new char[] { '=' }, 2);
                                string key = "";
                                string val = "";
                                try
                                {
                                    key = values[0];
                                    val = values[1];
                                }
                                catch { }
                                if (key != "")
                                    newetq.AddAttrib(key, val);
                            }
                            string content = etiqs[j + 1].Replace("\n", "").Replace("\t", "");
                            if (content[0] != '<')
                            {
                                newetq.Content = content.Split('<')[0];
                            }
                            return newetq;
                        }
                    }
                }
                catch { }
            }
            return null;
        }



    }
}
