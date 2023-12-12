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
                notParsedCommandLine = Console.ReadLine().Trim();
                //notParsedCommandLine = "hui \"hui hui\" \\\"";
                //notParsedCommandLine = "hui hui hui hui";
                commands = ParseCommandLine(notParsedCommandLine);
                switch (commands[0])
                {
                    case "help":
                        ListCommands();
                        break;
                    case "exit":
                        return;
                    case "deluser":
                        if (!int.TryParse(commands[1], out int delId)) 
                        {
                            Console.WriteLine("Введён некорректный id");
                            MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался удалить запись с ID = {commands[1]}, однако это не блоы распознано как число", "ERROR");
                            break;
                        }
                        userBD.RemoveUser(delId);
                        break;
                    case "adduser":
                        if (commands.Length>1 && commands[1] == "-m")
                        {
                            if (commands.Length <6)
                            {
                                Console.WriteLine("Введены не все параметры");
                                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, однако не указал всех требуемых параметров", "ERROR");
                            }
                            userBD.AddUser(commands[2], commands[3], commands[4], commands[5], commands[6]);
                        }
                        else
                        {
                            userBD.AddUserWizard();
                        }
                        break;
                    case "changeuser":
                        if (!int.TryParse(commands[1], out int changeId))
                        {
                            Console.WriteLine("Введён некорректный id");
                            MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись с ID = {commands[1]}, однако это не блоы распознано как число", "ERROR");
                            break;
                        }

                        if (commands.Length > 2)
                        {
                            

                            List<string> updateParams = new List<string>(); 
                            for (int i = 2; i < commands.Length; i++)
                            {
                                updateParams.Add(commands[i]);
                            }

                            userBD.ChangeUser(changeId,updateParams.ToArray());
                        }
                        else
                        {
                            Console.WriteLine("Укажите что желаете изменить. Чтобы вернуться введите 7");
                            Console.WriteLine("1) Изменить фамилию");
                            Console.WriteLine("2) Изменить имя");
                            Console.WriteLine("3) Изменить отчество");
                            Console.WriteLine("4) Сменить статус на противоположный");
                            Console.WriteLine("5) Изменить E-mail");
                            Console.WriteLine("6) Изменить телефон");
                            Console.WriteLine("7) Назад");
                            bool passed = false;
                            while (!passed)
                            {
                                string ans = Console.ReadLine();
                                
                                switch(ans)
                                {
                                    case "1":
                                        userBD.ChangeUserWizard(changeId, UpdateType.UpdateSurname);
                                        passed = !passed;
                                        break;
                                    case "2":
                                        userBD.ChangeUserWizard(changeId, UpdateType.UpdateName);
                                        passed = !passed;
                                        break;
                                    case "3":
                                        userBD.ChangeUserWizard(changeId, UpdateType.UpdatePatronymic);
                                        passed = !passed;
                                        break;
                                    case "4":
                                        userBD.ChangeUserWizard(changeId, UpdateType.UpdateisBlocked);
                                        passed = !passed;
                                        break;
                                    case "5":
                                        userBD.ChangeUserWizard(changeId, UpdateType.UpdateEmail);
                                        passed = !passed;
                                        break;
                                    case "6":
                                        userBD.ChangeUserWizard(changeId, UpdateType.UpdateTelephone);
                                        passed = !passed;
                                        break;
                                    case "7":
                                        passed = !passed;
                                        break;

                                }
                                
                                
                            }

                        }
                        break;
                    case "list":
                        userBD.ListUsers();
                        break;
                    default: Console.WriteLine("Команда не распознана"); break;
                    
                }

            }
            

            Console.ReadKey();
        }

        private static void ListCommands()
        {
            Console.WriteLine("Для просмотра пользователей введите list");
            Console.WriteLine("Для добавления пользователя с помощью помощника введите adduser");
            Console.WriteLine("Для добавления пользователя вручную введите \"adduser -m ФАМИЛИЯ ИМЯ ОТЧЕСТВО EMAIL ТЕЛЕФОН\"");
            Console.WriteLine("Для изменения пользователя с помощью помощника введите \"changeuser ID\", где ID - ID пользователя, которого хотите изменить");
            Console.WriteLine("Для изменения пользователя вручную введите \"changeuser ID -n ИМЯ -s ФАМИЛИЯ -p ФАМИЛИЯ -b true||false (заблокирован или нет) -e EMAIL -t ТЕЛЕФОН\"," +
                "где ID - ID пользователя, которого хотите изменить. Не обязательно указывать все ключи");
            Console.WriteLine("Для удаления пользователя введите \"deluser ID\", где ID - ID пользователя, которого хотите удалить");
            Console.WriteLine("Введите exit, чтобы выйти из программы");
            Console.WriteLine("Введите help для просмотра этой справки");
            Console.WriteLine("");
        }

        private static string[] ParseCommandLine(string? notParsedCommandLine)
        {
            if(notParsedCommandLine == null || notParsedCommandLine=="")
            {
                throw new ArgumentNullException("Входящая строка была null или пустой");
            }

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
