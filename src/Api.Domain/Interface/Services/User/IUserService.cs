using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.DTOs;
using Api.Domain.Entities;

namespace Api.Domain.Interface.Services.User
{
    public interface IUserService
    {
         Task<UserDTO> Get(Guid id);
         Task<IEnumerable<UserDTO>> GetAll();
         Task<UserDTOCreateResult> Post(UserDTOCreate user);
         Task<UserDTOUpdateResult> Put(UserDTO user);
         Task<bool> Delete(Guid id);

     }
}