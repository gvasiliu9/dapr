using Dapr.Actors;
using Dapr.Actors.Runtime;

namespace Dapr.API.Order.Actors
{
    public interface IOrderActor : IActor
    {
        Task Scan();
    }
}