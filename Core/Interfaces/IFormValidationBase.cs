using DynamicInterfaceBuilder.Core.Enums;
using DynamicInterfaceBuilder.Core.Models;

namespace DynamicInterfaceBuilder.Core.Interfaces
{
    public interface IFormValidationBase
    {
        public ValidationProperties Properties { get; }
        public ValidationType Type { get; }
    }
}