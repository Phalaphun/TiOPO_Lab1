using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;

namespace Laba7Liba
{
    public enum UpdateType
    {
        UpdateName=1,
        UpdateSurname,
        UpdatePatronymic,
        UpdateisBlocked,
        UpdateTelephone,
        UpdateEmail
    }
    [DataContract]
    public class UserBD
    {
        [DataMember(Name = "users")]
        private List<User> users = new List<User>();

        #region Методы по работе с БД

        public void AddUserWizard()
        {
            string answer = String.Empty;
            User tempUser;
            do
            {
                Console.WriteLine("Enter Surname");
                string surname = Console.ReadLine().Trim();

                Console.WriteLine("Enter Name");
                string name = Console.ReadLine().Trim();

                Console.WriteLine("Enter patronymic");
                string patronymic = Console.ReadLine().Trim();

                Console.WriteLine("Enter email");
                string email = Console.ReadLine().Trim();

                Console.WriteLine("Enter telephone number");
                string telnum = Console.ReadLine().Trim();
               

                tempUser = new User(surname, name, patronymic, false, email, telnum);

                Console.Write("YourUser: "); Console.WriteLine(tempUser.ToString());
                Console.WriteLine("Is all correct? [y/n]");
                answer = Console.ReadLine().Trim();
                if (answer == "n") continue;
                if (!CheckFIO(tempUser))
                {
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                        $"Указаны некорректный данные ФИО. Они должны начинаться с заглавной буквы.", "ERROR");
                    Console.WriteLine("Указаны некорректный данные ФИО. Они должны начинаться с заглавной буквы.");
                    continue;
                }
                if (!CheckTelephoneNumber(tempUser))
                {
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                        $"Указан некорректный номер телефона", "ERROR");
                    Console.WriteLine("Указан некорректный номер телефона");
                    continue;
                }
                if (!CheckEmail(tempUser))
                {
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                        $"Указан некорректный адрес e-mail", "ERROR");
                    Console.WriteLine("Указан некорректный адрес e-mail");
                    continue;
                }
                break;
            }
            while (true);


            MyLogger.WriteLog($"Пользователь {Environment.UserName} добавил запись: {tempUser.ToString()}", "INFO");
            users.Add(tempUser);
        }

        public void AddUser(string surname, string name, string patronymic, string email, string telnum)
        {


            User tempUser = new User(surname, name, patronymic, false, email, telnum);

            if (!CheckFIO(tempUser))
            {
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                    $"Указаны некорректный данные ФИО. Они должны начинаться с заглавной буквы.", "ERROR");
                throw new ArgumentException("Указаны некорректный данные ФИО. Они должны начинаться с заглавной буквы.");
            }
            if (!CheckTelephoneNumber(tempUser))
            {
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                    $"Указан некорректный номер телефона", "ERROR");
                throw new ArgumentException("Указан некорректный номер телефона");
            }
            if (!CheckEmail(tempUser))
            {
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                    $"Указан некорректный адрес e-mail", "ERROR");
                throw new ArgumentException("Указан некорректный адрес e-mail");
            }


            MyLogger.WriteLog($"Пользователь {Environment.UserName} добавил запись: {tempUser.ToString()}", "INFO");
            users.Add(tempUser);
        }

        public void ChangeUserWizard(int id, UpdateType updateType)
        {
            User? changeUser = users.Find((item) => item.Id == id);
            if (changeUser == null)
            {
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить несуществующую запись с id {id}", "ERROR");
                throw new ArgumentException("Пользователь с данным id не найден");
            }

            string newData = string.Empty;

            switch (updateType)
            {
                case UpdateType.UpdateName:
                   
                    while (true)
                    {
                        Console.WriteLine("Введите новое имя:");
                        newData = Console.ReadLine().Trim();

                        Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
                        string answer = Console.ReadLine().Trim();
                        if (answer == "n") continue;

                        if (CheckOnlyOnePartOfFIO(newData)) break;
                        Console.WriteLine("Указано неверное имя. Оно должно начинаться заглавной буквы");
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                            $"Указано некорретное имя", "ERROR");
                    }


                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Имя изменено с \"{changeUser.Name}\" на \"{newData}\" ", "INFO");
                    changeUser.Name = newData;
                    break;
                case UpdateType.UpdateSurname:
                    
                    while (true)
                    {
                        Console.WriteLine("Введите новую фамилию:");
                        newData = Console.ReadLine().Trim();

                        Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
                        string answer = Console.ReadLine().Trim();
                        if (answer == "n") continue;

                        if (CheckOnlyOnePartOfFIO(newData)) break;
                        Console.WriteLine("Указаа неверная фамилия. Она должна начинаться заглавной буквы");
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                            $"Указана некорретная фамилия", "ERROR");
                    }


                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Фамилия изменена с \"{changeUser.Surname}\" на \"{newData}\" ", "INFO");
                    changeUser.Surname = newData;
                    break;
                case UpdateType.UpdatePatronymic:

                    while (true)
                    {
                        Console.WriteLine("Введите новое отчество:");
                        newData = Console.ReadLine().Trim();

                        Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
                        string answer = Console.ReadLine().Trim();
                        if (answer == "n") continue;

                        if (CheckOnlyOnePartOfFIO(newData)) break;
                        Console.WriteLine("Указано неверное отчество. Оно должна начинаться заглавной буквы");
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                            $"Указано некорретное отчество", "ERROR");
                    }


                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Отчество изменено с \"{changeUser.Patronymic}\" на \"{newData}\" ", "INFO");
                    changeUser.Patronymic = newData;
                    break;
                case UpdateType.UpdateisBlocked:
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Статус блокировки изменён с \"{changeUser.IsBlocked}\" на \"{!changeUser.IsBlocked}\" ", "INFO");
                    changeUser.IsBlocked = !changeUser.IsBlocked;
                    break;
                case UpdateType.UpdateEmail:

                    while (true)
                    {
                        Console.WriteLine("Введите новый Email:");
                        newData = Console.ReadLine().Trim();

                        Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
                        string answer = Console.ReadLine().Trim();
                        if (answer == "n") continue;

                        if (CheckEmail(newData)) break;
                        Console.WriteLine("Указан некорректный Email.");
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                            $"Указан некорректный Email", "ERROR");
                    }


                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Email изменен с \"{changeUser.Email}\" на \"{newData}\" ", "INFO");
                    changeUser.Email = newData;
                    break;
                case UpdateType.UpdateTelephone:

                    while (true)
                    {
                        Console.WriteLine("Введите новый телефон:");
                        newData = Console.ReadLine().Trim();

                        Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
                        string answer = Console.ReadLine().Trim();
                        if (answer == "n") continue;

                        if (CheckTelephoneNumber(newData)) break;
                        Console.WriteLine("Указан некорректный телефон.");
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                            $"Указан некорректный телефон", "ERROR");
                    }


                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Телефон изменен с \"{changeUser.Telnum}\" на \"{newData}\" ", "INFO");
                    changeUser.Telnum = newData;
                    break;

            }
        }

        public void ChangeUser(int id, string[] data)
        {
            User? changeUser = users.Find((item) => item.Id == id);

            if (changeUser == null)
            {
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить несуществующую запись с id {id}", "ERROR");
                throw new ArgumentException("Пользователь с данным id не найден");
            }

            User oldUser = new User(changeUser, true);

            for (int i = 0; i < data.Length; i += 2)
            {
                //TODO сделать проверку что следующий элемент не ключ, а значение, иначе error true и выкидывать из цикла
                try
                {
                    if (data[i + 1][0] == '-')
                    {
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} ввел некорректный запрос: Ключ {data[i]} не имел значения, т.е. за ключом сразу шел ключ", "ERROR");
                        //Console.WriteLine($"Ключ {data[i]} не имел значения, т.е. за ключом сразу шел ключ");
                        RollBack(changeUser, oldUser);
                        throw new ArgumentException($"Ключ {data[i]} не имел значения, т.е. за ключом сразу шел ключ"); ;
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine($"Была введена некорректная последновательность ключей. Возможно за ключем не было значения");
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} ввел некорректный запрос: Была введена некорректная последновательность ключей. Возможно за ключем не было значения", "ERROR");
                }
                switch (data[i])
                {
                    case "-n": //name
                        if (!CheckOnlyOnePartOfFIO(data[i + 1]))
                        {
                            MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                                $"Указано некорретное имя", "ERROR");
                            RollBack(changeUser, oldUser);
                            throw new ArgumentException("Указано неверное имя. Оно должно начинаться заглавной буквы");

                        }
                        changeUser.Name = data[i + 1];
                        break;
                    case "-s": //surname
                        if (!CheckOnlyOnePartOfFIO(data[i + 1]))
                        {
                            MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                                $"Указана некорретная фамилия", "ERROR");

                            RollBack(changeUser, oldUser);
                            throw new ArgumentException("Указана неверная фамилия. Она должна начинаться заглавной буквы");
                        }
                        changeUser.Surname = data[i + 1];
                        break;
                    case "-p": //patr
                        if (!CheckOnlyOnePartOfFIO(data[i + 1]))
                        {
                            MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                                $"Указано некорретное отчество", "ERROR");
                            RollBack(changeUser, oldUser);
                            throw new ArgumentException("Указано неверное отчество. Оно должна начинаться заглавной буквы");
                        }
                        changeUser.Patronymic = data[i + 1];
                        break;
                    case "-b": //blocking
                        if (!bool.TryParse(data[i + 1], out bool result))
                        {
                            MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                                $"Указан неверный тип блокировки", "ERROR");
                            RollBack(changeUser, oldUser);
                            throw new ArgumentException("Указан неверный тип блокировки");
                        }
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Статус блокировки изменён с \"{changeUser.IsBlocked}\" на \"{!changeUser.IsBlocked}\" ", "INFO");
                        changeUser.IsBlocked = result;
                        break;
                    case "-e": //email
                        if (!CheckEmail(data[i + 1]))
                        {
                            MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                                $"Указан некорректный Email", "ERROR");
                            RollBack(changeUser, oldUser);
                            throw new ArgumentException("Указан некорректный Email.");
                        }
                        changeUser.Email = data[i + 1];
                        break;
                    case "-t": //tel
                        if (!CheckTelephoneNumber(data[i + 1]))
                        {
                            Console.WriteLine("Указан некорректный телефон.");
                            MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                                $"Указан некорректный телефон", "ERROR");
                            RollBack(changeUser, oldUser);
                            throw new ArgumentException("Указан некорректный телефон."); ;
                        }
                        changeUser.Telnum = data[i + 1];
                        break;
                    default:
                        break;


                }

            }

            void RollBack(User? changeUser, User oldUser)
            {
                users.Remove(changeUser);
                users.Add(oldUser);
                return;
            }

            //switch (updateType)
            //{
            //    case UpdateType.UpdateName:
            //        string newName = string.Empty;
            //        while (true)
            //        {
            //            Console.WriteLine("Введите новое имя:");
            //            newName = Console.ReadLine();

            //            Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
            //            string answer = Console.ReadLine();
            //            if (answer == "n") continue;

            //            if (CheckOnlyOnePartOfFIO(newName)) break;
            //            Console.WriteLine("Указано неверное имя. Оно должно начинаться заглавной буквы");
            //            MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
            //                $"Указано некорретное имя", "ERROR");
            //        }


            //        MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Имя изменено с \"{changeUser.Name}\" на \"{newName}\" ", "INFO");
            //        changeUser.Name = newName;
            //        break;
            //    case UpdateType.UpdateSurname:
            //        string newSurname = string.Empty;
            //        while (true)
            //        {
            //            Console.WriteLine("Введите новую фамилию:");
            //            newSurname = Console.ReadLine();

            //            Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
            //            string answer = Console.ReadLine();
            //            if (answer == "n") continue;

            //            if (CheckOnlyOnePartOfFIO(newSurname)) break;
            //            Console.WriteLine("Указано неверная фамилия. Она должна начинаться заглавной буквы");
            //            MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
            //                $"Указана некорретная фамилия", "ERROR");
            //        }


            //        MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Фамилия изменена с \"{changeUser.Surname}\" на \"{newSurname}\" ", "INFO");
            //        changeUser.Surname = newSurname;
            //        break;
            //    case UpdateType.UpdatePatronymic:
            //        string newPatronymic = string.Empty;
            //        while (true)
            //        {
            //            Console.WriteLine("Введите новое отчество:");
            //            newPatronymic = Console.ReadLine();

            //            Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
            //            string answer = Console.ReadLine();
            //            if (answer == "n") continue;

            //            if (CheckOnlyOnePartOfFIO(newPatronymic)) break;
            //            Console.WriteLine("Указано неверное отчество. Оно должна начинаться заглавной буквы");
            //            MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
            //                $"Указано некорретное отчество", "ERROR");
            //        }


            //        MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Отчество изменено с \"{changeUser.Patronymic}\" на \"{newPatronymic}\" ", "INFO");
            //        changeUser.Patronymic = newPatronymic;
            //        break;
            //    case UpdateType.UpdateisBlocked:
            //        MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Статус блокировки изменён с \"{changeUser.IsBlocked}\" на \"{!changeUser.IsBlocked}\" ", "INFO");
            //        changeUser.IsBlocked = !changeUser.IsBlocked;
            //        break;
            //    case UpdateType.UpdateEmail:
            //        string email = string.Empty;
            //        while (true)
            //        {
            //            Console.WriteLine("Введите новый Email:");
            //            email = Console.ReadLine();

            //            Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
            //            string answer = Console.ReadLine();
            //            if (answer == "n") continue;

            //            if (CheckEmail(email)) break;
            //            Console.WriteLine("Указан некорректный Email.");
            //            MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
            //                $"Указан некорректный Email", "ERROR");
            //        }


            //        MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Email изменен с \"{changeUser.Email}\" на \"{email}\" ", "INFO");
            //        changeUser.Email = email;
            //        break;
            //    case UpdateType.UpdateTelephone:
            //        string telnum = string.Empty;
            //        while (true)
            //        {
            //            Console.WriteLine("Введите новый телефон:");
            //            telnum = Console.ReadLine();

            //            Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
            //            string answer = Console.ReadLine();
            //            if (answer == "n") continue;

            //            if (CheckTelephoneNumber(telnum)) break;
            //            Console.WriteLine("Указан некорректный телефон.");
            //            MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
            //                $"Указан некорректный телефон", "ERROR");
            //        }


            //        MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Телефон изменен с \"{changeUser.Telnum}\" на \"{telnum}\" ", "INFO");
            //        changeUser.Telnum = telnum;
            //        break;

            //}
        }

        public void RemoveUser(int id)
        {
            User? delUser = users.Find((item) => item.Id == id);
            if (delUser != null)
            {
                MyLogger.WriteLog($"Пользователь {Environment.UserName} удалил запись: {delUser.ToString()}", "INFO");
                users.Remove(delUser);

            }
            else
            {
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался удалить несуществующую запись с id {id}", "ERROR");
                throw new ArgumentException("Пользователь с данным id не найден");

            }
        }

        public void ListUsers()
        {
            for (int i = 0; i < users.Count; i++)
            {
                Console.WriteLine((i + 1) + ")" + users[i]);
            }
        }

        #endregion


        #region Методы проверок ввода

        public static bool CheckOnlyOnePartOfFIO(string testSubject)
        {
            return char.IsUpper(testSubject[0]);
        }

        public static bool CheckFIO(User testUser)
        {
            //if (!char.IsUpper(testUser.Surname[0]))
            //{
            //    return false;
            //}
            //if (!char.IsUpper(testUser.Name[0]))
            //{
            //    return false;
            //}
            //if (!char.IsUpper(testUser.Patronymic[0]))
            //{
            //    return false;
            //}



            //return true;
            return CheckOnlyOnePartOfFIO(testUser.Name) && CheckOnlyOnePartOfFIO(testUser.Surname) && CheckOnlyOnePartOfFIO(testUser.Patronymic);
        }

        public static bool CheckEmail(User testUser)
        {
            //try
            //{
            //    MailAddress m = new MailAddress(testUser.Email);

            //    return true;
            //}
            //catch (FormatException)
            //{
            //    return false;
            //}
            return CheckEmail(testUser.Email);
        }

        public static bool CheckEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool CheckTelephoneNumber(string telnum)
        {
            //return Regex.Match(testUser.Telnum, @"^(\+[0-9]{11})$").Success;
            return Regex.Match(telnum, @"\(?\+[0-9]{1,3}\)? ?-?[0-9]{1,3} ?-?[0-9]{3,5} ?-?[0-9]{4}( ?-?[0-9]{3})? ?(\w{1,10}\s?\d{1,6})?").Success;
        }

        public static bool CheckTelephoneNumber(User testUser)
        {
            //return Regex.Match(testUser.Telnum, @"^(\+[0-9]{11})$").Success;
            return CheckTelephoneNumber(testUser.Telnum);
        }

        #endregion

        #region Создание тестовых пользователей для заполнения базы

        public void GenerateTestUSers(int numOfUsers)
        {

            for (int i = 0; i < numOfUsers; i++)
            {
                users.Add(new User(RandomString(5), RandomString(5), RandomString(5), "Chel@mail.com", "+79885553535"));
            }
        }

        public static string RandomString(int length)
        {
            Random rnd = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        #endregion

        #region Методы сохранения и загрузки
        public void Save()//Возвращать bool как признак успешности сохранения? Или лучше убрать тут try и оставить это на пользователя либы? 
        {
            var ser = new DataContractSerializer(typeof(List<User>));
            TextWriter tw = null;
            try
            {
                tw = new StreamWriter("savedBD1.xml");
                using (XmlWriter xw = XmlWriter.Create(tw))
                {
                    ser.WriteObject(xw, users);
                }
                tw.Close();
                tw = null;
            }
            catch (IOException ex)
            {
                Console.WriteLine("Произошла ошибка при попытке сохранения. Не удалось получить поток к файлу.");
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался сохранить базу данных, однако программа не смогла получить поток на запись к файлу.", "ERROR");
                
            }
            catch (SerializationException ex)
            {
                Console.WriteLine("Произошла ошибка при попытке сохранения. Не удалось сохранить файл.");
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался сохранить базу данных, однако программа не смогла сохранить файл.", "ERROR");
            }
            catch (InvalidDataContractException ex)
            {
                Console.WriteLine("Произошла ошибка при попытке сохранения. Пришел некорректный файл для сохранения");
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался сохранить базу данных, однако Пришел некорректный файл для сериализации.", "ERROR");
            }
            finally
            {
                if (tw != null)
                {
                    tw.Close();
                }
            }

        }
        
        public void Load()
        {
            var ser = new DataContractSerializer(typeof(List<User>));
            TextReader tw = null;
            try
            {
                tw = new StreamReader("savedBD1.xml");
                using (XmlReader xw = XmlReader.Create(tw))
                {
                    users = (List<User>)ser.ReadObject(xw);
                }
                tw.Close();
                tw = null;
            }
            catch (IOException ex)
            {
                Console.WriteLine("Произошла ошибка при попытке загрузки. Не удалось получить поток к файлу. Возможно файла нет, или он занят.");
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался сохранить базу данных, однако программа не смогла получить поток на запись к файлу. Возможно файла нет, или он занят.", "ERROR");
            }
            catch (SerializationException ex)
            {
                Console.WriteLine("Произошла ошибка при попытке десериализации. Не удалось десериализовать файл.");
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался сохранить базу данных, однако программа не смогла сохранить файл.", "ERROR");
            }
            catch (InvalidDataContractException ex)
            {
                Console.WriteLine("Произошла ошибка при попытке десериализации. Пришел некорректный файл для десериализации");
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался сохранить базу данных, однако Пришел некорректный файл для сериализации.", "ERROR");
            }
            finally
            {
                if (tw != null)
                {
                    tw.Close();
                }
            }

        }

        #endregion


        #region Методы, изменённые для работы с юнитТестами(т.е. обращения к консоли заменены на readline)

        public void AddUserWizardUnitTest(string _surname, string _name, string _patronymic, string _email, string _telnum,
            string _answer)
        {
            string answer = String.Empty;
            User tempUser;
            do
            {
                Console.WriteLine("Enter Surname");
                string surname = _surname.Trim();

                Console.WriteLine("Enter Name");
                string name = _name.Trim();

                Console.WriteLine("Enter patronymic");
                string patronymic = _patronymic.Trim();

                Console.WriteLine("Enter email");
                string email = _email.Trim();

                Console.WriteLine("Enter telephone number");
                string telnum = _telnum.Trim();


                tempUser = new User(surname, name, patronymic, false, email, telnum);

                Console.Write("YourUser: "); Console.WriteLine(tempUser.ToString());
                Console.WriteLine("Is all correct? [y/n]");
                answer = _answer.Trim();
                if (answer == "n") continue;
                if (!CheckFIO(tempUser))
                {
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                        $"Указаны некорректный данные ФИО. Они должны начинаться с заглавной буквы.", "ERROR");
                    Console.WriteLine("Указаны некорректный данные ФИО. Они должны начинаться с заглавной буквы.");
                    continue;
                }
                if (!CheckTelephoneNumber(tempUser))
                {
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                        $"Указан некорректный номер телефона", "ERROR");
                    Console.WriteLine("Указан некорректный номер телефона");
                    continue;
                }
                if (!CheckEmail(tempUser))
                {
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                        $"Указан некорректный адрес e-mail", "ERROR");
                    Console.WriteLine("Указан некорректный адрес e-mail");
                    continue;
                }
                break;
            }
            while (true);


            MyLogger.WriteLog($"Пользователь {Environment.UserName} добавил запись: {tempUser.ToString()}", "INFO");
            users.Add(tempUser);
        }

        public void ChangeUserWizardUnitTest(int id, UpdateType updateType, string _newData, string _answer)
        {
            User? changeUser = users.Find((item) => item.Id == id);
            if (changeUser == null)
            {
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить несуществующую запись с id {id}", "ERROR");
                throw new ArgumentException("Пользователь с данным id не найден");
            }
            string newData = _newData.Trim();




            switch (updateType)
            {
                case UpdateType.UpdateName:
                    
                    while (true)
                    {
                        Console.WriteLine("Введите новое имя:");
                        

                        Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
                        string answer = _answer.Trim();
                        if (answer == "n") continue;

                        if (CheckOnlyOnePartOfFIO(newData)) break;
                        Console.WriteLine("Указано неверное имя. Оно должно начинаться заглавной буквы");
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                            $"Указано некорретное имя", "ERROR");
                    }


                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Имя изменено с \"{changeUser.Name}\" на \"{newData}\" ", "INFO");
                    changeUser.Name = newData;
                    break;
                case UpdateType.UpdateSurname:
                   
                    while (true)
                    {
                        Console.WriteLine("Введите новую фамилию:");
                       

                        Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
                        string answer = _answer.Trim();
                        if (answer == "n") continue;

                        if (CheckOnlyOnePartOfFIO(newData)) break;
                        Console.WriteLine("Указаа неверная фамилия. Она должна начинаться заглавной буквы");
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                            $"Указана некорретная фамилия", "ERROR");
                    }


                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Фамилия изменена с \"{changeUser.Surname}\" на \"{newData}\" ", "INFO");
                    changeUser.Surname = newData;
                    break;
                case UpdateType.UpdatePatronymic:
                    
                    while (true)
                    {
                        Console.WriteLine("Введите новое отчество:");
                        

                        Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
                        string answer = _answer.Trim();
                        if (answer == "n") continue;

                        if (CheckOnlyOnePartOfFIO(newData)) break;
                        Console.WriteLine("Указано неверное отчество. Оно должна начинаться заглавной буквы");
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                            $"Указано некорретное отчество", "ERROR");
                    }


                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Отчество изменено с \"{changeUser.Patronymic}\" на \"{newData}\" ", "INFO");
                    changeUser.Patronymic = newData;
                    break;
                case UpdateType.UpdateisBlocked:
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Статус блокировки изменён с \"{changeUser.IsBlocked}\" на \"{!changeUser.IsBlocked}\" ", "INFO");
                    changeUser.IsBlocked = !changeUser.IsBlocked;
                    break;
                case UpdateType.UpdateEmail:
                    
                    while (true)
                    {
                        Console.WriteLine("Введите новый Email:");
                        

                        Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
                        string answer = _answer.Trim();
                        if (answer == "n") continue;

                        if (CheckEmail(newData)) break;
                        Console.WriteLine("Указан некорректный Email.");
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                            $"Указан некорректный Email", "ERROR");
                    }


                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Email изменен с \"{changeUser.Email}\" на \"{newData}\" ", "INFO");
                    changeUser.Email = newData;
                    break;
                case UpdateType.UpdateTelephone:

                    while (true)
                    {
                        Console.WriteLine("Введите новый телефон:");


                        Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
                        string answer = _answer.Trim();
                        if (answer == "n") continue;

                        if (CheckTelephoneNumber(newData)) break;
                        Console.WriteLine("Указан некорректный телефон.");
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                            $"Указан некорректный телефон", "ERROR");
                    }


                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Телефон изменен с \"{changeUser.Telnum}\" на \"{newData}\" ", "INFO");
                    changeUser.Telnum = newData;
                    break;

            }
        }

        #endregion
    }

}
