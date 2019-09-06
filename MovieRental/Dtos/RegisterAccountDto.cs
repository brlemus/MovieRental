﻿using MovieRental.Core.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRental.Dtos
{
    public class RegisterAccountDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

    }
}
