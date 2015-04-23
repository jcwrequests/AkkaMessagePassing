using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagePassing.Contracts
{
    public interface IMessage
    {
    }
    public interface ICommand : IMessage
    {

    }
    public interface IEvent : IMessage
    {

    }
    public interface IOneWayCommand : ICommand
    {

    }
    public interface IResponseCommand : ICommand
    {

    }
}
