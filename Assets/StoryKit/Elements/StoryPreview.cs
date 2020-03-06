using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace XFramework.StoryKit
{
    public class StoryPreview : VisualElement
    {
        private List<int> m_selectedId = new List<int>();
        private VisualElement m_scrollView;
        private Action<IEnumerable<int>> m_callback;

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
                RefreshList();
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
                RefreshList();
            }
        }

        private bool m_isSelected;
        private bool IsSelected
        {
            get
            {
                return m_isSelected;
            }
            set
            {
                m_isSelected = value;
                RefreshList();
            }
        }

        private Toggle selectedStoryToggle;

        public StoryPreview()
        {
            var types = Utility.GetSonTypes(typeof(StoryDataBase));

            // 工具栏
            Toolbar toolbar = new Toolbar()
            {
                style =
                {
                    marginBottom = 5,
                    marginTop = 5,
                    marginLeft = 5,
                    marginRight = 5,
                }
            };

            // 不同类型的Story
            ToolbarMenu toolbarMenu = new ToolbarMenu()
            {
                text = "All"
            };
            toolbarMenu.menu.AppendAction("All", (e) =>
            {
                CurrentType = null;
                toolbarMenu.text = "All";
            }, a => CurrentType == null ? DropdownMenuAction.Status.Checked : DropdownMenuAction.Status.Normal);

            foreach (var type in types)
            {
                toolbarMenu.menu.AppendAction(type.Name, (e) => 
                {
                    CurrentType = type;
                    toolbarMenu.text = type.Name;
                }, a =>  CurrentType == type? DropdownMenuAction.Status.Checked : DropdownMenuAction.Status.Normal);
            }
            toolbar.Add(toolbarMenu);

            // 显示所有选择的Story
            selectedStoryToggle = new ToolbarToggle();
            selectedStoryToggle.RegisterValueChangedCallback((e) =>
            {
                IsSelected = e.newValue;
            });
            selectedStoryToggle.text = "Selected";
            toolbar.Add(selectedStoryToggle);

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

            // 预览区
            m_scrollView = new VisualElement(/*ScrollViewMode.VerticalAndHorizontal*/)
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
                    flexDirection = FlexDirection.Row,
                }
            };

            Button confirmBtn = new Button (OnConfirmClick)
            {
                text = "Confirm",
                style =
                {
                    alignSelf = Align.FlexEnd,
                }
            };
            this.Add(toolbar);
            this.Add(m_scrollView);
            this.Add(confirmBtn);
        }

        private void OnConfirmClick()
        {
            m_callback?.Invoke(m_selectedId);
            RemoveFromHierarchy();
        }

        private void OnCancelClick()
        {
            RemoveFromHierarchy();
        }

        private void RefreshList()
        {
            m_scrollView.Clear();
            var storyDatas = StoryManager.instance.GetStorys(CurrentType, CurrentMatching);

            foreach (var story in storyDatas)
            {
                if(!IsSelected || m_selectedId.Contains(story.id))
                    AddStoryItem(story);
            }
        }

        private void ShowSelectedStory()
        {
            m_scrollView.Clear();

            foreach (var id in m_selectedId)
            {
                var data = StoryManager.instance.GetStory(id);
                AddStoryItem(data);
            }
        }

        private void AddStoryItem(StoryDataBase story)
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
            m_scrollView.Add(storyToggle);

            if (m_selectedId.Contains(story.id))
            {
                storyToggle.value = true;
            }
        }

        private void OnStoryToggleValueChange(StoryDataBase storyData, ChangeEvent<bool> e)
        {
            if (e.newValue)
            {
                AddId(storyData.id);
            }
            else
            {
                if (!m_selectedId.Remove(storyData.id))
                {
                    throw new Exception("有问题");
                }
            }
        }

        private void AddId(int id)
        {
            if (!m_selectedId.Contains(id))
            {
                m_selectedId.Add(id);
            }
        }

        public void Open(IEnumerable<int> storysId, Action<IEnumerable<int>> callback)
        {
            m_selectedId.Clear();
            if (storysId != null)
            {
                foreach (var item in storysId)
                {
                    AddId(item);
                }
            }
            
            m_callback = callback;

            if (selectedStoryToggle.value)
            {
                ShowSelectedStory();
            }
            else
            {
                RefreshList();
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