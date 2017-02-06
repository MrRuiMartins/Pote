using Pote.ServiceModel;
using ServiceStack;

namespace Pote.ServiceInterface
{
    public class MediatorService : Service
    {
        public object Post(Mediator request)
        {
            return new MediatorResponse {
                Id = 3
            };
        }
    }
}