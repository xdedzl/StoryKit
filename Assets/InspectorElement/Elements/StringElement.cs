using System;
using UnityEngine.UIElements;

namespace XFramework.UI
{
    [DefaultSportTypes(typeof(string))]
    public class StringElement : InspectorElement
    {
        private TextField input;

        public StringElement()
        {
            this.AddToClassList("string-element");
            input = new TextField();
            input.AddToClassList("input");
            this.Add(input);
            input.RegisterValueChangedCallback(OnValueChanged);
        }

        protected override void OnBound()
        {
            base.OnBound();
            input.value = Value?.ToString();
        }

        private void OnValueChanged(ChangeEvent<string> v)
        {
            Value = v.newValue;
        }
    }
}