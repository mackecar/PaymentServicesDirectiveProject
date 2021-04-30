using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApplicationService;
using Banks.ApplicationServiceInterfaces;
using Banks.ApplicationServices;
using Core.ApplicationService;
using Core.Domain.DTOs;
using Core.Domain.Enums;
using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.DataAccess.EFDataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Applications.TestAndUtility.ApplicationServicesTest
{
    [TestClass]
    public class TransactionServiceTests
    {
        private PSDDbContext _context;
        private UserService _userService;
        private TransactionService _transactionService;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private IBankServiceProvider _bankServiceProvider;


        [TestInitialize]
        public void Setup()
        {
            DbContextFactory dbContextFactory = new DbContextFactory();
            _context = dbContextFactory.CreateDbContext(new string[] { });
            _unitOfWorkFactory = new EfUnitOfWorkFactory();
            _bankServiceProvider = new BankServiceProvider();
            _bankServiceProvider.Add("dummy", new DummyBankService());
            _transactionService = new TransactionService(_unitOfWorkFactory, _bankServiceProvider);
            _userService = new UserService(_unitOfWorkFactory, _bankServiceProvider);
        }

        [TestCleanup()]
        public async Task Cleanup()
        {
            await _context.DisposeAsync();
            _unitOfWorkFactory = null;
        }

        [TestMethod]
        public async Task TestCreateBankDepositTransaction()
        {
            try
            {
                UserDto user = await _userService.CreateUser("Test", "Test", "0312985710066", "dummy", "160-9999-00", "1234");
                TransactionDto transaction = await _transactionService.CreateBankDepositTransaction(user.PersonalNumber, user.UserPass, 999);

                Assert.AreNotEqual(null, transaction, "Transaction must not be null");
                Assert.AreEqual(999, transaction.Amount, "Iznos mora biti 999!");
                Assert.AreEqual(TransactionType.BankDeposit, transaction.TransactionType, "Tip mora biti BankDeposit!");

                await _transactionService.DeleteTransactionAsync(transaction.Id);
                await _userService.DeleteUserAsync(user.PersonalNumber, user.UserPass);
            }
            catch (Exception ex)
            {

                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task TestCreateBankWithdrawTransaction()
        {
            try
            {
                UserDto user = await _userService.CreateUser("Test", "Test", "0312985710066", "dummy", "160-9999-00", "1234");
                TransactionDto transaction = await _transactionService.CreateBankDepositTransaction(user.PersonalNumber, user.UserPass, 999);

                Assert.AreNotEqual(null, transaction, "Transaction must not be null");
                Assert.AreEqual(999, transaction.Amount, "Iznos mora biti 999!");
                Assert.AreEqual(TransactionType.BankWithdraw, transaction.TransactionType, "Tip mora biti BankWithdraw!");

                await _transactionService.DeleteTransactionAsync(transaction.Id);
                await _userService.DeleteUserAsync(user.PersonalNumber, user.UserPass);
            }
            catch (Exception ex)
            {

                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task TestCreateUserToUserTransaction()
        {
            try
            {
                UserDto user1 = await _userService.CreateUser("Test", "1", "0312985710066", "dummy", "160-9999-00", "1234");
                UserDto user2 = await _userService.CreateUser("Test", "2", "0312985710067", "dummy", "160-9999-00", "1234");

                TransactionDto transaction = await _transactionService.CreateBankDepositTransaction(user1.PersonalNumber, user1.UserPass, 999);

                Tuple<TransactionDto, TransactionDto> transactions = await _transactionService.CreateUserToUserTransaction(user1.PersonalNumber, user1.UserPass, 999, user2.PersonalNumber,10000);

                Assert.AreNotEqual(null, transactions.Item1, "Transaction must not be null");
                Assert.AreEqual(999, transactions.Item1.Amount, "Iznos mora biti 999!");
                Assert.AreEqual(TransactionType.PayOut, transactions.Item1.TransactionType, "Tip mora biti PayOut!");
                Assert.AreEqual(0, transactions.Item1.Fee, "Provizija mora biti 0!");

                await _transactionService.DeleteTransactionAsync(transaction.Id);
                await _transactionService.DeleteTransactionAsync(transactions.Item1.Id);
                await _transactionService.DeleteTransactionAsync(transactions.Item2.Id);
                await _userService.DeleteUserAsync(user1.PersonalNumber, user1.UserPass);
                await _userService.DeleteUserAsync(user2.PersonalNumber, user2.UserPass);
            }
            catch (Exception ex)
            {

                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }
    }
}
