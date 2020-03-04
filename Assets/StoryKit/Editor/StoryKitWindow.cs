using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

        private int id = 10000;

        protected override void OnDataInstantiat(StoryDataBase data)
        {
            data.id = id++;
        }

        protected override void OnSave(Node<StoryDataBase> node)
        {
            base.OnSave(node);

            var nextNodes = node.GetNextNodes();
            var data = node.data;
            data.jumpId = new int[nextNodes.Length];

            for (int i = 0; i < nextNodes.Length; i++)
            {
                data.jumpId[0] = (nextNodes[i].data).id;
            }
        }

        protected override void OnOpen(Node<StoryDataBase>[] nodes)
        {
            base.OnOpen(nodes);

            Dictionary<int, Node<StoryDataBase>> dataDic = new Dictionary<int, Node<StoryDataBase>>();
            foreach (var item in nodes)
            {
                dataDic.Add(item.data.id, item);
            }

            foreach (var item in nodes)
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