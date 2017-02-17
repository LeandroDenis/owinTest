using System;
using System.Security.Claims;
using System.DirectoryServices.AccountManagement;
using Microsoft.Owin.Security;

namespace ProyectoOwin.Models
{
    public class AdAuthenticationService
    {
        public class AuthenticationResult
        {
            public AuthenticationResult(string errorMessage = null)
            {
                ErrorMessage = errorMessage;
            }

            public String ErrorMessage { get; private set; }
            public Boolean IsSuccess => String.IsNullOrEmpty(ErrorMessage);
        }

        private readonly IAuthenticationManager authenticationManager;

        public AdAuthenticationService(IAuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }

        public AuthenticationResult SignIn(String username, String password)
        {
            PrincipalContext principalContext;

            try
            {
                principalContext = new PrincipalContext(ContextType.Domain, "itrio-server.itrio.net");
            }
            catch (PrincipalServerDownException e)
            {
                return new AuthenticationResult("Servidor no disponible");
            }
            bool isAuthenticated = false;
            UserPrincipal userPrincipal = null;

            try
            {
                userPrincipal = UserPrincipal.FindByIdentity(principalContext, username);
                if (userPrincipal != null)
                {
                    isAuthenticated = principalContext.ValidateCredentials(username, password, ContextOptions.Negotiate);
                }
            }
            catch (Exception exception)
            {
                return new AuthenticationResult("Usuario o Contraseña no son correctas");
            }

            if (!isAuthenticated || userPrincipal == null)
            {
                return new AuthenticationResult("Usuario o Contraseña no son correctas");
            }

            var identity = CreateIdentity(userPrincipal);

            authenticationManager.SignOut(MyAuthentication.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);

            return new AuthenticationResult();
        }

        private ClaimsIdentity CreateIdentity(UserPrincipal userPrincipal)
        {
            var identity = new ClaimsIdentity(MyAuthentication.ApplicationCookie, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            identity.AddClaim(new Claim(ClaimTypes.Name, userPrincipal.Name));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userPrincipal.SamAccountName));
            if (!String.IsNullOrEmpty(userPrincipal.EmailAddress))
            {
                identity.AddClaim(new Claim(ClaimTypes.Email, userPrincipal.EmailAddress));
            }
            var groups = userPrincipal.GetAuthorizationGroups();
            foreach (var @role in groups)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, @role.Name));
            }
            return identity;
        }
    }
}