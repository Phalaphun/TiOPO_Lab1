namespace Lab3
{
    delegate int DoMath(int a, int b);
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Программа для составленя таблиц сложения, вычитания, умножения и деления");
            Console.WriteLine("Введите параметры в формате \"A B +\" где A, B - размерность в целых положительных числах, а \"+\" оператор из числа \"+ - / *\":");
            string[] s = Console.ReadLine().ToLower().Trim().Split(); ;
            try
            {
                if (!int.TryParse(s[0], out int x) || !int.TryParse(s[1], out int y))
                {
                    throw new ArgumentException("Данная операция не определена. Воспользуйтесь + - / *");
                }

                switch (s[2])
                {
                    case "+":
                        PrintTable(Calculate(x, y, (a, b) => { return a + b; })); // (a+b) => a+b
                        break;
                    case "-":
                        PrintTable(Calculate(x, y, (a, b) => { return a - b; }));
                        break;
                    case "*":
                        PrintTable(Calculate(x, y, (a, b) => { return a * b; }));
                        break;
                    case "/":
                        PrintTable(Calculate(x, y, (a, b) => { return a / b; }));
                        break;
                    default:
                        throw new ArgumentException("Данная операция не определена. Воспользуйтесь + - / *");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            catch (OverflowException ex)
            {
                //Console.WriteLine(ex.Message);
                Console.WriteLine("Вы указали слишком большие числа, снизьте их на несколько порядков");
            }
            catch (OutOfMemoryException ex)
            {
                Console.WriteLine("Слишком большая таблица, уменьшите на порядок.");
            }

            

        }

        private static void PrintTable(int[,] ints)
        {

            

            for (int i = 0; i < ints.GetLength(0); i++)
            {
                //Console.WriteLine();
                Console.WriteLine("\n" );
                Console.Write("|");
                for (int j = 0; j < ints.GetLength(1); j++)
                {
                    ////Console.Write($" {ints[i,j]} |");
                    Console.Write(string.Format("{0,3} {1,3}", ints[i,j].ToString(), "|"));
                }
                
            }
            Console.WriteLine("\n" );
        }

        private static int[,] Calculate(int a, int b, DoMath operation) 
        {
            int[,] matrix = new int[a+1, b+1];
            for (int i = 0; i< matrix.GetLength(0);i++)
            {
                matrix[i,0] = i;
            }
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                matrix[0, i] = i;
            }
            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                for (int j = 1; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = operation(i, j);
                }
            }
            return matrix;
        }
    }
}