using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using Bijector.Infrastructure.Types;
using Bijector.Infrastructure.Dispatchers;
using Bijector.Accounts.Messages.Queries;
using Bijector.Accounts.Repositories;
using Bijector.Accounts.Messages.Commands;

namespace Bijector.Accounts.Controllers
{
    [Route("Accounts")]
    public class AccountsController : Controller
    {        
        private readonly IAccountStore accountStore;

        private readonly IQueryDispatcher queryDispatcher;

        private readonly ICommandDispatcher commandDispatcher;

        public AccountsController(IAccountStore accountStore, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {            
            this.accountStore = accountStore;
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }        

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var account = await queryDispatcher.QueryAsync(registerRequest, null);
            if(account != null)
            {
                var properties = new AuthenticationProperties{
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(new TimeSpan(0,30,0))
                };
                
                await HttpContext.SignInAsync(account.Id.ToString(), account.Login, properties);
                return Ok();
            }               
            return BadRequest();
        }        
        
        [HttpGet("LoginView")]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            var vm = new LoginRequest 
            {
                ReturnUrl = returnUrl
            };            
            return View(vm);
        }

        [HttpPost("LoginView")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginView(LoginRequest loginQuery)
        {
            if(await accountStore.ValidateCredentialsAsync(loginQuery.Login, loginQuery.Password))
            {
                var account = await accountStore.GetAsync(loginQuery.Login);                                
                           
                var properties = new AuthenticationProperties{
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(new TimeSpan(0,30,0))
                };
                
                await HttpContext.SignInAsync(account.Id.ToString(), account.Login, properties);

                return Redirect(loginQuery.ReturnUrl);
            }            
            return View();
        }

        [HttpGet("Login")] 
        [AllowAnonymous]       
        public async Task<IActionResult> Login(LoginRequest loginQuery)
        {
            if(await accountStore.ValidateCredentialsAsync(loginQuery.Login, loginQuery.Password))
            {
                var account = await accountStore.GetAsync(loginQuery.Login);                            
                           
                var properties = new AuthenticationProperties{
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(new TimeSpan(0,30,0))
                };
                
                await HttpContext.SignInAsync(account.Id.ToString(), account.Login, properties);                
                
                return Ok();
            }
            return Unauthorized();
        }

        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        [HttpGet("LinkedServices")]
        public async Task<IActionResult> GetLinkedServices()
        {
            var id = Guid.Parse(HttpContext.User.Claims.First(cl => cl.Type == "sub").Value);
            var account =await accountStore.GetAsync(id);
            return new JsonResult(account.LinkedService);
        }

        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        [HttpPut("AddLinkedServices")]
        public async Task<IActionResult> AddLinkedServices(AddLinkedService command)
        {
            var id = Guid.Parse(HttpContext.User.Claims.First(cl => cl.Type == "sub").Value);
            var context = new BaseContext(Guid.Empty, id, null, null);
            await commandDispatcher.SendAsync(command, context);
            return Accepted();
        }
    }
}