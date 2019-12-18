using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace XFramewrok.StoryKit
{
    public class Node : VisualElement
    {
        // 用UXML
        public class Fatory : UxmlFactory<Node> { }


        private NodeData m_NodeData;

        private bool isMove;
        private VisualElement m_ContentUI;

        public Node() { }

        public Node(NodeData nodeData) : this()
        {
            m_NodeData = nodeData;

            VisualElement connectPointIn = new VisualElement();
            connectPointIn.AddToClassList("connectPoint");
            VisualElement connectPointOut = new VisualElement();
            connectPointOut.AddToClassList("connectPoint");
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
                this.BringToFront();
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
                if (v.clickCount == 2)
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
            });

            m_ContentUI = new VisualElement();
            m_ContentUI.AddToClassList("content");

            IntegerField idField = new IntegerField
            {
                value = m_NodeData.id,
            };
            idField.RegisterValueChangedCallback((v) => { m_NodeData.id = v.newValue; });
            idField.AddToClassList("item");
            TextField nameField = new TextField
            {
                value = m_NodeData.name,
            };

            nameField.RegisterValueChangedCallback((v) => { m_NodeData.name = v.newValue; });
            nameField.AddToClassList("item");
             
            m_ContentUI.Add(idField);
            m_ContentUI.Add(nameField);

            Add(title);
            Add(m_ContentUI);
        }
    }
}