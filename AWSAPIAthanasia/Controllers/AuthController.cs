using ApiAthanasia.Helpers;
using ApiAthanasia.Models;
using ApiAthanasia.Models.Tables;
using ApiAthanasia.Repositories;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ApiAthanasia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IRepositoryAthanasia repo;
        private HelperActionServicesOAuth helper;
        private string encriptKey;


        public AuthController(IRepositoryAthanasia repo, HelperActionServicesOAuth helper, SecretClient secretClient)
        {
            this.repo = repo;
            this.helper = helper;
            KeyVaultSecret secretIssuer = secretClient.GetSecret("EncriptKey");
            this.encriptKey = secretIssuer.Value;
        }

        /// <summary>
        /// Realiza el inicio de sesión de un usuario.
        /// </summary>
        /// <param name="model">Modelo de datos de inicio de sesión.</param>
        /// <response code="200">Inicio de sesión exitoso. Devuelve un token JWT válido.</response>
        /// <response code="401">Unauthorized. Las credenciales proporcionadas son inválidas.</response>
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            Usuario usuario = await this.repo.LogInUserAsync(model.Email, model.Password);
            if (usuario == null)
            {
                return Unauthorized();
            }
            SigningCredentials credentials = new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);
            string jsonUsuario = JsonConvert.SerializeObject(usuario);
            string encriptedUser=HelperCryptography.EncryptString(this.encriptKey, jsonUsuario);
            Claim[] informacion = new[]
                {
                    new Claim("UserData",encriptedUser),
                };
            JwtSecurityToken token = new JwtSecurityToken(
                claims: informacion,
                issuer: this.helper.Issuer,
                audience: this.helper.Audience,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddMinutes(30),
                notBefore: DateTime.UtcNow
                );
            return Ok(new { response = new JwtSecurityTokenHandler().WriteToken(token) });
        }


    }
}
