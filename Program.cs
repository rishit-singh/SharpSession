using System;
using System.Buffers.Text;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Newtonsoft.Json;
using OpenDatabase;
using OpenDatabaseAPI;
using SharpSession.Tools;

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


        public static void Main(string[] args)
        {
            PostGRESDatabase database = new PostGRESDatabase(DatabaseConfiguration.LoadFromFile("DatabaseConfig.json"));
            
            database.Connect();
            
            APIKeyManager manager = new APIKeyManager(database);
             
            Dictionary<string, bool> permissionsMap = new Dictionary<string, bool>();
            
            permissionsMap.Add("Read", true);
            
            manager.IssueAPIKey(Guid.NewGuid().ToString(), permissionsMap, new KeyValidityTime(DateTime.Now, new TimeDifference(1, 0, 0, 0)));
        }
    }  
}


