using System;
using FubuMVC.Core.Registration.ObjectGraph;

namespace Pipes
{
    public interface IHandlerFacility
    {
        void Register(Type messageType, ObjectDef def);
    }
}