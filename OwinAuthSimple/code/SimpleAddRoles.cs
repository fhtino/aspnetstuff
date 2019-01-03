using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace OwinAuthSimple
{
    public class SimpleAddRoles
    {



        public static Task Exec(IOwinContext context, Func<Task> next)
        {
            var ci = context.Authentication.User.Identity as ClaimsIdentity;

            if (ci.IsAuthenticated && ci.FindFirst(MyClaims.MyUID) == null)
            {
                // The user is authenticated using an authentication-provider but "my" claims are missing.
                // Adding my claims: id, roles, etc. and call LogIn() again.


                // Load "database"
                string dataXmlFileName = ConfigurationManager.AppSettings["FakeUsersDBFile"];
                var dbItems = Load(dataXmlFileName);

                // Get and update user data from "database"
                string providerName = ci.FindFirst(MyClaims.MyProviderID).Value;
                string idFromProvider = ci.FindFirst(ClaimTypes.NameIdentifier).Value;
                var item = dbItems.SingleOrDefault(x => x.ProviderName == providerName && x.IDFromProvider == idFromProvider);

                if (item == null)
                {
                    item = new FakeUsersDBItem()
                    {
                        ProviderName = providerName,
                        IDFromProvider = idFromProvider,
                        Roles = "genericuser",
                        UniqueID = Guid.NewGuid().ToString(),
                        CreationDT = DateTime.UtcNow
                    };
                    dbItems.Add(item);
                }

                // update user information
                item.LastLoginDT = DateTime.UtcNow;
                item.Name = ci.FindFirst(ClaimTypes.Name)?.Value;
                item.Email = ci.FindFirst(ClaimTypes.Email)?.Value;
                //...
                Save(dbItems, dataXmlFileName);

                // update Claims and Login
                ci.AddClaim(new Claim(MyClaims.MyUID, item.UniqueID));
                ci.AddClaim(new Claim("utcnow-debug", DateTime.UtcNow.ToString("o")));
                ci.AddClaims(item.Roles.Split('#').Select(role => new Claim(ClaimTypes.Role, role)));
                context.Authentication.SignIn(ci);    // create a new authentication cookie with the new data added to the ClaimsIdentity
            }

            return next();
        }


        private static List<FakeUsersDBItem> Load(string xmlFileName)
        {
            var xs = new XmlSerializer(typeof(List<FakeUsersDBItem>));
            using (var fs = File.OpenRead(xmlFileName))
            {
                return (List<FakeUsersDBItem>)xs.Deserialize(fs);
            }
        }

        private static void Save(List<FakeUsersDBItem> items, string xmlFileName)
        {
            var xs = new XmlSerializer(typeof(List<FakeUsersDBItem>));
            using (var fs = File.Create(xmlFileName))
            {
                xs.Serialize(fs, items);
            }
        }



        // ----------------------------------------------------
        public class FakeUsersDBItem
        {
            [XmlAttribute]
            public string UniqueID { get; set; }

            [XmlAttribute]
            public string IDFromProvider { get; set; }

            [XmlAttribute]
            public string ProviderName { get; set; }

            [XmlAttribute]
            public string Email { get; set; }

            [XmlAttribute]
            public string Name { get; set; }

            [XmlAttribute]
            public string Roles { get; set; }

            [XmlAttribute]
            public DateTime CreationDT { get; set; }

            [XmlAttribute]
            public DateTime LastLoginDT { get; set; }


        }
        // ----------------------------------------------------

    }
}