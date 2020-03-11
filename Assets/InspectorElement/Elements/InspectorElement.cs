using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace XFramework.UI
{
    public abstract class InspectorElement : VisualElement
    {
        public delegate object Getter();
        public delegate void Setter(object value);

        private Getter getter;
        private Setter setter;

        private object m_value;
        private Type m_boundVariableType;
        private Inspector m_inspector;
        private int m_depth;

        [SerializeField]
        public TextElement variableNameText { get; protected set; }

        public InspectorElement()
        {
            variableNameText = new TextElement();
            this.Add(variableNameText);

            this.AddToClassList("inspector-element");
            variableNameText.AddToClassList("label");
        }

        protected object Value
        {
            get
            {
                return m_value;
            }
            set
            {
                setter(value);
                m_value = value;
            }
        }

        protected Type BoundVariableType
        {
            get
            {
                return m_boundVariableType;
            }
        }

        public Inspector Inspector
        {
            protected get { return m_inspector; }
            set
            {
                if (m_inspector != value)
                {
                    m_inspector = value;
                }
            }
        }

        public int Depth
        {
            get
            {
                return m_depth;
            }
            set
            {
                m_depth = value;
                if (variableNameText != null)
                    variableNameText.transform.position = new Vector2(10 * Depth, 0f);
                OnDepthChange(value);
            }
        }

        protected string Name
        {
            get
            {
                if (variableNameText != null)
                    return variableNameText.text;
                else
                    return string.Empty;
            }
            set
            {
                if (variableNameText != null)
                    variableNameText.text = value;
            }
        }

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <param name="parent">UI</param>
        /// <param name="member">成员</param>
        /// <param name="variableName">变量名称</param>
        public void BindTo(InspectorElement parent, MemberInfo member, string proertyName)
        {
            string variableName = proertyName;

            if (member is FieldInfo field)
            {
                if (variableName == null)
                    variableName = field.Name;

                if (!field.FieldType.IsValueType)
                    BindTo(field.FieldType, variableName, () => field.GetValue(parent.Value), (value) =>
                    {
                        field.SetValue(parent.Value, value);
                    });
                else
                    BindTo(field.FieldType, variableName, () => field.GetValue(parent.Value), (value) =>
                    {
                        field.SetValue(parent.Value, value);
                        parent.Value = parent.Value;         // 这一步是为了将struct的值不断向上一级传递
                    });
            }
            else if (member is PropertyInfo property)
            {
                if (variableName == null)
                    variableName = property.Name;

                if (!property.PropertyType.IsValueType)
                    BindTo(property.PropertyType, variableName, () => property.GetValue(parent.Value, null), (value) =>
                    {
                        property.SetValue(parent.Value, value, null);
                    });
                else
                    BindTo(property.PropertyType, variableName, () => property.GetValue(parent.Value, null), (value) =>
                    {
                        property.SetValue(parent.Value, value, null);
                        parent.Value = parent.Value;
                    });
            }
            else
                throw new ArgumentException("Member can either be a field or a property");
        }

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <param name="variableType">变量类型</param>
        /// <param name="variableName">变量名称</param>
        /// <param name="getter"></param>
        /// <param name="setter"></param>
        public void BindTo(Type variableType, string variableName, Getter getter, Setter setter)
        {
            m_boundVariableType = variableType;
            Name = variableName;

            this.getter = getter;
            this.setter = setter;

            OnBound();
        }

        /// <summary>
        /// UI刷新
        /// </summary>
        public virtual void Refresh()
        {

        }

        /// <summary>
        /// 数据绑定时调用
        /// </summary>
        protected virtual void OnBound()
        {
            try
            {
                m_value = getter();
            }
            catch
            {
                if (BoundVariableType.IsValueType)
                    m_value = Activator.CreateInstance(BoundVariableType);
                else
                    m_value = null;
            }
        }

        /// <summary>
        /// 数据解绑时调用
        /// </summary>
        protected virtual void OnUnBound()
        {
            m_value = null;
        }

        protected virtual void OnDepthChange(int depth)
        {

        }
    }
}