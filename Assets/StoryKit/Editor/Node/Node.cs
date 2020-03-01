﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using XFramework.UI;

namespace XFramework.StoryKit
{
    public class Node : VisualElement
    {
        public NodeData data { get; private set; }

        private bool isMove;
        private Inspector m_ContentUI;
        private ConnectPoint connectPointIn;
        private ConnectPoint connectPointOut;

        private List<Node> m_NextNodes;
        private List<Node> m_PrevNodes;

        public static event System.Action<Node> onNodeDelete;
        public static event System.Action<Node, Node> onNextNodeAdd;
        public static event System.Action<Node, Node> onPrevNodeAdd;

        public Node(NodeData nodeData)
        {
            data = nodeData;
            m_NextNodes = new List<Node>();
            m_PrevNodes = new List<Node>();
            this.RegisterCallback<PointerDownEvent>((a) =>
            {
                this.BringToFront();
            }, TrickleDown.NoTrickleDown);

            #region 节点顶部

            #region 连接点

            connectPointIn = new ConnectPoint(this, Point.In);
            connectPointIn.AddToClassList("connectPoint");
            connectPointIn.RegisterCallback<PointerDownEvent>((a) => { a.StopPropagation(); });
            connectPointOut = new ConnectPoint(this, Point.Out);
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
                if (isMove)
                    this.transform.position += (Vector3)a.mouseDelta;
            });
            title.RegisterCallback<PointerDownEvent>((a) =>
            {
                isMove = true;
            });
            title.RegisterCallback<PointerUpEvent>((a) =>
            {
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
                        NodeData nextData = NodeManager.CreateNodeData();

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
                        NodeData prevData = NodeManager.CreateNodeData();

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

            m_ContentUI = new Inspector();
            m_ContentUI.AddToClassList("content");
            m_ContentUI.Bind(data);

            #endregion

            Add(title);
            Add(m_ContentUI);
        }

        public void AddNextNode(Node nextNode)
        {
            this.m_NextNodes.Add(nextNode);
            nextNode.m_PrevNodes.Add(this);
        }

        public void RemoveNextNode(Node nextNode)
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

        /// <summary>
        /// 获取所有的后续节点
        /// </summary>
        public Node[] GetNextNodes()
        {
            return m_NextNodes.ToArray();
        }

        public void DrawConnectLine()
        {
            Node waitRemoved = null;

            foreach (var item in m_NextNodes)
            {
                Vector2 p1 = GetPos(this.connectPointOut);
                Vector2 p2 = GetPos(item.connectPointIn);

                Handles.DrawBezier(
                    p1,
                    p2,
                    p1 + Vector2.left * 50f,
                    p2 - Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;

                if (Handles.Button((p1 + p2) * 0.5f, Quaternion.identity, 4, 8, Handles.CircleHandleCap))
                {
                    waitRemoved = item;
                }
            }

            if (waitRemoved != null)
            {
                RemoveNextNode(waitRemoved);
            }
        }

        private Vector2 GetPos(VisualElement visualElement)
        {
            return visualElement.worldBound.position;
        }
    }
}