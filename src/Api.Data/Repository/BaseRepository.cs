using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Data.Context;
using Api.Domain.Entities;
using Api.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly MyContext _context;
        public BaseRepository(MyContext context)
        {
            _context = context;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await _context.Set<T>().SingleOrDefaultAsync(u => u.Id.Equals(id));

            if(result == null){
                return false;
            }

             _context.Remove(result);

             await _context.SaveChangesAsync();

             return true;
        }

        public async Task<T> InsertAsync(T item)
        {
            if(item.Id == Guid.Empty){
                item.Id = Guid.NewGuid();
            }

            item.CreateAt = DateTime.UtcNow;

            await _context.Set<T>().AddAsync(item);

            await _context.SaveChangesAsync();

            return item;

        }

        public async Task<T> SelectAsync(Guid id)
        {
            var result = await _context.Set<T>().SingleOrDefaultAsync(u => u.Id.Equals(id));

            return result;
        }

        public async Task<IEnumerable<T>> SelectAsync()
        {
            var result = await _context.Set<T>().ToListAsync();

            return result; 
        }

        public async Task<T> UpdateAsync(T item)
        {
            var result = await _context.Set<T>().SingleOrDefaultAsync(u => u.Id.Equals(item.Id));

            if(result == null){
                return null;
            }

            item.CreateAt = DateTime.UtcNow;
            item.UpdateAt = result.CreateAt;

            _context.Entry(result).CurrentValues.SetValues(item);

            await _context.SaveChangesAsync();

            return item;
            
        }
    }
}