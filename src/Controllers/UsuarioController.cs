using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raiz.ViewModels;

namespace Raiz.Controllers
{
    [Authorize(Roles = "Gerente")] 
    public class UsuarioController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuarioController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _userManager.Users.ToListAsync();
            var listaUsuariosViewModel = new List<UsuarioViewModel>();

            foreach (var user in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(user);

                listaUsuariosViewModel.Add(new UsuarioViewModel
                {
                    Id = user.Id ?? "",
                    Email = user.Email ?? "Sem Email",
                    Roles = roles
                });
            }

            return View(listaUsuariosViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string email, string password, string role)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["Erro"] = "Email e Senha são obrigatórios.";
                return View();
            }

            var userExistente = await _userManager.FindByEmailAsync(email);
            if (userExistente != null)
            {
                TempData["Erro"] = "Este e-mail já está cadastrado.";
                return View();
            }

            var novoUsuario = new IdentityUser 
            { 
                UserName = email, 
                Email = email, 
                EmailConfirmed = true 
            };

            var result = await _userManager.CreateAsync(novoUsuario, password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                await _userManager.AddToRoleAsync(novoUsuario, role);
                TempData["Mensagem"] = $"Funcionário {email} criado com sucesso como {role}!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Erro"] = string.Join(" ", result.Errors.Select(e => e.Description));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AtribuirGerente(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (!await _roleManager.RoleExistsAsync("Gerente"))
                await _roleManager.CreateAsync(new IdentityRole("Gerente"));

            await _userManager.RemoveFromRoleAsync(user, "Operador");
            await _userManager.AddToRoleAsync(user, "Gerente");

            TempData["Mensagem"] = $"Usuário {user.Email} agora é Gerente!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AtribuirOperador(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (!await _roleManager.RoleExistsAsync("Operador"))
                await _roleManager.CreateAsync(new IdentityRole("Operador"));

            await _userManager.RemoveFromRoleAsync(user, "Gerente");
            await _userManager.AddToRoleAsync(user, "Operador");

            TempData["Mensagem"] = $"Usuário {user.Email} agora é Operador!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (user.Email == User.Identity?.Name)
            {
                TempData["Erro"] = "Você não pode excluir sua própria conta.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Mensagem"] = "Usuário removido com sucesso.";
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}