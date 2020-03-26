using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    /// <summary>Class Program.</summary>
    public static class Program
    {
        private static string formatType;
        private static string fileName;
        private static int amountRecords;
        private static int startId;

        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            ParseCommandLine(args);

            Records records = Generator(startId, amountRecords);

            switch (formatType)
            {
                case "csv":
                    ExportCSV(fileName, records.FileCabinetRecords);
                    Console.WriteLine($"{amountRecords} records were written to {fileName}");
                    break;

                case "xml":
                    ExportXML(fileName, records);
                    Console.WriteLine($"{amountRecords} records were written to {fileName}");
                    break;

                default:
                    Console.WriteLine($"Records were not written.");
                    throw new ArgumentException($"Incorrect {nameof(formatType)}.");
            }
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
            _ = int.TryParse(arguments[2], out amountRecords);
            _ = int.TryParse(arguments[3], out startId);
        }

        private static bool IsFullParameter(string[] args)
        {
            bool result = args.Any(x => x.Contains("--", StringComparison.Ordinal));

            return result;
        }

        private static FileCabinetRecord RecordGenerator(int id)
        {
            Random random = new Random();

            string genders = "MF";
            decimal[] workRates = new decimal[] { 0.25m, 0.5m, 0.75m, 1.0m, 1.25m, 1.5m };
            DateTime minDate = new DateTime(1950, 1, 1);
            DateTime maxDate = DateTime.Now;
            int days = Convert.ToInt32(maxDate.Subtract(minDate).TotalDays + 1);

            FileCabinetRecord record = new FileCabinetRecord()
            {
                Id = id,
                FullName = new FullName
                {
                    FirstName = "FirstName" + random.Next(0, 99999),
                    LastName = "LastName" + random.Next(0, 99999),
                },
                DateOfBirth = minDate.AddDays(random.Next(0, days)),
                Salary = (short)random.Next(100, 10001),
                WorkRate = workRates[random.Next(0, workRates.Length)],
                Gender = genders[random.Next(0, genders.Length)],
            };

            return record;
        }

        private static Records Generator(int startId, int amountRecords)
        {
            Records records = new Records(new List<FileCabinetRecord>(amountRecords));

            for (int i = 0; i < amountRecords; i++)
            {
                var record = RecordGenerator(startId++);
                records.FileCabinetRecords.Add(record);
            }

            return records;
        }

        private static void ExportCSV(string path, IEnumerable<FileCabinetRecord> records)
        {
            using StreamWriter writer = new StreamWriter(path);

            writer.WriteLine("Id,First Name,Last Name,Date of Birth,Salary,Work rate,Gender");
            foreach (var record in records)
            {
                writer.WriteLine(record.ToString());
            }
        }

        private static void ExportXML(string path, Records records)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Records));

            using StreamWriter writer = new StreamWriter(path);
            serializer.Serialize(writer, records);
        }
    }
}
