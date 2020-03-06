using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using XFramework.UI;

namespace XFramework.StoryKit
{
    public class StorySelectElement : InspectorElement
    {
        public StoryPreview storyPreview;

        public StorySelectElement()
        {
            this.Remove(variableNameText);

            var title = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                }
            };
            title.Add(variableNameText);

            Button addBtn = new Button(OnAddBtnClick)
            {
                text = "修改/查看",
            };
            title.Add(addBtn);

            this.Add(title);
        }

        private void OnAddBtnClick()
        {
            if (storyPreview == null)
            {
                storyPreview = new StoryPreview
                {
                    style =
                    {
                        position = Position.Absolute,
                        backgroundColor = new StyleColor(Color.gray),
                        borderBottomLeftRadius = new StyleLength(5),
                        borderBottomRightRadius = new StyleLength(5),
                        borderTopLeftRadius = new StyleLength(5),
                        borderTopRightRadius = new StyleLength(5),
                    },
                };
                storyPreview.transform.position = this.transform.position + new Vector3(200, 0, 0);
            }

            if (!Contains(storyPreview))
            {
                this.Add(storyPreview);
            }
            else
            {
                storyPreview.RemoveFromHierarchy();
            }

            var ids = (int[])Value;

            storyPreview.Open(ids, (v) =>
            {
                Value = v.ToArray();
            });
        }
    }
}