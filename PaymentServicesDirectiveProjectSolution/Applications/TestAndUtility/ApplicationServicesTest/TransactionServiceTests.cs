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
using Domain.Repositories;
using Infrastructure.DataAccess.EFDataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Applications.TestAndUtility.ApplicationServicesTest
{
    [TestClass]
    public class TransactionServiceTests
    {
        private PSDDbContext _context;
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
                TransactionDto transaction = await _transactionService.CreateBankDepositTransaction("0312984710064", "TAPGO2", 999);

                Assert.AreNotEqual(null, transaction, "Transaction must not be null");
                Assert.AreEqual(999, transaction.Amount, "Iznos mora biti 999!");
                Assert.AreEqual(TransactionType.BankDeposit, transaction.TransactionType, "Tip mora biti BankDeposit!");
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
                TransactionDto transaction = await _transactionService.CreateBankWithdrawTransaction("0312984710064", "TAPGO2", 999);

                Assert.AreNotEqual(null, transaction, "Transaction must not be null");
                Assert.AreEqual(999, transaction.Amount, "Iznos mora biti 999!");
                Assert.AreEqual(TransactionType.BankWithdraw, transaction.TransactionType, "Tip mora biti BankWithdraw!");
            }
            catch (Exception ex)
            {

                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }
    }
}
