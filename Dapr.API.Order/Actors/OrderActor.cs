using Dapr.Actors.Runtime;

namespace Dapr.API.Order.Actors
{
    [Actor(TypeName = "OrderActor")]
    public class OrderActor : Actor, IOrderActor
    {
        public OrderActor(ActorHost host) : base(host)
        {
        }

        public async Task Scan()
        {
            // Long running operation
            await Task.Delay(TimeSpan.FromMinutes(5));
        }
    }
}