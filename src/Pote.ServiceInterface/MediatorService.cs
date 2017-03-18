using Pote.ServiceModel;
using ServiceStack;

namespace Pote.ServiceInterface
{
    public class MediatorService : Service
    {

        public object Post(Mediator request)
        {
            //using (var db = dbFactory.Open())
            //{
            //}

            return new MediatorResponse
            {
                Id = 3
            };
        }
    }
}