using System;
using FubuMVC.Core;
using FubuMVC.StructureMap;

namespace HelloWorld.WebApp
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            FubuApplication.For<HelloWorldRegistry>()
                .StructureMapObjectFactory()
                .Bootstrap();
        }
    }
}