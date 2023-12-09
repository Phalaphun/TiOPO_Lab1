using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Laba7
{
    internal class UserBD
    {
        private List<User> users = new List<User>();


        public void AddUser()
        {
            string answer = "n";
            bool isCorrcet = false;
            User tempUser;
            do {
                Console.WriteLine("Enter Surname");
                string surname = Console.ReadLine();

                Console.WriteLine("Enter Name");
                string name = Console.ReadLine();

                Console.WriteLine("Enter patronymic");
                string patronymic = Console.ReadLine();

                Console.WriteLine("Enter telephone number");
                string telnum = Console.ReadLine();

                Console.WriteLine("Enter email");
                string email = Console.ReadLine();

                tempUser = new User(surname, name, patronymic, false, email, telnum);

                Console.Write("YourUser: "); Console.WriteLine(tempUser.ToString());
                Console.WriteLine("Is all correct? [y/n]");
                answer=Console.ReadLine();
                if (answer == "n") continue;
                if (CheckFIO(tempUser))
                {
                    if (CheckTelephoneNumber(tempUser))
                    {
                        if (CheckEmail(tempUser))
                        {
                            Console.WriteLine("Указан некорректный адрес e-mail");
                            return;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Указан некорректный номер телефона");
                        return;
                    }
                    
                }
                else
                {
                    Console.WriteLine("Указаны некорректные данные ФИО. Они должны начинатьсяс заглавной буквы");
                    return;
                }
            }
            while (true);



            users.Add(tempUser);
        }

        public void ChangeUser(int id, int option)
        {
            //Console.WriteLine("Выберите опцию для изменения");
        }

        public void RemoveUser(int id)
        {
            User? delUser = users.Find((item) => item.Id == id);
            if (delUser != null)
            {
                users.Remove(delUser);
            }
            else
            {
                Console.WriteLine("Пользователь с данным id не найден");
            }
        }

        public void ListUsers()
        {
            for (int i = 0; i < users.Count; i++)
            {
                Console.WriteLine((i+1)+")" + users[i]);
            }
        }

        public void GenerateTestUSers(int numOfUsers)
        {

            for(int i = 0; i< numOfUsers;i++)
            {
                users.Add(new User(RandomString(5),RandomString(5),RandomString(5),"Chel@mail.com","+79885553535"));
            }
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

                return false;
            }
            catch (FormatException)
            {
                return true;
            }
        }

        public static bool CheckTelephoneNumber(User testUser)
        {
            //return Regex.Match(testUser.Telnum, @"^(\+[0-9]{11})$").Success;
            return Regex.Match(testUser.Telnum, @"\(?\+[0-9]{1,3}\)? ?-?[0-9]{1,3} ?-?[0-9]{3,5} ?-?[0-9]{4}( ?-?[0-9]{3})? ?(\w{1,10}\s?\d{1,6})?").Success;
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
