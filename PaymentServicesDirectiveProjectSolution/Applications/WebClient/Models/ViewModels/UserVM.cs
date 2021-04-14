﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs;

namespace WebClient.Models.ViewModels
{
    public class UserVM
    {
        public int Id { get; set; }

        [Display(Name = "Ime")]
        public string FirstName { get; set; }

        [Display(Name = "Prezime")]
        public string LastName { get; set; }

        [Display(Name = "JMBG")]
        public string PersonalNumber { get; set; }

        [Display(Name = "Banka")]
        public string BankName { get; set; }

        [Display(Name = "Broj  racuna u banci")]
        public string BankAccountNumber { get; set; }

        [Display(Name = "PIN")]
        public string BankPinNumber { get; set; }

        [Display(Name = "Šifra")]
        public string UserPass { get; set; }

        [Display(Name = "Datum kreiranja")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Iznos")]
        public decimal Amount { get; set; }

        public UserVM() { }

        public UserVM(UserDto user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            PersonalNumber = user.PersonalNumber;
            BankName = user.BankName;
            BankAccountNumber = user.BankAccountNumber;
            BankPinNumber = user.BankPinNumber;
            UserPass = user.UserPass;
            CreationDate = user.CreationDate;
            Amount = user.Amount;
        }
    }

    public static class UserVMExtension
    {
        public static UserVM ToUserVm(this UserDto user)
        {
            return new UserVM(user);
        }

        public static List<UserVM> ToUserVms(this List<UserDto> users)
        {
            return users.Select(u => new UserVM(u)).ToList();
        }
    }
}