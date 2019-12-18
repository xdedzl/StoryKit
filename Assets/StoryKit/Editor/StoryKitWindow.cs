using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace XFramewrok.StoryKit
{
    public class StoryKitWindow : EditorWindow
    {
        [MenuItem("XFramework/StoryKit")]
        public static void ShowExample()
        {
            StoryKitWindow window = GetWindow<StoryKitWindow>();
            window.minSize = new Vector2(450, 514);
            window.titleContent = new GUIContent("StoryKit");
        }

        private Vector2 offset;
        private Vector2 drag;

        private VisualElement m_NodeRoot;

        private StyleSheet nodeStyle;
        private void OnEnable()
        {
            nodeStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/StoryKit/Editor/Node/Node.uss"); 

            var root = rootVisualElement;
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/StoryKit/Editor/StoryKitWindow.uss"));
            m_NodeRoot = new VisualElement
            {
                style =
                {
                    
                }
            };
            root.Add(m_NodeRoot);

            IMGUIContainer imgui = new IMGUIContainer(() =>
            {
                DrawGrid(20, 0.2f, Color.gray);
                DrawGrid(100, 0.4f, Color.gray);

                ProcessEvents(Event.current);
            });
            root.Add(imgui);
        }

        private void ProcessEvents(Event e)
        {
            drag = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        //ClearConnectionSelection();
                    }

                    if (e.button == 1)
                    {
                        ProcessContextMenu(e.mousePosition);
                    }
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0)
                    {
                        //OnDrag(e.delta);
                    }
                    break;
            }
        }

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add node"), false, () =>
            {
                NodeData nodeData = new NodeData() { id = 1001, name = "傻逼" };
                Node node = new Node(nodeData)
                {
                    transform =
                    {
                        position = mousePosition
                    },
                };
                node.styleSheets.Add(nodeStyle);
                rootVisualElement.Add(node);
            });
            genericMenu.ShowAsContext();
        }

        /// <summary>
        /// 绘制面板网格
        /// </summary>
        /// <param name="gridSpacing">间隔</param>
        /// <param name="gridOpacity">透明度</param>
        /// <param name="gridColor">颜色</param>
        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            offset += drag * 0.5f;
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }
    }
}