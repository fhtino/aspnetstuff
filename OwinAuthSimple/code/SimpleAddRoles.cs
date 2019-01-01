using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;


namespace OwinAuthSimple
{
    public class SimpleAddRoles
    {

        private static FakeRoleDB _fakeRoleDB = null;

        public static Task AddRoleAndSID(IOwinContext context, Func<Task> next)
        {
            var ci = context.Authentication.User.Identity as ClaimsIdentity;

            if (ci.IsAuthenticated)
            {
                if (_fakeRoleDB == null)
                {
                    _fakeRoleDB = new FakeRoleDB(ConfigurationManager.AppSettings["FakeUsersDBFile"]);
                }

                //var email = ci.FindFirst(ClaimTypes.Expired).Value;

                string uniqueID = null;
                string[] roles = null;
                _fakeRoleDB.GetUserIDandRoles(
                        ci.FindFirst(ClaimTypes.NameIdentifier).Value,
                        ci.FindFirst(MyConstants.MyClaimdProviderID).Value,
                        out uniqueID,
                        out roles);

                if (uniqueID != null)
                {
                    ci.AddClaim(new Claim("myUID", uniqueID));
                    ci.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));
                    ci.AddClaim(new Claim("utcnow-debug", DateTime.UtcNow.ToString("o")));
                }
            }

            return next();
        }
    }
}