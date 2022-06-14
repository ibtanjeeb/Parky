using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Repository.IRepository
{
   public interface IRepository<T> where T:class
    {
        Task<T> GetAsync(string url, int Id,string Token);
        Task<IEnumerable<T>> GetAllAsync(string url, string Token);
        Task<bool> CreateAsync(string url, T objToCreate, string Token);
        Task<bool> UpdateAsync(string url, T objToUpdate, string Token);
        Task<bool> DeleteAsync(string url, int Id, string Token);

    }
}
