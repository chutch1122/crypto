using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamalHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = 7899;
            var p = 31847;

            DecryptElGamal("ElGamal-Ciphertext.txt", a, p);

            Console.ReadLine();
        }

        static void DecryptElGamal(string fileName, int a, int p)
        {
            var ciphertext = ReadCiphertext(fileName);
            var plaintextList = DecryptCiphertext(ciphertext, a, p);
            PrintPlaintext(plaintextList);
        }

        static void PrintPlaintext(List<string> plaintextList)
        {
            for (var i = 0; i < plaintextList.Count; i++)
            {
                Console.Write("{0} ", plaintextList[i]);

                if ((i + 1) % 5 == 0)
                    Console.WriteLine();
            }
        }

        static List<string> DecryptCiphertext(List<Tuple<int,int>> ciphertext, int a, int p)
        {
            var plaintext = new List<string>();

            foreach (var tuple in ciphertext)
            {
                var decryptedTuple = DecryptTuple(tuple, a, p);
                plaintext.Add(DecryptedNumberToString(decryptedTuple));
            }

            return plaintext;
        }

        static string DecryptedNumberToString(int packedNumber)
        {
            int[] unpackedNumbers = UnpackNumber(packedNumber);

            return UnpackedNumbersToString(unpackedNumbers);
        }

        static int DecryptTuple(Tuple<int,int> tuple, int a, int p)
        {
            int y1 = tuple.Item1;
            int y2 = tuple.Item2;

            // y2 * ( y1 ^ a ) ^ (-1)  modulo p
            int y1ToTheA = SquareAndMultiply(y1, a, p);
            int modularInverseOfY1ToTheA = ExtendedGCD(y1ToTheA, p).Item2;

            while (modularInverseOfY1ToTheA < 0) modularInverseOfY1ToTheA += p; // Make it positive

            int packedNumber = (y2 * modularInverseOfY1ToTheA) % p;

            return packedNumber;
        }

        static List<Tuple<int,int>> ReadCiphertext(string fileName)
        {
            List<Tuple<int, int>> ciphertext = new List<Tuple<int, int>>();

            using (TextReader reader = File.OpenText(@"..\..\" + fileName))
            {
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    // Remove whitespace from end of the line as well as parentheses
                    line = line.TrimEnd().Replace("(", string.Empty).Replace(")", string.Empty);
                    // Create an array of strings that represent each individual pair of values in a line
                    var elGamalTupleArray = line.Split(' ');

                    foreach (var elGamalTupleString in elGamalTupleArray)
                    {
                        if (elGamalTupleString == string.Empty) continue;

                        var elGamalStringArray = elGamalTupleString.Split(',');

                        int[] elGamalValueArray = new int[2];
                        elGamalValueArray[0] = int.Parse(elGamalStringArray[0]);
                        elGamalValueArray[1] = int.Parse(elGamalStringArray[1]);

                        var elGamalTuple = new Tuple<int, int>(elGamalValueArray[0], elGamalValueArray[1]);
                        ciphertext.Add(elGamalTuple);
                    }
                }
            }

            return ciphertext;
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

        static Tuple<int, int, int> ExtendedGCD(int a, int b)
        {
            int aa, bb, tt, t, ss, s, q, r, temp;
            aa = Math.Abs(a);
            bb = Math.Abs(b);
            tt = s = 0;
            t = ss = 1;

            q = (aa / bb); // Integer division

            r = aa - q * bb;

            while (r > 0)
            {
                temp = tt - q * t;

                tt = t;
                t = temp;

                temp = ss - q * s;

                ss = s;
                s = temp;

                aa = bb;

                bb = r;

                q = aa / bb; // Integer division

                r = aa - q * bb;
            }

            r = bb;

            return new Tuple<int, int, int>(r, s, t);
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
            result += Convert.ToChar(numbers[0] + 65);
            result += Convert.ToChar(numbers[1] + 65);
            result += Convert.ToChar(numbers[2] + 65);

            return result;
        }
    }
}
