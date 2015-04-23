using MessagePassing.Contracts;
using MessagePassing.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagePassing.Messages
{
    public class StartSession : IResponseCommand
    {
        public StartSession(string userName, string groupName)
        {
            if (userName == null) throw new ArgumentNullException("userName");
            if (groupName == null) throw new ArgumentNullException("groupName");

            this.userName = userName;
            this.groupName = groupName;
            
        }
        private readonly string userName;
        private readonly string groupName;
       

        public string UserName
        {
            get { return userName; }
        }

        public string GroupName
        {
            get { return groupName; }
        }
        
    }

    public class EndSession : IResponseCommand
    {
        public EndSession(string userName, string groupName)
        {
            if (userName == null) throw new ArgumentNullException("userName");
            if (groupName == null) throw new ArgumentNullException("groupName");

            this.userName = userName;
            this.groupName = groupName;
            
        }
        private readonly string userName;
        private readonly string groupName;

        public string UserName
        {
            get { return userName; }
        }

        public string GroupName
        {
            get { return groupName; }
        }
        
    }

    public class IncrementState : IOneWayCommand
    {
        public IncrementState(string userName, string groupName)
        {
            if (userName == null) throw new ArgumentNullException("userName");
            if (groupName == null) throw new ArgumentNullException("groupName");

            this.userName = userName;
            this.groupName = groupName;
            
        }
        private readonly string userName;
        private readonly string groupName;

        public string UserName
        {
            get { return userName; }
        }

        public string GroupName
        {
            get { return groupName; }
        }
        
    }
    public class DecrementState : IOneWayCommand
    {
        public DecrementState(string userName, string groupName)
        {
            if (userName == null) throw new ArgumentNullException("userName");
            if (groupName == null) throw new ArgumentNullException("groupName");

            this.userName = userName;
            this.groupName = groupName;
            
        }
        private readonly string userName;
        private readonly string groupName;
        
        public string UserName
        {
            get { return userName; }
        }

        public string GroupName
        {
            get { return groupName; }
        }
        
    }
    public class QueryUserActorState : IResponseCommand
    {
        public QueryUserActorState(string userName, string groupName)
        {
            if (userName == null) throw new ArgumentNullException("userName");
            if (groupName == null) throw new ArgumentNullException("groupName");

            this.userName = userName;
            this.groupName = groupName;
        }
        private readonly string userName;
        private readonly string groupName;
       
        public string UserName
        {
            get { return userName; }
        }

        public string GroupName
        {
            get { return groupName; }
        }
        
    }
    public class QueryUserActionStateResult : IMessage
    {
        public QueryUserActionStateResult(UserActorState state)
        {
            if (state == null) throw new ArgumentNullException("state");
            this.State = state;
        }
        public readonly UserActorState State;
    }
    public class CommandProcessedAck : IMessage
    {
        public CommandProcessedAck() { }
    }
    public class SessionEnded : IEvent
    {
        public SessionEnded(string userName,string groupName) 
        {
            this.GroupName = groupName;
            this.UserName = userName;
        }
        public readonly string GroupName;
        public readonly string UserName;

    }
    public class GroupEnded : IEvent
    {
        public GroupEnded(string groupName) 
        {
            this.GroupName = groupName;
        }
        public readonly string GroupName;
    }

}
