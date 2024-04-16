using Dapr.Actors;

namespace Dapr.API.Product.Actors
{
    public interface IOrderActor : IActor
    {
        Task Scan();
    }
}