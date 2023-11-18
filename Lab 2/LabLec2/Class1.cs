using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;

using Newtonsoft.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Newtonsoft.Json.Linq;

namespace LabLec2

{
    [Serializable, XmlInclude(typeof(NumberException)), XmlInclude(typeof(DiscMinException)), XmlInclude(typeof(DiscZeroException))]
    public class MyException : Exception, ILoggable
    {

        private string systemState;
        private DateTime date;
        private string mymessage;
        public string SystemState { get => systemState; set => systemState = value; }
        public DateTime Date { get => date; set => date = value; }

        public MyException(string message) : base(message)
        {
            date = DateTime.UtcNow;
            mymessage = message;
        }
        public override string Message 
        {
            get { return this.mymessage; }
        }

        public string Mymessage { get => mymessage; set => mymessage = value; }


        public MyException() { }

        public void SaveLogJSON(MyException ex, string path = "log.json")
        {
            
            if (File.Exists(path))
            {
                JArray array;
                using(StreamReader sr = new StreamReader(path))
                using (JsonTextReader reader = new JsonTextReader(sr))
                {

                    array = (JArray)JToken.ReadFrom(reader);
                }


                JObject exceptionJson = new JObject(
                    new JProperty("date", ex.Date.ToString()),
                    new JProperty("name", ex.Source),
                    new JProperty("message", ex.Message),
                    new JProperty("stacktrace", ex.StackTrace),
                    new JProperty("systemState", ex.SystemState)


                    );
                array.Add(exceptionJson);
                string json = array.ToString(Formatting.Indented);
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine(json);
                    sw.Close();
                }

            }
            else
            {
                JArray array = new JArray();

                JObject exceptionJson = new JObject( 
                    new JProperty("date", ex.Date.ToString()),
                    new JProperty("name", ex.Source),
                    new JProperty("message", ex.Message),
                    new JProperty("stacktrace", ex.StackTrace),
                    new JProperty("systemState", ex.SystemState)


                    );
                array.Add (exceptionJson);

                string json = array.ToString();
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine(json);
                    sw.Close();
                }

            }

            
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

                sw.WriteLine($"{ex.Date.ToString()}..|ERROR in: ..|{Console.Title}..|{ex.Message}..|{ex.StackTrace}..|Значение системных параметров:..|{systemState}");
                
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

            if(!File.Exists(path))
            {
                XDocument dox = new XDocument();

                XElement start = new XElement("MyExceptions");
                
                XElement exception = new XElement("MyException");
                exception.Add(new XElement("date", date.ToString()));
                exception.Add(new XElement("name", Console.Title));
                exception.Add(new XElement("message", ex.Message));
                exception.Add(new XElement("stacktrace", ex.StackTrace));
                exception.Add(new XElement("systemState", ex.SystemState));

                start.Add(exception);

                dox.Add(start);

                using (StreamWriter sw = new StreamWriter(path))
                {
                    dox.Save(sw);
                    sw.Close();
                };

            }
            else
            {
                XDocument dox = XDocument.Load(path);

                XElement exception = new XElement("MyException");
                exception.Add(new XElement("date", date.ToString()));
                exception.Add(new XElement("name", Console.Title));
                exception.Add(new XElement("message", ex.Message));
                exception.Add(new XElement("stacktrace", ex.StackTrace));
                exception.Add(new XElement("systemState", ex.SystemState));

                dox.Element("MyExceptions").Add(exception);

                using (StreamWriter sw = new StreamWriter(path))
                {
                    dox.Save(sw);
                    sw.Close();
                };


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
       : base(message) { SystemState = "Некорректное число:"+a; SaveLogTxt(this); SaveLogXML(this); SaveLogJSON(this); }

    }
    internal class DiscMinException : MyException
    {
        private double[] coef;
        public DiscMinException(string message): base(message) { }
        public DiscMinException(string message, double[] coef): base(message) 
        {
            foreach (var a in coef)
                SystemState += "Коэффициент:" +a.ToString();

            SaveLogTxt(this); SaveLogXML(this); SaveLogJSON(this);
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
            SaveLogTxt(this); SaveLogXML(this); SaveLogJSON(this);
        }

        public double[] Coef { get => coef; set => coef = value; }
    }

    internal class MyFileNotFoundException : MyException
    {

        public MyFileNotFoundException(string message)
        : base(message) 
        {
            SaveLogTxt(this); SaveLogXML(this); SaveLogJSON(this);
        }

    }
}
