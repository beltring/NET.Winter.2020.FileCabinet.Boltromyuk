using System;
using System.Collections.Generic;
using System.Linq;

namespace FileCabinetGenerator
{
    class Program
    {
        private static string formatType;
        private static string fileName;
        private static int amountRecords;
        private static int startId;

        static void Main(string[] args)
        {
            ParseCommandLine(args);
            Console.WriteLine($"{amountRecords} records were written to {fileName}");
        }

        private static void ParseCommandLine(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException($"{nameof(args)} can't be null.");
            }

            if (args.Length < 4)
            {
                throw new ArgumentException($"{nameof(args)} length can't be less 1.");
            }

            List<string> arguments;

            if (IsFullParameter(args))
            {
                arguments = args.Select(x => x.Split("=")[1]).ToList();
            }
            else
            {
                arguments = Enumerable.Range(0, args.Length).Where(x => x % 2 != 0).Select(x => args[x]).ToList();
            }

            formatType = arguments[0];
            fileName = arguments[1];
            int.TryParse(arguments[2], out amountRecords);
            int.TryParse(arguments[3], out startId);
        }

        private static bool IsFullParameter(string[] args)
        {
            bool result = args.Any(x => x.Contains("--"));

            return result;
        }
    }
}
