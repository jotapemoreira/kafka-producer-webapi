namespace Kafka.Producer.Application.Services
{
    public interface IProducerService
    {
        Task ProduzirAsync(List<string> mensagens);
    }
}
