using System;

namespace XFramework.UI
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

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ElementPriorityAttribute : Attribute
    {
        public int priority;
        public ElementPriorityAttribute(int priority)
        {
            this.priority = priority;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DefaultSportTypesAttribute : Attribute
    {
        public Type[] types;
        public DefaultSportTypesAttribute(params Type[] types)
        {
            this.types = types;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SupportHelperAttribute : Attribute
    {
        public ISupport support;
        public SupportHelperAttribute(Type supportType)
        {
            this.support = Activator.CreateInstance(supportType) as ISupport;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ElementIngoreAttribute : Attribute { }

    public interface ISupport
    {
        bool Support(Type type);
    }
}