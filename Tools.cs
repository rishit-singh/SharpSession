using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpSession.Tools
{
    public class GeneralTools
    {
        public static void Swap<T>(ref T val, ref T val1)
        {
            T temp = val;

            val = val1;
            val1 = val;
        }

        public static void ArrayCopy<T>(ref T[] source, ref T[] destination)
        {
            int size = source.Length;

            for (int x = 0; x < size; x++)
                destination[x] = source[x];

            GeneralTools.PrintArray<T>(destination);
        }

        public static bool InRange(long val, long min, long max)
        {
            return (val > min && val < max);
        }

        public static int Clip(ref int num, int min, int max)
        {
            if (num  < min)
                num = min;

            else if (num > max)
                num = max;

            return num;
        }

        public static void PrintArray<T>(T[] array)
        {
            int size = array.Length;

            Console.Write("{ ");

            for (int x = 0; x < size; x++)
                Console.Write((x == size - 1) ? $"{array[x]}" : $"{array[x]}, ");

            Console.Write(" }");
        }

        public static int DateTimeCmp(DateTime dateTime, DateTime dateTime1)
        {
            int[,] dateTimeMatrix = new int[2, 6]{
                { dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second },
                { dateTime1.Year, dateTime1.Month, dateTime1.Day, dateTime1.Hour, dateTime1.Minute, dateTime1.Second },
            };

            for (int x = 0; x < 6; x++)
                if (dateTimeMatrix[0, x] > dateTimeMatrix[1, x])
                    return 1;
                else if (dateTimeMatrix[0, x] < dateTimeMatrix[1, x])
                    return -1;

            return 0;
        }
    }

    public class KeyTools
    {
        public static void SortKeys(ref APIKey[] keys)
        {
        }

        public static void KeyQuickSort(ref APIKey[] keys)
        {

        }
    }
}
