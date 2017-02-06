using ServiceStack;

namespace Pote.ServiceModel
{
    [Route("/mediator")]
    public class Mediator : IReturn<MediatorResponse>
    {
        public int Id {get; set;}
    }
}