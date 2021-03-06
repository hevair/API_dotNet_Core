using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.DTOs;
using Api.Domain.Entities;
using Api.Domain.Interface;
using Api.Domain.Interface.Services.User;
using Api.Domain.Models;
using AutoMapper;

namespace Api.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserEntity> _repository;
        private readonly IMapper _mapper;

        public UserService(IRepository<UserEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> Delete(Guid id)
        {
           return await _repository.DeleteAsync(id);
        }

        public async Task<UserDTO> Get(Guid id)
        {
             var entity = await _repository.SelectAsync(id);
             var dto = _mapper.Map<UserDTO>(entity);

             return dto;
        }

        public async Task<IEnumerable<UserDTO>> GetAll()
        {
           var entities =  await _repository.SelectAsync();
           var listEntities = _mapper.Map<IEnumerable<UserDTO>>(entities);

            return listEntities;

        }

        public async Task<UserDTOCreateResult> Post(UserDTOCreate user)
        {   
            var model = _mapper.Map<UserModel>(user);
            var entity = _mapper.Map<UserEntity>(model);
            var result = await _repository.InsertAsync(entity);

            return _mapper.Map<UserDTOCreateResult>(result);
        }

        public async Task<UserDTOUpdateResult> Put(UserDTO user)
        {
            
            var model = _mapper.Map<UserModel>(user);
            var entity = _mapper.Map<UserEntity>(model);
            var result =  await _repository.UpdateAsync(entity);
            return _mapper.Map<UserDTOUpdateResult>(result);

        }
    }
}