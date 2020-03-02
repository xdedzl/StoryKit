using Newtonsoft.Json;
using XFramework.UI;
/// <summary>
/// 剧情数据基类
/// </summary>
public abstract class StoryDataBase
{
    [ElementIngore]
    public int id;
    public string name;
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
    [ElementIngore]
    public int[] jumpId;
}