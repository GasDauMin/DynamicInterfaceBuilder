namespace DynamicInterfaceBuilder
{
    public class ApplicationBase : IApplicationBase
    {
        public Application App { get; init; }

        public ApplicationBase(Application application)
        {
            App = application;
        }
    }
}