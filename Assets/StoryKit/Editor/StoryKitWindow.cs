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
            window.minSize = new Vector2(450, 514);
            window.titleContent = new GUIContent("StoryKit");
        }
    }
}