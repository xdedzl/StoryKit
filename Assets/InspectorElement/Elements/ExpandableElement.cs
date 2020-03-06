using UnityEngine.UIElements;

namespace XFramework.UI
{
    public abstract class ExpandableElement : InspectorElement
    {
        protected VisualElement title;

        protected VisualElement elementsContent;

        public ExpandableElement()
        {
            this.Remove(variableNameText);
            title = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                }
            };
            var arrow = new Toggle()
            {
                //style =
                //{
                //    width = 8,
                //}
            };
            arrow.value = true;
            arrow.RegisterValueChangedCallback((e) =>
            {
                if (e.newValue)
                {
                    Refresh();
                }
                else
                {
                    elementsContent.Clear();
                }
            });
            title.Add(arrow);
            title.Add(variableNameText);

            elementsContent = new VisualElement();

            this.Add(title);
            this.Add(elementsContent);
        }

        public override void Refresh()
        {
            base.Refresh();
            ClearElements();
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