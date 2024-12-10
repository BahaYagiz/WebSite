﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebSite.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
       
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
