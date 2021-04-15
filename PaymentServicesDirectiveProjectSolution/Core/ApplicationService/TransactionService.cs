using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Banks.ApplicationServiceInterfaces;
using Core.ApplicationService.Exceptions;
using Core.ApplicationService.Extensions;
using Core.Domain.DTOs;
using Core.Domain.Enums;
using Core.Domain.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Transaction = Domain.Entities.Transaction;

namespace Core.ApplicationService
{
    public class TransactionService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IBankServiceProvider _bankServiceProvider;

        public TransactionService(IUnitOfWorkFactory unitOfWorkFactory,
            IBankServiceProvider bankServiceProvider)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _bankServiceProvider = bankServiceProvider;
        }

        public async Task<TransactionDto> CreateBankDepositTransaction(string userPersonalNumber,string userPass,decimal amount)
        {
            using IUnitOfWork unitOfWork = _unitOfWorkFactory.CreateUnitOfWork();
            User user = await GetUserAsync(userPersonalNumber, unitOfWork);
            if(user.UserPass != userPass)throw new TransactionServiceException("Korisnik nije autorizovan!");

            IBankService bank = _bankServiceProvider.Get(user.BankName);
            await bank.DepositAsync(userPersonalNumber, user.BankPinNumber, amount);

            Transaction transaction = new Transaction(TransactionType.BankDeposit,user.Id,null,null,amount,0);
            user.Deposit(transaction.Amount);

            try
            {
                await unitOfWork.TransactionRepository.Insert(transaction);
                await unitOfWork.UserRepository.Update(user);
            }
            catch (Exception ex)
            {
                throw new TransactionException($"Transver novca sa bankovnog racuna na nalog nije moguce! {ex.Message}");
            }

            await unitOfWork.SaveChangesAsync();

            return transaction.ToTransactionDto();
        }

        private async Task<User> GetUserAsync(string userPersonalNumber, IUnitOfWork unitOfWork)
        {
            User user = await unitOfWork.UserRepository.GetUserByPersonalNumberAsync(userPersonalNumber);
            if (user == null) throw new TransactionServiceException("Korisnik nije pronadjen!");
            return user;
        }
    }
}
