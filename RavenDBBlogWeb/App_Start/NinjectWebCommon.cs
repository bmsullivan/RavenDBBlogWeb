[assembly: WebActivator.PreApplicationStartMethod(typeof(RavenDBBlogWeb.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(RavenDBBlogWeb.App_Start.NinjectWebCommon), "Stop")]

namespace RavenDBBlogWeb.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Ajax.Utilities;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Client.Indexes;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var docStore = new DocumentStore()
                           {
                               Url = "http://localhost:8080"
                           };

            docStore.Initialize();

            IndexCreation.CreateIndexes(typeof(Posts_PostsByTag).Assembly, docStore);

            kernel.Bind<IDocumentStore>().ToConstant(docStore).InSingletonScope();
            kernel.Bind<IDocumentSession>().ToMethod(c => c.Kernel.Get<IDocumentStore>().OpenSession()).InRequestScope();
        }        
    }
}
