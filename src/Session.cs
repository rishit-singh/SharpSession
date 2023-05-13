using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using Newtonsoft.Json;
using OpenDatabase;
using OpenDatabaseAPI;
using SharpSession.Tools;


namespace SharpSession
{
    public enum SessionStatus
    {
        LoggedOut,
        LoggedIn
    }


    /// <summary>
    /// Contains info about a session.
    /// </summary>
    public class Session : IRecord 
    {
        public string ID;

        public string UserID;

        public APIKey SessionAPIKey;

        public SessionStatus Status { get { return this.Status; } set { this.SetStatus(value); } }

        public static string[] SessionStatusStrings = new string[] {
            "LoggedIn",
            "LoggedOut"
        };
        
        public Record ToRecord()
        {
            return new Record(new string[] {
                    "SessionID",
                    "APIKey",
                    "UserID",
                    "Status"
                },
                new object[] {
                    this.ID,
                    this.SessionAPIKey.Key,
                    this.UserID,
                    (int)this.Status
                });
        }


        public void SetStatus(SessionStatus status)
        {
            int statusInt = (int)status;

            this.Status = (SessionStatus)GeneralTools.Clip(ref statusInt, (int)SessionStatus.LoggedOut, (int)SessionStatus.LoggedIn);
        }

        public virtual bool Start()
        {
            this.Status = SessionStatus.LoggedIn;
            return true;
        }

        public virtual bool Stop()
        {
            this.Status  = SessionStatus.LoggedOut;
            return true;
        }

        public Session()
        {
            this.ID = null;
            this.SessionAPIKey = null;
            this.Status = SessionStatus.LoggedOut;
        }

        public Session(string id, string userID, APIKey apiKey, SessionStatus status)
        {
            this.ID = id;
            this.UserID = userID;
            this.SessionAPIKey = apiKey;
            this.Status = status;
        }
    }
    public class SessionManager
    {
        public Dictionary<string, Session> SessionMap;

        public PostGRESDatabase SessionDB;

        public APIKeyManager KeyManager;

        public string SessionTable;
        
        public void AddSession(Session session, bool updateDB = true)
        {
            this.SessionMap.Add(session.ID, session);
        }

        public Session CreateSession(string apiKey)
        {
            APIKey key;

            Session session;
            
            if (!this.KeyManager.KeyExists(apiKey))
                return null;

            this.SessionDB.InsertRecord((session = new Session(Guid.NewGuid().ToString(), (key = this.KeyManager.GetAPIKey(apiKey)).UserID, key, SessionStatus.LoggedIn)).ToRecord(), this.SessionTable);

            return session;
        }

        public void AssertSessionTable()
        {
            bool exists;
            if (!(exists = this.SessionTableExists()))
            {
                this.SessionDB.ExecuteQuery(this.GetTableSchema().GetCreateQuery());
                
                Console.WriteLine($"Table exists: {exists}");
            }
        }

        protected bool SessionTableExists()
        {
            return ((int)this.SessionDB.FetchQueryData($"SELECT * FROM information_schema.tables WHERE table_name=\'{this.SessionTable.ToLower()}\'", this.SessionTable.ToLower()).Length != 0);
        }
        
        protected Table GetTableSchema()
        {
            return new Table(this.SessionTable, new Field[]
            {
                new Field("SessionID", FieldType.Char, new  Flag[] { Flag.NotNull, Flag.PrimaryKey }, 64), 
                new Field("UserID", FieldType.Char, new Flag[] { Flag.NotNull }, 64),
                new Field("Status", FieldType.Int) 
            });
        }

        public Session GetSessionFromRecord(Record record)
        {
            SessionStatus status = SessionStatus.LoggedOut;
            
            for (int x = 0; x < Session.SessionStatusStrings.Length; x++)
                if (record.Values[2].ToString() == Session.SessionStatusStrings[x])
                    status = (SessionStatus)x;
                    
            return new Session(record.Values[0].ToString(), record.Values[1].ToString(), this.KeyManager.GetAPIKey(record.Values[1].ToString()), status);
        } 
        
        public Session GetSessionByID(string id, bool checkDB = false)
        {
            if (checkDB)
                return this.GetSessionFromRecord(this.SessionDB.FetchQueryData($"SELECT * FROM {this.SessionTable} WHERE SessionID=\'{id}\'", this.SessionTable)[0]); 
            
            else if (this.SessionMap.ContainsKey(id))
                return this.SessionMap[id];
            
            return null;
        }

        public SessionManager(DatabaseConfiguration databaseConfiguration, string table = "Sessions")
        {
            this.SessionMap = new Dictionary<string, Session>();
            this.SessionDB = new PostGRESDatabase(databaseConfiguration);
            this.SessionTable = table;
            this.KeyManager = new APIKeyManager(new PostGRESDatabase(databaseConfiguration));

            this.SessionDB.Connect();
            this.AssertSessionTable();
        }
    }
}
