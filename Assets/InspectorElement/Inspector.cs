using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace XFramework.UI
{
    public class Inspector : VisualElement
    {
        /// <summary>
        /// 默认UI
        /// </summary>
        private Dictionary<Type, Type> m_DefaultTypeToDrawer = new Dictionary<Type, Type>();
        private Dictionary<ISupport, Type> m_OtherDrawer = new Dictionary<ISupport, Type>();

        private object m_TargetObj;

        public Inspector()
        {
            Type[] types = GetSonTypes(typeof(InspectorElement));
            foreach (var type in types)
            {
                var supportType = type.GetCustomAttribute<DefaultSportTypesAttribute>();
                if (supportType != null)
                {
                    foreach (var item in supportType.types)
                    {
                        m_DefaultTypeToDrawer.Add(item, type);
                    }
                }
                else
                {
                    var supportHelper = type.GetCustomAttribute<SupportHelperAttribute>();
                    if(supportHelper != null)
                    {
                        m_OtherDrawer.Add(supportHelper.support, type);
                    }
                }
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="obj"></param>
        public void Bind(object obj)
        {
            if (obj == null)
            {
                Debug.LogWarning("Null Obj!");
                return;
            }
            if (obj.GetType().IsValueType)
            {
                Debug.LogError("Can't bind a value type!");
                return;
            }

            InspectorElement uiItem = CreateDrawerForMemberType(obj.GetType());
            this.Add(uiItem); 
            if (uiItem != null)
            {
                m_TargetObj = obj;
                uiItem.BindTo(obj.GetType(), string.Empty, () => m_TargetObj, (value) => m_TargetObj = value);
                uiItem.Refresh();
            }
            else
                m_TargetObj = null;
        }

        /// <summary>
        /// 通过成员变量类型获取UIItem
        /// </summary>
        public InspectorElement CreateDrawerForMemberType(Type memberType, int depth = 0)
        {
            if (m_DefaultTypeToDrawer.TryGetValue(memberType, out Type elementType))
            {
                return CreateDrawerForType(elementType, depth);
            }
            else
            {
                foreach (var item in m_OtherDrawer)
                {
                    if (item.Key.Support(memberType))
                    {
                        return CreateDrawerForType(item.Value, depth);
                    }
                }

                return CreateDrawerForType(typeof(ObjectElement), depth);
            }
        }

        /// <summary>
        /// 通过UI类型获取UIItem
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="drawerParent"></param>
        /// <returns></returns>
        public InspectorElement CreateDrawerForType(Type elementType, int depth = 0)
        {
            InspectorElement element = Activator.CreateInstance(elementType) as InspectorElement;
            element.Inspector = this;
            return element;
        }

        private Type[] GetSonTypes(Type typeBase, string assemblyName = "Assembly-CSharp")
        {
            List<Type> typeNames = new List<Type>();
            Assembly assembly;
            try
            {
                assembly = Assembly.Load(assemblyName);
            }
            catch
            {
                return new Type[0];
            }

            if (assembly == null)
            {
                return new Type[0];
            }

            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeBase))
                {
                    typeNames.Add(type);
                }
            }
            return typeNames.ToArray();
        }
    }
}