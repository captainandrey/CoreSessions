﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Session1.Api.Model
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsReal { get; set; }

        public string GivenName { get; set; }
    }
}