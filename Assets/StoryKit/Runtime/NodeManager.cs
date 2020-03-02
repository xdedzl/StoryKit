using System;
using System.Collections.Generic;

namespace XFramework.StoryKit
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

        private NodeManager()
        {
            m_IdPools = new List<int>();
        }

        private NodeData CreateNodeInternal(Type type)
        {
            if (type.IsSubclassOf(typeof(NodeData)))
            {
                NodeData nodeData = Activator.CreateInstance(type) as NodeData;
                //nodeData.id = instance.Id;
                nodeData.name = "新节点";

                return nodeData;
            }
            else
            {
                throw new Exception("[NodeData Create Error] 类型错误");
            }
        }

        #region Static 调用

        public static NodeData CreateNodeData(Type type)
        {
            return instance.CreateNodeInternal(type);
        }

        #endregion
    }
}