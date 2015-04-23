using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagePassing.ValueObjects
{
    
    public class UserActorState
    {
        public UserActorState(string userName, string groupName, int currentValue)
        {
            if (userName == null) throw new ArgumentNullException("userName");
            if (groupName == null) throw new ArgumentNullException("groupName");

            this.UserName = userName;
            this.CurrentValue = currentValue;
            this.GroupName = groupName;
        }
        public static UserActorState Increment(UserActorState currentState)
        {
            return new UserActorState(currentState.UserName, currentState.GroupName, currentState.CurrentValue + 1);
        }
        public static UserActorState Decrement(UserActorState currentState)
        {
            return new UserActorState(currentState.UserName, currentState.GroupName, currentState.CurrentValue - 1);
        }
        public readonly string UserName;
        public readonly string GroupName;
        public readonly int CurrentValue;
    }
}
