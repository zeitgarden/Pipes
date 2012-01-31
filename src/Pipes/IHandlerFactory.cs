using System;

namespace Pipes
{
    public interface IHandlerFactory
    {
        IHandler<T> GetHandler<T>(Guid id);
    }
}