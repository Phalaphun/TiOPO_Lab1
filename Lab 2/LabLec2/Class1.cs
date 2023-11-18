using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;


using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

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


        //public MyException(SerializationInfo info, StreamingContext context) :base (info.GetString("_message"))
        //{
        //    if(info!=null)
        //    {
        //        this.systemState = info.GetString("systemState");
        //        this.date = info.GetDateTime("date");
        //    }
        //}

        public MyException() { }

        public void SaveLogJSON(MyException ex, string path = "log.json")
        {
            //using (FileStream fs = new FileStream(path, FileMode.Append))
            //{
            //    JsonSerializerOptions options = new JsonSerializerOptions()
            //    {
            //        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), // Вот эта строка Вам поможет с кодировкой
            //        WriteIndented = true,
            //    };


            //    JsonSerializer.Serialize<MyException>(fs, this, options);
            //    Console.WriteLine("Data has been saved to file");
            //}


            if (File.Exists(path))
            {
                string json = "";
                using (StreamReader sr = new StreamReader(path))
                {
                    json = sr.ReadLine();
                }
                List<MyException> list = JsonSerializer.Deserialize<List<MyException>>(json);
                list.Add(ex);

                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    JsonSerializerOptions options = new JsonSerializerOptions()
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), // Вот эта строка Вам поможет с кодировкой
                        //WriteIndented = true,
                    };

                   

                    JsonSerializer.Serialize<List<MyException>>(fs, list, options);
                    Console.WriteLine("Data has been saved to file");
                }
            }
            else
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    JsonSerializerOptions options = new JsonSerializerOptions()
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), // Вот эта строка Вам поможет с кодировкой
                        //WriteIndented = true,
                    };

                    List<MyException> list = new List<MyException> { ex};

                    JsonSerializer.Serialize<List<MyException>>(fs, list, options);
                    Console.WriteLine("Data has been saved to file");
                }
            }

            //if (File.Exists(path))
            //{
            //    string json = "";
            //    using(StreamReader sr = new StreamReader(path))
            //    {
            //        json = sr.ReadLine();
            //    }
            //    var list = JsonConvert.DeserializeObject<List<MyException>>(json);
            //    list.Add(ex);

            //    JsonSerializer serializer = new JsonSerializer();
            //    serializer.Converters.Add(new JavaScriptDateTimeConverter());
            //    serializer.NullValueHandling = NullValueHandling.Include;
                

            //    using (StreamWriter sw = new StreamWriter(path))
            //    using (JsonWriter writer = new JsonTextWriter(sw))
            //    {
            //        serializer.Serialize(writer, list);
            //    }
            //}
            //else
            //{
            //    JsonSerializer serializer = new JsonSerializer();
            //    serializer.Converters.Add(new JavaScriptDateTimeConverter());
            //    serializer.NullValueHandling = NullValueHandling.Include;
            //    List<MyException> list = new List<MyException> {ex };

            //    using (StreamWriter sw = new StreamWriter(path))
            //    using (JsonWriter writer = new JsonTextWriter(sw))
            //    {
            //        serializer.Serialize(writer, list);
            //    }
            //}
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

                dox.Save(path);
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
