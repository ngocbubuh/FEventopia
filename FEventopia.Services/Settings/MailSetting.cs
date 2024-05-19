﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Settings
{
    public class MailSetting
    {
        public required string Mail { get; set; }
        public required string DisplayName { get; set; }
        public required string Password { get; set; }
        public required string Host { get; set; }
        public int Port { get; set; }
    }
}
