using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFramewrok.StoryKit
{
    public class NodeManager
    {
        #region 单例

        public static NodeManager m_instance;

        public static NodeManager instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new NodeManager();
                return m_instance;
            }
        }

        #endregion

        #region ID

        private List<int> m_IdPools;
        private int m_Id;
        private int Id { get { return m_Id++; } }

        #endregion

        private Dictionary<int, NodeData> m_NodeDataDic;

        private NodeManager()
        {
            Debug.Log("...");
            m_IdPools = new List<int>();

            m_NodeDataDic = new Dictionary<int, NodeData>(); 
        }

        private NodeData CreateNodeInternal()
        {
            NodeData nodeData = new NodeData
            {
                id = instance.Id,
                name = "新节点",
            };

            m_NodeDataDic.Add(nodeData.id, nodeData);

            return nodeData;
        }


        #region Static 调用

        /// <summary>
        /// 创建一个节点
        /// </summary>
        /// <returns></returns>
        public static NodeData CreateNodeData()
        {
            return instance.CreateNodeInternal();
        }

        /// <summary>
        /// 连接节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nextNode"></param>
        public static void Connect(NodeData node, NodeData nextNode)
        {
            if (node.nextNodes == null)
            {
                node.nextNodes = new List<int>();
            }

            node.nextNodes.Add(nextNode.id);
        }

        /// <summary>
        /// 接初节点连接
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nextNode"></param>
        public static void Disconnect(NodeData node, NodeData nextNode)
        {
            if (node.nextNodes == null || !node.nextNodes.Remove(nextNode.id))
            {
                throw new System.Exception("两个节点没有连接");
            }
        }

        #endregion
    }
}