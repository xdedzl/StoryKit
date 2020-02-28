using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SFramework.UI
{
    public class Inspector : VisualElement
    {
        /// <summary>
        /// 默认UI
        /// </summary>
        Dictionary<Type, Type> m_DefaultTypeToDrawer = new Dictionary<Type, Type>();

        private object m_TargetObj;

        public Inspector()
        {
            Type type = typeof(InspectorElement);
            //foreach (var item in Setting.DefaultDrawers)
            //{
            //    if (m_Drawer.ContainsKey(item.GetType()))
            //        Debug.LogWarning($"重复添加{item.GetType().FullName}");
            //    m_Drawer.Add(item.GetType(), item);
            //}

            //foreach (var item in Setting.CustomDrawers)
            //{
            //    if (m_Drawer.ContainsKey(item.GetType()))
            //        Debug.LogWarning($"重复添加{item.GetType().FullName}");
            //    m_Drawer.Add(item.GetType(), item);
            //}
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

            InspectorElement uiItem = CreateDrawerForMemberType(obj.GetType(), this);
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
        public InspectorElement CreateDrawerForMemberType(Type memberType, VisualElement parentElement)
        {
            if (m_DefaultTypeToDrawer.TryGetValue(memberType, out Type elementType))
            {
                return CreateDrawerForType(elementType, parentElement);
            }
            //foreach (var uiItem in Setting.DefaultDrawers)
            //{
            //    if (uiItem.SupportType(memberType))
            //    {
            //        m_DefaultTypeToDrawer.Add(memberType, uiItem);
            //        return InstantiateDrawer(uiItem, drawerParent);
            //    }
            //}
            throw new Exception($"没有适用于{memberType.Name}的UIItem");
        }

        /// <summary>
        /// 通过UI类型获取UIItem
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="drawerParent"></param>
        /// <returns></returns>
        public InspectorElement CreateDrawerForType(Type elementType, VisualElement parentElement)
        {
            InspectorElement element = Activator.CreateInstance(elementType) as InspectorElement;
            parentElement.Add(element);
            element.Inspector = this;
            return element;
        }
    }
}