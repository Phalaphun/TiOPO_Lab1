using Laba7Liba;

namespace Laba7Main
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserBD userBD = new UserBD();
            //userBD.GenerateTestUSers(5);

            Console.WriteLine("CLI для управления контактными данными. Введите команду");

            string[] commands;
            string notParsedCommandLine = string.Empty;

            //var ser = new DataContractSerializer(typeof(UserBD));
            //TextWriter tw = new StreamWriter("test.xml");
            //using (XmlWriter xw = XmlWriter.Create(tw))
            //{
            //    ser.WriteObject(xw, userBD);
            //}

            //var ser = new DataContractSerializer(typeof(UserBD));
            //TextReader tw = new StreamReader("test.xml");
            //using (XmlReader xw = XmlReader.Create(tw))
            //{
            //    //ser.WriteObject(xw, userBD);
            //    userBD = (UserBD)ser.ReadObject(xw);
            //}


            while (true)
            {
                Console.Write("--> ");
                notParsedCommandLine = Console.ReadLine().Trim();
                if(notParsedCommandLine.Length == 0 || notParsedCommandLine == null )
                {
                    continue;
                }

                commands = ParseCommandLine(notParsedCommandLine);
                switch (commands[0])
                {
                    case "help":
                        ListCommands();
                        break;
                    case "exit":
                        return;
                    case "deluser":
                        DelUser(userBD, commands);
                        //if (!int.TryParse(commands[1], out int delId))
                        //{
                        //    Console.WriteLine("Введён некорректный id");
                        //    MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался удалить запись с ID = {commands[1]}, однако это не блоы распознано как число", "ERROR");
                        //    break;
                        //}


                        //try
                        //{
                        //    userBD.RemoveUser(delId);
                        //}
                        //catch (ArgumentException ex)
                        //{
                        //    Console.WriteLine(ex.Message);
                        //}
                        break;
                    case "adduser":
                        AddUser(userBD, commands);
                        break;
                    case "changeuser":
                        ChangeUser(userBD, commands);
                        break;
                    case "list":
                        userBD.ListUsers();
                        break;
                    case "save":
                        userBD.Save();
                        break;
                    case "load":
                        userBD.Load();
                        break;
#if DEBUG
                    case "gentestdata":
                        if (commands.Length <= 1 || !int.TryParse(commands[1], out int numOfTestUsers))
                        {
                            Console.WriteLine("Указано некорректное число тестовых записей");
                            MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить тестовые записи, однако указал некорректное число записей", "ERROR");
                            break;
                        }
                        userBD.GenerateTestUSers(numOfTestUsers);
                        break;
#endif
                    default: Console.WriteLine("Команда не распознана. Для прсомотра списка команд введите help"); break;

                }

            }

        }
        private static void DelUser(UserBD userBD, string[] commands)
        {
            if (!int.TryParse(commands[1], out int delId))
            {
                Console.WriteLine("Введён некорректный id");
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался удалить запись с ID = {commands[1]}, однако это не блоы распознано как число", "ERROR");
                return;
            }


            try
            {
                userBD.RemoveUser(delId);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void AddUser(UserBD userBD, string[] commands)
        {
            if (commands.Length > 1 && commands[1] == "-m")
            {
                if (commands.Length < 6)
                {
                    Console.WriteLine("Введены не все параметры");
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, однако не указал всех требуемых параметров", "ERROR");
                }
                try
                {
                    userBD.AddUser(commands[2], commands[3], commands[4], commands[5], commands[6]);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                userBD.AddUserWizard();
            }
        }

        private static void ChangeUser(UserBD userBD, string[] commands)
        {
            if (!int.TryParse(commands[1], out int changeId))
            {
                Console.WriteLine("Введён некорректный id");
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись с ID = {commands[1]}, однако это не блоы распознано как число", "ERROR");
                return;
            }

            if (commands.Length > 2)
            {

                List<string> updateParams = new List<string>();
                for (int i = 2; i < commands.Length; i++)
                {
                    updateParams.Add(commands[i]);
                }

                try
                {
                    userBD.ChangeUser(changeId, updateParams.ToArray());
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
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
                    string ans = Console.ReadLine().Trim();

                    try
                    {
                        switch (ans)
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
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }


                }

            }
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
            Console.WriteLine("Введите \"exit\", чтобы выйти из программы");
            Console.WriteLine("Введите \"help\" для просмотра этой справки");
            Console.WriteLine("Введите \"save\" сохранения базы данных в файл");
            Console.WriteLine("Введите \"load\" загрузки базы данных из файла");
#if DEBUG
            Console.WriteLine("Введите \"gentestdata N\" для создания тестовых записей, где N - число записей");
#endif
            Console.WriteLine("");
        }

        //private static void Save(UserBD userBD)
        //{
        //    var ser = new DataContractSerializer(typeof(UserBD));
        //    TextWriter tw = null;
        //    try
        //    {
        //        tw = new StreamWriter("savedBD.xml");
        //        using (XmlWriter xw = XmlWriter.Create(tw))
        //        {
        //            ser.WriteObject(xw, userBD);
        //        }
        //        tw.Close();
        //        tw = null;
        //    }
        //    catch (IOException ex)
        //    {
        //        Console.WriteLine("Произошла ошибка при попытке сохранения. Не удалось получить поток к файлу.");
        //        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался сохранить базу данных, однако программа не смогла получить поток на запись к файлу.", "ERROR");
        //    }
        //    catch (SerializationException ex)
        //    {
        //        Console.WriteLine("Произошла ошибка при попытке сохранения. Не удалось сохранить файл.");
        //        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался сохранить базу данных, однако программа не смогла сохранить файл.", "ERROR");
        //    }
        //    catch (InvalidDataContractException ex)
        //    {
        //        Console.WriteLine("Произошла ошибка при попытке сохранения. Пришел некорректный файл для сохранения");
        //        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался сохранить базу данных, однако Пришел некорректный файл для сериализации.", "ERROR");
        //    }
        //    finally
        //    {
        //        if (tw != null)
        //        {
        //            tw.Close();
        //        }
        //    }
        //}

        //private static UserBD Load(UserBD userBD)
        //{
        //    var ser = new DataContractSerializer(typeof(UserBD));
        //    TextReader tw = null;
        //    try
        //    {
        //        tw = new StreamReader("savedBD.xml");
        //        using (XmlReader xw = XmlReader.Create(tw))
        //        {
        //            userBD = (UserBD)ser.ReadObject(xw);
        //        }
        //        tw.Close();
        //        tw = null;
        //    }
        //    catch (IOException ex)
        //    {
        //        Console.WriteLine("Произошла ошибка при попытке сохранения. Не удалось получить поток к файлу. Возможно файла нет, или он занят.");
        //        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался сохранить базу данных, однако программа не смогла получить поток на запись к файлу. Возможно файла нет, или он занят.", "ERROR");
        //    }
        //    catch (SerializationException ex)
        //    {
        //        Console.WriteLine("Произошла ошибка при попытке сохранения. Не удалось сохранить файл.");
        //        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался сохранить базу данных, однако программа не смогла сохранить файл.", "ERROR");
        //    }
        //    catch (InvalidDataContractException ex)
        //    {
        //        Console.WriteLine("Произошла ошибка при попытке сохранения. Пришел некорректный файл для сохранения");
        //        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался сохранить базу данных, однако Пришел некорректный файл для сериализации.", "ERROR");
        //    }
        //    finally
        //    {
        //        if (tw != null)
        //        {
        //            tw.Close();
        //        }
        //    }
        //    return userBD;
        //}

        private static string[] ParseCommandLine(string? notParsedCommandLine)
        {
            if (notParsedCommandLine == null || notParsedCommandLine == "")
            {
                throw new ArgumentNullException("Входящая строка была null или пустой");
            }

            List<string> commands = new List<string>();
            string buffer = string.Empty;
            bool kavicha = false;

            for (int i = 0; i < notParsedCommandLine.Length; i++)
            {
                try
                {
                    if (notParsedCommandLine[i] == '\\')
                    {
                        //buffer += notParsedCommandLine[i].ToString() + notParsedCommandLine[++i];
                        buffer += notParsedCommandLine[++i];
                        continue;
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine("Ошибка в синтаксисе команды.");
                    return Array.Empty<string>();
                }
                if (notParsedCommandLine[i] == '"')
                    kavicha = !kavicha;

                if (notParsedCommandLine[i] != ' ' && !kavicha && notParsedCommandLine[i] != '"')
                    buffer += notParsedCommandLine[i];
                else if (kavicha && notParsedCommandLine[i] != '"')
                {
                    buffer += notParsedCommandLine[i];
                }
                else if (notParsedCommandLine[i] == ' ' && !kavicha)
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
}

