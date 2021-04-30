using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ApplicationService.Extensions;
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

            User user = await GetUserAndValidateAsync(userPersonalNumber, userPass,unitOfWork);
            if(user.IsBlocked)throw new TransactionServiceException("CreateBankDepositTransaction - akcija nije moguca! Korisnik je blokiran!");

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
                throw new TransactionException($"CreateBankDepositTransaction - Transver novca sa bankovnog racuna na nalog nije moguce! {ex.Message}");
            }

            await unitOfWork.SaveChangesAsync();

            return transaction.ToTransactionDto();
        }

        public async Task<TransactionDto> CreateBankWithdrawTransaction(string userPersonalNumber, string userPass, decimal amount)
        {
            using IUnitOfWork unitOfWork = _unitOfWorkFactory.CreateUnitOfWork();

            User user = await GetUserAndValidateAsync(userPersonalNumber, userPass,unitOfWork);

            IBankService bank = _bankServiceProvider.Get(user.BankName);
            await bank.WithdrawAsync(userPersonalNumber, user.BankPinNumber, amount);

            Transaction transaction = new Transaction(TransactionType.BankWithdraw, user.Id, null, null, amount, 0);
            user.Withdraw(transaction.Amount);

            try
            {
                await unitOfWork.TransactionRepository.Insert(transaction);
                await unitOfWork.UserRepository.Update(user);
            }
            catch (Exception ex)
            {
                throw new TransactionException($"Transver novca na racun u banci nije moguc! {ex.Message}");
            }

            await unitOfWork.SaveChangesAsync();

            return transaction.ToTransactionDto();
        }

        public async Task<Tuple<TransactionDto,TransactionDto>> CreateUserToUserTransaction(string userPersonalNumber, 
            string userPass,
            decimal amount, 
            string destinationUserPersonalNumber, 
            int maxMonthLimit)
        {
            using IUnitOfWork unitOfWork = _unitOfWorkFactory.CreateUnitOfWork();

            User sourceUser = await GetUserAndValidateAsync(userPersonalNumber, userPass, unitOfWork);
            if (sourceUser.IsBlocked) throw new TransactionServiceException("CreateUserToUserTransaction - akcija nije moguca! Korisnik je blokiran!");

            bool isTransactionPossibleForSourceUser =  IsTransactionPossible(sourceUser, amount, maxMonthLimit, TransactionType.PayOut);
            if(!isTransactionPossibleForSourceUser) throw new TransactionServiceException("Soruce korisnik je premasio mesecni limi!");

            if (sourceUser.Amount < amount) throw new TransactionServiceException("CreateBankWithdrawTransaction - Korisnik nema dovoljno sredstava!");

            User destinationUser = await GetUserAndValidateAsync(destinationUserPersonalNumber, unitOfWork);
            if (destinationUser.IsBlocked) throw new TransactionServiceException("CreateUserToUserTransaction - akcija nije moguca! Korisnik je blokiran!");

            bool isTransactionPossibleForDestinationUser = IsTransactionPossible(sourceUser, amount, maxMonthLimit, TransactionType.PayIn);
            if (!isTransactionPossibleForDestinationUser) throw new TransactionServiceException("Destination korisnik je premasio mesecni limi!");

            decimal fee = FeeCalculator(sourceUser, amount);

            await unitOfWork.BeginTransactionAsync();

            try
            {
                Transaction payOutTransaction = new Transaction(TransactionType.PayOut, sourceUser.Id, sourceUser.Id, destinationUser.Id, amount, fee);
                sourceUser.Withdraw(amount + fee);

                await unitOfWork.TransactionRepository.Insert(payOutTransaction);
                await unitOfWork.UserRepository.Update(sourceUser);

                Transaction payInTransaction = new Transaction(TransactionType.PayIn, destinationUser.Id, sourceUser.Id, destinationUser.Id, amount, 0);
                destinationUser.Deposit(amount);

                await unitOfWork.TransactionRepository.Insert(payInTransaction);
                await unitOfWork.UserRepository.Update(destinationUser);

                await unitOfWork.SaveChangesAsync();

                await unitOfWork.CommitTransactionAsync();

                return new Tuple<TransactionDto, TransactionDto>(payOutTransaction.ToTransactionDto(),payInTransaction.ToTransactionDto());
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                throw new TransactionException($"CreateBankWithdrawTransaction - Transver novca izmedju korisnika nije moguce! {ex.Message}");
            }
        }


        private decimal FeeCalculator(User user, decimal amount)
        {
            if ((DateTime.Now - user.CreationDate).Days > 7)
            {
                if (user.Transactions.Count(t => t.TransactionDate.Month == DateTime.Now.Month) == 0)
                {
                    return 0;
                }
                
                if (amount <= 10000)
                {
                    return 100;
                }

                if (amount > 10000)
                {
                    return amount / 100;
                }
            }
            

            return 0;
        }

        private async Task<User> GetUserAndValidateAsync(string userPersonalNumber, IUnitOfWork unitOfWork)
        {
            User user = await unitOfWork.UserRepository.GetUserByPersonalNumberAsync(userPersonalNumber);
            if (user == null) throw new TransactionServiceException("Korisnik nije pronadjen!");
            return user;
        }

        private async Task<User> GetUserAndValidateAsync(string userPersonalNumber, string userPass,IUnitOfWork unitOfWork)
        {
            User user = await unitOfWork.UserRepository.GetUserByPersonalNumberAsync(userPersonalNumber);
            if (user == null) throw new TransactionServiceException("Korisnik nije pronadjen!");
            if (user.UserPass != userPass) throw new TransactionServiceException("Korisnik nije autorizovan!");
            return user;
        }

        private bool IsTransactionPossible(User user, decimal transactionAmount,int maxMonthLimit, TransactionType transactionType)
        {
            return user.Transactions.Where(t=>t.TransactionType == transactionType).Sum(t => t.Amount) + transactionAmount < maxMonthLimit;
        }

        public async Task DeleteTransactionAsync(int transactionId)
        {
            using IUnitOfWork unitOfWork = _unitOfWorkFactory.CreateUnitOfWork();
            Transaction transaction = await unitOfWork.TransactionRepository.GetTransaction(transactionId);
            if(transaction == null) throw new TransactionServiceException("Transakcija ne postoji!");
            await unitOfWork.TransactionRepository.Delete(transaction);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
