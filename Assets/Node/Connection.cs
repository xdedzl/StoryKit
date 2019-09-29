using System;
using UnityEditor;
using UnityEngine;

namespace XFramewrok.Editor
{
    /// <summary>
    /// 节点连接
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// 入点
        /// </summary>
        public ConnectionPoint inPoint;
        /// <summary>
        /// 出点
        /// </summary>
        public ConnectionPoint outPoint;
        public Action<Connection> OnClickRemoveConnection;

        public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> OnClickRemoveConnection)
        {
            this.inPoint = inPoint;
            this.outPoint = outPoint;
            this.OnClickRemoveConnection = OnClickRemoveConnection;
        }

        /// <summary>
        /// 画连接线（使用贝塞尔曲线）
        /// </summary>
        public void Draw()
        {
            Handles.DrawBezier(
                inPoint.rect.center,
                outPoint.rect.center,
                inPoint.rect.center + Vector2.left * 50f,
                outPoint.rect.center - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.CircleHandleCap))
            {
                OnClickRemoveConnection?.Invoke(this);
            }
        }
    }
}