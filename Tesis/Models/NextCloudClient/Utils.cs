using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tesis.Models.NextCloudClient
{
    public class Utils
    {
        public static string sizeof_fmt(double len)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }
        public static string time_fmt(double seconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            return string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                t.Hours,
                t.Minutes,
                t.Seconds,
                t.Milliseconds);
        }
        public static string progres_bar(string blanco, string negro, double index, double max, int size = 21, int step_size = 5)
        {



            string bar = "";
            try
            {
                if (max < 1) max += 1;
                var porcent = index / max;
                porcent *= 100;
                porcent = Math.Round(porcent);
                var index_make = 1;
                while (index_make < size)
                {
                    if (porcent >= index_make * step_size)
                        bar += negro;
                    else
                        bar += blanco;
                    index_make += 1;
                }
            }
            catch { }

            return bar;
        }



        public static string stringBetween(string Source, string Start, string End)
        {
            string result = "";
            if (Source.Contains(Start) && Source.Contains(End))
            {
                int StartIndex = Source.IndexOf(Start, 0) + Start.Length;
                int EndIndex = Source.IndexOf(End, StartIndex);
                result = Source.Substring(StartIndex, EndIndex - StartIndex);
                return result;
            }

            return result;
        }



        string CompareDate(string fecha)
        {
            if (!fecha.Equals(""))
            {
                // actual date
                var hoy = DateTime.Now.ToString("yyyy-MM-dd");

                var hoyy = DateTime.Now.ToString("yyyy");
                var hoym = DateTime.Now.ToString("MM");
                var hoyd = DateTime.Now.ToString("dd");

                //date to compare

                string an = fecha.Substring(0, 4);

                string me = fecha.Substring(5, 2);

                string di = fecha.Substring(8, 2);



                string sTexto = me.Substring(0, 1);

                var meslocal = Removefirstifcero(me, sTexto);

                sTexto = di.Substring(0, 1);

                var dialocal = Removefirstifcero(di, sTexto);

                sTexto = hoym.Substring(0, 1);

                var mesactual = Removefirstifcero(hoym, sTexto);

                sTexto = hoyd.Substring(0, 1);

                var diaactual = Removefirstifcero(hoyd, sTexto);



                DateTime date1 = new DateTime(Convert.ToInt16(hoyy), Convert.ToInt16(hoym), Convert.ToInt16(hoyd), 0, 0, 0);
                DateTime date2 = new DateTime(Convert.ToInt16(an), Convert.ToInt16(meslocal), Convert.ToInt16(dialocal), 0, 0, 0);


                int result = DateTime.Compare(date2, date1);
                string relationship;

                if (result < 0)
                    relationship = "is earlier than";
                else if (result == 0)
                    relationship = "is the same time as";
                else
                    relationship = "is later than";


                // The example displays the following output for en-us culture:
                //    8/1/2009 12:00:00 AM is earlier than 8/1/2009 12:00:00 PM

                return relationship;
            }
            else
                return "";
        }
        string Removefirstifcero(string cadena, string comparar)
        {

            if (comparar.Equals("0"))
            {

                cadena = cadena.Substring(1);
            }

            return cadena;


        }


        //true es debil // false es fuerte

        public Boolean ContrasenaSegura(String contraseñaSinVerificar)
        {
            string clave = contraseñaSinVerificar;
            bool debil = false;
            Regex caracEsp = new Regex("[!\"#\\$%&'()*+,-./:;=?@\\[\\]^_`{|}~]");
            if (!string.IsNullOrWhiteSpace(clave))
            {
                debil = (!Regex.IsMatch(clave, @"([a-z])")) ? true : debil;

                debil = (!Regex.IsMatch(clave, @"([A-Z])")) ? true : debil;

                debil = (!Regex.IsMatch(clave, @"([0-9])")) ? true : debil;

                //debil = (!clave.Contains("!")) ? true : debil;

                debil = (clave.Length < 7) ? true : debil;


                //si no contiene los caracteres especiales, regresa false
                if (!caracEsp.IsMatch(contraseñaSinVerificar))
                {
                    debil = true;
                }

            }
            else
            {
                debil = false;
            }

            if (debil)
            {
                return debil;
            }
            else
            {
                //Imprimo clave fuerte 
                return debil;
            }
        }




        string Gentxt(string ruta, List<string> lineas)
        {

            string path = ruta;

            if (File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {


                    for (int i = 0; i < lineas.Count; i++)
                    {
                        sw.WriteLine(lineas[i]);
                    }


                }

                return path + ".txt";
            }



            return path + ".txt";



        }



        void RenameExtension(string source, string destiny, string extension)
        {


            string ruta = System.IO.Path.GetDirectoryName(source);
            string nombre = System.IO.Path.GetFileNameWithoutExtension(source);
            nombre = nombre.Replace(".", "");
            string word1 = nombre.Substring(0, 1);
            string result = System.IO.Path.ChangeExtension(System.IO.Path.GetExtension(source), extension);



            File.Move(source, ruta + "\\" + nombre + extension);


        }

    }
}
