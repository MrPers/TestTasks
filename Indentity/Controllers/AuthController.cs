using Duende.IdentityServer.Services;
using Indentity.Data;
using Indentity.Models;
using Indentity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Indentity.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly DataContext _context;

        public AuthController(
            IIdentityServerInteractionService interactionService,
            DataContext context,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _context = context;
            _interactionService = interactionService;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> LoginAsync(string returnUrl)
        {
            return View(new LoginVM
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LoginAsync(LoginVM model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        var externalProviders = await _signInManager.GetExternalAuthenticationSchemesAsync();

                        return Redirect(model.ReturnUrl);
                    }
                }

                throw new Exception("User not found");
            }

            throw new Exception("invalid return URL");

        }

        [HttpGet("[action]")]
        public async Task<IActionResult> LogoutAsync(string logoutId)
        {           
            await _signInManager.SignOutAsync();
            var result = await _interactionService.GetLogoutContextAsync(logoutId);
            if (string.IsNullOrEmpty(result.PostLogoutRedirectUri))
            {
                return RedirectToAction("Index", "Site");
            }

            return Redirect(result.PostLogoutRedirectUri);
        }

        [HttpGet("[action]")]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = new User(vm.UserName);
            IdentityResult result = await _userManager.CreateAsync(user, vm.Password);
            await _userManager.CreateAsync(user, vm.Password);

            _userManager.AddToRoleAsync(user, "User").GetAwaiter().GetResult(); //add role User

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

                return Redirect(vm.ReturnUrl);
            }

            return View();
        }








        //[Route("[action]")]
        //public IActionResult ExternalLoginStart(string provider, string returnUrl)
        //{
        //    var redirectUri = Url.Action(nameof(ExteranlLoginCallbackAsync), "Auth", new { returnUrl });
        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUri);
        //    return Challenge(properties, provider);
        //}

        //[Route("[action]")]
        //public async Task<IActionResult> ExteranlLoginCallbackAsync(string returnUrl)
        //{
        //    var info = await _signInManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    var result = await _signInManager
        //        .ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

        //    if (result.Succeeded)
        //    {
        //        return Redirect(returnUrl);
        //    }

        //    var username = info.Principal.FindFirst(ClaimTypes.Name.Replace(" ", "_")).Value;
        //    return View("ExternalRegister", new ExternalRegisterViewModel
        //    {
        //        UserName = username,
        //        ReturnUrl = returnUrl
        //    });
        //}

        //[Route("[action]")]
        //public async Task<IActionResult> ExternalRegisterAsync(ExternalRegisterViewModel vm)
        //{
        //    var info = await _signInManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    var user = new User(vm.UserName);
        //    var result = await _userManager.CreateAsync(user);
        //    _userManager.AddToRoleAsync(user, "User").GetAwaiter().GetResult(); //add role User

        //    if (!result.Succeeded)
        //    {
        //        return View(vm);
        //    }

        //    result = await _userManager.AddLoginAsync(user, info);

        //    if (!result.Succeeded)
        //    {
        //        return View(vm);
        //    }

        //    await _signInManager.SignInAsync(user, false);

        //    return Redirect(vm.ReturnUrl);
        //}

    }
}
