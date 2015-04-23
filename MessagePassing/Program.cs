using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Akka.Actor;
using Akka.DI.Core;
using MessagePassing.Actors;


namespace MessagePassing
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ActorSystem system =  ActorSystem.Create("messageSystem"))
            {
                var coordinator = system.ActorOf(Props.Create<CoordinatorActor>(), "server1");

                var start = new Messages.StartSession("user1", "group1");

                //create a new user session
                var command = 
                    coordinator.
                    Ask<Messages.CommandProcessedAck>(start);
                //wait until complete 
                command.Wait();
                
                //Tell the coordinator to increment the state of the newly created
                //user session
                coordinator.Tell(new Messages.IncrementState("user1", "group1"));
                
                //Tell the newly created actor directly to increment state using actor selection
                var handler = system.ActorSelection("akka://messageSystem/user/server1/group1/user1");
                handler.Tell(new Messages.IncrementState("user1", "group1"));

                //Query through the coordinator the current state of the newly created actor
                var query1 = coordinator.Ask(new Messages.QueryUserActorState("user1", "group1"));
                query1.Wait();
                var result1 = query1.Result as Messages.QueryUserActionStateResult;
                Console.WriteLine("Results from query 1");
                Console.WriteLine(result1.State.CurrentValue);


                //query the state directly of the newly created actor using Actor Selection
                var query2 = handler.Ask(new Messages.QueryUserActorState("user1", "group1"));
                query2.Wait();
                var result = query2.Result as Messages.QueryUserActionStateResult;

                //query the state of the newly created actor through the group using Actor Selection
                var handler2 = system.ActorSelection("akka://messageSystem/user/server1/group1");
                var query3 = handler2.Ask(new Messages.QueryUserActorState("user1", "group1"));
                query3.Wait();
                var result2 = query3.Result as Messages.QueryUserActionStateResult;




                Console.WriteLine("Results from query 2");
                Console.WriteLine(result.State.CurrentValue);

                Console.WriteLine("Results for query 3");
                Console.WriteLine(result2.State.CurrentValue);

                handler.Tell(new Messages.EndSession("user1", "group1"));

                Console.WriteLine("Finished");
                Console.ReadLine();


            }
        }
    }
}
