using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using AGL.BusinessLogic.Interfaces;
using AGL.BusinessLogic;
using RestSharp;
using AGL.DataAccess.Interfaces;
using AGL.DataAccess;

namespace AGLProgrammingTest
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IPetsBusinesslogic, PetsBusinesslogic>();
            container.RegisterType<IPetsDataAccess, PetsDataAccess>();
            container.RegisterType<IRestClient, RestClient>(new InjectionConstructor());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}