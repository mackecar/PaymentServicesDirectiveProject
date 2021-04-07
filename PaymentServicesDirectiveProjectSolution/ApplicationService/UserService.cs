using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationService.Extensions;
using Domain.DTOs;
using Domain.Entities;
using Domain.Repositories;

namespace Domain.ApplicationService
{
    public class UserService
    {
        private readonly IUserRepository UserRepository;
        private readonly IUnitOfWork UnitOfWork;
        private readonly IUnitOfWorkFactory UnitOfWorkFactory;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IUnitOfWorkFactory unitOfWorkFactory)
        {
            UserRepository = userRepository;
            UnitOfWork = unitOfWork;
            UnitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<UserDto> CreateUser(string firstName, 
            string lastName, 
            string personalNumber, 
            string bankAccountNumber,
            string bankPinNumber)
        {
            using IUnitOfWork unitOfWork = UnitOfWorkFactory.CreateUnitOfWork();

            bool doesExist = await DoesExist(personalNumber);
            if(doesExist)throw new Exception("User already exist!");
            
            User user = new User(firstName,lastName,personalNumber,bankAccountNumber,bankPinNumber);
            string userPass = RandomString(6);
            
            user.SetUserPass(userPass);

            await unitOfWork.UserRepository.InsertAsync(user);

            await unitOfWork.SaveChangesAsync();

            return user.ToUserDto();
        }

        public async Task<UserDto> GetUserByPersonalNumber(string personalNumber)
        {
            using IUnitOfWork unitOfWork = UnitOfWorkFactory.CreateUnitOfWork();
            User user = await unitOfWork.UserRepository.GetUserByPersonalNumberAsync(personalNumber);
            if(user == null) throw new NullReferenceException("User does not exist!");

            return user.ToUserDto();
        }

        private async Task<bool> DoesExist(string personalNumber)
        {
            using IUnitOfWork unitOfWork = UnitOfWorkFactory.CreateUnitOfWork();
            User user = await unitOfWork.UserRepository.GetUserByPersonalNumberAsync(personalNumber);
            return user != null;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
