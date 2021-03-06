using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Interface;

namespace Api.Domain.Repository
{
    public interface IUserRepository : IRepository<UserEntity>
    {
         Task<UserEntity> FindByUser(string email);
    }
}