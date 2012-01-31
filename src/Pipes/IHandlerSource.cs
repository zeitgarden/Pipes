using System;
using System.Collections.Generic;

namespace Pipes
{
    public interface IHandlerSource
    {
        IEnumerable<Type> GetHandlers();
    }
}