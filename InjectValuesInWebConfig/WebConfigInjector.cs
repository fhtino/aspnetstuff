using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;

[assembly: PreApplicationStartMethod(typeof(InjectValuesInWebConfig.WebConfigInjector), "Start")]

namespace InjectValuesInWebConfig
{
    public class WebConfigInjector
    {

        // Reference:  https://gist.github.com/davidebbo/2b961ad298683987547c  by davidebbo

        public static void Start()
        {
            string xmlFileName = ConfigurationManager.AppSettings["WebConfigCustomValuesFile"];

            if (!String.IsNullOrEmpty(xmlFileName))
            {
                var xdoc = new XmlDocument();
                xdoc.Load(xmlFileName);

                foreach (XmlElement appSetNode in xdoc.SelectNodes("/myvalues/appsettings/kv"))
                {
                    string key = appSetNode.GetAttribute("key");
                    string value = appSetNode.GetAttribute("value");
                    ConfigurationManager.AppSettings[key] = value;
                }

                foreach (XmlElement connStringNode in xdoc.SelectNodes("/myvalues/connectionstring/conn"))
                {
                    string name = connStringNode.GetAttribute("name");
                    string connstring = connStringNode.GetAttribute("connstring");
                    string provider = connStringNode.GetAttribute("provider");

                    ConnectionStringSettings css = ConfigurationManager.ConnectionStrings[name];

                    if (css == null)
                    {
                        //Note: ConfigurationElementCollection is the base class of ConnectionStringSettingsCollection
                        FieldInfo readOnlyField = typeof(ConfigurationElementCollection).GetField("bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                        readOnlyField.SetValue(ConfigurationManager.ConnectionStrings, value: false);

                        css = new ConnectionStringSettings()
                        {
                            Name = name,
                            ConnectionString = connstring,
                            ProviderName = provider
                        };

                        ConfigurationManager.ConnectionStrings.Add(css);
                    }
                    else
                    {
                        FieldInfo readOnlyField = typeof(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                        readOnlyField.SetValue(css, value: false);
                        css.ConnectionString = connstring;
                        if (provider != null)
                        {
                            css.ProviderName = provider;
                        }
                    }
                }
            }
        }

    }
}