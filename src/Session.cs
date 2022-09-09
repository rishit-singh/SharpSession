using System;
using System.Collections;
using OpenDatabase;

using SharpSession.Tools;

namespace SharpSession
{
    public enum SessionStatus
    {
        LoggedOut,
        LoggedIn
    }

    public class Session
    {
        public string ID;

        public APIKey SessionAPIKey;

        public string UserID;

        public SessionStatus Status;

        public static string[] StatusStrings = new string[] {
            "LoggedOut",
            "LoggedIn"
        };

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
                    Session.StatusStrings[(int)this.Status]
                });
        }

        public void SetStatus(SessionStatus status)
        {
            int statusInt = (int)status;

            this.Status = (SessionStatus)GeneralTools.Clip(ref statusInt, (int)SessionStatus.LoggedOut, (int)SessionStatus.LoggedIn);
        }

        public bool Start()
        {
            if (this.Status != SessionStatus.LoggedIn)
                this.Status = SessionStatus.LoggedIn;



            return true;
        }

        public bool Stop()
        {
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
}
