    using System;
    using System.Collections;
    using System;

    using OpenDatabase;
    using OpenDatabaseAPI;
    using SharpSession.Cryptography;


    namespace SharpSession
    {
        /// <summary>
        /// Routines for creating and managing API keys.
        /// </summary>
        public class APIKeyManager
        {
            public PostGRESDatabase DBInstance; // Database instance where the keys will be stored.

            public Dictionary<string, APIKey> APIKeyMap;

            public string APIKeyTable; // Default API key table

            public static TimeDifference DefaultKeyTimeDifference = new TimeDifference();

            public bool AutoBackup;

            public bool KeyExists(APIKey key)
            {
                return this.APIKeyMap.ContainsKey(key.Key);
            }

            public void BackupKeys()
            {
            }
            
            public void LoadKeys()
            {
                Record[] keyRecords = this.DBInstance.FetchQueryData($"SELECT * FROM {this.APIKeyTable};", this.APIKeyTable);

                APIKey temp;
                
                for (int x = 0; x < keyRecords.Length; x++)
                    this.APIKeyMap.Add((temp = new APIKey(keyRecords[x])).Key, temp);
            }

            public int GetAPIKeyCount()
            {
                return this.APIKeyMap.Count;
            }

            public APIKey IssueAPIKey(string userID)
            {
                return new APIKey();
            }

            public APIKey IssueAPIKey(string userID, KeyValidityTime validityTime)
            {
                return new APIKey();
            }

            public void UpdateKeys()
            {
            }

            public APIKeyManager(PostGRESDatabase dbInstance, bool load = false, bool autoBackup = false)
            {
                this.DBInstance = dbInstance;
                this. 
                    AutoBackup = autoBackup;
                this.APIKeyTable = "APIKeys";

                this.LoadKeys();
            }

            ~APIKeyManager()
            {
                if (this.AutoBackup)
                    this.BackupKeys();
            }
        } 
    }
