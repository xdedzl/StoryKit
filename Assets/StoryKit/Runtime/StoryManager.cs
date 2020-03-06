using System;
using System.Collections.Generic;
using System.Linq;

namespace XFramework.StoryKit
{
    public class StoryManager
    {
        #region 单例

        private static StoryManager m_instance;

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

        private int id = 10000;

        private int Id
        {
            get
            {
                return id++;
            }
        }

        private Dictionary<int, StoryDataBase> m_storyDic = new Dictionary<int, StoryDataBase>();

        private StoryManager()
        {

        }

        /// <summary>
        /// 根据类型创建一条剧情
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public StoryDataBase CreateData(Type type)
        {
            if (!type.IsSubclassOf(typeof(StoryDataBase)))
            {
                throw new Exception($"类型错误, {type.Name}不是StoryDataBase派生类");
            }
            StoryDataBase data = (StoryDataBase)Activator.CreateInstance(type);
            data.id = Id;

            while (m_storyDic.ContainsKey(data.id))
            {
                data.id = Id;
            }

            AddData(data);

            return data;
        }

        public void AddData(StoryDataBase data)
        {
            m_storyDic.Add(data.id, data);
        }

        public void DeleteData(StoryDataBase data)
        {
            if (!m_storyDic.Remove(data.id))
            {
                throw new Exception($"没有id为{data.id}的Story");
            }
        }

        public IEnumerable<StoryDataBase> GetStorys(Type type = null, string matching = null)
        {
            var quary = from story in m_storyDic.Values
                        where (type == null || type == story.GetType()) && (string.IsNullOrEmpty(matching) || story.name.Contains(matching))
                        select story;

            return quary;
        }

        public StoryDataBase GetStory(int id)
        {
            if(m_storyDic.TryGetValue(id, out StoryDataBase storyData))
            {
                return storyData;
            }

            throw new Exception($"没有id为{id}的Story");
        }

        #region Static 调用

        #endregion
    }
}