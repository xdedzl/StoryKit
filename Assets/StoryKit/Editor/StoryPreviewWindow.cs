using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

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

        private List<int> m_selectedId = new List<int>();
        private VisualElement scrollView;

        private Type m_currentType;
        private Type CurrentType
        {
            get
            {
                return m_currentType;
            }
            set
            {
                m_currentType = value;
                ChangeList();
            }
        }
        private string m_currentMatching;
        private string CurrentMatching
        {
            get
            {
                return m_currentMatching;
            }
            set
            {
                m_currentMatching = value;
                ChangeList();
            }
        }

        private void OnEnable()
        {
            var types = Utility.GetSonTypes(typeof(StoryDataBase));
            Toolbar toolbar = new Toolbar();
            scrollView = new VisualElement(/*ScrollViewMode.VerticalAndHorizontal*/)
            {
                style =
                {
                    marginLeft = 10,
                    marginTop = 10,
                    marginRight = 10,
                    marginBottom = 10,
                    // Example of an horizontal container that wraps its contents
                    // over several lines depending on available space
                    flexWrap = Wrap.Wrap,
                    flexDirection = FlexDirection.Row
                }
            };

            ToggleGroup toggleGroup = new ToggleGroup();

            foreach (var type in types)
            {
                var toggle = new ToolbarToggle();
                toggleGroup.Add(toggle);
                toggle.RegisterValueChangedCallback((e) =>
                {
                    if (e.newValue)
                    {
                        CurrentType = type;
                    }
                });
                toggle.text = type.Name;
                toolbar.Add(toggle);
            }

            var lastToggle = new ToolbarToggle();
            toggleGroup.Add(lastToggle);
            lastToggle.RegisterValueChangedCallback((e) =>
            {
                if (e.newValue)
                {
                    CurrentType = null;
                }
            });
            lastToggle.text = "All";
            toolbar.Add(lastToggle);

            ToolbarSearchField toolbarSearch = new ToolbarSearchField
            {
                style =
                {
                    width = 100,
                    flexDirection = FlexDirection.Row,
                    alignSelf = Align.FlexEnd,
                }
            };
            toolbarSearch.RegisterValueChangedCallback((e) =>
            {
                CurrentMatching = e.newValue;
            });
            toolbar.Add(toolbarSearch);

            rootVisualElement.Add(toolbar);
            rootVisualElement.Add(scrollView);
        }

        private void ChangeList()
        {
            scrollView.Clear();
            var storyDatas = StoryManager.instance.GetStorys(CurrentType, CurrentMatching);

            foreach (var story in storyDatas)
            {
                var storyToggle = new ToolbarToggle()
                {
                    text = story.name,
                    style =
                    {
                        width = 60,
                        height = 60,
                        marginLeft = 5,
                        marginTop = 5,
                        marginRight = 5,
                        marginBottom = 5,
                        unityTextAlign = TextAnchor.MiddleCenter,
                    }
                };
                storyToggle.RegisterValueChangedCallback((e) =>
                {
                    OnStoryToggleValueChange(story, e);
                });
                scrollView.Add(storyToggle);

                if (m_selectedId.Contains(story.id))
                {
                    storyToggle.value = true;
                }
            }
        }

        private void OnStoryToggleValueChange(StoryDataBase storyData, ChangeEvent<bool> e)
        {
            if (e.newValue)
            {
                if (!m_selectedId.Contains(storyData.id))
                {
                    m_selectedId.Add(storyData.id);
                }
            }
            else
            {
                if (!m_selectedId.Remove(storyData.id))
                {
                    throw new Exception("有问题");
                }
            }
        }
    }

    public class ToggleGroup
    {
        private List<Toggle> m_toggleList = new List<Toggle>();

        public Toggle this[int index]
        {
            get
            {
                return m_toggleList[index];
            }
        }

        public bool IsAllFalse()
        {
            foreach (var item in m_toggleList)
            {
                if (item.value == true)
                    return false;
            }
            return true;
        }

        public void Add(Toggle toggle)
        {
            toggle.RegisterValueChangedCallback((e) =>
            {
                if (e.newValue)
                {
                    foreach (var item in m_toggleList)
                    {
                        if (item != toggle)
                        {
                            item.value = false;
                        }
                    }
                }
            });
            m_toggleList.Add(toggle);
        }
    }
}