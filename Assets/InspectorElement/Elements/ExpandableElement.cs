using System;
using System.Reflection;
using UnityEngine.UIElements;

namespace XFramework.UI
{
    public abstract class ExpandableElement : InspectorElement
    {
        public override void Refresh()
        {
            base.Refresh();
            CreateElements();
        }

        protected virtual void CreateElements()
        {
            
        }

        protected virtual void ClearElements()
        {

        }
    }
}