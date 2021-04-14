using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationService.Extensions;
using Banks.ApplicationServiceInterfaces;
using Banks.ApplicationServiceInterfaces.DTOs;
using Domain.DTOs;
using Domain.Entities;
using Domain.Repositories;

namespace Domain.ApplicationService
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IBankServiceProvider _bankServiceProvider;

        public UserService(IUserRepository userRepository, 
            IUnitOfWork unitOfWork, 
            IUnitOfWorkFactory unitOfWorkFactory, 
            IBankServiceProvider bankServiceProvider)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _unitOfWorkFactory = unitOfWorkFactory;
            _bankServiceProvider = bankServiceProvider;
        }

        public async Task<UserDto> CreateUser(string firstName, 
            string lastName, 
            string personalNumber, 
            string bankName, 
            string bankAccountNumber,
            string bankPinNumber)
        {
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
            {
                bool doesExist = await DoesExist(personalNumber, unitOfWork);
                if (doesExist) throw new Exception("User already exist!");

                IBankService bank = _bankServiceProvider.Get(bankName);
                CheckStatusDto status = await bank.CheckStatusAsync(personalNumber, bankPinNumber);
                if (!status.Status) throw new Exception(status.Message);

                User user = new User(firstName, lastName, personalNumber, bankName, bankAccountNumber, bankPinNumber);
                string userPass = RandomString(6);

                user.SetUserPass(userPass);

                await unitOfWork.UserRepository.InsertAsync(user);

                await unitOfWork.SaveChangesAsync();

                return user.ToUserDto();
            }
        }

        public async Task<UserDto> GetUserByPersonalNumber(string personalNumber)
        {
            using IUnitOfWork unitOfWork = _unitOfWorkFactory.CreateUnitOfWork();
            User user = await unitOfWork.UserRepository.GetUserByPersonalNumberAsync(personalNumber);
            if(user == null) throw new NullReferenceException("User does not exist!");

            return user.ToUserDto();
        }

        private async Task<bool> DoesExist(string personalNumber, IUnitOfWork unitOfWork)
        {
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
