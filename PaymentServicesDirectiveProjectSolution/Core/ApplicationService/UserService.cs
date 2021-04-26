using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationService.Extensions;
using Banks.ApplicationServiceInterfaces;
using Banks.ApplicationServiceInterfaces.DTOs;
using Domain.DTOs;
using Domain.Entities;
using Domain.Repositories;

namespace Core.ApplicationService
{
    public class UserService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IBankServiceProvider _bankServiceProvider;

        public UserService(
            IUnitOfWorkFactory unitOfWorkFactory, 
            IBankServiceProvider bankServiceProvider)
        {
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
            using IUnitOfWork unitOfWork = _unitOfWorkFactory.CreateUnitOfWork();
            User user = new User(firstName, lastName, personalNumber, bankName, bankAccountNumber, bankPinNumber);

            bool doesExist = await DoesExist(personalNumber, unitOfWork);
            if (doesExist) throw new Exception("Korisnik postoji!");

            IBankService bank = _bankServiceProvider.Get(bankName);
            CheckStatusDto status = await bank.CheckStatusAsync(personalNumber, bankPinNumber);
            if (!status.Status) throw new Exception(status.Message);

            string userPass = RandomString(6);

            user.SetUserPass(userPass);

            await unitOfWork.UserRepository.Insert(user);

            await unitOfWork.SaveChangesAsync();

            return user.ToUserDto();
        }

        public async Task<UserDto> GetUserByPersonalNumber(string personalNumber,string userPass)
        {
            using IUnitOfWork unitOfWork = _unitOfWorkFactory.CreateUnitOfWork();
            User user = await unitOfWork.UserRepository.GetUserByPersonalNumberAsync(personalNumber);
            if(user == null) throw new NullReferenceException("Korisnik ne postoji!");
            if(user.UserPass != userPass) throw new NullReferenceException("Korisnik nije autorizovan!");


            return user.ToUserDto();
        }

        public async Task<UserDto> ChangeUserPass(string personalNumber, string oldUserPass, string newUserPass)
        {
            using IUnitOfWork unitOfWork = _unitOfWorkFactory.CreateUnitOfWork();
            User user = await unitOfWork.UserRepository.GetUserByPersonalNumberAsync(personalNumber);
            if (user == null) throw new NullReferenceException("Korisnik ne postoji!");
            if (user.UserPass != oldUserPass) throw new NullReferenceException("Korisnik nije autorizovan!");

            user.SetUserPass(newUserPass);

            await unitOfWork.UserRepository.Update(user);

            await unitOfWork.SaveChangesAsync();

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
