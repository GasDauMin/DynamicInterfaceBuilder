using DynamicInterfaceBuilder.Core.Interfaces;

namespace DynamicInterfaceBuilder.Core.Models
{
    public class AppBase : IAppBase
    {
        public App App { get; init; }

        public AppBase(App application)
        {
            App = application;
        }
    }
}