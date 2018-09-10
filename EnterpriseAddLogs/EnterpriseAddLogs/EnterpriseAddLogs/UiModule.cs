namespace EnterpriseAddLogs
{
    using Autofac;
    using EnterpriseAddLogs.Helpers;
    using EnterpriseAddLogs.Messaging;
    using EnterpriseAddLogs.ViewModels;
    using Xamarin.Forms;

    public sealed class UiModule : Module
    {
        public UiModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(UiModule).Assembly)
                .Where(t => t.IsSubclassOf(typeof(Page)));
            builder.RegisterAssemblyTypes(typeof(UiModule).Assembly)
                .Where(t => t.IsSubclassOf(typeof(ViewModel)));

            builder.RegisterType<ViewResolver>().AsImplementedInterfaces();
            builder.RegisterType<Navigator>().SingleInstance().AsImplementedInterfaces();
            builder.RegisterType<MessageBus>().SingleInstance().AsImplementedInterfaces();

        }
    }
}
