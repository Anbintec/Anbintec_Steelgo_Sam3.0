using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace SecurityManager.Login
{
    public class Logger
    {
        private static readonly object _mutex = new object();
        private static Logger _instance;

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static Logger Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new Logger();
                    }
                }
                return _instance;
            }
        }
        private string fileName;

        // si pasamos la ruta de un archivo, su utilizará ese para hacer el log
        //public LoggerBd(string file) {
        //    fileName = file;
        //}

        // caso contrario se utiliza uno por defecto
        public Logger()
        {
            fileName = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["dirLogger"] + "log.txt");
        }

        public void EscribirLog(string logText)
        {
            try
            {
                using (StreamWriter w = File.AppendText(fileName))
                {
                    w.WriteLine(DateTime.Now.ToString() + " - " + logText);
                }
            }
            catch { }
        }

        public void EscribirLog(Exception ex)
        {
            try
            {
                using (StreamWriter w = File.AppendText(fileName))
                {
                    w.WriteLine("--------------------------------------------------------------------------------");
                    w.WriteLine("--------------------------------------------------------------------------------");
                    w.WriteLine(DateTime.Now.ToString() + " - EXCEPCION");
                    w.WriteLine("Message: " + ex.Message);
                    w.WriteLine("Source: " + ex.Source);
                    w.WriteLine("TargetSite: " + ex.TargetSite);
                    w.WriteLine("StackTrace: " + ex.StackTrace);
                    w.WriteLine("InnerException: " + ex.InnerException);
                    w.WriteLine("Exception Data: " + ex.Data);
                    w.WriteLine("--------------------------------------------------------------------------------");
                    w.WriteLine("--------------------------------------------------------------------------------");
                }
            }
            catch (Exception err)
            {
 
            }
        }
    }
}