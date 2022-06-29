using Kafka.Producer.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kafka.Producer.WebApi.Controllers
{
    [Route("v1/mensagens")]
    public class ProducerController: ControllerBase
    {
        private readonly IProducerService _producerService;

        public ProducerController(IProducerService producerService)
        {
            _producerService=producerService;
        }

        [HttpPost()]
        public async Task<ActionResult> SalvarMensagens([FromBody] List<string> mensagens)
        {
            await _producerService.ProduzirAsync(mensagens);
            return Ok();
        }
    }
}
