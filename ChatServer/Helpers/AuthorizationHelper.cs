﻿using ChatServer.Controllers;
using ChatServer.Models;
using Microsoft.AspNetCore.Identity;
using SharedItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Helpers
{
    public class AuthorizationHelper
    {
        public async Task<string> TryRegistration(RegistrationUserData model, UserManager<User> _userManager, AccountController account)
        {
            string error = "";

            if (await _userManager.FindByNameAsync(model.Username) != null)
            {
                error = "Username is already exist";
            }
            else if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                error = "Email is already exist";
            }
            else
            {
                IdentityResult completedRegistration = await account.Register(model);

                if (!completedRegistration.Succeeded)
                {
                    error = completedRegistration.Errors.ToList()[0].Description.ToString();
                }
            }

            return error;
        }
    }
}
