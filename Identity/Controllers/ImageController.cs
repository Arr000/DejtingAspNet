﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    public class ImageController : Controller
    {

        public IActionResult Add()
        {
            return View();
        }
    }
}
