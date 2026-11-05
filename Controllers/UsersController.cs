using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductsWithRouting.Models;
using ProductsWithRouting.Services;

namespace ProductsWithRouting.Controllers
{
    public class UsersController : Controller
    {
        private const string AdminAccessKey = "df2323eoT";

        private readonly List<User> myUsers;

        public UsersController(Data data)
        {
            myUsers = data.Users;
        }

        [Route("users/index")]
        public IActionResult Index([FromForm(Name = "id")] string accessKey)
        {
            if (accessKey != AdminAccessKey)
            {
                return Unauthorized();
            }

            return View(myUsers);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
