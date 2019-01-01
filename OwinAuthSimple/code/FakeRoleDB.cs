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


        // private List<FakeUsersDBItem> _items;

        private string _xmlFileName = null;

        public FakeRoleDB(string xmlFileName)
        {
            _xmlFileName = xmlFileName;

        }


        private List<FakeUsersDBItem> Load()
        {
            var xs = new XmlSerializer(typeof(List<FakeUsersDBItem>));
            using (var fs = File.OpenRead(_xmlFileName))
            {
                return (List<FakeUsersDBItem>)xs.Deserialize(fs);
            }
        }

        private void Save(List<FakeUsersDBItem> items)
        {
            var xs = new XmlSerializer(typeof(List<FakeUsersDBItem>));
            using (var fs = File.OpenWrite(_xmlFileName))
            {
                xs.Serialize(fs, items);
            }
        }


        public void GetUserIDandRoles(string idFromProvider, string providerName, out string ID, out string[] roles)
        {
            List<FakeUsersDBItem> _items = Load();

            var item = _items.SingleOrDefault(x => x.ProviderName == providerName && x.IDFromProvider == idFromProvider);

            if (item == null)
            {
                item = new FakeUsersDBItem() { ProviderName = providerName, IDFromProvider = idFromProvider, Roles = "genericuser", UniqueID = Guid.NewGuid().ToString() };
                _items.Add(item);
                Save(_items);
            }

            ID = item.UniqueID;
            roles = item.Roles.Split('#');
        }



    }
}