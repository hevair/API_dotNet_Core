using System.Threading.Tasks;
using Api.Domain.DTOs;
using Api.Domain.Entities;

namespace Api.Domain.Interface.Services.User
{
    public interface ILoginService
    {
         Task<object> FindByUser(LoginDTO login);
    }
}