﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Model
{
    public class Account : BaseObject
    {
        public User AccountHolder { get; set; }
    }
}
