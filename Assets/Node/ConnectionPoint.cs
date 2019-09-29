using System;
using UnityEngine;

namespace XFramewrok.Editor
{
    public enum ConnectionPointType { In, Out }

    /// <summary>
    /// 节点连接点
    /// </summary>
    public class ConnectionPoint
    {
        public Rect rect;
        /// <summary>
        /// 连接点类型
        /// </summary>
        public ConnectionPointType type;

        public Node node;

        private readonly GUIStyle style;

        public Action<ConnectionPoint> OnClickConnectionPoint;

        public ConnectionPoint(Node node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint)
        {
            this.node = node;
            this.type = type;
            this.style = style;
            this.OnClickConnectionPoint = OnClickConnectionPoint;
            rect = new Rect(0, 0, 10f, 20f);
        }

        public void Draw()
        {
            rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;

            switch (type)
            {
                case ConnectionPointType.In:
                    rect.x = node.rect.x - rect.width + 8f;
                    break;

                case ConnectionPointType.Out:
                    rect.x = node.rect.x + node.rect.width - 8f;
                    break;
            }

            if (GUI.Button(rect, "", style))
            {
                OnClickConnectionPoint?.Invoke(this);
            }
        }
    }
}