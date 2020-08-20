using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlumsailTest_API
{
    public class Helpers
    {
        /// <summary>
        /// convert list of Bson documents to list of dictionary(for better using in front-end)
        /// </summary>
        /// <param name="dotNetObjList"></param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> ConvertToCompactList(List<BsonDocument> docs)
        {
            List<object> res = new List<object>();
            foreach (var d in docs)
            {
                var test = d.Where(x => x.Name == "props").Select(x => x.Value).ToList();
                List<object> dotNetObjList1 = test.ConvertAll(BsonTypeMapper.MapToDotNetValue);
                res.AddRange(dotNetObjList1);
            }


            List<object> dotNetObjList = docs.ConvertAll(BsonTypeMapper.MapToDotNetValue);

            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            foreach (var d in res)
            {
                var obj = new Dictionary<string, string>();

                var ss = JsonSerializer.Serialize(d);
                JToken t = JArray.Parse(ss);//["props"];

                
                //пробегаем по пропсам и добавляем в словарь
                foreach (var g in t)
                {
                    //var ggg = g.ToDictionary();

                   // obj.Add(g.ToBsonDocument(), g.ToString());
                }
                result.Add(obj);
            }

            return result;
        }


    }
}
