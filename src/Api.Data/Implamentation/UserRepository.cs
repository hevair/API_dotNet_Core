using System.Threading.Tasks;
using Api.Data.Context;
using Api.Data.Repository;
using Api.Domain.Entities;
using Api.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Implamentation
{
    public class UserRepository: BaseRepository<UserEntity>, IUserRepository
    {

        private readonly DbSet<UserEntity> _db;


        public UserRepository(MyContext context): base(context)
        {
            _db = context.Set<UserEntity>();
        }
        public async Task<UserEntity> FindByUser(string email){
            
            var result = await _db.FirstOrDefaultAsync(u => u.Email.Equals(email)); 
            
            if(result == null){
                return null;
            }

            return result;
        }
    }
}