using System;
using System.Collections;
using System.Collections.Generic;
using Npgsql;

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

        /// <summary>
        /// Clips the provided int to the nearest maximum or minimum value.
        /// </summary>
        /// <param name="val"> Value to be clipped. </param>
        /// <param name="min"> Minimum value. </param>
        /// <param name="max"> Maximum value. </param>
        /// <returns></returns>
        public static long Clip(long val, long min, long max)
        {
            if (val < min)
                return min;
            else if (val > max)
                return max;

            return val;
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
            return (val >= min && val < max);
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

        public static string ConvertBytesToString(Byte[] bytes)
        {
            string str = null;

            int size = bytes.Length;

            for (int x = 0; x < size; x++)
                str += bytes[x].ToString("x2");

            return str;
        }
    }

    public enum SortingAlgorithm
    {
        BubbleSort,
        QuickSort
    }

    public delegate void SortFunction<T>(ref T[] array);
    
    public class KeyTools
    {
        protected static SortFunction<APIKey>[] SortFunctions = new Tools.SortFunction<APIKey>[] {
            KeyTools.BubbleSort,
            KeyTools.QuickSortWrapper
        };

        protected static int Partition(ref APIKey[] keys, int start, int end)
        {
            APIKey pivot = keys[end];

            int i = start - 1;

            for (int x = start; x < end; x++)
                if (keys[x].CompareTo(pivot) >= 0)
                    GeneralTools.Swap<APIKey>(ref keys[i++], ref keys[x]);

            GeneralTools.Swap<APIKey>(ref keys[++i], ref keys[end]);
            

            return i;
        }

        protected static void QuickSort(ref APIKey[] keys, int start, int end)
        {
            int q;

            if (start < end)
            {
                q = KeyTools.Partition(ref keys, start, end);

                QuickSort(ref keys, start, q - 1);
                QuickSort(ref keys, q + 1, end);
            }
        }

        public static void BubbleSort(ref APIKey[] keys)
        {
            int size = keys.Length;

            for (int y = 0; y < size - 1; y++)
                for (int x = 0; x < size - y - 1; x++)
                    if (keys[x].CompareTo(keys[x + 1]) < 0)
                        GeneralTools.Swap<APIKey>(ref keys[x], ref keys[x + 1]); 
        }

        protected static void QuickSortWrapper(ref APIKey[] keys)
        {
            KeyTools.QuickSort(ref keys, 0, keys.Length - 1);
        }

        /// <summary>
        /// Sorts the provided APIKey array.
        /// </summary>  
        /// <param name="keys"></param>
        public static void Sort(ref APIKey[] keys, SortingAlgorithm sortingAlgorithm = SortingAlgorithm.QuickSort)
        {
            try
            {
                KeyTools.SortFunctions[GeneralTools.Clip((long)sortingAlgorithm, 0, (long)SortingAlgorithm.QuickSort)](ref keys);    
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static int BinarySearch(APIKey val, APIKey[] keys, int start, int end)
        {
            int mid = (start + end) / 2;

            if (start < end)
            {
                if (keys[mid].Key.CompareTo(val.Key) == 0)
                    return mid;

                else if (keys[mid].Key.CompareTo(val.Key) > 0)
                    return KeyTools.BinarySearch(val, keys, 0, mid);

                else if (keys[mid].Key.CompareTo(val.Key) < 0)
                    return KeyTools.BinarySearch(val, keys, mid + 1, end);
            }

            return -1;
        }

        public static string GetPermissionsString(Dictionary<string, bool> permissionsMap)
        {
            string permissionsString = null;

            string[] keys = permissionsMap.Keys.ToArray();

            bool[] values = permissionsMap.Values.ToArray();

            for (int x = 0; x < keys.Length; x++)
                permissionsString += $"{keys[x]}={values[x]};";

            return permissionsString;
        }

        public static Dictionary<string, bool> GetPermissionsMap(string permissionsString)
        {
            Dictionary<string, bool> permissionsMap = new Dictionary<string, bool>();

            string[] split = permissionsString.Split(';'), split1;

            for (int x = 0; x < split.Length; x++)
                permissionsMap.Add((split1 = split[x].Split('='))[0], split[1] == "true");
            
            return permissionsMap;
        } 
        
        public static int Search(APIKey val, APIKey[] array)
        {
            KeyTools.Sort(ref array);

            return KeyTools.BinarySearch(val, array, 0, array.Length);
        }
    }
}
