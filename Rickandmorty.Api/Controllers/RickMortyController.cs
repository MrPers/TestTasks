using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rickandmorty.Api.Models;
using Rickandmorty.Contracts.Services;
using Rickandmorty.DTO;

namespace Rickandmorty.Api.Controllers
{
    [ApiController]
    [Route("/api/v1")]
    public class RickMortyController : ControllerBase
    {
        private readonly IRickMortyService _rickMortyService;
        private readonly IMapper _mapper;

        public RickMortyController(
            IMapper mapper,
            IRickMortyService rickMortyService
        )
        {
            _mapper = mapper;
            _rickMortyService = rickMortyService;
        }

        [HttpPost("check-person")]
        public async Task<IActionResult> GetCheckPerson(CheckPersonVM checkPersoVM)// как проверить клас правильно
        {
            if (string.IsNullOrEmpty(checkPersoVM.PersonName) || string.IsNullOrEmpty(checkPersoVM.EpisodeName))
            {
                return NotFound();
            }

            try
            {
                var checkPersoDto = _mapper.Map<CheckPersonDto>(checkPersoVM);
                var answer = await _rickMortyService.GetCheckPersonAsync(checkPersoDto);
                return Ok(answer);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("person")]
        public async Task<IActionResult> GetPerson(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return ValidationProblem();
            }

            try
            {
                var persons = await _rickMortyService.GetPersonsFullInformationAsync(name);
                var answer = _mapper.Map<List<PersonVM>>(persons);
                return Ok(answer);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}