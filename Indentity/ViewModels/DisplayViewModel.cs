﻿using Indentity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indentity.ViewModels
{
    public class DisplayViewModel
    {
        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<User> Users { get; set; }
        //public IEnumerable<I> Users { get; set; }
    }
}
