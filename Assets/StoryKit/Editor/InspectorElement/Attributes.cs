using System;

namespace SFramework.UI
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class InspectorElementAttribute : Attribute
    {
        public string name;
        public Type type;
        public InspectorElementAttribute(string name = null, Type type = null)
        {
            if (type != null && !type.IsSubclassOf(typeof(InspectorElement)))
            {
                throw new Exception($"参数type必须为{typeof(InspectorElement).Name}的派生类   type{type.Name}");
            }
            this.name = name;
            this.type = type;
        }
    }
}