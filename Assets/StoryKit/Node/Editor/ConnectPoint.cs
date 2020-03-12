using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XFramework.StoryKit
{
    public enum Point
    {
        In,
        Out
    }

    public class ConnectPoint<T> : VisualElement
    {
        public static ConnectPoint<T> StartPoint { get; private set; }

        public Point point;
        public Node<T> node;

        public ConnectPoint(Node<T> node, Point point)
        {
            this.node = node;
            this.point = point;

            RegisterCallback<MouseDownEvent>((e) =>
            {
                if(StartPoint != null)  // 被连接点
                {
                    if(StartPoint.node != this.node && StartPoint.point != this.point)
                    {
                        if(this.point == Point.In)
                        {
                            StartPoint.node.AddNextNode(this.node);
                        }
                        else
                        {
                            this.node.AddNextNode(StartPoint.node);
                        }
                    }
                    ClearLine();
                }
                else  // 连接点
                {
                    StartPoint = this;
                    var drawLine = new IMGUIContainer(() =>
                    {
                        Vector2 p1 = this.transform.position;
                        Vector2 p2 = UnityEngine.Event.current.mousePosition;
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
                    })
                    {
                        transform =
                        {
                            position = new Vector2(5,5),
                        }
                    };
                    Add(drawLine);
                }
            });
        }

        public static void ClearLine()
        {
            if (StartPoint != null)
            {
                StartPoint.RemoveAt(StartPoint.childCount - 1);
            }

            StartPoint = null;
        }
    }
}