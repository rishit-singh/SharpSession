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
            SessionManager manager = new SessionManager(DatabaseConfiguration.LoadFromFile("DatabaseConfig.json"));

            Session session;
            
            if ((session = manager.CreateSession("yd1ZEUOafBKGJO4fpeSu+R9RCAqkH3Vmt5iVzoLxON33pyhWQ890Px2+dVT0V5FfJSh36CIHy7PTrDFff7vpXQ==")) != null)
                Console.WriteLine($"Session created: {session.ID}");
        }
    }  
}


