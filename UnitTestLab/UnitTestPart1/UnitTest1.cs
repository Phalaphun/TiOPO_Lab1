using UnitTestLabPart1;
using System.Diagnostics;
using System;

namespace UnitTestPart1
{
    [TestClass]
    public class CalcTests
    {
        private Calculator calc;
        private Random rnd;

        int a;
        int b;
        double da;
        double db;

        [TestInitialize]
        public void InitializeCalculatorAndParametrs()
        {
            rnd = new Random();

            a = rnd.Next();
            b = rnd.Next();

            da = rnd.NextDouble();
            db = rnd.NextDouble();

            calc = new Calculator();

            Debug.WriteLine($"����������� int a = {a}");
            Debug.WriteLine($"����������� int b = {b}");
            Debug.WriteLine($"����������� double da = {da}");
            Debug.WriteLine($"����������� double db = {db}\n");

        }

        [TestMethod]
        public void SumTest()
        {
            int rez = a + b;
            Debug.WriteLine($"�������������� ��������� a+b: {rez}\n");
            Assert.AreEqual(rez, calc.Sum(a, b));
        }
        [TestMethod]
        public void MultiplyTest()
        {
            int rez = a * b;
            Debug.WriteLine($"�������������� ��������� a*b: {rez} \n");
            Assert.AreEqual(rez, calc.Multiply(a, b));
        }
        [TestMethod]
        public void SubtractionTest()
        {
            double rez = da - db;
            Debug.WriteLine($"�������������� ��������� da-db: {rez}\n");
            Assert.AreEqual(rez, calc.Subtraction(da, db));
        }
        [TestMethod]
        public void DivisionTest()
        {
            double rez = da / db;
            Debug.WriteLine($"�������������� ��������� da/db: {rez}\n");
            Assert.AreEqual(rez, calc.Division(da, db));
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void TestDivideByZeroExceptionOccurrence()
        {
            
            double db = 0;
            Debug.WriteLine($"� ��������� da/db ��������� db = {db}");
            double rez = da / db;

            if (double.IsInfinity(rez))
            {
                throw new DivideByZeroException();
            }

        }


        [TestCleanup]
        public void Cleanup()
        {
            calc = null;
            rnd = null;
            GC.Collect();
        }
    }

    [TestClass]
    public class UserTests
    {
        static List<User> users;
        static Random rnd;

        [ClassInitialize]
        public static void Init(TestContext test)
        {
            users = new List<User>();
            rnd = new Random();

            users.Add(new User(RandomString(6), RandomString(5),true));
            users.Add(new User(RandomString(6), RandomString(5), true));
            users.Add(new User(RandomString(6), RandomString(5), false));
            users.Add(new User(RandomString(6), RandomString(5), true));
            users.Add(new User(RandomString(6), RandomString(5), true));
            users.Add(new User(RandomString(6), RandomString(5), false));
            users.Add(new User(RandomString(6), RandomString(5), false));
            users.Add(new User(RandomString(6), RandomString(5), true));
            users.Add(new User());
            users.Add(new User());
            users.Add(new User(RandomString(6), RandomString(5), true));
            users.Add(new User(RandomString(6), RandomString(5), true));
            users.Add(new User(RandomString(6), RandomString(5), true));


            
            
            
        }

        [TestMethod]
        public void TestIsUsersUniq()
        {
            PrintUsersID();
            CollectionAssert.AllItemsAreUnique(users);
        }

        [TestMethod]
        public void TestIsNullUserExist()
        {
            PrintUsersID();
            CollectionAssert.AllItemsAreNotNull(users);
        }

        [TestMethod]
        public void TestIsAnTeastableUserExist()
        {
            User testableUser = users[rnd.Next(users.Count + 1)];
            Debug.WriteLine($"\n�������� ������������: {testableUser.ToString()}");

            CollectionAssert.Contains(users,testableUser);
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        public void PrintUsersID()
        {
            Debug.WriteLine("������� ����� �� ���������� id");
            foreach (var user in users)
            {
                if (user.Id == null)
                {
                    Debug.WriteLine("nullID -> maybe user in null");
                }
                else
                {
                    Debug.WriteLine("id: " + user.Id.ToString());
                }

            }
        }
    }


    // 5 ������
    //1)���� ����� ������������ � ����� ��� ���� �����,
    //����� ������ ���� ������, ������� ��������� ������� �� ����������� ������ ��� ����. ��� ������� ������ ����� ����� ������ ������ 
    // 2) ������� ����� �����: �����, ������, bool ���� ��� ���, � id
    // 3) �������� ���� ��� ������ ������������ ��� ��� ��� ����������, ��� ���� �����, ��������� �� ������� ����������� ������������ � ���������
}