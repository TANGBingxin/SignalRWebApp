using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SignalRWebApp.Models;
using SignalRWebApp.Services;

namespace SignalRWebApp.Controllers;

public class HomeController : Controller
{
    private readonly IUserService _userService;

    public HomeController(IUserService userService)
    {
        _userService = userService;
    }

    public bool CodeFirst()
    {
        return _userService.CodeFirst();
    }

    public IActionResult Index()
    {
        var userName = HttpContext.Session.GetString("userName");
        if (string.IsNullOrEmpty(userName))
        {
            return Redirect("/Home/Login");
        }
        ViewBag.UserName = userName;
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Submit(string name, string password)
    {
        UserInfo userInfo = _userService.GetUser(name, password);
        if (string.IsNullOrEmpty(userInfo.Id))
        {
            return Redirect("/Home/Error");
        }
        HttpContext.Session.SetString("userName", userInfo.Name);
        Response.Cookies.Append("userName", userInfo.Name);
        return Redirect("/");
    }

    public IActionResult LogOut()
    {
        HttpContext.Session.Remove("userName");
        Response.Cookies.Delete("userName");
        return Redirect("/Home/Login");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public JsonResult GetMessage(int pageIndex, int pageSize)
    {
        return Json(_userService.GetMessages(pageIndex, pageSize));
    }
}

