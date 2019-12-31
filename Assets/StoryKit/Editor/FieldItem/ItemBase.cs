using UnityEngine.UIElements;
using System.Reflection;

public class ItemBase : VisualElement
{
    private object m_Value;
    public object Value
    {
        get
        {
            return m_Value;
        }
        set
        {
            m_Value = Value;
        }
    }
}

public class StringItem : VisualElement
{
    public StringItem(object target, FieldInfo info)
    {
        TextField textField = new TextField
        {
            label = info.Name,
            value = info.GetValue(target) as string,
        };
        textField.RegisterValueChangedCallback((v) => 
        {
            info.SetValue(target, v.newValue);
        });
        textField.AddToClassList("item");
        this.Add(textField);
    }
}