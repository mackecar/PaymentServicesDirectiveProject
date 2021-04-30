using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Applications.WebClient.Helpers;
using Applications.WebClient.Models.ViewModels;
using Applications.WebClient.Models.ViewModels.TransactionVMS;
using Core.ApplicationService;
using Core.Domain.DTOs;

namespace Applications.WebClient.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public IActionResult CreateBankDepositTransaction()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBankDepositTransaction(CreateTransactionVM model)
        {
            await _transactionService.CreateBankDepositTransaction(model.PersonalNumber, model.UserPass, model.Amount);

            return RedirectToAction("TransactionConfirmation",new{message = "Transacija je uspesno izvrsena!"});
        }


        public IActionResult CreateBankWithdrawTransaction()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBankWithdrawTransaction(CreateTransactionVM model)
        {
            await _transactionService.CreateBankWithdrawTransaction(model.PersonalNumber, model.UserPass, model.Amount);

            return RedirectToAction("TransactionConfirmation", new { message = "Transacija je uspesno izvrsena!" });
        }

        public IActionResult CreateUserTransaction()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserTransaction(CreateTransactionVM model)
        {
            await _transactionService.CreateUserToUserTransaction(model.PersonalNumber, model.UserPass, model.Amount,model.DestinationPersonalNumber, BusinessRulesHelper.MaxMonthLimit);

            return RedirectToAction("TransactionConfirmation", new { message = "Transacija je uspesno izvrsena!" });
        }


        public IActionResult TransactionConfirmation(string message)
        {
            ViewBag.Message = message;
            return View();
        }
    }
}
