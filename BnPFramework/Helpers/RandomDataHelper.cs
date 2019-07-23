using System;
using System.Linq;

namespace BnPBaseFramework.Web.Helpers
{
     /// <summary>
    /// Contains useful actions connected with dates
    /// </summary>
    public static class RandomDataHelper
    {
        public static string RandomAlphanumericString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            System.Random random = new System.Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomAlphanumericStringWithSpecialChars(int length)
        {
            const string chars1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@!#$%&*@\"";
            System.Random random = new System.Random();
            string result = new string(Enumerable.Repeat(chars1, 1).Select(s => s[random.Next(s.Length - (s.Length-1))]).ToArray())+new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length -1)]).ToArray());
            return result;
        }

        public static string RandomPassword(int length)
        {
            string result = RandomString(1).ToUpper() + RandomAlphanumericStringWithSpecialChars(length - 1);
            return result;
        }

        public static string RandomNumber(int length)
        {
            const string chars = "0123456789";
            System.Random random = new System.Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static string RandomString(int length)
        {

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            System.Random random = new System.Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomSpecialCharactersString(int length)
        {
            const string chars = @"!#$%&*@\";
            System.Random random = new System.Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}