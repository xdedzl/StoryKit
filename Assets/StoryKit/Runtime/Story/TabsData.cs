﻿/// <summary>
/// 选项卡
/// </summary>
public class TabsData : StoryDataBase
{
    /// <summary>
    /// 标题
    /// </summary>
    public string title;
    /// <summary>
    /// 选项组
    /// </summary>
    public OptionInfo[] options;

    public NodeData data;
}

/// <summary>
/// 选项信息
/// </summary>
public struct OptionInfo
{
    /// <summary>
    /// 内容
    /// </summary>
    public string content;
    /// <summary>
    /// 跳转id
    /// </summary>
    public int jumpId;
    /// <summary>
    /// 触发
    /// </summary>
    public int[] triggers;
}