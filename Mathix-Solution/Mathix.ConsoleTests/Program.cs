using Mathix.Types;
using System;

namespace Mathix.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            IntCalc();
        }

        private static void IntCalc()
        {
            string command = GetCommandInt();

            while (command.ToUpperInvariant() != "X")
            {
                Console.Write("Введите число a: ");
                string a = Console.ReadLine();

                BigInt uiA;
                if (a[0] == 'I')
                {
                    a = a.Replace("I", "");
                    uiA = new BigInt(a);
                }
                else
                {
                    uiA = BigInt.Parse(a);
                }

                Console.Write("Введите число b: ");
                string b = Console.ReadLine();
                BigInt uiB;
                if (b[0] == 'I')
                {
                    b = b.Replace("I", "");
                    uiB = new BigInt(b);
                }
                else
                {
                    uiB = BigInt.Parse(b);
                }

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

                command = GetCommandInt();
            }
        }

        private static void FractionCalc()
        {
            string command = GetCommandFraction();

            while (command.ToUpperInvariant() != "X")
            {
                Console.Write("Введите число a: ");
                string a = Console.ReadLine();

                BigInt uiA;
                if (a[0] == 'I')
                {
                    a = a.Replace("I", "");
                    uiA = new BigInt(a);
                }
                else
                {
                    uiA = BigInt.Parse(a);
                }

                Console.Write("Введите число b: ");
                string b = Console.ReadLine();
                BigInt uiB;
                if (b[0] == 'I')
                {
                    b = b.Replace("I", "");
                    uiB = new BigInt(b);
                }
                else
                {
                    uiB = BigInt.Parse(b);
                }

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

                command = GetCommandInt();
            }
        }

        private static string GetCommandInt()
        {
            PrintBreak();
            Console.WriteLine("Решение целочисленной операции a?b=: ");
            Console.Write("Введите оператор (+, - , *, /, %. X - выход): ");
            string command = Console.ReadLine();
            return command;
        }

        private static string GetCommandFraction()
        {
            PrintBreak();
            Console.WriteLine("Решение вещественной операции a?b=: ");
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

