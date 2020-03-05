using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;
using System;

namespace XFramework.StoryKit
{
    public class StoryKitWindow : NodeKitWindow<StoryDataBase>
    {
        [MenuItem("XFramework/StoryKit")]
        public static void ShowExample()
        {
            var window = GetWindow<StoryKitWindow>();
            window.minSize = new Vector2(800, 400);
            window.titleContent = new GUIContent("StoryKit");
        }

        protected override void CreateData(Type type, out StoryDataBase data)
        {
            data = StoryManager.instance.CreateData(type);
        }

        public override void OnNodeDelete(StoryDataBase data)
        {
            StoryManager.instance.DeleteData(data);
        }

        protected override void OnSave(string path)
        {
            var serializableNodeDatas = new List<SerializableNodeData<StoryDataBase>>();
            foreach (var node in m_NodeList)
            {
                Node2StoryLink(node);
                serializableNodeDatas.Add(new SerializableNodeData<StoryDataBase>
                {
                    postion = node.transform.position,
                    data = node.data
                });
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(serializableNodeDatas);
            File.WriteAllText(path, json);
        }

        protected override void OnOpen(string path)
        {
            string json = File.ReadAllText(path);

            var datas = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializableNodeData<StoryDataBase>[]>(json);

            foreach (var item in datas)
            {
                StoryManager.instance.AddData(item.data);
                Node<StoryDataBase> node = CreateNode(item.data, item.postion);
            }

            Story2NodeLink();
        }

        /// <summary>
        /// 根据节点连接信息设置storydata的跳转
        /// </summary>
        /// <param name="node"></param>
        private void Node2StoryLink(Node<StoryDataBase> node)
        {
            var nextNodes = node.GetNextNodes();
            var data = node.data;
            data.jumpId = new int[nextNodes.Length];

            for (int i = 0; i < nextNodes.Length; i++)
            {
                data.jumpId[0] = (nextNodes[i].data).id;
            }
        }

        /// <summary>
        /// 根据storydata的跳转关系设置连接信息
        /// </summary>
        /// <param name="node"></param>
        private void Story2NodeLink()
        {
            Dictionary<int, Node<StoryDataBase>> dataDic = new Dictionary<int, Node<StoryDataBase>>();
            foreach (var item in m_NodeList)
            {
                dataDic.Add(item.data.id, item);
            }

            foreach (var item in m_NodeList)
            {
                var data = item.data;
                if (data.jumpId != null)
                {
                    foreach (var id in data.jumpId)
                    {
                        item.AddNextNode(dataDic[id]);
                    }
                }
            }
        }
    }
}