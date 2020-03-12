using XFramework.UI;
/*
* 所有依赖剧情的可配置数据
*/
namespace Casket
{
    /// <summary>
    /// 剧情
    /// </summary>
    public class CommonStoryData : StoryDataBase
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string[] contents;
    }

    /// <summary>
    /// CG剧情
    /// </summary>
    public class CGStoryData : StoryDataBase
    {
        /// <summary>
        /// CG图集
        /// </summary>
        [ArrayCustomrElement(typeof(TexturePathElement))]
        public string[] images;
        /// <summary>
        /// 内容
        /// </summary>
        public string[] contents;
    }

    /// <summary>
    /// 客人对话数据
    /// </summary>
    public class CustomerDialogData : StoryDataBase
    {
        /// <summary>
        /// 顾客编号
        /// </summary>
        public int customerId;
        /// <summary>
        /// 来时对话id
        /// </summary>
        [ElementProperty("来时对话")]
        public int dialogId_c;
        /// <summary>
        /// 出货正确的对话
        /// </summary>
        [ElementProperty("出货正确的对话")]
        public int dialogId_gr;
        /// <summary>
        /// 出货错误的对话
        /// </summary>
        [ElementProperty("出货错误的对话")]
        public int dialogId_gw;
        /// <summary>
        /// 找钱正确的对话
        /// </summary>
        [ElementProperty("找钱正确的对话")]
        public int dialogId_cr;
        /// <summary>
        /// 找钱错误的对话
        /// </summary>
        [ElementProperty("找钱错误的对话")]
        public int dialogId_cw;
        /// <summary>
        /// 出货正确跳转
        /// </summary>
        public int jumpId_r;
        /// <summary>
        /// 出货错误跳转
        /// </summary>
        public int jumpId_w;
        /// <summary>
        /// 内容
        /// </summary>
        [ArrayCustomrElement(typeof(TextArea), 50)]
        public string[] contents;
    }

    /// <summary>
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
    }

    /// <summary>
    /// 进货数据
    /// </summary>
    public class StockData : StoryDataBase
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int date;
        /// <summary>
        /// 商品id
        /// </summary>
        public int[] goodsId;
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
}