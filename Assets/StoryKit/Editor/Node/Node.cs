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

        public static event System.Action<Node> onNodeDelete;
        public static event System.Action<Node, Node> onNextNodeAdd;
        public static event System.Action<Node, Node> onPrevNodeAdd;

        public Node() { }

        public Node(NodeData nodeData) : this()
        {
            m_NodeData = nodeData;
            m_NextNodes = new List<Node>();
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
                //Debug.Log("PointerEnterEvent");

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
                    genericMenu.AddItem(new GUIContent("Delete"), false, () =>
                    {
                        onNodeDelete?.Invoke(this);
                        this.RemoveFromHierarchy();
                    });
                    genericMenu.AddItem(new GUIContent("Add Next Node"), false, () =>
                    {
                        NodeData nextData = NodeManager.CreateNode();

                        Node nextNode = new Node(nextData)
                        {
                            transform =
                            {
                                position = this.transform.position + new Vector3(10,10,0),
                            },
                        };

                        this.m_NextNodes.Add(nextNode);

                        onNextNodeAdd(this, nextNode);
                    });
                    genericMenu.AddItem(new GUIContent("Add Prev Node"), false, () =>
                    {
                        NodeData prevData = NodeManager.CreateNode();

                        Node prevNode = new Node(prevData)
                        {
                            transform =
                            {
                                position = this.transform.position - new Vector3(10,10,0),
                            },
                        };

                        prevNode.m_NextNodes.Add(this);

                        onPrevNodeAdd(this, prevNode);
                    });
                    genericMenu.ShowAsContext();
                }
            });

            #endregion

            #region 数据内容

            m_ContentUI = new VisualElement();
            m_ContentUI.AddToClassList("content");

            //IntegerField idField = new IntegerField
            //{
            //    label = "Id",
            //    value = m_NodeData.id,
            //};
            //idField.RegisterValueChangedCallback((v) => { m_NodeData.id = v.newValue; });
            //idField.AddToClassList("item");
            TextField nameField = new TextField
            {
                label = "Name",
                value = m_NodeData.name,
            };

            nameField.RegisterValueChangedCallback((v) => { m_NodeData.name = v.newValue; });
            nameField.AddToClassList("item");

            //m_ContentUI.Add(idField);
            m_ContentUI.Add(nameField);

            #endregion

            Add(title);
            Add(m_ContentUI);

            Add(new IMGUIContainer()
            {
                onGUIHandler = () => { DrawConnectLine(); },
                style =
                {
                    position = Position.Absolute,
                }
            });
        }

        public void DrawConnectLine()
        {
            foreach (var item in m_NextNodes)
            {
                //Handles.DrawBezier(
                //    connectPointOut.transform.position,
                //    item.connectPointIn.transform.position,
                //    connectPointOut.transform.position + (Vector3)Vector2.left * 50f,
                //    item.connectPointIn.transform.position - (Vector3)Vector2.left * 50f,
                //    Color.white,
                //    null,
                //    2f
                //);
                Handles.DrawLine(
                   connectPointOut.transform.position,
                   item.connectPointIn.transform.position
                );

                Debug.Log($"{connectPointOut.transform.position}:{item.connectPointIn.transform.position}");
                
                GUI.changed = true;
            }
        }
    }
}