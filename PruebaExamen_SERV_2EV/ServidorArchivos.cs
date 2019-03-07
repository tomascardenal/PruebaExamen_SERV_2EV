using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaExamen_SERV_2EV
{
    class ServidorArchivos
    {
        public string LeeArchivo(string nombreArchivo, int nLineas)
        {
            string result = "";
            int count = 0;
            try
            {
                using (StreamReader sr = new StreamReader(Path.Combine(Environment.GetEnvironmentVariable("EXAMEN"), nombreArchivo)))
                {
                    while (sr.Peek() != -1 && count <= nLineas)
                    {
                        result += sr.ReadLine();
                        count++;
                    }
                }
            }
            catch (IOException)
            {
                return "<ERROR_IO>";
            }
            return result;
        }

        public int LeePuerto()
        {
            string firstLine = LeeArchivo("puerto.txt", 1);
            int port;
            if (int.TryParse(firstLine, out port))
            {
                return port;
            }
            return 31415;
        }

        public bool GuardaPuerto(int numero)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(Environment.GetEnvironmentVariable("EXAMEN"), "puerto.txt"), false))
                {
                    sw.WriteLine(numero);
                }
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        public string listaArchivos()
        {
            string fileList = "";
            string dir = Environment.GetEnvironmentVariable("EXAMEN");
            string[] files = Directory.GetFileSystemEntries(dir, "*.txt");
            foreach (string s in files)
            {
                fileList += s + "\n";
            }
            return fileList;
        }
    }
}
