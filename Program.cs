using System;

namespace SharpSession
{
    public class Program
    {
        public static void Main()
        {
            TimeDifference td = new TimeDifference(new int[] { 2, 3, 43 });
			
            Tools.GeneralTools.PrintArray<int>(td.TimeDifferenceArray);
        }
    }
}


