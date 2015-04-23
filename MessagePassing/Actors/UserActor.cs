using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using MessagePassing.ValueObjects;

namespace MessagePassing.Actors
{
    public class UserActor :ReceiveActor
    {
        private UserActorState state;

        public UserActor()
        {

            Receive<Messages.StartSession>(message =>
                {
                    Console.WriteLine("{0} - {1}", message.GetType().Name, Context.Self.Path.ToStringWithAddress());
                    state = new UserActorState(message.UserName, message.GroupName, 0);
                    Sender.Tell(new Messages.CommandProcessedAck());

                });
            Receive<Messages.IncrementState>(message =>
                {
                    Console.WriteLine("{0} - {1}", message.GetType().Name, Context.Self.Path.ToStringWithAddress());
                    state = UserActorState.Increment(state);
                });

            Receive<Messages.DecrementState>(message =>
                {
                    Console.WriteLine("{0} - {1}", message.GetType().Name, Context.Self.Path.ToStringWithAddress());
                    state = UserActorState.Decrement(state);
                });

            Receive<Messages.QueryUserActorState>(message =>
                {
                    Console.WriteLine("{0} - {1}", message.GetType().Name, Context.Self.Path.ToStringWithAddress());
                    Sender.Tell(new Messages.QueryUserActionStateResult(state));
                });

            Receive<Messages.EndSession>(message =>
                {
                    Console.WriteLine("{0} - {1}", message.GetType().Name, Context.Self.Path.ToStringWithAddress());
                    state = null;
                    //Context.Self.Tell(PoisonPill.Instance);
                    //Context.Self.Tell(Kill.Instance);
                    //Context.Stop(Context.Self);
                    //var result = Context.Self.GracefulStop(TimeSpan.FromSeconds(30));
                    //result.Wait();
                    Context.Parent.Tell(new Messages.SessionEnded(message.UserName,message.GroupName));
                });
        }
    }
}
