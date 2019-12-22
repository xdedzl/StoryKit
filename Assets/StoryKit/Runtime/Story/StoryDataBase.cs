using Newtonsoft.Json;
/// <summary>
/// 剧情数据基类
/// </summary>
public class StoryDataBase : NodeData
{
    /// <summary>
    /// 前置条件id
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int[] conditions;
    /// <summary>
    /// 触发一组条件状态为true
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int[] triggers;
    /// <summary>
    /// 跳转id
    /// </summary>
    public int jumpId;
}