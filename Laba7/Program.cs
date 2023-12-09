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
            userBD.ListUsers();
            userBD.AddUser();
            userBD.ListUsers();

            Console.ReadKey();
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
