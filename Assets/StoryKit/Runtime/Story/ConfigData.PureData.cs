/*
* 所有不依赖剧情的可配置数据
*/
using System;
using System.Text;
using Newtonsoft.Json;

namespace Casket
{
    /// <summary>
    /// 人物信息
    /// </summary>
    public class NPCData : ConfigData
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string name;
        /// <summary>
        /// 工号
        /// </summary>
        public string jobNumber;
        /// <summary>
        /// 年龄
        /// </summary>
        public byte age;
        /// <summary>
        /// 电话号码
        /// </summary>
        public string phoneNumber;
        /// <summary>
        /// 职业
        /// </summary>
        public Profession profession;
        /// <summary>
        /// 过敏的商品标签
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public Tag irritabilityTags;
        /// <summary>
        /// 因职业而不能购买的商品
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public Tag tags;
    }

    /// <summary>
    /// 电话数据
    /// </summary>
    public class PhoneData : ConfigData
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// 号码
        /// </summary>
        public string number;

        public override string ToString()
        {
            return string.Format("Id:{0},Name:{1},Number:{2}", id, name, number);
        }
    }

    /// <summary>
    /// 商品信息
    /// </summary>
    public class GoodsData : ConfigData
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// 商品类型
        /// </summary>
        public GoodType type;
        /// <summary>
        /// 标签
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public Tag tags;
        /// <summary>
        /// 描述
        /// </summary>
        public string description;
        /// <summary>
        /// 商品效果图
        /// </summary>
        public string image;
        /// <summary>
        /// 价格
        /// </summary>
        public int price;
    }

    /// <summary>
    /// 每天的顾客数据
    /// </summary>
    public class CustomerData : ConfigData
    {
        public int day;
        public int npcId;
        public int goodsId;
        public int storyId;
    }

    /// <summary>
    /// 存在对话池中的对话数据
    /// </summary>
    public class DialogData : ConfigData
    {
        /// <summary>
        /// 对话内容
        /// </summary>
        public string[] contents;
        /// <summary>
        /// 使用场景
        /// </summary>
        public Situation situation;
    }

    /// <summary>
    /// json中使用int数组表示经过 | 运算之后的枚举
    /// </summary>
    public class EnumConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int[] enumArray = serializer.Deserialize<int[]>(reader);

            int value = 0;
            foreach (var item in enumArray)
            {
                value |= (int)Math.Pow(2, item);
            }

            return value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var enumArray = new System.Collections.Generic.List<int>();

            Type type = value.GetType();

            var values = Enum.GetValues(type);

            foreach (var item in values)
            {
                var intValue = (int)item;
                if (intValue != 0 && ((int)value & intValue) == intValue)
                {
                    int count = 0;
                    while (intValue != 0)
                    {
                        intValue = intValue >> 1;
                        count++;
                    }

                    enumArray.Add(count - 1);
                }
            }

            serializer.Serialize(writer, enumArray);
        }
    }

    public class DialogConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string str = serializer.Deserialize<string>(reader);
            string[] strArray = str.Split(new string[] { "\n" }, StringSplitOptions.None);

            return strArray;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string[] strArray = (string[])value;

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < strArray.Length - 1; i++)
            {
                stringBuilder.Append(strArray[i]);
                stringBuilder.Append("\n");
            }

            stringBuilder.Append(strArray[strArray.Length - 1]);

            serializer.Serialize(writer, stringBuilder.ToString());
        }
    }
}