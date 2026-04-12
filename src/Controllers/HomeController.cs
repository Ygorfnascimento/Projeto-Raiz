using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Raiz.Models;

namespace Raiz.Controllers;

[Authorize] 
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (User.IsInRole("Gerente"))
        {
            ViewData["MensagemBoasVindas"] = "Bem-vindo ao Painel de Gestão, Gerente.";
        }
        else
        {
            ViewData["MensagemBoasVindas"] = "Bem-vindo ao Sistema, Operador.";
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}