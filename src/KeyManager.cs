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

            public bool KeyExists(APIKey key, bool dbCheck = false)
            {
                if (!dbCheck)
                    return this.APIKeyMap.ContainsKey(key.Key);

                return (this.DBInstance.FetchQueryData($"SELECT * FROM {this.APIKeyTable}", this.APIKeyTable).Length != 0);
            }


            public void BackupKeys()
            {
                Dictionary<string, APIKey>.ValueCollection keys = this.APIKeyMap.Values;

                foreach (APIKey key in keys)
                    if (this.KeyExists(key, true))
                        this.DBInstance.UpdateRecord(new Record(new string[] { "Key" }, new object[] { key.Key }),
                            key.ToRecord(), this.APIKeyTable);
                    else
                        this.DBInstance.InsertRecord(key.ToRecord(), this.APIKeyTable);
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

            public APIKey IssueAPIKey(string userID, Dictionary<string, bool> permissionsMap, bool backUp = true)
            {
                
                APIKey key = new APIKey(Guid.NewGuid().ToString(), userID, permissionsMap, true);
                
                this.APIKeyMap.Add(key.Key, key);

                if (backUp)
                    this.DBInstance.InsertRecord(key.ToRecord(), this.APIKeyTable);

                return key;
            }

            public APIKey IssueAPIKey(string userID, Dictionary<string, bool> permissionsMap, KeyValidityTime validityTime, bool backUp = true)
            {
                APIKey key = new APIKey(Guid.NewGuid().ToString(), userID, permissionsMap, validityTime);
                
                this.APIKeyMap.Add(key.Key, key);

                if (backUp)
                    this.DBInstance.InsertRecord(key.ToRecord(), this.APIKeyTable);

                return key;
            }

            public APIKeyManager(PostGRESDatabase dbInstance, bool load = false, bool autoBackup = false)
            {
                this.DBInstance = dbInstance;
                this.AutoBackup = autoBackup;
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
