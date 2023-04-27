using System;

namespace SharpSession
{
    public class Program
    {
        public static void PrintKeys(APIKey[] keys)
        {
            int size = keys.Length;

            for (int x = 0; x < size; x++)
                Console.WriteLine(keys[x].Key);
        }


        public static void Main()
        {
        }
    }
}


