﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Enums
{
    public enum ConnectionStatus
    {
        None,
        Connecting,
        Connected,
        Disconnected,
        Reconnecting
    }
}
