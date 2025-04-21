using DynamicInterfaceBuilder.Core.Interfaces;

namespace DynamicInterfaceBuilder.Core.Models
{
    public class AppBase(App application) : IAppBase
    {
        public App App { get; init; } = application;
    }
}