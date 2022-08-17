namespace Inventory.Consumers
{
    public interface IOrderConsumer
    {
        Task RegisterReceiveMessageHandler();
    }
}
