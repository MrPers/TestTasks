using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Сoin.Api.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {

        [Authorize]
        [HttpGet("get-secrets")]
        public async Task<IActionResult> getSecrets()
        {
            try
            {
                return Ok("secrets-get");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("post-secrets")]
        public async Task<IActionResult> postSecrets()
        {
            try
            {
                return Ok("secrets-pot");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("get-coinExchanges")]
        //public async Task<IActionResult> GetCoinsById(CoinRateQuestion coinRateQuestion)
        //{
        //    if (coinRateQuestion == null || coinRateQuestion.Id < 1 || coinRateQuestion.Step < 24)
        //    {
        //        throw new ArgumentNullException(nameof(coinRateQuestion));
        //    }

        //    try
        //    {
        //        var coins = await _coinService.GetCoinRateAllByIdAsync(coinRateQuestion.Id, coinRateQuestion.Step);

        //        return Ok(coinRateQuestion.InTick ? _mapper.Map<List<CoinRateVMInTicks>>(coins) : _mapper.Map<List<CoinRateVMInDateTime>>(coins));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


    }
}
