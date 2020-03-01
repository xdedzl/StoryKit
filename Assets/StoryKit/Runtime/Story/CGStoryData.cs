using UnityEngine;
using XFramework.UI;
/// <summary>
/// CG剧情
/// </summary>
[System.Serializable]
public class CGStoryData : NodeData
{
    /// <summary>
    /// CG图集
    /// </summary>
    [InspectorElement(type =typeof(TexturePathElement))]
    public string image;
    /// <summary>
    /// 内容
    /// </summary>
    public string[] contents = new string[5];
}