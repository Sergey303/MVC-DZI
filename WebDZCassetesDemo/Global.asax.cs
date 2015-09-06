using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using Fogid.DocumentStorage;

namespace WebDZCassetesDemo
{
    public class Global : System.Web.HttpApplication
    {
        public static DStorage Storage;

        protected void Application_Start(object sender, EventArgs e)
        {
              RouteTable.Routes.MapMvcAttributeRoutes();
              //string path = "../../../TestData/";
            string path = @"C:\cassettes\";
              Storage = new DStorage();
              Storage.Init(GetConfig(path));
              DBAdapter adapter = new XmlDbAdapter(); //null;//new PBEAdapter();
              Storage.InitAdapter(adapter);
              Storage.LoadFromCassettesExpress();
        }

        public static XElement GetConfig(string path)
        {
            string config_text = @"<?xml version='1.0' encoding='utf-8' ?>
<config>

  <database dbname='polar20150825' connectionstring='polar:D:\home\FactographDatabases\polartest\'/>

  <!--<LoadCassette regime='nodata'>" + path + "SampleOfCassette" + @"</LoadCassette>-->
  <LoadCassette regime='nodata'>" + path + "newspaper" + @"</LoadCassette>
  <!--LoadCassette write='yes'>D:\home\FactographProjects\Test20130213</LoadCassette-->
</config>    
";
            return XElement.Parse(config_text);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}