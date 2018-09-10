﻿using System;
using tcalc.Evaluation;
using tcalc.Parsing;

namespace tcalc
{
    class Program
    {
        static void Main()
        {
            Console.Write("tcalc> ");
            var line = Console.ReadLine();
            while (line != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    try
                    {
                        if (ExpressionParser.TryParse(line, out var expr, out var error))
                        {
                            var result = ExpressionEvaluator.Evaluate(expr);

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine(result);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(error);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                    }

                    Console.ResetColor();
                    Console.WriteLine();
                }

                Console.Write("tcalc> ");
                line = Console.ReadLine();
            }
        }
    }
}