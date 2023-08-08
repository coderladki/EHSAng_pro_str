//using System.Collections.Generic;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using CRM.Server.Models;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin;
//using Microsoft.Owin.Security;

//namespace CRM.Server.Security.Application
//{
//    public class ApplicationSignInManager : SignInManager<ApplicationUser, long>
//    {
//        private readonly ApplicationUserManager _userManager;

//        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
//        {
//            _userManager = userManager;
//        }

//        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
//        {
//            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
//        }

//        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
//        {
//            return _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
//        }

//        public Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
//        {
//            return AuthenticationManager.GetExternalLoginInfoAsync();
//        }

//        public Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string xsrfKey, string expectedValue)
//        {
//            return AuthenticationManager.GetExternalLoginInfoAsync(xsrfKey, expectedValue);
//        }

//        public IEnumerable<AuthenticationDescription> GetExternalAuthenticationTypes()
//        {
//            return AuthenticationManager.GetExternalAuthenticationTypes();
//        }

//        public void SignOut(params string[] authenticationTypes)
//        {
//            AuthenticationManager.SignOut(authenticationTypes);
//        }
//    }
//}