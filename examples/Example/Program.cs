using System;
using System.Collections.Generic;
using System.Linq;
using Utility.CommandLine;

namespace Examples
{
    /// <summary>
    ///     Provides an eval/print loop for command line argument strings.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     Gets or sets a value indicating whether the Bool argument was supplied.
        /// </summary>
        [Argument('b', "boolean", "Gets or sets a value indicating whether the Bool argument was supplied.")]
        private static bool Bool { get; set; }

        /// <summary>
        ///     Gets or sets the value of the Double argument.
        /// </summary>
        [Argument('f', "float", "Gets or sets the value of the Double argument.")]
        private static double Double { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether show the help.
        /// </summary>
        [Argument('h', "help", "Gets or sets a value indicating whether show the help.")]
        private static bool Help { get; set; }

        /// <summary>
        ///     Gets or sets the value of the Int argument.
        /// </summary>
        [Argument('i', "integer", "Gets or sets the value of the Int argument.")]
        private static int Int { get; set; }

        /// <summary>
        ///     Gets or sets the value of the List argument.
        /// </summary>
        [Argument('l', "list", "Gets or sets the value of the List argument.")]
        private static List<int> List { get; set; }

        /// <summary>
        ///     Gets or sets the list of operands.
        /// </summary>
        [Operands]
        private static string[] Operands { get; set; }

        /// <summary>
        ///     Gets or sets the String argument.
        /// </summary>
        [Argument('s', "string")]
        private static string String { get; set; }

        /// <summary>
        ///     Returns a "pretty" string representation of the provided Type; specifically, corrects the naming of generic Types
        ///     and appends the type parameters for the type to the name as it appears in the code editor.
        /// </summary>
        /// <param name="type">The type for which the colloquial name should be created.</param>
        /// <returns>A "pretty" string representation of the provided Type.</returns>
        public static string ToColloquialString(this Type type)
        {
            return (!type.IsGenericType ? type.Name : type.Name.Split('`')[0] + "<" + String.Join(", ", type.GetGenericArguments().Select(a => a.ToColloquialString())) + ">");
        }

        /// <summary>
        ///     Application entry point
        /// </summary>
        /// <param name="args">Command line arguments</param>
        private static void Main(string[] args)
        {
            // enable ctrl+c
            Console.CancelKeyPress += (o, e) =>
            {
                Environment.Exit(1);
            };

            Console.WriteLine("At the prompt, enter text as if it were a string of command line arguments. Enter 'exit' to exit.");

            while (true)
            {
                Console.Write("> ");

                string input = Console.ReadLine();

                if (input == "exit")
                {
                    break;
                }

                Print(input);
            }
        }

        /// <summary>
        ///     Parses the specified command line string and displays the resulting dictionary, then populates the application's
        ///     properties with the values.
        /// </summary>
        /// <param name="commandLine">The command line string to parse.</param>
        private static void Print(string commandLine)
        {
            Reset();

            // populate properties
            Arguments.Populate(commandLine);

            if (Help)
            {
                ShowHelp();

                // It guarantees that you will not proceed because you asked to show help.
                return;
            }

            Console.WriteLine("\r\nArgument\tValue");
            Console.WriteLine("-------\t\t-------");

            Dictionary<string, object> argumentDictionary = Arguments.Parse(commandLine).ArgumentDictionary;

            foreach (string key in argumentDictionary.Keys)
            {
                Console.WriteLine(key + "\t\t" + argumentDictionary[key]);
            }

            Console.WriteLine("\r\nProperty\tValue");
            Console.WriteLine("-------\t\t-------");

            Console.WriteLine("String\t\t" + String);
            Console.WriteLine("Bool\t\t" + Bool);
            Console.WriteLine("Int\t\t" + Int);
            Console.WriteLine("Double\t\t" + Double);

            if (List != null)
            {
                Console.WriteLine("List\t\t" + string.Join(",", List.Select(o => o.ToString())?.ToArray()));
            }

            Console.WriteLine("\r\nOperands\n-------");

            for (int i = 0; i < Operands.Length; i++)
            {
                Console.WriteLine(i + ".\t" + Operands[i]);
            }
        }

        /// <summary>
        ///     Resets properties to their default values.
        /// </summary>
        private static void Reset()
        {
            String = string.Empty;
            Bool = false;
            Help = false;
            Int = 0;
            Double = 0;
            List = new List<int>();
        }

        /// <summary>
        ///     Show help for arguments.
        /// </summary>
        private static void ShowHelp()
        {
            var helpAttributes = Arguments.GetArgumentInfo(typeof(Program));

            var maxLen = helpAttributes.Select(a => a.Property.PropertyType.ToColloquialString()).OrderByDescending(s => s.Length).FirstOrDefault().Length;

            Console.WriteLine($"Short\tLong\t{"Type".PadRight(maxLen)}\tFunction");
            Console.WriteLine($"-----\t----\t{"----".PadRight(maxLen)}\t--------");

            foreach (var item in helpAttributes)
            {
                var result = item.ShortName + "\t" + item.LongName + "\t" + item.Property.PropertyType.ToColloquialString().PadRight(maxLen) + "\t" + item.HelpText;
                Console.WriteLine(result);
            }
        }
    }
}