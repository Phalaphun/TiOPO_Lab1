using System.Diagnostics;

namespace Laba7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //MyLogger.WriteLog("HHHH","INFO");
            //Console.WriteLine("123");

            UserBD userBD = new UserBD();
            userBD.GenerateTestUSers(5);

            //userBD.ListUsers();
            //userBD.ChangeUserWizard(1, UpdateType.UpdateName);
            //userBD.ChangeUser(1, new string[] { "-n", "Hui", "-s", "kek" });
            //userBD.AddUserWizard();
            //userBD.ListUsers();
            Console.WriteLine("CLI для убправления контактными данными. Введите команду");

            string[] commands;
            string notParsedCommandLine = string.Empty;
            while (true)
            {
                Console.Write("--> ");
                notParsedCommandLine = Console.ReadLine();
                //notParsedCommandLine = "hui \"hui hui\" \\\"";
                //notParsedCommandLine = "hui hui hui hui";
                commands = ParseCommandLine(notParsedCommandLine);
                
            }
            

            Console.ReadKey();
        }

        private static string[] ParseCommandLine(string? notParsedCommandLine)
        {
            List<string> commands = new List<string>();
            string buffer = string.Empty;
            bool kavicha = false;
            
            for (int i = 0;i<notParsedCommandLine.Length;i++)
            {
                try
                {
                    if (notParsedCommandLine[i] == '\\')
                    {
                        //buffer += notParsedCommandLine[i].ToString() + notParsedCommandLine[++i];
                        buffer +=  notParsedCommandLine[++i];
                        continue;
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine("Ошибка в синтаксисе команды.");
                    return new string[] { }; 
                }
                if(notParsedCommandLine[i] == '"')
                    kavicha = !kavicha;

                if (notParsedCommandLine[i] != ' ' && !kavicha && notParsedCommandLine[i] != '"')
                    buffer += notParsedCommandLine[i];
                else if(kavicha && notParsedCommandLine[i] != '"')
                {
                    buffer += notParsedCommandLine[i];
                }
                else if(notParsedCommandLine[i] == ' ' && !kavicha)
                {
                    commands.Add(buffer);
                    buffer = string.Empty;
                }


            }
            if (buffer != string.Empty)
            {
                commands.Add(buffer);
                buffer = String.Empty;
            }



            return commands.ToArray();
        }
    }
    /*
        Пишем adress book: Вывод инфы из справичника: ФИО + номер телефона + статус;
        USER: FIO TEL EMAIL BLOCKED ID ;

        Смотреть все записи, уметь блокировать записи
        Уметь разблокировать уметь удалять, добавлять изменять. Без GUI. 

        Требования e-mail: проверка на то, что это e-mail. На телефоне тоже проверка. 

        В момент CURD долно откладывать что и кто изменил (кто - учетка винды, дата время) + откидывания ошибок (мол некорректный mail). 

        В EXCEL описать все тесты, с учетом замечаний. 

        Тесты: 
        Тестирование метода всех проверок (телефон или мыло). 
        Метод проверки добавления
        Метод проверки удаления (по ID) 
        Метод проверки Изменения 
        Тестим что все ФИО начинаются с заглавных букв 
    */
}
