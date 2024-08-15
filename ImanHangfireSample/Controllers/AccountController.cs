using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SampleHangfire.Entities;
using SampleHangfire.Infrastrucures;
using SampleHangfire.Models.ViewModels;

namespace SampleHangfire.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly SmsService _smsService;
        private readonly EmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            EmailService emailService,
            SmsService smsService,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _smsService = smsService;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View(model);
            }

            //Fire and Forget Job
            //enject in hangfire
            BackgroundJob.Enqueue<EmailService>(p => p.SendWellcome(model.Email));

            //Fire and Forget Job
            //inject in controller
            BackgroundJob.Enqueue(() => _smsService.SendWellcome(model.PhoneNumber));

            //Delayed Job
            BackgroundJob.Schedule<EmailService>(p => p.SendDiscount(model.Email), TimeSpan.FromSeconds(10));

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.UserName);
            if(user is null)
            {
                ModelState.TryAddModelError("notFound", "notFound");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if(!result.Succeeded)
            {
                ModelState.TryAddModelError("notFound", "notFound");
                return View(model);
            }

            //enject in hangfire
            BackgroundJob.Enqueue<EmailService>(p => p.SendWellcome(model.UserName));
            //inject in controller
            BackgroundJob.Enqueue(() => _emailService.SendWellcome(model.UserName));

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task AddUserToRole()
        {
            await _roleManager.CreateAsync(new IdentityRole
            {
                Name = "hangfire"
            });
            var user = await _userManager.FindByNameAsync("iman@gmail.com");
            await _userManager.AddToRoleAsync(user, "hangfire");
        }
    }
}
