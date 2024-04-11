using Core.Business;
using Core.Dtos.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [Authorize("Bearer")]
    [Authorize(AuthenticationSchemes = "CustomAuth")]
    [Route("[controller]")]
    [Tags("Authenticação")]
    [ApiController]
    public class AuthController(IAuthBusiness authBusiness) : ControllerBase
    {
        private readonly IAuthBusiness _authBusiness = authBusiness;

        /// <summary>
        /// SignIn
        /// </summary>
        /// <remarks>
        /// This method allow anonymous.
        /// 
        /// You dont have to call this method with a token in the header.
        /// 
        /// You can call this method to authenticate and get a Bearer token.
        /// 
        /// Fill correctly all parameters to call this method.
        /// </remarks>
        /// <param name="model">Modelo de entrada</param>
        [AllowAnonymous]
        [HttpPost("SignIn")]
        [SwaggerResponse(200, "Informações", typeof(SignInResponseDto))]
        [SwaggerResponse(400, "Erro", typeof(string))]
        public IActionResult SignInAsync([FromBody] SignInDto model)
        {
            try
            {
                return Ok(_authBusiness.SignIn(model));
            }
            catch (Exception ex)
            {
                return BadRequest($"AuthController | SignInAsync | {ex.Message}");
            }
        }

    }
}
