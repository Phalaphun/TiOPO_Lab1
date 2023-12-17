using Laba7Liba;
using System.Diagnostics;

// https://stackoverflow.com/questions/63776942/whats-net-core-alternative-of-datasource-attribute-for-mstest
// https://github.com/Microsoft/testfx/issues/233

namespace UserBDTests
{
    [TestClass]
    public class UnitTestUserBDTestValidations
    {


        [TestCleanup]
        public void GC()
        {
            System.GC.Collect();
        }


        [TestMethod]
        public void TestEmailValidation()
        {
            string[] emails = System.IO.File.ReadAllLines(@"C:\Users\zhuri\Desktop\Документы от вуза\4_Для учебы\5 cем\Тестирование и отладка программного обеспечения\Лабы\AllLabs\UserBDTests\emails.txt");

            for (int i = 0; i< emails.Length; i++)
            {
                string[] email = emails[i].Split();

                bool rez = Laba7Liba.UserBD.CheckEmail(email[0]);
                Debug.WriteLine($"CheckEmailResult:{rez}||||| true result:{email[1]}   email:{email[0]}");
                if (rez != bool.Parse(email[1])) {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void TestTelephoneValidation()
        {
            string[] telephones = System.IO.File.ReadAllLines(@"C:\Users\zhuri\Desktop\Документы от вуза\4_Для учебы\5 cем\Тестирование и отладка программного обеспечения\Лабы\AllLabs\UserBDTests\telephones.txt");

            for (int i = 0; i < telephones.Length; i++)
            {
                string[] telephone = telephones[i].Split();

                bool rez = Laba7Liba.UserBD.CheckTelephoneNumber(telephone[0]);
                Debug.WriteLine($"CheckTelephoneNumber:{rez}||||| true result:{telephone[1]}  tel:{telephone[0]}");
                if (rez != bool.Parse(telephone[1]))
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void TestFIOValidation()
        {
            string[] fios = System.IO.File.ReadAllLines(@"C:\Users\zhuri\Desktop\Документы от вуза\4_Для учебы\5 cем\Тестирование и отладка программного обеспечения\Лабы\AllLabs\UserBDTests\fio.txt");

            for (int i = 0; i < fios.Length; i++)
            {
                string[] fio = fios[i].Split();

                bool rez = Laba7Liba.UserBD.CheckOnlyOnePartOfFIO(fio[0]);
                Debug.WriteLine($"CheckOnlyOnePartOfFIO:{rez}||||| true result:{fio[1]}  OnePartOfFIO:{fio[0]}");
                if (rez != bool.Parse(fio[1]))
                {
                    Assert.Fail();
                }
            }
        }
    }

    [TestClass]
    public class UnitTestUserBDTestCURD
    {

        Random random = new Random();

        Laba7Liba.UserBD userBD;

        [TestInitialize]
        public void Init()
        {
            userBD = new Laba7Liba.UserBD();
        }

        [TestCleanup]
        public void GC()
        {
            userBD = null;
            System.GC.Collect();
        }





        [TestMethod]
        public void TestUserAdd()
        {
            string[] commands = System.IO.File.ReadAllLines(@"C:\Users\zhuri\Desktop\Документы от вуза\4_Для учебы\5 cем\Тестирование и отладка программного обеспечения\Лабы\AllLabs\UserBDTests\useraddmanual.txt");

            for (int i = 0; i < commands.Length; i++)
            {
                //string[] command = Parser.ParseCommandLine(commands[i]);
                string[] command = commands[i].Split();
                Debug.WriteLine($"Попытка создать пользователя с такими параметрами: {command[0]} {command[1]} {command[2]} {command[3]} {command[4]} ");
                userBD.AddUser(command[0], command[1], command[2], command[3], command[4]);

                //try
                //{
                //    userBD.AddUser(command[0], command[1], command[2], command[3], command[4]);
                //}
                //catch (ArgumentException ex)
                //{
                //    Assert.Fail(ex.Message);
                //}


            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestUserNotAdd()
        {
            string[] commands = System.IO.File.ReadAllLines(@"C:\Users\zhuri\Desktop\Документы от вуза\4_Для учебы\5 cем\Тестирование и отладка программного обеспечения\Лабы\AllLabs\UserBDTests\usernotaddmanual.txt");

            for (int i = 0; i < commands.Length; i++)
            {
                //string[] command = Parser.ParseCommandLine(commands[i]);
                string[] command = commands[i].Split();
                Debug.WriteLine($"Попытка создать пользователя с такими параметрами: {command[0]} {command[1]} {command[2]} {command[3]} {command[4]} ");
                userBD.AddUser(command[0], command[1], command[2], command[3], command[4]);


            }
        }

        [TestMethod]
        public void TestUserAddWizard()
        {
            string[] commands = System.IO.File.ReadAllLines(@"C:\Users\zhuri\Desktop\Документы от вуза\4_Для учебы\5 cем\Тестирование и отладка программного обеспечения\Лабы\AllLabs\UserBDTests\useraddauto.txt");

            for (int i = 0; i < commands.Length; i++)
            {
                //string[] command = Parser.ParseCommandLine(commands[i]);
                string[] command = commands[i].Split();
                Debug.WriteLine($"Попытка создать пользователя с такими параметрами: {command[0]} {command[1]} {command[2]} {command[3]} {command[4]} answer {command[5]} rez {command[6]} ");
                bool rez = userBD.AddUserWizardUnitTest(command[0], command[1], command[2], command[3], command[4], command[5]);

                if (rez != bool.Parse(command[6]))
                {
                    Assert.Fail();
                }


            }

        }




        [TestMethod]
        public void TestUserChange()
        {

            userBD.Load();

            string[] commands = System.IO.File.ReadAllLines(@"C:\Users\zhuri\Desktop\Документы от вуза\4_Для учебы\5 cем\Тестирование и отладка программного обеспечения\Лабы\AllLabs\UserBDTests\userchangemanual.txt");

            for (int i = 0; i < commands.Length; i++)
            {

                string[] command = commands[i].Split();


                Debug.WriteLine(commands[i].Substring(2));
                string[] data = commands[i].Substring(2).Split();
                //stringdata = data.Split();

                Debug.WriteLine($"Команда пользователя: {commands[i]}.");

                userBD.ChangeUser(int.Parse(command[0]), data);



            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestUserNotChange()
        {

            userBD.Load();

            string[] commands = System.IO.File.ReadAllLines(@"C:\Users\zhuri\Desktop\Документы от вуза\4_Для учебы\5 cем\Тестирование и отладка программного обеспечения\Лабы\AllLabs\UserBDTests\usernotchangemanual.txt");

            for (int i = 0; i < commands.Length; i++)
            {

                string[] command = commands[i].Split();


                Debug.WriteLine(commands[i].Substring(2));
                string[] data = commands[i].Substring(2).Split();
                //stringdata = data.Split();

                Debug.WriteLine($"Команда пользователя: {commands[i]}.");

                userBD.ChangeUser(int.Parse(command[0]), data);



            }
        }

        [TestMethod]
        public void TestUserChangeWizard()
        {
            string[] commands = System.IO.File.ReadAllLines(@"C:\Users\zhuri\Desktop\Документы от вуза\4_Для учебы\5 cем\Тестирование и отладка программного обеспечения\Лабы\AllLabs\UserBDTests\userchangeauto.txt");

            userBD.Load();

            for (int i = 0; i < commands.Length; i++)
            {
                //string[] command = Parser.ParseCommandLine(commands[i]);
                string[] command = commands[i].Split();
                Debug.WriteLine($"Попытка изменить пользователя с id: {command[0]}, параметры: {command[1]} {command[2]} answer {command[3]} rez {command[4]}   ");
                bool rez = userBD.ChangeUserWizardUnitTest(int.Parse(command[0]), (UpdateType)int.Parse(command[1]), command[2], command[3]);

                if (rez != bool.Parse(command[4]))
                {
                    Assert.Fail();
                }


            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestUserNotChangeWizard()
        {
            string[] commands = System.IO.File.ReadAllLines(@"C:\Users\zhuri\Desktop\Документы от вуза\4_Для учебы\5 cем\Тестирование и отладка программного обеспечения\Лабы\AllLabs\UserBDTests\usernotchangeauto.txt");

            userBD.Load();

            for (int i = 0; i < commands.Length; i++)
            {
                //string[] command = Parser.ParseCommandLine(commands[i]);
                string[] command = commands[i].Split();
                Debug.WriteLine($"Попытка изменить пользователя с id: {command[0]}, параметры: {command[1]} {command[2]} answer {command[3]} rez {command[4]}   ");
                bool rez = userBD.ChangeUserWizardUnitTest(int.Parse(command[0]), (UpdateType)int.Parse(command[1]), command[2], command[3]);

                


            }
        }



        [TestMethod]
        public void TestUserDelete()
        {
            string[] commands = System.IO.File.ReadAllLines(@"C:\Users\zhuri\Desktop\Документы от вуза\4_Для учебы\5 cем\Тестирование и отладка программного обеспечения\Лабы\AllLabs\UserBDTests\userdelete.txt");

            userBD.Load();
            
            for (int i = 0; i < commands.Length; i++)
            {
                Debug.WriteLine($"Попытка удалить пользователя с id {commands[i]}");
                userBD.RemoveUser(int.Parse(commands[i]));

            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNotUserDelete()
        {
            string[] commands = System.IO.File.ReadAllLines(@"C:\Users\zhuri\Desktop\Документы от вуза\4_Для учебы\5 cем\Тестирование и отладка программного обеспечения\Лабы\AllLabs\UserBDTests\usernotdelete.txt");

            userBD.Load();

            for (int i = 0; i < commands.Length; i++)                
            {
                Debug.WriteLine($"Попытка удалить пользователя с id {commands[i]}");
                userBD.RemoveUser(int.Parse(commands[i]));

            }
        }


    }


    public static class Parser
    {
        public static string[] ParseCommandLine(string? notParsedCommandLine)
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