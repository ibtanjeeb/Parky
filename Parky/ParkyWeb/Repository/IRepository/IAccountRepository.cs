using ParkyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Repository.IRepository
{
     public interface IAccountRepository:IRepository<User>
    {
        Task<User> LogInAsync(string url, User obj);
        Task<bool> RegisterAsync(string url, User objtoCreate);
    }
}
