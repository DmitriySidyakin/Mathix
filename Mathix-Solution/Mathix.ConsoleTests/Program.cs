using Mathix.Types;
using System;

namespace Mathix.ConsoleTests
{
    class Program
    {
        /*
        static void Main(string[] args)
        {

        }
        */
        /*
        private static Random rnd = new Random(49634);

        private static byte[] GetNewByteArray()
        {
            byte rndByte = GetNewByteArraySize();
            byte[] arr = new byte[rndByte];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = (byte)rnd.Next(byte.MinValue, byte.MaxValue);
            return arr;
        }

        private static byte GetNewByteArraySize()
        {
            return (byte)rnd.Next(1, 10);
        }

        private static byte GetRandomByte()
        {
            return (byte)rnd.Next(0, 15);
        }
        */
        static void Main(string[] args)
        {
            string command = GetCommand();

            while (command.ToUpperInvariant() != "X")
            {
                Console.Write("Введите число a: ");
                string a = Console.ReadLine();
                BigInt uiA = BigInt.Parse(a);

                Console.Write("Введите число b: ");
                string b = Console.ReadLine();
                BigInt uiB = BigInt.Parse(b);

                PrintBreak();

                BigInt result = BigInt.Zero();

                switch (command)
                {
                    case "+":
                        { result = uiA + uiB; }
                        break;
                    case "-":
                        { result = uiA - uiB; }
                        break;
                    case "*":
                        { result = uiA * uiB; }
                        break;
                    case "/":
                        { result = uiA / uiB; }
                        break;
                    case "%":
                        { result = uiA % uiB; }
                        break;
                }

                Console.WriteLine("Ответ: " + result.ToString(10));

                command = GetCommand();
            }
        }

        private static string GetCommand()
        {
            PrintBreak();
            Console.WriteLine("Решение целочисленной операции a?b=: ");
            Console.Write("Введите оператор (+, - , *, /, %. X - выход): ");
            string command = Console.ReadLine();
            return command;
        }

        private static void PrintBreak()
        {
            Console.WriteLine("_____________________________________");
        }
    }
}

