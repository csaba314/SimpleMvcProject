﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Containers
{
    public class Options : ILoadingOptions
    {
        public bool LoadMakesWithModel { get; set; } = false;
    }
}