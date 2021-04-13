using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task InsertAsync(User user);
        Task<User> GetUserByPersonalNumberAsync(string personalNumber);
    }
}
