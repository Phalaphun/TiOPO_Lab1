#define test
using LabLec2;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LabLec1
{
 
    internal class Program
    {
        
        static void Main()
        {
            List<Exception> exList = new List<Exception>();

            Console.Title="Бульбулятор";
            Console.WriteLine("Программа для решения квадратных уравнений с двумя корнями");
            Console.WriteLine("Коэффициенты следует указать в файле *.txt. В каждой строке указать только число - коэффициент");
            Console.WriteLine("Введите путь до файла с коэффициентам." +
                " Если файл лежит в каталоге с программой,\nукажите только название и расширение файла");

#if !test
            //string path = Console.ReadLine() ?? throw new FileNotFoundException("Указан некорректный путь");
            string? path = Console.ReadLine();
#elif test
            string path = "prim.txt";
#endif

            try
            {
                if (!File.Exists(path))
                {
                    FileNotFoundException exception = new FileNotFoundException($"Файла {path} нет. Завершаю работу");

                    throw exception; //Если убрать if то там выбросится все таки другое исключение
                }
            }
            catch (FileNotFoundException e)
            {
                Serialize(e);
                Console.WriteLine(e.Message);
                return;
            }
            StreamReader sr = new StreamReader(path);
            double[] ans = new double[2];
            bool doOut = false;
            double[] coef = new double[3];

            try
            {


                
                int i = 0;
                while (!sr.EndOfStream)
                {
                    if (i == 3)
                    {
                        break;
                    }
                    string? A = sr.ReadLine();
                    if (!double.TryParse(A, out coef[i]))
                    {
                        throw new NumberException($" Коэффициент {A} не был распознан как число", A);
                        
                    }
                    i++;
                }
                Console.WriteLine($"\n\n\nВаше уравнение: {coef[0]}x^2+{coef[1]}x+{coef[2]}");
                double disc = Math.Pow(coef[1], 2) - 4 * coef[0] * coef[2];
                if (disc < 0)
                {
                    throw new DiscMinException($" Дискриминант оказался меньше нуля: {disc}", coef);
                }

                ans[0] = (-coef[1] + Math.Sqrt(disc)) / (2 * coef[1]);
                ans[1] = (-coef[1] - Math.Sqrt(disc)) / (2 * coef[1]);

                if (Math.Abs(disc - 0) < 0.0000001)
                {
                    throw new DiscZeroException($"Дискриминант оказался равен 0", coef);
                }



            }

            catch (NumberException nb)
            {
                Console.WriteLine(nb.Message);
                doOut = true;
            }
            catch (DiscZeroException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Ответ: " + ans[0]);
                doOut = true;
                

            }
            catch (DiscMinException ex)
            {
                Console.WriteLine(ex.Message);
                doOut = true;
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (!doOut)
                {
                    Console.WriteLine("Ответы: ");
                    Console.WriteLine(ans[0]);
                    Console.WriteLine(ans[1]);
                }
                sr.Close();
                SerializeExList(exList);
            }





        }

        private static void SerializeExList(List<Exception> exList)
        {
            if (exList.Count != 0)
            {
                foreach (var item in exList)
                {
                    Serialize(item);

                }
                exList.Clear();
            }
            
        }

        private static void Serialize(Exception ex, string path = "log")
        {
            ToTXT();
            ToXML();
            ToJson();


            void ToTXT()
            {
                using (StreamWriter sw = new StreamWriter(path+".txt", true))
                {

                    sw.WriteLine($"{DateTime.UtcNow.ToString() + " GTM: " + TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours.ToString("+#;-#;0")}" +
                    $"..|ERROR in: ..|{AppDomain.CurrentDomain.FriendlyName}..|Message..|{ex.Message}..|StackTrace..|{ex.StackTrace}..|OSVersion..|{Environment.OSVersion.ToString()}..|WorkingSet..|{Environment.WorkingSet}..|PROCESSOR_ARCHITECTURE..|{Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")}" +
                    $"..|PROCESSOR_IDENTIFIER.||{Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER")}.||Число ядер..|{Environment.ProcessorCount}");

                    sw.Close();
                }
            }
            void ToXML()
            {
                if (!File.Exists(path+".xml"))
                {
                    XDocument dox = new XDocument();

                    XElement start = new XElement("MyExceptions");

                    XElement exception = new XElement("FileNotFoundException");
                    exception.Add(new XElement("date", DateTime.UtcNow.ToString() + " GTM: " + TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours.ToString("+#;-#;0")));
                    exception.Add(new XElement("name", AppDomain.CurrentDomain.FriendlyName));
                    exception.Add(new XElement("message", ex.Message));
                    exception.Add(new XElement("OSVersion", Environment.OSVersion.ToString()));
                    exception.Add(new XElement("WorkingSet", Environment.WorkingSet));
                    exception.Add(new XElement("PROCESSOR_ARCHITECTURE", Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")));
                    exception.Add(new XElement("PROCESSOR_IDENTIFIER", Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER")));
                    exception.Add(new XElement("ProcessorCount", Environment.ProcessorCount));


                    start.Add(exception);

                    dox.Add(start);

                    using (StreamWriter sw = new StreamWriter(path + ".xml"))
                    {
                        dox.Save(sw);
                        sw.Close();
                    };
                }
                else
                {
                    XDocument dox = XDocument.Load(path + ".xml");

                    XElement exception = new XElement("FileNotFoundException");
                    exception.Add(new XElement("date", DateTime.UtcNow.ToString() + " GTM: " + TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours.ToString("+#;-#;0")));
                    exception.Add(new XElement("name", AppDomain.CurrentDomain.FriendlyName));
                    exception.Add(new XElement("message", ex.Message));
                    exception.Add(new XElement("OSVersion", Environment.OSVersion.ToString()));
                    exception.Add(new XElement("WorkingSet", Environment.WorkingSet));
                    exception.Add(new XElement("PROCESSOR_ARCHITECTURE", Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")));
                    exception.Add(new XElement("PROCESSOR_IDENTIFIER", Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER")));
                    exception.Add(new XElement("ProcessorCount", Environment.ProcessorCount));

                    //ex.Source

                    dox.Element("MyExceptions").Add(exception);

                    using (StreamWriter sw = new StreamWriter(path + ".xml"))
                    {
                        dox.Save(sw);
                        sw.Close();
                    };


                }
            }
            void ToJson()
            {
                if (File.Exists(path+".json"))
                {
                    JArray array;
                    using (StreamReader sr = new StreamReader(path + ".json"))
                    using (JsonTextReader reader = new JsonTextReader(sr))
                    {

                        array = (JArray)JToken.ReadFrom(reader);
                    }


                    JObject exceptionJson = new JObject(
                        new JProperty("date", DateTime.UtcNow.ToString() + " GTM: " + TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours.ToString("+#;-#;0")),
                        new JProperty("name", AppDomain.CurrentDomain.FriendlyName),
                        new JProperty("message", ex.Message),
                        new JProperty("stacktrace", ex.StackTrace),
                        new JProperty("OSVersion", Environment.OSVersion.ToString()),
                        new JProperty("WorkingSet", Environment.WorkingSet),
                        new JProperty("PROCESSOR_ARCHITECTURE", Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")),
                        new JProperty("PROCESSOR_IDENTIFIER", Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER")),
                        new JProperty("ProcessorCount", Environment.ProcessorCount)


                        );
                    array.Add(exceptionJson);
                    string json = array.ToString(Formatting.Indented);
                    using (StreamWriter sw = new StreamWriter(path + ".json"))
                    {
                        sw.WriteLine(json);
                        sw.Close();
                    }

                }
                else
                {
                    JArray array = new JArray();

                    JObject exceptionJson = new JObject(
                        new JProperty("date", DateTime.UtcNow.ToString() + " GTM: " + TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours.ToString("+#;-#;0")),
                        new JProperty("name", AppDomain.CurrentDomain.FriendlyName),
                        new JProperty("message", ex.Message),
                        new JProperty("stacktrace", ex.StackTrace),
                        new JProperty("OSVersion", Environment.OSVersion.ToString()),
                        new JProperty("WorkingSet", Environment.WorkingSet),
                        new JProperty("PROCESSOR_ARCHITECTURE", Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")),
                        new JProperty("PROCESSOR_IDENTIFIER", Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER")),
                        new JProperty("ProcessorCount", Environment.ProcessorCount)


                        );
                    array.Add(exceptionJson);

                    string json = array.ToString();
                    using (StreamWriter sw = new StreamWriter(path + ".json"))
                    {
                        sw.WriteLine(json);
                        sw.Close();
                    }

                }
            }
        }



        // Что произошло
        // Когда произошло
        // Имя приложения
        // Trace log
        // Внутренние параметры
        // Реализуем интерфейс ILogSealed - будет 1 метод - SaveLog (Принимает тип ошибки). Пишем две реализации интерфейса - savelogtext savelog xml + куда сохранять нужно продумать.
        // Файл может быть занят, предусмотреть это в отдельном потоке. 
        // Событие перед крашем системы - поискать про эту штуку. 
        // 


        // По факту возникновения любой ошибки реализовать логирование xml txt 
        
    }

   
}

/*
 * Делаем лабу где решаем квадратное уравнение через дискриминант
 * При решении квадратного лвл учитываем 2 параметра: 
 * Дискриминант, если боль 0 то два корня, если 0, то корень один, иначе корней нет
 * Кодим программку которая решает, абс читаем с файла.
 * 1)Пишем кастомную ошибку если Д меньше 0
 * 2) Обрабатываем ошибку если файл отсутствует
 * 3) Когда открываем файл и при парсинге у нас пошли ошибки - тоже кастомная (типо читаем строку и если там не цифра то кастомная ошибка)
 * 4)Если д=0, то тоже кастомную ошибку выкидываем, но в тесте ошибки тупо Уравнение имеет 1 корень.
 */