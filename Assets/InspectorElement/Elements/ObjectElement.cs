using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XFramework.UI
{
    public class ObjectElement : ExpandableElement
    {
        protected override void CreateElements()
        {
            foreach (MemberInfo member in GetFields(BoundVariableType))
            {
                var a = CreateItemForMember(member);
                if (a != null)
                {
                    this.Add(a);
                    a.BindTo(this, member);
                    a.Refresh();
                }
            }
        }

        /// <summary>
        /// 通过成员变量信息获取UIItem
        /// </summary>
        /// <param name="member"></param>
        /// <param name="parentElement"></param>
        /// <returns></returns>
        private InspectorElement CreateItemForMember(MemberInfo member)
        {
            if(Attribute.IsDefined(member, typeof(ElementIngoreAttribute)))
            {
                return null;
            }

            Type variableType = member is FieldInfo ? ((FieldInfo)member).FieldType : ((PropertyInfo)member).PropertyType;

            InspectorElementAttribute attribute = member.GetCustomAttribute<InspectorElementAttribute>();

            if (attribute != null && attribute.type != null)
            {
                if (!attribute.type.IsSubclassOf(typeof(InspectorElement)))
                {
                    throw new Exception($"{member.Name}设置的{attribute.type.Name}不派生自UIItem");
                }

                return Inspector.CreateDrawerForType(attribute.type);
            }
            else
            {
                return Inspector.CreateDrawerForMemberType(variableType);
            }
        }

        /// <summary>
        /// 对获得FieldInfos排序
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<FieldInfo> GetFields(Type type)
        {
            List<FieldInfo> result = new List<FieldInfo>();

            do
            {
                var temp = new List<FieldInfo>(type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
                temp.AddRange(result);
                result = temp;
                type = type.BaseType;
            }
            while (type != typeof(NodeData).BaseType);

            return result;
        }
    }
}