using System.Security.Cryptography;
using System.Text;

namespace UnitTestLabPart1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
    public class Calculator
    {
        public int Sum(int a, int b)
        {
            return a + b;
        }
        public int Multiply(int a, int b)
        {
            return a * b;
        }
        public double Division(double a, double b)
        {
            return a / b;
        }
        public double Subtraction(double a, double b)
        {
            return a - b;
        }


    }
   public class User
   {
        // 2) Создаем класс юзера: логин, пароль, bool блок или нет, и id
        private string login;
        private string password;
        private bool isBlocked;
        private int? id;
        static int counter = 0;
        public User()
        {
        }

        public User(string login, string password)
        {
            this.login = login;
            this.password = password;
            isBlocked = false;
            id = new Random().Next();

        }

        public User(string login, string password, bool isBlocked)
        {
            this.login = login;
            this.password = password;
            this.isBlocked = isBlocked;
            id = counter;
            counter++;
        }

        public User(string login, string password, bool isBlocked, int id)
        {
            this.login = login;
            this.password = password;
            this.isBlocked = isBlocked;
            this.id = id;
        }

        public string Login { get => login; set => login = value; }
        public string Password { get => password; set => password = value; }
        public bool IsBlocked { get => isBlocked; set => isBlocked = value; }
        public int? Id { get => id; set => id = value; }

        public override string ToString()
        {
            return $"Пользователь {login}: пароль - {password}; заблокирован - {isBlocked}; ID - {id}";
        }
    }
}