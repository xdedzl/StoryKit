using UnityEngine.UIElements;
using UnityEngine;

namespace XFramework.UI
{
    public abstract class ExpandableElement : InspectorElement
    {
        private Foldout foldout;
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
            foldout = new Foldout()
            {
                //style =
                //{
                //    color = new StyleColor(Color.white),
                //}
            };
            foldout.value = true;
            foldout.RegisterValueChangedCallback((e) =>
            {
                if (e.newValue)
                {
                    this.Add(elementsContent);
                }
                else
                {
                    this.Remove(elementsContent);
                }
            });
            title.Add(foldout);
            title.Add(variableNameText);
            foldout.style.right = 18;
            variableNameText.style.right = 18;

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

        public void SetArrowActive(bool value)
        {
            if (value && !title.Contains(foldout))
            {
                title.Insert(0, foldout);
            }
            else if(!value && title.Contains(foldout))
            {
                title.Remove(foldout);
            }
        }

        protected override void OnDepthChange(int depth)
        {
            foldout.transform.position = new Vector2(10 * Depth, 0f);
        }
    }
}