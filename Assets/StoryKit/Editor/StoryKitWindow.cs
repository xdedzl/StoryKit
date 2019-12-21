using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

namespace XFramewrok.StoryKit
{
    public class StoryKitWindow : EditorWindow
    {
        [MenuItem("XFramework/StoryKit")]
        public static void ShowExample()
        {
            StoryKitWindow window = GetWindow<StoryKitWindow>();
            window.minSize = new Vector2(450, 514);
            window.titleContent = new GUIContent("StoryKit");
        }

        private StyleSheet nodeStyle;

        private Vector2 offset;
        private Vector2 drag;
        private bool activeDrag;

        private VisualElement m_NodeRoot;

        private List<Node> m_NodeList;

        [ContextMenu("dsad")]
        private void TTT()
        {
            Debug.Log(564);
        }

        private void OnEnable()
        {
            m_NodeList = new List<Node>();

            nodeStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/StoryKit/Editor/Node/Node.uss"); 

            var root = rootVisualElement;
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/StoryKit/Editor/StoryKitWindow.uss"));

            Toolbar toolbar = new Toolbar();
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

                ProcessEvents(Event.current);
            });

            Node.onNodeDelete += (n) =>
            {
                m_NodeList.Remove(n);
            };
            Node.onNextNodeAdd += (node, nextNode) =>
            {
                AddNode(nextNode);
            };
            Node.onPrevNodeAdd += (node, prevNode) =>
            {
                AddNode(prevNode);
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

        private void ProcessEvents(Event e)
        {
            drag = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        activeDrag = true;

                        if(ConnectPoint.StartPoint != null)
                        {
                            Node node = CreateNode(e.mousePosition);

                            if(ConnectPoint.StartPoint.point == Point.In)
                            {
                                node.AddNextNode(ConnectPoint.StartPoint.node);
                            }
                            else
                            {
                                ConnectPoint.StartPoint.node.AddNextNode(node);
                            }

                            ConnectPoint.ClearLine();
                        }
                    }

                    if (e.button == 1)
                    {
                        ConnectPoint.ClearLine();

                        ProcessContextMenu(e.mousePosition);
                    }

                    if(e.button == 2)
                    {
                        ConnectPoint.ClearLine();
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

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add node"), false, () =>
            {
                CreateNode(mousePosition);
            });
            genericMenu.ShowAsContext();
        }

        private Node CreateNode(Vector2 pos)
        {
            return CreateNode(pos, NodeManager.CreateNodeData());
        }

        /// <summary>
        /// 创建一个节点
        /// </summary>
        /// <param name="pos"></param>
        private Node CreateNode(Vector2 pos, NodeData nodeData)
        {
            Node node = new Node(nodeData)
            {
                transform =
                    {
                        position = pos
                    },
            };

            AddNode(node);

            return node;
        }

        /// <summary>
        /// 添加一个节点
        /// </summary>
        /// <param name="node"></param>
        private void AddNode(Node node)
        {
            m_NodeList.Add(node);
            node.styleSheets.Add(nodeStyle);
            m_NodeRoot.Add(node);
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

        #region 数据

        private void SaveAsset()
        {
            string fileName = EditorUtility.SaveFilePanel("保存路径", Application.dataPath, "NewNodeDatas", ".json");

            List<SerializableNodeData> serializableNodeDatas = new List<SerializableNodeData>();
            foreach (var node in m_NodeList)
            {
                var nextNodes = node.GetNextNodes();
                node.data.nextNodes?.Clear();

                if(nextNodes.Length > 0)
                    node.data.nextNodes = node.data.nextNodes ?? new List<int>();

                foreach (var item in nextNodes)
                {
                    node.data.nextNodes.Add(item.data.id);
                }

                serializableNodeDatas.Add(new SerializableNodeData
                {
                    postion = node.transform.position,
                    nodeData = node.data
                });
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(serializableNodeDatas);
            System.IO.File.WriteAllText(fileName, json);

            Debug.Log("Save successfully");
        }

        private void OpenAsset()
        {
            string fileName = EditorUtility.OpenFilePanel("打开路径", Application.dataPath, ".json");

            string json = System.IO.File.ReadAllText(fileName);

            var datas = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializableNodeData[]>(json);

            Dictionary<int, Node> dataDic = new Dictionary<int, Node>();
            foreach (var item in datas)
            {
                Node node = CreateNode(item.postion, item.nodeData);
                dataDic.Add(item.nodeData.id, node);
            }

            foreach (var item in m_NodeList)
            {
                if (item.data.nextNodes != null)
                {
                    foreach (var id in item.data.nextNodes)
                    {
                        item.AddNextNode(dataDic[id]);
                    }
                }
            }
        }

        #endregion
    }
}