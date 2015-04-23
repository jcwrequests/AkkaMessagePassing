# Akka Message Passing #


This project is to document and demonstrate state-full actors that are in a hierarchy. It scales up and scales down actors based on command messages.

The system is based on three actors. 

- CoordinatorActor
- GroupActor
- UserActor

###CoordinatorActor###
This Actor is the master of the system. It is responsible for creating new GroupActors and Shutting then down plus it forwards messages to the next level.

###GroupActor###
This Actor is the master to the UserActor. It is responsible for creating new UserActors and Shutting them down plus forwards messages to the UserActor.

###UserActor###
This Actor maintains the state of our system. It receives various commands that will change it's state and queries that return the current state.