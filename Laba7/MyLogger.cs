using System.Diagnostics;
using System.Reflection;

namespace Laba7
{


    public static class MyLogger  
    {
        public static void WriteLog(string message, string type, string path="log.txt", string[] state = null)
        {
            string additionalInfo = "";
            if (state != null)
            {
                for(int i=0; i<state.Length; i += 2)
                {
                    additionalInfo += state[i] + "..." + state[i+1];
                }
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    string text = $"Time|{DateTime.UtcNow + " GTM: " + TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours.ToString("+#;-#;0")}|Type|{type.ToUpper()}|" +
                        $"Message|{message}|Stacktrace|{new StackTrace(1,true)}|OSversion|{Environment.OSVersion}|Program|{Assembly.GetExecutingAssembly().GetName().Name}|" +
                        $"Version|{Assembly.GetExecutingAssembly().GetName().Version}|" +
                        $"additionalInfo|{additionalInfo}|"+"ENDLINE..ENDLINE";
                    sw.WriteLine(text);
                    sw.Close();
                }
            }

            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        //static List<Task> tasks = new List<Task>();
        ////public static async void WriteInfo1(string message, string path = "log.txt", string[] systemstate = null)
        ////{

            


        ////    using (StreamWriter writer = new StreamWriter(path, true))
        ////    {
               
        ////        await writer.WriteLineAsync(message);



        ////        writer.Close();

        ////    }
        ////}

        //public static void WriteInfo1(string message, string path = "log.txt", string[] systemstate = null)
        //{

        //    Task task = new Task(Writer(message));

        //}

        //void Writer(string message, string path = "log.txt", string[] systemstate = null)
        //{
        //    int counter = 0;
        //    while (true)
        //    {
        //        try
        //        {
        //            StreamWriter sw = new StreamWriter(path, true);
        //            sw.WriteLine(message);
        //        }
        //        catch (IOException ex)
        //        {
        //            if(counter >= 5)
        //            {
        //                Console.WriteLine("Не удалось записать логи. Не поулчен доступ к файлу");
        //                return;
        //            }
        //            counter++;
        //            Thread.Sleep(1000);
        //        } 
        //    }
        //}

        ////public static async void WriteInfo2(string message, string path = "log.txt", string[] systemstate = null)
        ////{
        ////    UnicodeEncoding uniencoding = new UnicodeEncoding();
        ////    byte[] text = uniencoding.GetBytes(message);

        ////    using (FileStream SourceStream = File.Open(path, FileMode.Append))
        ////    {
        ////        //SourceStream.Seek(0, SeekOrigin.End);
        ////        SourceStream.WriteTimeout = 2000;
        ////        await SourceStream.WriteAsync(text, 0, text.Length);
        ////    }
        ////}

        ////public static async Task WriteInfo3(string message, string path = "log.txt", string[] systemstate = null)
        ////{
        ////    StreamWriter sw1 = new StreamWriter(path, true);

        ////    int counter = 0;
        ////    while (true)
        ////    {
        ////        try
        ////        {
        ////            //FileStream sourceStream = File.Open(path, FileMode.Append);

        ////            //UnicodeEncoding uniencoding = new UnicodeEncoding();
        ////            //byte[] text = uniencoding.GetBytes(message);

        ////            ////await sourceStream.WriteAsync(text, 0, text.Length);
        ////            //await sourceStream.WriteAsync(text);
        ////            //sourceStream.Close();

        ////            StreamWriter sw = new StreamWriter(path, true);
        ////            await sw.WriteLineAsync(message);
        ////            sw.Close();
        ////        }
        ////        catch (IOException ex)
        ////        {
        ////            counter++;
        ////            if (counter >= 5) 
        ////            {
        ////                await Console.Out.WriteLineAsync("Не удалось записать в лог файл. Файл был занят слишком долго");
        ////                return;
        ////            }
        ////            //await Task.Delay(1000);
        ////            //Task.Run(() => Thread.Sleep(10000));
        ////            //Thread.Sleep(10000);
        ////            Task delay = Task1();
        ////            continue;
        ////        }
        ////    }


        ////    //UnicodeEncoding uniencoding = new UnicodeEncoding();
        ////    //byte[] text = uniencoding.GetBytes(message);

        ////    //using (FileStream SourceStream = File.Open(path, FileMode.Append))
        ////    //{
        ////    //    //SourceStream.Seek(0, SeekOrigin.End);
        ////    //    SourceStream.WriteTimeout = 2000;
        ////    //    await SourceStream.WriteAsync(text, 0, text.Length);
        ////    //}
        ////}

        ////private static async Task Task1()
        ////{
        ////    Task delay = Task.Delay(2000);
        ////    delay.Wait();
        ////    await delay;
        ////}
    }
}
