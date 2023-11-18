using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace LabLec2

{
    [Serializable, XmlInclude(typeof(NumberException)), XmlInclude(typeof(DiscMinException)), XmlInclude(typeof(DiscZeroException))]
    public class MyException : Exception, ILoggable
    {
        private string systemState;
        public string SystemState { get => systemState; set => systemState = value; }

        public MyException(string message) : base(message)
        {
           
        }
        public MyException() { }

        public void SaveLogJSON(MyException ex, string path = "log.xml")
        {
            throw new NotImplementedException();
        }

        public void SaveLogTxt(MyException ex, string path = "log.txt")
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Файл не найден, идёт запись по умолчанию");
                path = "log.txt";
            }

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                //sw.WriteLine("Ошибка");
                //sw.WriteLine(Console.Title);
                //sw.WriteLine($"{DateTime.UtcNow}: {ex.Message}");
                //sw.WriteLine(ex.StackTrace);
                //sw.WriteLine("Значение системных параметров:");
                //foreach (DictionaryEntry d in ex.Data)
                //    sw.WriteLine("-> {0} {1}", d.Key, d.Value);
                //sw.WriteLine("Конец Ошибки");

                //sw.Close();

                sw.WriteLine($"{DateTime.UtcNow}..|ERROR in: ..|{Console.Title}..|{ex.Message}..|{ex.StackTrace}..|Значение системных параметров:..|{systemState}");
                
                //sw.WriteLine("Ошибка");
                //sw.WriteLine(Console.Title);
                //sw.WriteLine($"{DateTime.UtcNow}: {ex.Message}");
                //sw.WriteLine(ex.StackTrace);
                //sw.WriteLine("Значение системных параметров:");
                ////foreach (DictionaryEntry d in ex.Data)
                ////    sw.WriteLine("-> {0} {1}", d.Key, d.Value);
                //sw.WriteLine("Конец Ошибки");

                sw.Close();



            }
        }

        public void SaveLogXML(MyException ex, string path = "log.xml")
        {
            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(MyException));
            //using (FileStream fs = new FileStream("log.xml", FileMode.Append))
            //{
            //    xmlSerializer.Serialize(fs, ex);

            //    Console.WriteLine("Object has been serialized");
            //}

            //XmlDocument xmlDocument = new XmlDocument();
            //xmlDocument.Load(path);
            //XmlNode rootNode = xmlDocument.CreateElement("MyException");
            //xmlDocument.AppendChild(rootNode);

            //XmlNode dateNode = xmlDocument.CreateElement("DateNode");
            //dateNode.InnerText = DateTime.Now.ToString();
            //rootNode.AppendChild(dateNode);

            //XmlNode nameNode = xmlDocument.CreateElement("NameNode");
            //nameNode.InnerText = Console.Title;
            //rootNode.AppendChild(nameNode);

            //XmlNode messageNode = xmlDocument.CreateElement("MessageNode");
            //messageNode.InnerText = ex.Message;
            //rootNode.AppendChild(messageNode);

            //XmlNode stackTraceNode = xmlDocument.CreateElement("stackTrace");
            //stackTraceNode.InnerText = ex.StackTrace;
            //rootNode.AppendChild(stackTraceNode);

            //XmlNode systemStateNode = xmlDocument.CreateElement("systemStateNode");
            //systemStateNode.InnerText = ex.SystemState;
            //rootNode.AppendChild(systemStateNode);

            //xmlDocument.Save(path);
            if(!File.Exists(path))
            {
                XDocument dox = new XDocument();

                XElement start = new XElement("MyExceptions");
                

                XElement exception = new XElement("MyException");
                exception.Add(new XElement("date", DateTime.Now.ToString()));
                exception.Add(new XElement("name", Console.Title));

                start.Add(exception);

                dox.Add(start);

                //dox.Element("MyExceptions").Add(exception);


                dox.Save(path);
            }
            else
            {
                XDocument dox = XDocument.Load(path);
                XElement root = new XElement("MyException");
                root.Add(new XElement("date", DateTime.Now.ToString()));
                root.Add(new XElement("name", Console.Title));
                dox.Element("MyExceptions").Add(root);
                dox.Save(path);


            }

        }
    }

   
    internal class NumberException : MyException
    {
        
        public NumberException(string message)
        : base(message)
        {

            SaveLogTxt(this);
            SaveLogXML(this);
        }
        public NumberException(string message, string a)
       : base(message) { SystemState = a; SaveLogTxt(this); SaveLogXML(this); }

    }
    internal class DiscMinException : MyException
    {
        private double[] coef;
        public DiscMinException(string message): base(message) { }
        public DiscMinException(string message, double[] coef): base(message) 
        {
            foreach (var a in coef)
                SystemState += "Коэффициент:" +a.ToString();
                
        }

        public double[] Coef { get => coef; set => coef = value; }
    }
    internal class DiscZeroException : MyException
    {
        private double[] coef;
        public DiscZeroException(string message)
        : base(message) { }
        public DiscZeroException(string message, double[] coef)
        : base(message)
        {
            foreach (var a in coef)
                SystemState += "Коэффициент:" + a.ToString();
        }

        public double[] Coef { get => coef; set => coef = value; }
    }
}
