using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace XFramework.StoryKit
{
    /// <summary>
    /// 节点窗口基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NodeKitWindow<T> : EditorWindow
    {
        private Vector2 offset;
        private Vector2 drag;
        private bool activeDrag;

        private VisualElement m_NodeRoot;

        protected List<Node<T>> m_NodeList;

        protected Toolbar toolbar;

        [ContextMenu("dsad")]
        private void TTT()
        {
            Debug.Log(564);
        }

        protected virtual void OnEnable()
        {
            m_NodeList = new List<Node<T>>();

            var nodeStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/StoryKit/Node/Node.uss");
            var inspectorStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/InspectorElement/InspectorElement.uss");

            var root = rootVisualElement;
            root.styleSheets.Add(nodeStyle);
            root.styleSheets.Add(inspectorStyle);

            toolbar = new Toolbar();
            ToolbarButton openBtn = new ToolbarButton();
            openBtn.text = "OpenAsset";
            openBtn.clicked += OpenAsset;
            ToolbarButton saveBtn = new ToolbarButton();
            saveBtn.text = "SaveAsset";
            saveBtn.clicked += SaveAsset;
            toolbar.Add(openBtn);
            toolbar.Add(saveBtn);

            m_NodeRoot = new VisualElement();
            IMGUIContainer gridDrawAndEvent = new IMGUIContainer(() =>
            {
                DrawGrid(20, 0.2f, Color.gray);
                DrawGrid(100, 0.4f, Color.gray);

                ProcessEvents(UnityEngine.Event.current);
            });

            Node<T>.onNodeDelete += (n) =>
            {
                m_NodeList.Remove(n);
            };

            IMGUIContainer drawLine = new IMGUIContainer(() =>
            {
                DrawConnectLine();
            })
            {
                style =
                {
                    position = Position.Absolute
                },
                transform =
                {
                    position = new Vector2(5,-15),
                }
            };

            root.Add(toolbar);
            root.Add(gridDrawAndEvent);
            root.Add(m_NodeRoot);
            root.Add(drawLine);
        }

        private void ProcessEvents(UnityEngine.Event e)
        {
            drag = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        activeDrag = true;

                        if (ConnectPoint<T>.StartPoint != null)
                        {
                            ShowCreateNodeMenu(e.mousePosition, "", (node) =>
                            {
                                if (ConnectPoint<T>.StartPoint.point == Point.In)
                                {
                                    node.AddNextNode(ConnectPoint<T>.StartPoint.node);
                                }
                                else
                                {
                                    ConnectPoint<T>.StartPoint.node.AddNextNode(node);
                                }

                                ConnectPoint<T>.ClearLine();
                            });
                        }
                    }

                    if (e.button == 1)
                    {
                        ConnectPoint<T>.ClearLine();

                        ShowCreateNodeMenu(e.mousePosition, "Add Node");
                    }

                    if (e.button == 2)
                    {
                        ConnectPoint<T>.ClearLine();
                    }
                    break;
                case EventType.MouseUp:
                    if (e.button == 0)
                    {
                        activeDrag = false;
                    }
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && activeDrag)
                    {
                        OnCanvasDrag(e.delta);
                    }
                    break;
            }
        }

        /// <summary>
        /// 根据类型创建节点
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        protected Node<T> CreateNode(Type type, Vector2 pos)
        {
            CreateData(type, out T data);
            return CreateNode(data, pos);
        }

        /// <summary>
        /// 创建一个节点
        /// </summary>
        /// <param name="pos"></param>
        protected Node<T> CreateNode(T nodeData, Vector2 pos)
        {
            Node<T> node = new Node<T>(nodeData)
            {
                transform =
                {
                    position = pos,
                }
            };
            node.NodeWindow = this;
            m_NodeList.Add(node);
            m_NodeRoot.Add(node);
            return node;
        }

        /// <summary>
        /// 绘制面板网格
        /// </summary>
        /// <param name="gridSpacing">间隔</param>
        /// <param name="gridOpacity">透明度</param>
        /// <param name="gridColor">颜色</param>
        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            offset += drag * 0.5f;
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        /// <summary>
        /// 面板拖拽时
        /// </summary>
        /// <param name="delta"></param>
        private void OnCanvasDrag(Vector2 delta)
        {
            drag = delta;

            for (int i = 0; i < m_NodeList.Count; i++)
            {
                m_NodeList[i].transform.position += (Vector3)delta;
            }

            GUI.changed = true;
        }

        /// <summary>
        /// 画连接线
        /// </summary>
        private void DrawConnectLine()
        {
            foreach (var item in m_NodeList)
            {
                item.DrawConnectLine();
            }
        }

        /// <summary>
        /// 节点创建菜单
        /// </summary>
        /// <param name="postion">节点位置</param>
        /// <param name="path">菜单路径</param>
        /// <param name="callback">创建完成回调</param>
        private void ShowCreateNodeMenu(Vector2 position, string path = "", Action<Node<T>> callback = null)
        {
            GenericMenu genericMenu = new GenericMenu();
            AddCreateNodeMenu(genericMenu, position, path, callback);
            genericMenu.ShowAsContext();
        }

        /// <summary>
        /// 给菜单添加创建节点的按钮
        /// </summary>
        public void AddCreateNodeMenu(GenericMenu genericMenu, Vector2 position, string path = "", Action<Node<T>> callback = null)
        {
            path = string.IsNullOrEmpty(path) ? "" : path + "/";

            foreach (var item in Utility.GetSonTypes(typeof(StoryDataBase)))
            {
                genericMenu.AddItem(new GUIContent($"{path}{item.Name}"), false, () =>
                {
                    Node<T> node = CreateNode(item, position);
                    callback?.Invoke(node);
                });
            }
        }

        #region 数据

        /// <summary>
        /// 保存数据（需自行实现连接信息的转化）
        /// </summary>
        private void SaveAsset()
        {
            string fileName = EditorUtility.SaveFilePanel("保存路径", Application.dataPath, "NewNodeDatas", "json");

            if (string.IsNullOrEmpty(fileName)) return;

            OnSave(fileName);

            Debug.Log("Save successfully");
        }

        /// <summary>
        /// 打开数据（需自行实现连接信息的转化）
        /// </summary>
        private void OpenAsset()
        {
            string fileName = EditorUtility.OpenFilePanel("打开路径", Application.dataPath, "json");

            if (string.IsNullOrEmpty(fileName)) return;

            OnOpen(fileName);

            Debug.Log("Open successfully");
        }

        #endregion

        #region 自定义

        protected abstract void CreateData(Type type, out T data);

        protected virtual void OnSave(string path) { }

        protected virtual void OnOpen(string path) { }

        public virtual void OnNodeDelete(T data) { }

        #endregion
    }
}