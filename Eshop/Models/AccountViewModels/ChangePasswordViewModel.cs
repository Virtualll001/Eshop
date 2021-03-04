﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Models.AccountViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Aktuální heslo je povinné")]
        [DataType(DataType.Password)]
        [Display(Name = "Aktuální Heslo")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Nové heslo je povinné")]
        [StringLength(100, ErrorMessage = "Heslo musí být alespoň 8 znaků dlouhé", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Nové heslo")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Zadaná hesla se neshodují")]
        [DataType(DataType.Password)]
        [Display(Name = "Potvrzení nového hesla")]
        public string ConfirmPassword { get; set; }
    }
}
