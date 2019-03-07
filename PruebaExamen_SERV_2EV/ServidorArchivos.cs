using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PruebaExamen_SERV_2EV
{
    class ServidorArchivos
    {

        public bool Run = true;
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

        public string ListaArchivos()
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

        public void IniciaServidorArchivos()
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Any, LeePuerto());
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                serverSocket.Bind(ipe);
            }
            catch (SocketException)
            {
                Console.WriteLine("El puerto " + ipe.Port + " no se puede utilizar");
                Run = false;
            }
            while (Run)
            {
                serverSocket.Listen(10);
                Socket client = serverSocket.Accept();
                IPEndPoint clientEndPoint = (IPEndPoint)client.RemoteEndPoint;
                Console.WriteLine("Conectado cliente: ip={0} puerto={1}", clientEndPoint.Address, clientEndPoint.Port);
                Thread th = new Thread(() => hiloCliente(client));
                th.Start();
            }
        }

        public void hiloCliente(object socket)
        {
            NetworkStream ns = null;
            StreamReader reader = null;
            StreamWriter writer = null;
            string msg, args;
            bool run = true;
            try
            {
                ns = new NetworkStream((Socket)socket);
                reader = new StreamReader(ns);
                writer = new StreamWriter(ns);
                writer.WriteLine("CONEXION ESTABLECIDA");
                while (run)
                {
                    msg = reader.ReadLine();
                    if (msg != null)
                    {
                        string command = msg.Substring(0, msg.IndexOf(" "));
                        switch (command)
                        {
                            case "GET":
                                args = msg.Substring(5);
                                string arg1 = args.Substring(0, args.IndexOf(","));
                                string arg2Text = args.Substring(args.IndexOf(",") + 1);
                                int arg2;
                                if (int.TryParse(arg2Text, out arg2))
                                {
                                    writer.WriteLine(LeeArchivo(arg1, arg2));
                                }
                                break;
                            case "PORT":
                                break;
                            case "LIST":
                                break;
                            case "CLOSE":
                                break;
                            case "HALT":
                                break;
                        }
                    }
                    else
                    {
                        run = false;
                    }
                }
            }
            catch (IOException e)
            {

            }

        }
    }
}
