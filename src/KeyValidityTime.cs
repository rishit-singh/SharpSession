﻿    using System;
    using SharpSession.Tools;

    namespace SharpSession
    { 
        public class TimeDifference
        {
            public int Years;

            public int Months;

            public int Days;

            public int Hours;

            public int Minutes;

            public int Seconds;

            public int[] TimeDifferenceArray;

            public static int TimeDifferenceCmp(TimeDifference timeDifference, TimeDifference timeDifference1)
            {
                int size = timeDifference.TimeDifferenceArray.Length;

                for (int x = 0; x < size; x++)
                    if (timeDifference.TimeDifferenceArray[x] > timeDifference1.TimeDifferenceArray[x])
                        return 1;
                    else if (timeDifference.TimeDifferenceArray[x] > timeDifference1.TimeDifferenceArray[x])
                        return -1;

                return 0;
            }

            public static TimeDifference GetTimeDifference(DateTime dateTime, DateTime dateTime1)
            {
                TimeDifference timeDifference = null;

                try
                {
                    timeDifference = new TimeDifference(
                        dateTime.Year - dateTime1.Year,
                        dateTime.Month - dateTime1.Month,
                        dateTime.Day - dateTime1.Day,
                        dateTime.Hour - dateTime1.Hour,
                        dateTime.Second - dateTime1.Second
                    );
                }
                catch (Exception e)
                {
                }

                return timeDifference;
            }

            protected void MapTimeDifferenceArray()
            {
                this.TimeDifferenceArray = new int[] {
                        this.Years,
                        this.Months,
                        this.Hours,
                        this.Minutes,
                        this.Seconds
                    };
            }

            protected void MapProperties()
            {
                this.Years = this.TimeDifferenceArray[0];
                this.Months = this.TimeDifferenceArray[1];
                this.Days = this.TimeDifferenceArray[2];
                this.Hours = this.TimeDifferenceArray[3];
                this.Minutes = this.TimeDifferenceArray[4];
                this.Seconds = this.TimeDifferenceArray[5];
            }

            public static implicit operator TimeDifference(int[] timeDifference)
            {
                return new TimeDifference(timeDifference);
            }

            public TimeDifference(int years = 0, int months = 0, int days = 0, int hours = 0, int minutes = 0, int seconds = 0)
            {
                this.Years = years;
                this.Months = months;
                this.Days = days;
                this.Hours = hours;
                this.Minutes = minutes;
                this.Seconds = seconds;

                this.MapTimeDifferenceArray();
            }

            public TimeDifference(int[] timeDifferenceArray)
            {
                this.TimeDifferenceArray = new int[6];

                GeneralTools.ArrayCopy<int>(ref timeDifferenceArray, ref this.TimeDifferenceArray);

                this.MapProperties();
            }
        }

        public class KeyValidityTime
        {
            public DateTime CreationTime { get; set; }

            public DateTime ExpiryTime { get; set; }

            public TimeDifference Difference;
            
            public bool IsExpired { get { return this.GetIsExpired(); } set { } }

            public void SetTimeDifference(TimeDifference timeDifference)
            {
                this.Difference = timeDifference;
                
                this.ExpiryTime.AddMonths(this.Difference.Months);
                this.ExpiryTime.AddDays(this.Difference.Days);
                this.ExpiryTime.AddHours(this.Difference.Hours);
                this.ExpiryTime.AddMinutes(this.Difference.Minutes);
                this.ExpiryTime.AddSeconds(this.Difference.Seconds);
            }

            public bool GetIsExpired()
            {
                return (GeneralTools.DateTimeCmp(DateTime.Now, this.ExpiryTime) > 0);
            }

            protected void AssertValidity()
            {
                int comp;

                if ((comp = GeneralTools.DateTimeCmp(this.CreationTime, this.ExpiryTime)) >= 0)
                    this.ExpiryTime = this.CreationTime;

                this.SetTimeDifference(new TimeDifference());
            }

            public KeyValidityTime(DateTime creationTime, DateTime expiryTime)
            {
                this.CreationTime = creationTime;
                this.ExpiryTime = expiryTime;
            }

            public KeyValidityTime(DateTime creationTime, TimeDifference timeDifference)
            {
                this.CreationTime = creationTime;
                this.ExpiryTime = this.CreationTime;
                this.SetTimeDifference(timeDifference);
            }
        }
    }

