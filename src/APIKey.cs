using System;
using System.Collections;
using System.Collections.Generic;

using OpenDatabase;

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

    public struct KeyValidityTime
    {
        public DateTime CreationTime { get; set; }

        public DateTime ExpiryTime { get; set; }

        public bool IsExpired { get { return this.GetIsExpired(); } set {} }

        public void SetTimeDifference(TimeDifference timeDifference)
        {
            this.ExpiryTime.AddMonths(timeDifference.Months);
            this.ExpiryTime.AddDays(timeDifference.Days);
            this.ExpiryTime.AddHours(timeDifference.Hours);
            this.ExpiryTime.AddMinutes(timeDifference.Minutes);
            this.ExpiryTime.AddSeconds(timeDifference.Seconds);
        }

        public bool GetIsExpired()
        {
            return (GeneralTools.DateTimeCmp(this.CreationTime, this.ExpiryTime) <= 0);
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
    
    public class APIKey
    {
        public string Key;

        public string UserID;

        public KeyValidityTime ValidityTime;

        public bool IsLimitless;

        public Record GetRecord()
        {
            Record record = new Record();

            try
            {
                record = new Record(new string[] {
                        "Key",
                        "UserID",
                        "CreationTime",
                        "ExpiryTime",
                        "IsLimitless"
                    }, new object[] {
                        this.Key,
                        this.UserID,
                        this.ValidityTime.CreationTime.ToString(),
                        this.ValidityTime.ExpiryTime.ToString(),
                        this.IsLimitless
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return record;
        }

        public bool Refresh()
        {

            return true;
        }

        public bool IsValid()
        {
            return (
                this.Key != null &&
                this.UserID != null &&
                this.ValidityTime.IsExpired != true
            );
        }

        public APIKey()
        {
            this.Key = null;
            this.UserID = null;
            this.IsLimitless = false;
        }

        public APIKey(string key, string userID, KeyValidityTime validityTime = new KeyValidityTime(), bool isLimitless = true)
        {
            this.Key = key;
            this.UserID = userID;
            this.ValidityTime = validityTime;
        }

        public APIKey(string key, string userID, DateTime creationTime, TimeDifference validityTime)
        {
            this.Key = key;
            this.UserID = userID;
            this.ValidityTime = new KeyValidityTime(creationTime, validityTime);
            this.IsLimitless = false;
        }

        public APIKey(Record record)
        {
            this.Key = (string)record.Values[0];
            this.UserID = (string)record.Values[1];
            this.ValidityTime = new KeyValidityTime(DateTime.Parse((string)record.Values[2]), DateTime.Parse((string)record.Values[3]));
            this.IsLimitless = (bool)record.Values[4];
        }

        public APIKey(string key, string userID, KeyValidityTime validityTime)
        {
            this.Key = key;
            this.UserID = userID;
            this.ValidityTime = validityTime;
            this.IsLimitless = false;
        }
    }
}
