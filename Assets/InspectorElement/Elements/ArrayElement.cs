﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace XFramework.UI
{
    [SupportHelper(typeof(ArrarySupport))]
    public class ArrayElement : ExpandableElement
    {
        private Type elementType;
        private bool IsArray { get { return BoundVariableType.IsArray; } }

        private TextField sizeInput;
        private Type customerElementType;

        private int Length
        {
            get
            {
                if (Value == null)
                    return 0;
                else if (Value is Array array)
                    return array.Length;
                else if (Value is IList list)
                    return list.Count;

                throw new Exception("类型错误");
            }
        }

        public ArrayElement()
        {
            sizeInput = new TextField();

            title.Add(sizeInput);

            sizeInput.RegisterValueChangedCallback(OnSizeChange);

            this.AddToClassList("array-element");
            sizeInput.AddToClassList("input");
        }

        public ArrayElement(Type type) : this()
        {
            customerElementType = type;
        }

        protected override void OnBound()
        {
            base.OnBound();
            elementType = IsArray ? BoundVariableType.GetElementType() : BoundVariableType.GetGenericArguments()[0];
            sizeInput.value = Length.ToString();
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
                    InspectorElement elementDrawer = CreateDrawerForMemberType();
                    if (elementDrawer == null)
                        break;

                    int index = i;
                    elementDrawer.BindTo(elementType, "Element " + i, () => ((Array)Value).GetValue(index), (value) =>
                    {
                        Array _array = (Array)Value;
                        _array.SetValue(value, index);
                        Value = _array;
                    });
                    elementDrawer.Refresh();

                    elementsContent.Add(elementDrawer);
                }
            }
            else
            {
                IList list = (IList)Value;
                for (int i = 0; i < list.Count; i++)
                {
                    InspectorElement elementDrawer = CreateDrawerForMemberType();
                    if (elementDrawer == null)
                        break;

                    int j = i;
                    elementDrawer.BindTo(elementType, "Element " + i, () => ((IList)Value)[j], (value) =>
                    {
                        IList _list = (IList)Value;
                        _list[j] = value;
                        Value = _list;
                    });
                    elementDrawer.Refresh();

                    elementsContent.Add(elementDrawer);
                }
            }
        }

        protected override void ClearElements()
        {
            elementsContent.Clear();
        }

        private InspectorElement CreateDrawerForMemberType()
        {
            if(customerElementType != null)
            {
                return Inspector.CreateDrawerForType(customerElementType, Depth + 1);
            }
            else
            {
                return Inspector.CreateDrawerForMemberType(elementType, Depth + 1);
            }
        }

        private void OnSizeChange(ChangeEvent<string> input)
        {
            if (int.TryParse(input.newValue, out int size))
            {
                if (size != Length && size >= 0)
                {
                    int currLength = Length;
                    if (IsArray)
                    {
                        Array array = (Array)Value;
                        Array newArray = Array.CreateInstance(BoundVariableType.GetElementType(), size);
                        if (size > currLength)
                        {
                            if (array != null)
                                Array.ConstrainedCopy(array, 0, newArray, 0, currLength);

                            for (int i = size - currLength; i < currLength; i++)
                            {
                                object v = Activator.CreateInstance(BoundVariableType);
                                newArray.SetValue(v, i);
                            }
                        }
                        else
                            Array.ConstrainedCopy(array, 0, newArray, 0, size);

                        Value = newArray;
                    }
                    else
                    {
                        IList list = (IList)Value;

                        int differLength = size - currLength;
                        if (differLength > 0)
                        {
                            if (list == null)
                                list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(BoundVariableType.GetGenericArguments()[0]));

                            for (int i = 0; i < differLength; i++)
                                list.Add(default);
                        }
                        else
                        {
                            for (int i = 0; i > differLength; i--)
                                list.RemoveAt(list.Count - 1);
                        }

                        Value = list;
                    }

                    Refresh();
                }
            }
            else if(string.IsNullOrEmpty(input.newValue))
            {
                sizeInput.value = "0";
            }
            else
            {
                sizeInput.value = input.previousValue;
            }
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