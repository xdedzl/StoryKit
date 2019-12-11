using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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

    private void OnEnable()
    {
        var root = rootVisualElement;

        var m_DropArea = new VisualElement();
        m_DropArea.AddToClassList("droparea");
        m_DropArea.Add(new Label { text = "Drag and drop anything here" });
        root.Add(m_DropArea);

        m_DropArea.RegisterCallback<MouseEnterEvent>((a)=> { Debug.Log("Enter"); });
        m_DropArea.RegisterCallback<MouseDownEvent>((a) => { Debug.Log("Down"); });
        m_DropArea.RegisterCallback<MouseUpEvent>((a) => { Debug.Log("Up"); });
        m_DropArea.RegisterCallback<MouseLeaveEvent>((a) => { Debug.Log("Level"); });

        IMGUIContainer imgui = new IMGUIContainer(() =>
        {
            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

        });
        root.Add(imgui);
    }

    private void OnGUI()
    {
        
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