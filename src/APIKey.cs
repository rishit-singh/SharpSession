using System;
using System.Collections;
using System.Collections.Generic;

using OpenDatabase;

using SharpSession.Tools;

namespace SharpSession
{
    /// <summary>
    /// Stores an API key with related methods.
    /// </summary>
    public class APIKey : IRecord, IComparable<APIKey>
    {
        public string Key;

        public string UserID;

        public KeyValidityTime ValidityTime;

        public Dictionary<string, bool> Permissions;

        public bool IsLimitless;

        public bool IsExpired { get { return this.GetIsExpired(); } set { } }

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

        public bool GetIsExpired()
        {
            if (this.IsLimitless)
                return false;

            return this.ValidityTime.IsExpired;
        }
            
        public bool IsValid()
        {
            return (
                this.Key != null &&
                this.UserID != null &&
                this.ValidityTime.IsExpired != true
            );
        }

        public int CompareTo(APIKey? key)
        {
            return this.Key.CompareTo(key?.Key);
        }

        public APIKey()
        {
            this.Key = null;
            this.UserID = null;
            this.IsLimitless = false;
        }

        public APIKey(string key, string userID, bool isLimitless = true)
        {
            this.Key = key;
            this.UserID = userID;
                this.IsLimitless = isLimitless;
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
