using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSAHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = 18923;
            var a = 5797;

            Console.WriteLine("Table 5.1 - Plaintext");
            ConvertToPlaintext(a, n, "ciphertext-table51.txt");

            var n2 = 31313;
            var a2 = 6497;

            Console.WriteLine("Table 5.2 - Plaintext");
            ConvertToPlaintext(a2, n2, "ciphertext-table52.txt");

            Console.ReadLine();
        }

        static void ConvertToPlaintext(int a, int n, string fileName)
        {
            string finalPlaintext = string.Empty;

            using (TextReader reader = File.OpenText(@"..\..\" + fileName))
            {
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    // Remove whitespace from end of the line
                    line = line.TrimEnd();
                    // Create an array of strings that represent each individual value in a line
                    var ciphertextNumberStrings = line.Split(' ');
                    // Cycle through the ciphertext numbers
                    foreach (var ciphertextNumberString in ciphertextNumberStrings)
                    {
                        // Convert the string to an integer
                        var ciphertextNumber = int.Parse(ciphertextNumberString);
                        // Convert to plaintext using the Square And Multiply algorithm
                        var plaintextNumber = SquareAndMultiply(ciphertextNumber, a, n);
                        // Unpack the plaintext number into an array of integers that represent
                        // the characters in the plaintext
                        var unpackedPlaintextNumber = UnpackNumber(plaintextNumber);
                        // Convert the array of integers into a string of characters that is
                        // the plaintext
                        var plaintext = UnpackedNumbersToString(unpackedPlaintextNumber);

                        // Append the plaintext to our final plaintext
                        finalPlaintext += plaintext + "";
                    }
                    // Add newline characters to our plaintext before we read the next line
                    finalPlaintext += "\r\n";
                }
            }

            Console.WriteLine(finalPlaintext);
        }

        static int SquareAndMultiply(int x, int n, int m)
        {
            var z = 1;

            while (n > 0)
            {
                if (n % 2 == 1)
                {
                    z = x * z % m;
                    n = n - 1;
                }
                else
                {
                    x = (x * x) % m;
                    n = n / 2;
                }
            }

            return z;
        }

        static int[] UnpackNumber(int number)
        {
            var value = new int[3];

            value[2] = number % 26;
            value[1] = (number / 26) % 26;
            value[0] = ((number / 26) / 26) % 26;

            return value;
        }

        static string UnpackedNumbersToString(int[] numbers)
        {
            if (numbers.Length != 3) return string.Empty;

            string result = string.Empty;
            result += Convert.ToChar(numbers[0]+65);
            result += Convert.ToChar(numbers[1]+65);
            result += Convert.ToChar(numbers[2]+65);

            return result;
        }
    }
}
