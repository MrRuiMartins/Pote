using System;
using System.Collections.Generic;
using Pote.ServiceModel;

namespace Pote.Tests.Support
{
    public static class PoteFactory
    {
        public static readonly List<Type> ModelTypes = new List<Type> {
            typeof(Mediator)
        };
    }
}