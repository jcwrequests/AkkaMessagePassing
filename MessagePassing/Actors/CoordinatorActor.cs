using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace MessagePassing.Actors
{
    public class CoordinatorActor : ReceiveActor
    {
        public CoordinatorActor()
        {
            Receive<Messages.StartSession>(message =>
            {
                if (Context.Child(message.GroupName).IsNobody())
                {
                    Console.WriteLine("{0} - {1}", message.GetType().Name, Context.Self.Path.ToStringWithAddress());
                    IActorRef group = Context.ActorOf(Props.Create<GroupActor>(), message.GroupName);
                    group.Ask(message).PipeTo(Sender);
                }
            });
            
            Receive<Messages.IncrementState>(message =>
                {
                    Console.WriteLine("{0} - {1}", message.GetType().Name, Context.Self.Path.ToStringWithAddress());
                    IActorRef group = Context.Child(message.GroupName); 
                    group.Tell(message);
                });
            Receive<Messages.DecrementState>(message =>
                {
                    Console.WriteLine("{0} - {1}", message.GetType().Name, Context.Self.Path.ToStringWithAddress());
                    IActorRef group = Context.Child(message.GroupName);
                    group.Tell(message);
                });
            Receive<Messages.GroupEnded>(message =>
                {
                    Console.WriteLine("{0} - {1}", message.GetType().Name, Context.Self.Path.ToStringWithAddress());
                    IActorRef group = Context.Child(message.GroupName);
                    if (!group.IsNobody())
                    {
                       var gs = group.GracefulStop(TimeSpan.FromSeconds(30));
                       gs.Wait();
                    };
                });

            Receive<Messages.QueryUserActorState>(message =>
                {
                    Console.WriteLine("{0} - {1}", message.GetType().Name, Context.Self.Path.ToStringWithAddress());

                    //This is used to check the existence of the group and if the group did not exist then
                    //it could either spin up the actor in question then forward the message or send a failure
                    //message to a track the error or just return a null message.

                    IActorRef group = Context.Child(message.GroupName);
                    var handler = GetSelection(message.GroupName,message.UserName);

                    handler.
                        Ask<Messages.
                        QueryUserActionStateResult>(message).
                        PipeTo(Sender);

                });

        }

       
        private ActorSelection GetSelection(string groupName, string userName)
        {
            string path = string.Format("{0}/{1}/{2}", Self.Path.ToStringWithAddress(), groupName, userName);

            var handler = Context.System.ActorSelection(path);
            return handler;
        }
    }
}
