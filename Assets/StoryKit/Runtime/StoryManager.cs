using System;
using System.Collections.Generic;
using System.Linq;

namespace XFramework.StoryKit
{
    public class StoryManager
    {
        #region 单例

        public static StoryManager m_instance;

        public static StoryManager instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new StoryManager();
                return m_instance;
            }
        }

        #endregion

        private List<StoryDataBase> m_storyList = new List<StoryDataBase>();

        private StoryManager()
        {

        }

        public StoryDataBase[] GetStorys(Type type)
        {
            var quary = from story in m_storyList
                        where type == story.GetType()
                        select story;

            return quary.ToArray();
        }

        #region Static 调用

        #endregion
    }
}