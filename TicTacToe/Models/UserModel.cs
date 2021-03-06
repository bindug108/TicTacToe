﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        [Required()]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required()]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(), DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public DateTime? EmailConfirmationDate { get; set; }

        public int Score { get; set; }

    }
}
