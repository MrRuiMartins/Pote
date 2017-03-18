using System.Collections.Generic;
using Pote.ServiceModel;

namespace Pote.Tests.Support {
    public static class PoteData 
    {
        public static List<Mediator> Mediators { get; set; }
        public static void LoadData()
        {
            Mediators = new List<Mediator> {
                new Mediator{},
                new Mediator{}
            };
        }
    }
}