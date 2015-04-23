using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.Core;

namespace MessagePassing.Actors
{
    public class GroupActor : ReceiveActor
    {
        public GroupActor()
        {
            Receive<Messages.StartSession>(message =>
                {
                    Console.WriteLine("{0} - {1}", message.GetType().Name, Context.Self.Path.ToStringWithAddress());
                    if (Context.Child(message.UserName).IsNobody())
                    {
                        IActorRef user = Context.ActorOf(Props.Create<UserActor>(), message.UserName);
                        user.Ask(message).PipeTo(Sender);
                    }
                });

            Receive<Messages.DecrementState>(message =>
                {
                    Console.WriteLine("{0} - {1}", message.GetType().Name, Context.Self.Path.ToStringWithAddress());
                    IActorRef user = Context.Child(message.UserName);
                    //if (user.IsNobody) { Send Nack}
                    user.Tell(message);
                });

            Receive<Messages.IncrementState>(message =>
                {
                    Console.WriteLine("{0} - {1}", message.GetType().Name, Context.Self.Path.ToStringWithAddress());
                    IActorRef user = Context.Child(message.UserName);
                    //if (user.IsNobody) { Send Nack}
                    user.Tell(message);
                });

            Receive<Messages.EndSession>(message =>
                {
                    Console.WriteLine("{0} - {1}", message.GetType().Name, Context.Self.Path.ToStringWithAddress());
                    IActorRef user = Context.Child(message.UserName);
                    //if (user.IsNobody) { Send Nack}
                    var ack = user.Ask<Messages.SessionEnded>(message);
                    ack.Wait();
                    if (!ack.IsFaulted) Context.Self.Tell(new Messages.SessionEnded(message.UserName,message.GroupName));
                    
                });

            Receive<Messages.SessionEnded>(message =>
            {
                Console.WriteLine("{0} - {1}", message.GetType().Name, Context.Self.Path.ToStringWithAddress());

                IActorRef user = Context.Child(message.UserName);
                var gs = user.GracefulStop(TimeSpan.FromSeconds(30));
                gs.Wait();

                //if (!user.IsNobody()) Context.Stop(user);

                if (Context.GetChildren().All(child => ((IInternalActorRef)child).IsTerminated))
                {

                    //Context.Self.Tell(PoisonPill.Instance);
                    //Context.Self.Tell(Kill.Instance);
                    //Context.Stop(Context.Self);
                    //var result = Context.Self.GracefulStop(TimeSpan.FromSeconds(30));
                    //result.Wait();
                    //if (result.Result.Equals(true)) ;
                    //if (result)
                    Context.Parent.Tell(new Messages.GroupEnded(message.GroupName));
                }
            });

            Receive<Messages.QueryUserActorState>(message =>
                {
                    Console.WriteLine("{0} - {1}", message.GetType().Name, Context.Self.Path.ToStringWithAddress());

                    IActorRef user = Context.Child(message.UserName);
                    //if (user.IsNobody) { Send Nack}
                    user.
                        Ask<Messages.QueryUserActionStateResult>(message).
                        PipeTo(Sender);
 
                });
        }

        

    }
}
