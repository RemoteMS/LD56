using System;

namespace ServiceLocator.Reflection
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class RegisterFieldAttribute : Attribute
    {
        public Type[] InterfaceTypes { get; }

        public RegisterFieldAttribute(params Type[] interfaceTypes)
        {
            InterfaceTypes = interfaceTypes;
        }
    }
}
