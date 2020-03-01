using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace XFramework.UI
{
    [SupportHelper(typeof(ArrarySupport))]
    public class ArrayElement : ExpandableElement
    {
        private Type elementType;
        private bool IsArray { get { return BoundVariableType.IsArray; } }

        protected override void OnBound()
        {
            base.OnBound();
            elementType = IsArray ? BoundVariableType.GetElementType() : BoundVariableType.GetGenericArguments()[0];
        }

        protected override void CreateElements()
        {
            if (Value == null)
                return;

            if (IsArray)
            {
                Array array = (Array)Value;
                for (int i = 0; i < array.Length; i++)
                {
                    InspectorElement elementDrawer = Inspector.CreateDrawerForMemberType(elementType);
                    if (elementDrawer == null)
                        break;

                    int index = i;
                    elementDrawer.BindTo(elementType, "Element " + i, () => ((Array)Value).GetValue(index), (value) =>
                    {
                        Array _array = (Array)Value;
                        _array.SetValue(value, index);
                        Value = _array;
                    });

                    elementDrawer.variableNameText.transform.position += new Vector3(10, 0, 0);
                    this.Add(elementDrawer);
                }
            }
            else
            {
                IList list = (IList)Value;
                for (int i = 0; i < list.Count; i++)
                {
                    InspectorElement elementDrawer = Inspector.CreateDrawerForMemberType(elementType);
                    if (elementDrawer == null)
                        break;

                    int j = i;
                    elementDrawer.BindTo(elementType, "Element " + i, () => ((IList)Value)[j], (value) =>
                    {
                        IList _list = (IList)Value;
                        _list[j] = value;
                        Value = _list;
                    });

                    this.Add(elementDrawer);
                }
            }

            //sizeInput.Text = "" + Length;
        }

        private struct ArrarySupport : ISupport
        {
            public bool Support(Type type)
            {
                return (type.IsArray && type.GetArrayRank() == 1) ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>));
            }
        }
    }
}