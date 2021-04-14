using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApplicationService;
using Banks.ApplicationServiceInterfaces;
using Banks.ApplicationServices;
using Domain.ApplicationService;
using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.DataAccess.EFDataAccess;
using Infrastructure.DataAccess.EFDataAccess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Applications.TestAndUtility.ApplicationServicesTest
{
    [TestClass]
    public class UserServiceTest
    {
        private PSDDbContext _context;
        private UserService _userService;
        private IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private IBankServiceProvider _bankServiceProvider;

        [TestInitialize]
        public void Setup()
        {
            DbContextFactory dbContextFactory = new DbContextFactory();
            _context = dbContextFactory.CreateDbContext(new string[] { });
            _unitOfWorkFactory = new EfUnitOfWorkFactory();
            _unitOfWork = _unitOfWorkFactory.CreateUnitOfWork();
            _userRepository = new EFUserRepository(_context);
            _bankServiceProvider = new BankServiceProvider();
            _bankServiceProvider.Add("dummy", new DummyBankService());
            _userService = new UserService(_userRepository,_unitOfWork,_unitOfWorkFactory, _bankServiceProvider);

        }

        [TestCleanup()]
        public async Task Cleanup()
        {
            await _context.DisposeAsync();
            _unitOfWork = null;
        }

        [TestMethod]
        public async Task TestCreateCorrectUser()
        {
            try
            {
                UserDto user = await _userService.CreateUser("Dragan", "Macura", "0312984710064", "dummy", "160-9999-00",
                        "1234");

                Assert.AreNotEqual(null, user, "User must not be null");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected error: " + ex.Message);
            }
        }
    }
}
