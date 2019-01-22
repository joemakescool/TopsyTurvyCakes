﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TopsyTurvyCakes.Pages.Account
{
    public class LoginModel : PageModel
    {

        // Properities
        [BindProperty]
        [Required]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [HttpPost]
        public IActionResult OnPost()
        {
            var isValidUser =
                EmailAddress == "Admin@TTCD.com" && Password == "OU812";

            if (!isValidUser)
            {
                ModelState.AddModelError("", "Invalid User name or Password");
            }

            // Are valid user
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //assume they are valid
            var scheme = CookieAuthenticationDefaults.AuthenticationScheme;
            var user = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[] {new Claim(ClaimTypes.Name, EmailAddress)}, scheme
                    )
                );

            return SignIn(user, scheme);
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }
    }
}