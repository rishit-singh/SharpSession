    using System;
    using System.Collections;
    using System;
    using System.Runtime.CompilerServices;
    using Newtonsoft.Json;
    using OpenDatabase;
    using OpenDatabaseAPI;
    using SharpSession.Cryptography;
    using SharpSession.Tools;


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


            public static Table GetTableSchema(string tableName)
            {
                return new Table(tableName,
                    new Field[] {
                         new Field("Key", FieldType.Char, new Flag[] { Flag.NotNull }, 88),
                         new Field("UserID", FieldType.Char, new Flag[] { Flag.PrimaryKey, Flag.NotNull }, 64),
                         new Field("Permissions", FieldType.VarChar, new Flag[]{}, 1024),
                         new Field("CreationTime", FieldType.VarChar, new Flag[] { Flag.NotNull}, 22),
                         new Field("ExpiryTime", FieldType.VarChar, new Flag[]{}, 22),
                         new Field("IsLimitless", FieldType.Bool, new Flag[] { Flag.NotNull })
                    }); 
            }
            
            public bool KeyExists(APIKey key, bool dbCheck = false)
            {
                if (!dbCheck)
                    return this.APIKeyMap.ContainsKey(key.Key);

                return (this.DBInstance.FetchQueryData($"SELECT * FROM {this.APIKeyTable} WHERE Key=\'{key.Key}\'", this.APIKeyTable).Length != 0);
            }
    
            public bool KeyExists(string key, bool dbCheck = false)
            {
                if (!dbCheck)
                    return this.APIKeyMap.ContainsKey(key);

                return (this.DBInstance.FetchQueryData($"SELECT * FROM {this.APIKeyTable} WHERE Key=\'{key}\'", this.APIKeyTable).Length != 0);
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

            protected bool KeyTableExists()
            {
                return ((int)this.DBInstance.FetchQueryData("SELECT * FROM information_schema.tables WHERE table_name=\'apikeys\'", this.APIKeyTable).Length != 0);
            }

            public APIKey IssueAPIKey(string userID, Dictionary<string, bool> permissionsMap, bool backUp = true)
            {
                APIKey key = new APIKey(GeneralTools.GetRandomBase64(64), userID, permissionsMap, true);
                
                this.
                    APIKeyMap.Add(key.Key, key);

                if (backUp)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(key.ToRecord()));
                    this.DBInstance.InsertRecord(key.ToRecord(), this.APIKeyTable);
                }

                return key;
            }

            public APIKey IssueAPIKey(string userID, Dictionary<string, bool> permissionsMap, KeyValidityTime validityTime, bool backUp = true)
            {
                APIKey key = new APIKey(GeneralTools.GetRandomBase64(64), userID, permissionsMap, validityTime);
                
                this.APIKeyMap.Add(key.Key, key);

                if (backUp)
                    this.DBInstance.InsertRecord(key.ToRecord(), this.APIKeyTable);

                return key;
            }

            public bool RevokeAPIKey(string key)
            {
                if (this.KeyExists(key, true))
                {
                    this.DBInstance.ExecuteQuery($"DELETE FROM {this.APIKeyTable} WHERE Key=\'{key}\'");
                    
                    return true;
                }

                return false;
            }

            public APIKeyManager(PostGRESDatabase dbInstance, string tableName = "APIKeys", bool load = false, bool autoBackup = false)
            {
                this.DBInstance = dbInstance;
                this.AutoBackup = autoBackup;
                this.APIKeyTable = tableName;
                
                this.APIKeyMap = new Dictionary<string, APIKey>();

                if (!this.KeyTableExists())
                    this.DBInstance.ExecuteQuery(APIKeyManager.GetTableSchema("APIKeys").GetCreateQuery());
                
                if (load) 
                    this.LoadKeys();
            }
       

            ~APIKeyManager()
            {
                if (this.AutoBackup)
                    this.BackupKeys();
            }
        }
    }
