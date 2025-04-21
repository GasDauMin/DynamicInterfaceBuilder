using Newtonsoft.Json;

namespace DynamicInterfaceBuilder.Core.Helpers
{
    public static class ObjectHelper
    {
        /// <summary>
        /// Creates a deep clone of an object using JSON serialization/deserialization
        /// </summary>
        /// <typeparam name="T">The type of object to clone</typeparam>
        /// <param name="source">The source object to clone</param>
        /// <returns>A deep copy of the source object, or a new instance if source is null</returns>
        public static T Clone<T>(T source) where T : class, new()
        {
            if (source == null)
                return new T();
                
            var json = JsonConvert.SerializeObject(source);
            var clone = JsonConvert.DeserializeObject<T>(json);
            
            return clone ?? new T();
        }
    }
}
