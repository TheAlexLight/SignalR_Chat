﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedItems.Models
{
    public class UserProfileModel
    {
        public UserProfileModel()
        {
            //StatusMessage = "Active";
            UsernameColor = "#1E90FF";
            Role = "User";
        }

        public int Id { get; set; }
        public int UserModelId { get; set; }
        public string Username { get; set; }
        public string StatusMessage { get; set; }
        public string ImagePath { get; set; }
        public string UsernameColor { get; set; }
        public bool IsOnline { get; set; }
        public string Role { get; set; }
    }
}
