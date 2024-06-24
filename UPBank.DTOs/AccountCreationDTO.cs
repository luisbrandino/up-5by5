﻿using UPBank.Enums;
using UPBank.Models;

namespace UPBank.DTOs
{
    public class AccountCreationDTO
    {
        public double Overdraft { get; set; }
        public EProfile Profile { get; set; }
        public string AgencyNumber { get; set; }
        public List<string> Customers { get; set; } // only CPF
    }
}