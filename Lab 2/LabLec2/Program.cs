//#define test
using System;
using System.IO;


namespace LabLec1
{

    internal class NumberException : Exception
    {
        public NumberException(string message)
        : base(message) { }
    }
    internal class DiscMinException : Exception
    {
        public DiscMinException(string message)
        : base(message) { }
    }
    internal class DiscZeroException : Exception
    {
        public DiscZeroException(string message)
        : base(message) { }
    }
    internal class Program
    {
        static void Main()
        {
        
            Console.WriteLine("Программа для решения квадратных уравнений с двумя корнями");
            Console.WriteLine("Коэффициенты следует указать в файле *.txt. В каждой строке указать только число - коэффициент");
            Console.WriteLine("Введите путь до файла с коэффициентам." +
                " Если файл лежит в каталоге с программой,\nукажите только название и расширение файла");

#if !test
            //string path = Console.ReadLine() ?? throw new FileNotFoundException("Указан пустой путь");
            string? path = Console.ReadLine();
#elif test
            string path = "prim.txt";
#endif

            try
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("Такого файла нет. Завершаю работу"); //Если убрать if то там выбросится все таки другое исключение

                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            StreamReader sr = new StreamReader(path);
            double[] ans = new double[2];
            bool doOut=false;

            try
            {
                

                double[] coef = new double[3];
                int i = 0;
                while (!sr.EndOfStream )
                {
                    if( i == 3 )
                    {
                        break;
                    }
                    string? A = sr.ReadLine();
                    if (!double.TryParse(A, out coef[i]))
                    {
                        throw new NumberException($" Коэффициент {A} не был распознан как число");
                    }
                    i++;
                }
                Console.WriteLine($"\n\n\nВаше уравнение: {coef[0]}x^2+{coef[1]}x+{coef[2]}");
                double disc = Math.Pow(coef[1],2) - 4*coef[0]*coef[2];
                if (disc < 0)
                {
                    throw new DiscMinException($" Дискриминант оказался меньше нуля: {disc}");
                }

                ans[0] = (-coef[1] + Math.Sqrt(disc)) / (2 * coef[1]);
                ans[1] = (-coef[1] - Math.Sqrt(disc)) / (2 * coef[1]);

                if (Math.Abs(disc - 0) < 0.0000001)
                {
                    throw new DiscZeroException($"Дискриминант оказался равен 0");
                }
                
                //Console.WriteLine("Ответы: ");
                //Console.WriteLine(ans[0]);
                //Console.WriteLine(ans[1]);


            }
            
            catch(NumberException nb)
            {
                Console.WriteLine(nb.Message);
            }
            catch(DiscZeroException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Ответ: "+ans[0]);
                doOut = true;
                
            }
            catch(DiscMinException ex)
            {
                Console.WriteLine(ex.Message);
                doOut = true;
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (!doOut)
                {
                    Console.WriteLine("Ответы: ");
                    Console.WriteLine(ans[0]);
                    Console.WriteLine(ans[1]);
                }
                sr.Close();
            }


            

            
        }
    }
}

/*
 * Делаем лабу где решаем квадратное уравнение через дискриминант
 * При решении квадратного лвл учитываем 2 параметра: 
 * Дискриминант, если боль 0 то два корня, если 0, то корень один, иначе корней нет
 * Кодим программку которая решает, абс читаем с файла.
 * 1)Пишем кастомную ошибку если Д меньше 0
 * 2) Обрабатываем ошибку если файл отсутствует
 * 3) Когда открываем файл и при парсинге у нас пошли ошибки - тоже кастомная (типо читаем строку и если там не цифра то кастомная ошибка)
 * 4)Если д=0, то тоже кастомную ошибку выкидываем, но в тесте ошибки тупо Уравнение имеет 1 корень.
 */