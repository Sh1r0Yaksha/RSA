using System;
using System.Text;
using System.Security.Cryptography;
using System.Numerics;
using System.Globalization;

namespace RSA
{
    class Program
    {
        public static void Main(String[] args)
        {
            // Lambda function for returning empty string if null value is provided
            Func<string?, string> processInput = (string? message) => string.IsNullOrWhiteSpace(message) ? string.Empty : message;

            // Prompt for user to enter messsage
            Console.WriteLine("Enter the message: ");

            // Message after turning null value to empty string
            string? message = processInput(Console.ReadLine());

            // Encode message in UTF8 format and stores it in an array
            byte[] encodedMessage = Encoding.UTF8.GetBytes(message);

            // Encryption
            BigInteger prime1 = 43; // Change the prime numbers according to need
            BigInteger prime2 = 53;

            // Variable for storing secret key
            BigInteger secretKey;

            // Stores public key and N = prime1 * prime2
            PublicKey publicKey = CalculatePublicKey(prime1, prime2, out secretKey);

            // calculates cipher text for message, stores it in an array
            BigInteger[] cipher = CalculateCipher(encodedMessage, publicKey);

            // Convert cipher text into hexadecimal string format
            string[] cipherText = ConvertToHexString(cipher);

            // Decryption
            string decoded = Decrypt(cipherText, secretKey, publicKey.N);
            Console.WriteLine(decoded);
        }

        /// <summary>
        /// Method for calculating Public and Private key
        /// </summary>
        /// <param name="prime1">The first prime number</param>
        /// <param name="prime2">The second prime number</param>
        /// <param name="secretKey">The secret key which will be the output</param>
        /// <returns>PublicKey which contains both 'e' and 'N'</returns>
        static PublicKey CalculatePublicKey(BigInteger prime1, BigInteger prime2, out BigInteger secretKey)
        {
            BigInteger N = prime1 * prime2;

            BigInteger phiN = (prime1 - 1) * (prime2 - 1);

            BigInteger e = 2;

            while (e < phiN)
            {
                if (GCD(e, phiN) == 1)
                    break;
                else
                    e++;
            }

            BigInteger d = Inverse(e, phiN);
            secretKey = d;
            return new PublicKey(N, e);
        }
        
        /// <summary>
        /// Calculates the cipher text in an array format
        /// </summary>
        /// <param name="message">Byte array of the message text</param>
        /// <param name="pubKey">Public key calculated earlier</param>
        /// <returns>An array of BigIntegers, all containing ciphertexts 'c'</returns>
        static BigInteger[] CalculateCipher(byte[] message, PublicKey pubKey)
        {
            BigInteger[] cipher = new BigInteger[message.Length];
            for (int i = 0; i < message.Length; i++)
            {
                BigInteger c = BigInteger.ModPow(message[i], pubKey.pubKey, pubKey.N);
                cipher[i] = c;
            }
            return cipher;
        }

        /// <summary>
        /// Decrypts an array of cipher text
        /// </summary>
        /// <param name="cipherText">Array of cipher texts</param>
        /// <param name="sk">Secret Key, calculated earlier</param>
        /// <param name="N">N = prime1 * prime2</param>
        /// <returns>The decrypted message</returns>
        static string Decrypt(string[] cipherText, BigInteger sk, BigInteger N)
        {

            byte[] mPrime = new byte[cipherText.Length];

            for (int i = 0; i < mPrime.Length; i++)
            {
                BigInteger cipherBigInt = BigInteger.Parse(cipherText[i], NumberStyles.HexNumber);
                mPrime[i] = (byte)BigInteger.ModPow(cipherBigInt, sk, N);
            }

            return Encoding.UTF8.GetString(mPrime);
        }

        /// <summary>
        /// Helper function to convert BigInteger array to array of strings in hexadecimal format
        /// </summary>
        /// <param name="cipher">Array of BigIntegers</param>
        /// <returns>Array of string in hexadecimal format</returns>
        static string[] ConvertToHexString(BigInteger[] cipher)
        {
            string[] cipherText = new string[cipher.Length];
            Console.WriteLine("Ciphertext: ");
            for (int i = 0; i < cipher.Length; i++)
            {
                cipherText[i] = cipher[i].ToString("X");
                Console.Write($"{cipherText[i]} ");
            }
            Console.WriteLine("\n");
            return cipherText;
        }
        
        /// <summary>
        /// Helper function for calculating the greatest common divisor (gcd)
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns>gcd of the two numbers</returns>
        static BigInteger GCD(BigInteger num1, BigInteger num2)
        {
            while (num2 != 0)
            {
                BigInteger temp = num2;
                num2 = num1 % num2;
                num1 = temp;
            }
            return num1;
        }

        /// <summary>
        /// Calculating the inverse of 'a' using extended euclidean algorithm
        /// </summary>
        /// <param name="a"></param>
        /// <param name="n"></param>
        /// <returns> d such that d * a = 1 mod n</returns>
        static BigInteger Inverse(BigInteger a, BigInteger n)
        {
            BigInteger t = 0;
            BigInteger newt = 1;
            BigInteger r = n;
            BigInteger newr = a;

            while (newr != 0)
            {
                BigInteger quotient = (r / newr);
                BigInteger temp = newt;
                newt = t - (quotient * newt);
                t = temp;

                BigInteger temp2 = newr;
                newr = r - (quotient * temp2);
                r = temp2;
            }
            if (r > 1)
                return 0;

            if (t < 0)
                t += n;

            return t;
        }

    }
}