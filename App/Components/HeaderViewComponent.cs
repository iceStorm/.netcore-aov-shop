using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Components
{
    public class HeaderViewComponent : ViewComponent
    {


        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View();
        }
    }
}
