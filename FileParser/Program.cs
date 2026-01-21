using System;
using System.IO;

class Program
{
    static void Main()
    {
        /*
         * The input file is expected to be located inside an "Incoming" folder
         * relative to the execution directory.
         */

        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string inputDirectory = Path.Combine(baseDirectory, "Incoming");
        string inputFilePath = Path.Combine(inputDirectory, "InputData.txt");

        Console.WriteLine("Attempting to read input file from:");
        Console.WriteLine(inputFilePath);

        // Validate that the input file exists
        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine("Input file not found.");
            return;
        }

        /*
         * The output file will be created inside an "Outgoing" folder.
         * If the folder does not exist, it will be created automatically.
         */

        string outputDirectory = Path.Combine(baseDirectory, "Outgoing");

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        string outputFilePath = Path.Combine(outputDirectory, "OutputData.txt");

        /*
         * Each line is parsed using the pipe '|' delimiter.
         * The first fields represent customer data.
         * Remaining fields represent category and amount pairs.
         */

        string[] lines = File.ReadAllLines(inputFilePath);

        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            foreach (string line in lines)
            {
                // Skip empty or whitespace-only lines
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                // Split the line into individual fields
                string[] parts = line.Split('|');

                /*
                 * CUSTOMER RECORD
                 */

                string customerId = parts[0];
                string firstName = parts[1];
                string lastName = parts[2];
                string phoneNumber = parts[3];
                string city = parts[6];
                string state = parts[7];

                writer.WriteLine(
                    $"CUSTOMER,{customerId},{firstName},{lastName},{phoneNumber},{city},{state}"
                );

                /*
                 * DETAIL RECORDS
                 * Each pair of fields represents:
                 * - Category
                 * - Amount
                 */

                for (int i = 10; i < parts.Length; i += 2)
                {
                    string category = parts[i];
                    string amount = parts[i + 1];

                    writer.WriteLine(
                        $"DETAIL,{customerId},{category},{amount}"
                    );
                }
            }
        }

        Console.WriteLine("Output file generated successfully in Outgoing folder.");
    }
}

