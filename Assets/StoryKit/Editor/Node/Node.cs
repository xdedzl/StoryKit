using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

namespace XFramewrok.StoryKit
{
    public class Node : VisualElement
    {
        // 用UXML
        public class Fatory : UxmlFactory<Node> { }

        private NodeData m_NodeData;

        private bool isMove;
        private VisualElement m_ContentUI;
        private VisualElement connectPointIn;
        private VisualElement connectPointOut;

        private List<Node> m_NextNodes;
        private List<Node> m_PrevNodes;

        public static event System.Action<Node> onNodeDelete;
        public static event System.Action<Node, Node> onNextNodeAdd;
        public static event System.Action<Node, Node> onPrevNodeAdd;

        public Node() { }

        public Node(NodeData nodeData) : this()
        {
            m_NodeData = nodeData;
            m_NextNodes = new List<Node>();
            m_PrevNodes = new List<Node>();
            this.RegisterCallback<PointerDownEvent>((a) =>
            {
                this.BringToFront();
            }, TrickleDown.NoTrickleDown);

            #region 节点顶部

            #region 连接点

            connectPointIn = new VisualElement();
            connectPointIn.AddToClassList("connectPoint");
            connectPointIn.RegisterCallback<PointerDownEvent>((a) => { a.StopPropagation(); });
            connectPointOut = new VisualElement();
            connectPointOut.AddToClassList("connectPoint");
            connectPointOut.RegisterCallback<PointerDownEvent>((a) => { a.StopPropagation(); });

            #endregion

            Label label = new Label()
            {
                text = $"节点",
            };
            label.AddToClassList("text");

            VisualElement title = new VisualElement();
            title.AddToClassList("title");

            title.RegisterCallback<MouseMoveEvent>((a) =>
            {
                //Debug.Log("MouseMove");
                if (isMove)
                    this.transform.position += (Vector3)a.mouseDelta;
            });
            title.RegisterCallback<PointerDownEvent>((a) =>
            {
                //Debug.Log("PointerDownEvent");
                isMove = true;
            });
            title.RegisterCallback<PointerUpEvent>((a) =>
            {
                //Debug.Log("PointerUpEvent");

                isMove = false;
            });
            title.RegisterCallback<PointerEnterEvent>((a) =>
            {
                isMove = false;
            });

            title.Add(connectPointIn);
            title.Add(label);
            title.Add(connectPointOut);

            title.RegisterCallback<MouseDownEvent>((v) =>
            {
                if (v.button == 0 && v.clickCount == 2)
                {
                    if (Contains(m_ContentUI))
                    {
                        Remove(m_ContentUI);
                    }
                    else
                    {
                        Add(m_ContentUI);
                    }
                }
                else if (v.button == 1 && v.clickCount == 1)
                {
                    GenericMenu genericMenu = new GenericMenu();
                    genericMenu.AddItem(new GUIContent("Delete"), false, Delete);
                    genericMenu.AddItem(new GUIContent("Add Next Node"), false, () =>
                    {
                        NodeData nextData = NodeManager.CreateNode();

                        Node nextNode = new Node(nextData)
                        {
                            transform =
                            {
                                position = this.transform.position + new Vector3(150,60,0),
                            },
                        };

                        AddNextNode(nextNode);

                        onNextNodeAdd?.Invoke(this, nextNode);
                    });
                    genericMenu.AddItem(new GUIContent("Add Prev Node"), false, () =>
                    {
                        NodeData prevData = NodeManager.CreateNode();

                        Node prevNode = new Node(prevData)
                        {
                            transform =
                            {
                                position = this.transform.position - new Vector3(20,20,0),
                            },
                        };

                        prevNode.AddNextNode(this);

                        onPrevNodeAdd?.Invoke(this, prevNode);
                    });
                    genericMenu.ShowAsContext();
                }
            });

            #endregion

            #region 数据内容

            m_ContentUI = new VisualElement();
            m_ContentUI.AddToClassList("content");

            TextField nameField = new TextField
            {
                label = "Name",
                value = m_NodeData.name,
            };

            nameField.RegisterValueChangedCallback((v) => { m_NodeData.name = v.newValue; });
            nameField.AddToClassList("item");

            m_ContentUI.Add(nameField);

            #endregion

            Add(title);
            Add(m_ContentUI);
        }

        private void AddNextNode(Node nextNode)
        {
            this.m_NextNodes.Add(nextNode);
            nextNode.m_PrevNodes.Add(this);
        }

        private void RemoveNextNode(Node nextNode)
        {
            this.m_NextNodes.Remove(nextNode);
            nextNode.m_PrevNodes.Remove(this);
        }

        /// <summary>
        /// 删除自身
        /// </summary>
        public void Delete()
        {
            onNodeDelete?.Invoke(this);
            foreach (var item in m_NextNodes)
            {
                item.m_PrevNodes.Remove(this);
            }
            m_NextNodes.Clear();
            foreach (var item in m_PrevNodes)
            {
                item.m_NextNodes.Remove(this);
            }
            m_PrevNodes.Clear();
            this.RemoveFromHierarchy();
        }

        public void DrawConnectLine()
        {
            foreach (var item in m_NextNodes)
            {
                Handles.DrawBezier(
                    GetPos(this.connectPointOut),
                    GetPos(item.connectPointIn),
                    GetPos(this.connectPointOut) + Vector2.left * 50f,
                    GetPos(item.connectPointIn) - Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }
        }

        private Vector2 GetPos(VisualElement visualElement)
        {
            return visualElement.worldBound.position;
        }
    }
}