using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Api.Domain.DTOs;
using Api.Domain.Entities;
using Api.Domain.Interface.Services.User;
using Api.Domain.Repository;
using Api.Domain.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Service.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _repository;

        private SigningConfigurations _signingConfigurations;
        private TokenConfigurations _tokenConfigutations;

        private IConfiguration _configutarions;
        public LoginService(IUserRepository repository, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations, IConfiguration configuration)
        {
            _repository = repository;
            _signingConfigurations = signingConfigurations;
            _tokenConfigutations = tokenConfigurations;
            _configutarions = configuration;
        }

        public async Task<object> FindByUser(LoginDTO Login)
        {

            var user = new UserEntity();

            if(Login.Email == null){
                return null;
            }

            user = await _repository.FindByUser(Login.Email);

            if(user == null){
                return new { 
                    authenticate = false,
                    menssage = "Falha ao autenticar"
                };
            }else{
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(user.Email),
                    new []{
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                    }
                );

                DateTime createDate = DateTime.Now;
                DateTime expirationDate = createDate + TimeSpan.FromSeconds(_tokenConfigutations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                string token = CreateToken(identity, createDate, expirationDate, handler);
                return SuccessObject(createDate, expirationDate, token, Login);
            } 

        }

        private string CreateToken(ClaimsIdentity identity, DateTime createDate ,DateTime expirationDate, JwtSecurityTokenHandler handler  ){

            var jwtSecurityToken = handler.CreateToken(new SecurityTokenDescriptor{
                Issuer = _tokenConfigutations.Issuer,
                Audience = _tokenConfigutations.Audience,
                SigningCredentials = _signingConfigurations.signingCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate
            });

            var token = handler.WriteToken(jwtSecurityToken);

            return token;
        }

        private object SuccessObject(DateTime createDate, DateTime expirationDate, string token, LoginDTO login){
            return new {
                authenticated = true,
                created = createDate.ToString("yyyy-mm-dd HH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-mm-dd HH:mm:ss"),
                acessToken = token,
                userName = login.Email,
                message = "Usuario authenticado com sucesso"
            };
        }
    }
}