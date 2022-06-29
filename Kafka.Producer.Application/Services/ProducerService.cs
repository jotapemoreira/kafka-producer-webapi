using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Kafka.Producer.Application.Services
{
    public class ProducerService : IProducerService
    {
        public static string _bootstrapServer;
        public static string _topic;
        public ProducerService(IConfiguration configuration)
        {
            var config = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _bootstrapServer = config["Kafka:Producer:BootstrapServer"] ?? throw new ArgumentNullException(nameof(ProducerService), "Seção Kafka:Producer section não está configurada.");
            _topic = config["Kafka:Producer:Topic"] ?? throw new ArgumentNullException(nameof(ProducerService), "Seção Kafka:Producer section não está configurada.");
        }

        public async Task ProduzirAsync(List<string> mensagens)
        {
            await ProduzirMensagensAsync(mensagens);
        }

        private static async Task ProduzirMensagensAsync(List<string> mensagens)
        {
            string bootstrapServers = _bootstrapServer;
            string nomeTopico = _topic;

            var logger = new LoggerConfiguration().WriteTo.Console(theme: AnsiConsoleTheme.Literate).CreateLogger();
            logger.Information("Produzindo mensagem com Kafka");

            logger.Information($"BootstrapServers = {bootstrapServers}");
            logger.Information($"Topic = {nomeTopico}");

            try
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = bootstrapServers
                };

                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {
                    foreach (var m in mensagens)
                    {
                        var result = await producer.ProduceAsync(nomeTopico, new() { Value = m });

                        logger.Information($"Mensagem: {m} | Status: {result.Status}");
                    }
                }

                logger.Information("Envio de mensagens finalizado.");
            }
            catch (Exception ex)
            {
                logger.Error($@"Exceção: {ex.GetType().FullName} | Mensagem: {ex.Message}");
            }
        }

    }
}
