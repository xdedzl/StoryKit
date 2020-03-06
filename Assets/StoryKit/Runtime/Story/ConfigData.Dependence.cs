using Newtonsoft.Json;
using XFramework.StoryKit;
using XFramework.UI;

/// <summary>
/// 职业
/// </summary>
public enum Profession
{
    /// <summary>
    /// 普通人
    /// </summary>
    Normal,
    /// <summary>
    /// 政府人员
    /// </summary>
    Government,
    /// <summary>
    /// 医生
    /// </summary>
    Doctor,
    /// <summary>
    /// 教师
    /// </summary>
    Teacher,
}

/// <summary>
/// 标签
/// </summary>
[System.Flags]
public enum Tag
{
    /// <summary>
    /// 甜食
    /// </summary>
    Sweet = 0b_0000_0001,
    /// <summary>
    /// 含酒精
    /// </summary>
    Alcohol = 0b_0000_0010,
    /// <summary>
    /// 含尼古丁
    /// </summary>
    Nicotine = 0b_0000_0100,
}

/// <summary>
/// 商品类型
/// </summary>
public enum GoodType
{
    Drink,     // 饮料
    Food,      // 食物
    Cigarette  // 香烟
}

/// <summary>
/// 场景
/// </summary>
public enum Situation
{
    /// <summary>
    /// 到来
    /// </summary>
    Enter,
    /// <summary>
    /// 交易正确
    /// </summary>
    Correct,
    /// <summary>
    /// 找零错误
    /// </summary>
    CoinWrong,
    /// <summary>
    /// 出货错误
    /// </summary>
    GoodsWrong,
}

/// <summary>
/// 可配置数据接口
/// </summary>
public interface IConfigData<T>
{
    /// <summary>
    /// 编号
    /// </summary>
    T Key { get; }
}

public class ConfigData : IConfigData<int>
{
    /// <summary>
    /// 编号
    /// </summary>
    public int id;

    [JsonIgnore]
    public int Key => id;
}

/// <summary>
/// 剧情数据基类
/// </summary>
public class StoryDataBase
{
    /// <summary>
    /// 编号
    /// </summary>
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
    [CustomerElement(typeof(StorySelectElement))]
    public int[] triggers;
    /// <summary>
    /// 跳转id
    /// </summary>
    [ElementIngore]
    public int[] jumpId;
}