using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApplicationService;
using Banks.ApplicationServiceInterfaces;
using Banks.ApplicationServices;
using Core.ApplicationService;
using Core.ApplicationService.Exceptions;
using Core.Domain.Exceptions;
using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.DataAccess.EFDataAccess;
using Infrastructure.DataAccess.EFDataAccess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Applications.TestAndUtility.ApplicationServicesTest
{
    [TestClass]
    public class UserServiceTests
    {
        private PSDDbContext _context;
        private UserService _userService;
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
            _userService = new UserService(_unitOfWorkFactory, _bankServiceProvider);

        }

        [TestCleanup()]
        public async Task Cleanup()
        {
            await _context.DisposeAsync();
            _unitOfWorkFactory = null;
        }

        [TestMethod]
        public async Task TestCreateCorrectUser()
        {
            try
            {
                UserDto user = await _userService.CreateUser("Test", "Test", "0312985710064", "dummy", "160-9999-00", "1234");

                Assert.AreNotEqual(null, user, "User must not be null");
                Assert.AreEqual("Test", user.FirstName, "Ime mora da bude 'Test'");
                Assert.AreEqual("Test", user.LastName, "Prezime mora da bude 'Test'");
                Assert.AreEqual("0312985710064", user.PersonalNumber, "JMBG mora da bude '0312985710064'");
                Assert.AreEqual("dummy", user.BankName, "Ime banke mora da bude 'dummy'");
                Assert.AreEqual("160-9999-00", user.BankAccountNumber, "Broj bankovnog racuna mora da bude '160-9999-00'");
                Assert.AreEqual("1234", user.BankPinNumber, "PIN mora da bude '1234'");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task TestCreateExistedUser()
        {
            try
            {
                UserDto user = await _userService.CreateUser("Test", "Test", "0312985710064", "dummy", "160-9999-00", "1234");

                Assert.AreEqual(user, user, "Korisnik postoji!");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task TestCreateNotAdultUser()
        {
            try
            {
                await Assert.ThrowsExceptionAsync<UserException>(async () => await _userService.CreateUser("Test", "Test", "0312020710064", "dummy", "160-9999-00", "1234"),"Korisnik nije punoletan!");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }
    }
}
