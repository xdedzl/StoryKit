using System;

namespace XFramework.UI
{
    /// <summary>
    /// 定义自定义Element类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CustomerElementAttribute : Attribute
    {
        public Type type;

        public CustomerElementAttribute(Type type)
        {
            if (type != null && !type.IsSubclassOf(typeof(InspectorElement)))
            {
                throw new Exception($"参数type必须为{typeof(InspectorElement).Name}的派生类   type{type.Name}");
            }
            this.type = type;
        }
    }

    /// <summary>
    /// 定义变量在UI上的显示名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ElementPropertyAttribute : Attribute
    {
        public string propertyName;
        public ElementPropertyAttribute(string propertyName)
        {
            this.propertyName = propertyName;
        }
    }

    /// <summary>
    /// 忽略该变量
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ElementIngoreAttribute : Attribute { }

    /// <summary>
    /// 定义该Element型支持的类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DefaultSportTypesAttribute : Attribute
    {
        public Type[] types;
        public DefaultSportTypesAttribute(params Type[] types)
        {
            this.types = types;
        }
    }

    /// <summary>
    /// 定义该Element型支持的类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SupportHelperAttribute : Attribute
    {
        public ISupport support;
        public SupportHelperAttribute(Type supportType)
        {
            this.support = Activator.CreateInstance(supportType) as ISupport;
        }
    }

    public interface ISupport
    {
        bool Support(Type type);
    }
}