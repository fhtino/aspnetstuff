using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;


namespace OwinAuthSimple
{
    public class FakeRoleDB
    {
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
            public string Roles { get; set; }
        }
        // ----------------------------------------------------


        private List<FakeUsersDBItem> _items;


        public FakeRoleDB(string xmlFileName)
        {
            var xs = new XmlSerializer(typeof(List<FakeUsersDBItem>));
            using (var fs = File.OpenRead(xmlFileName))
            {
                _items = (List<FakeUsersDBItem>)xs.Deserialize(fs);
            }
        }


        public void GetUserIDandRoles(string idFromProvider, string providerName, out string ID, out string[] roles)
        {
            ID = null;
            roles = null;

            var item = _items.SingleOrDefault(x => x.ProviderName == providerName && x.IDFromProvider == idFromProvider);

            if (item != null)
            {
                ID = item.UniqueID;
                roles = item.Roles.Split('#');
            }
        }

    }
}