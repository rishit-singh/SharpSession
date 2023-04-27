using System;
using System.Collections;
using System.Runtime.CompilerServices;
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
    public class Session 
    {
        public string ID;

        public APIKey SessionAPIKey;

        public string UserID;

        public SessionStatus Status { get { return this.Status; } set { this.SetStatus(value); } }

        public Record GetRecord()
        {
            return new Record(new string[] {
                    "ID",
                    "APIKey",
                    "UserID",
                    "Status"
                },
                new object[] {
                    this.ID,
                    this.SessionAPIKey.Key,
                    this.UserID,
                    this.Status.ToString()
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
            this.SessionAPIKey = new APIKey();
            this.Status = SessionStatus.LoggedOut;
        }

        public Session(string id, APIKey apiKey, SessionStatus status)
        {
            this.ID = id;
            this.SessionAPIKey = apiKey;
            this.Status = status;
        }
    }


    public class SessionManager
    {
        public Dictionary<string, Session> SessionMap;

        public PostGRESDatabase SessionDB;
        
        public void AddSession(Session session)
        {
            this.SessionMap.Add(session.ID, session);
        }

        public Session GetSessionByID(string id)
        {
            if (this.SessionMap.ContainsKey(id))
                return this.SessionMap[id];

            return null;
        }

        public SessionManager(DatabaseConfiguration databaseConfiguration)
        {
            this.SessionMap = new Dictionary<string, Session>();
            this.SessionDB = new PostGRESDatabase(databaseConfiguration);
        }
    }
}
