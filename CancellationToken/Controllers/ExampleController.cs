using CancellationTokenExampl.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace CancellationTokenExampl.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExampleController: ControllerBase
    {
        private readonly IExampleRepository exampleRepository;

        public ExampleController(IExampleRepository exampleRepository) =>
            this.exampleRepository = exampleRepository;

        [HttpGet("without-cansellation")]
        public async Task<IActionResult> GetWithoutCancellation()
        {
            await exampleRepository.GetExampleDataAsync();
            return Ok(0);
        }

        [HttpGet("with-cansellation")]
        public async Task<IActionResult> GetWithCancellation(CancellationToken cancellationToken)
        {
            await exampleRepository.GetExampleDataAsync(cancellationToken);
            return Ok(0);
        }
    }

}
