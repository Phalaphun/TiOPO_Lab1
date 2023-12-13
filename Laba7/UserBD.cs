using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Laba7
{
    enum UpdateType
    {
        UpdateName=1,
        UpdateSurname,
        UpdatePatronymic,
        UpdateisBlocked,
        UpdateTelephone,
        UpdateEmail
    }
    [DataContract]
    internal class UserBD
    {
        [DataMember(Name = "users")]
        private List<User> users = new List<User>();


        public void AddUserWizard()
        {
            string answer = String.Empty;
            User tempUser;
            do {
                Console.WriteLine("Enter Surname");
                string surname = Console.ReadLine().Trim();

                Console.WriteLine("Enter Name");
                string name = Console.ReadLine().Trim();

                Console.WriteLine("Enter patronymic");
                string patronymic = Console.ReadLine().Trim();

                Console.WriteLine("Enter telephone number");
                string telnum = Console.ReadLine().Trim();

                Console.WriteLine("Enter email");
                string email = Console.ReadLine().Trim();

                tempUser = new User(surname, name, patronymic, false, email, telnum);

                Console.Write("YourUser: "); Console.WriteLine(tempUser.ToString());
                Console.WriteLine("Is all correct? [y/n]");
                answer=Console.ReadLine().Trim();
                if (answer == "n") continue;
                if (!CheckFIO(tempUser))
                {
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                        $"Указаны некорректный данные ФИО. Они должны начинаться с заглавной буквы.","ERROR");
                    Console.WriteLine("Указаны некорректный данные ФИО. Они должны начинаться с заглавной буквы.");
                    return;
                }
                if (!CheckTelephoneNumber(tempUser))
                {
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                        $"Указан некорректный номер телефона", "ERROR");
                    Console.WriteLine("Указан некорректный номер телефона");
                    return;
                }
                if (!CheckEmail(tempUser))
                {
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался добавить запись, но случилась ошибка:" +
                        $"Указан некорректный адрес e-mail", "ERROR");
                    Console.WriteLine("Указан некорректный адрес e-mail");
                    return;
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
            if(changeUser == null)
            {
                MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить несуществующую запись с id {id}", "ERROR");
                throw new ArgumentException("Пользователь с данным id не найден");
            }

            switch (updateType)
            {
                case UpdateType.UpdateName:
                    string newName = string.Empty;
                    while (true)
                    {
                        Console.WriteLine("Введите новое имя:");
                        newName = Console.ReadLine().Trim();

                        Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
                        string answer = Console.ReadLine().Trim();
                        if (answer == "n") continue;

                        if (CheckOnlyOnePartOfFIO(newName)) break;
                        Console.WriteLine("Указано неверное имя. Оно должно начинаться заглавной буквы");
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                            $"Указано некорретное имя", "ERROR");
                    }
                    

                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Имя изменено с \"{changeUser.Name}\" на \"{newName}\" ", "INFO");
                    changeUser.Name = newName;
                    break;
                case UpdateType.UpdateSurname:
                    string newSurname = string.Empty;
                    while (true)
                    {
                        Console.WriteLine("Введите новую фамилию:");
                        newSurname = Console.ReadLine().Trim();

                        Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
                        string answer = Console.ReadLine().Trim();
                        if (answer == "n") continue;

                        if (CheckOnlyOnePartOfFIO(newSurname)) break;
                        Console.WriteLine("Указаа неверная фамилия. Она должна начинаться заглавной буквы");
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                            $"Указана некорретная фамилия", "ERROR");
                    }


                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Фамилия изменена с \"{changeUser.Surname}\" на \"{newSurname}\" ", "INFO");
                    changeUser.Surname = newSurname;
                    break;
                case UpdateType.UpdatePatronymic:
                    string newPatronymic = string.Empty;
                    while (true)
                    {
                        Console.WriteLine("Введите новое отчество:");
                        newPatronymic = Console.ReadLine().Trim();

                        Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
                        string answer = Console.ReadLine().Trim();
                        if (answer == "n") continue;

                        if (CheckOnlyOnePartOfFIO(newPatronymic)) break;
                        Console.WriteLine("Указано неверное отчество. Оно должна начинаться заглавной буквы");
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                            $"Указано некорретное отчество", "ERROR");
                    }


                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Отчество изменено с \"{changeUser.Patronymic}\" на \"{newPatronymic}\" ", "INFO");
                    changeUser.Patronymic = newPatronymic;
                    break;
                case UpdateType.UpdateisBlocked:
                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Статус блокировки изменён с \"{changeUser.IsBlocked}\" на \"{!changeUser.IsBlocked}\" ", "INFO");
                    changeUser.IsBlocked = !changeUser.IsBlocked;
                    break;
                case UpdateType.UpdateEmail:
                    string email = string.Empty;
                    while (true)
                    {
                        Console.WriteLine("Введите новый Email:");
                        email = Console.ReadLine().Trim();

                        Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
                        string answer = Console.ReadLine().Trim();
                        if (answer == "n") continue;

                        if (CheckEmail(email)) break;
                        Console.WriteLine("Указан некорректный Email.");
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                            $"Указан некорректный Email", "ERROR");
                    }


                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Email изменен с \"{changeUser.Email}\" на \"{email}\" ", "INFO");
                    changeUser.Email = email;
                    break;
                case UpdateType.UpdateTelephone:
                    string telnum = string.Empty;
                    while (true)
                    {
                        Console.WriteLine("Введите новый телефон:");
                        telnum = Console.ReadLine().Trim();

                        Console.WriteLine("Подтвердите корректность введеных данных или попытайтесь ещё раз [y/n]");
                        string answer = Console.ReadLine().Trim();
                        if (answer == "n") continue;

                        if (CheckTelephoneNumber(telnum)) break;
                        Console.WriteLine("Указан некорректный телефон.");
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                            $"Указан некорректный телефон", "ERROR");
                    }


                    MyLogger.WriteLog($"Пользователь {Environment.UserName} изменил запись: {changeUser.ToString()}. Телефон изменен с \"{changeUser.Telnum}\" на \"{telnum}\" ", "INFO");
                    changeUser.Telnum = telnum;
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

            for(int i = 0; i < data.Length; i += 2)
            {
                //TODO сделать проверку что следующий элемент не ключ, а значение, иначе error true и выкидывать из цикла
                try
                {
                    if (data[i + 1][0] == '-')
                    {                      
                        MyLogger.WriteLog($"Пользователь {Environment.UserName} ввел некорректный запрос: Ключ {data[i]} не имел значения, т.е. за ключом сразу шел ключ", "ERROR");
                        //Console.WriteLine($"Ключ {data[i]} не имел значения, т.е. за ключом сразу шел ключ");
                        RollBack(changeUser, oldUser);
                        throw new ArgumentException($"Ключ {data[i]} не имел значения, т.е. за ключом сразу шел ключ");;
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
                        if(!CheckOnlyOnePartOfFIO(data[i + 1]))
                        {
                            MyLogger.WriteLog($"Пользователь {Environment.UserName} попытался изменить запись {changeUser.ToString()}, но случилась ошибка:" +
                                $"Указано некорретное имя", "ERROR");
                            RollBack(changeUser, oldUser);
                            throw new ArgumentException("Указано неверное имя. Оно должно начинаться заглавной буквы");
                           
                        }
                        changeUser.Name = data[i+1];
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
                        if (!bool.TryParse(data[i+1], out bool result))
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
                Console.WriteLine((i+1)+")" + users[i]);
            }
        }





        public static bool CheckOnlyOnePartOfFIO(string testSubject)
        {
            return char.IsUpper(testSubject[0]);
        }

        public static bool CheckFIO(User testUser)
        {
            if (!char.IsUpper(testUser.Surname[0]))
            {
                return false;
            }
            if (!char.IsUpper(testUser.Name[0]))
            {
                return false;
            }
            if (!char.IsUpper(testUser.Patronymic[0]))
            {
                return false;
            }



            return true;
        }

        public static bool CheckEmail(User testUser)
        {
            try
            {
                MailAddress m = new MailAddress(testUser.Email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
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
            return Regex.Match(testUser.Telnum, @"\(?\+[0-9]{1,3}\)? ?-?[0-9]{1,3} ?-?[0-9]{3,5} ?-?[0-9]{4}( ?-?[0-9]{3})? ?(\w{1,10}\s?\d{1,6})?").Success;
        }





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


    }
}
