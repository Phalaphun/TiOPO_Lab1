using Microsoft.VisualStudio.TestTools.UnitTesting;
using lablec5_2;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {



        }
        [TestMethod]
        public void TestSum_10sum20result30()
        {
            Calculator cal = new Calculator();

            int a = 10;
            int b = 20;
            int expend = 31;

            int tt = cal.Sumn(a, b);

            Assert.AreEqual(tt, expend);
            
        }

        [TestInitialize] // То, что выполняется перед тестом 
        public void TestInit()
        {

        }

        [TestCleanup] // То, что выполняется после теста. По хорошему должен быть один 
        public void TestCleanup()
        {

        }

        
    }
    [TestClass]
    public class UnitTestException1 {

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void MyTestError()
        {
            int b = 0;
            int a = 5 / b;

            
        }    
    }



    [TestClass]
    public class UnitTestException2
    {
        static List<string> _item;

        [ClassInitialize]
        public static void Init(TestContext test)
        {
            _item = new List<string>();
            _item.Add("1");
            _item.Add("2");
            _item.Add("3");
            Debug.WriteLine("123");
        }

        [TestMethod]
        public void AllItemsAreNotNull()
        {
            CollectionAssert.AllItemsAreNotNull(_item, "Нулл был");
        }
    }



}

// 5 тестов
//1)Берём класс калькулятора и пилим для него тесты,
//также должен быть тестик, который приверяет деление на возвращение ошибки при нуле. Для каждого нового теста новые наборы данных 
// 2) Создаем класс юзера: логин, пароль, bool блок или нет, и id
// 3) Написать кейс что создаём пользователя что они все уникальный, нет нулл юзера, проверить на наличие конкретного пользователя в коллекции