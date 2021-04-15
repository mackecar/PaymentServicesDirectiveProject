using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Applications.WebClient.Models.ViewModels;
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


        public IActionResult TransactionConfirmation(string message)
        {
            ViewBag.Message = message;
            return View();
        }
    }
}
