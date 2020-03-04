using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace XFramework.StoryKit
{
    public class StoryPreviewWindow : EditorWindow
    {
        [MenuItem("XFramework/StoryPreview")]
        public static void ShowExample()
        {
            var window = GetWindow<StoryPreviewWindow>();
            window.minSize = new Vector2(800, 400);
            window.titleContent = new GUIContent("StoryPreview");
        }

        private void OnEnable()
        {
            var types = Utility.GetSonTypes(typeof(StoryDataBase));

            Toolbar toolbar = new Toolbar();

            foreach (var item in types)
            {
                var toggle = new ToolbarToggle();
                toggle.RegisterValueChangedCallback((a) =>
                {
                    toggle.value = false;

                });
                toggle.text = item.Name;
                toolbar.Add(toggle);
            }

            rootVisualElement.Add(toolbar);
        }
    }
}