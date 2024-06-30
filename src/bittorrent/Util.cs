using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bittorrent
{
    public static class Util
    {
        //todo: don't need this use json serializer
        public static string FormatDictionary(Dictionary<string, object> dictionary)
        {
            string result = "{";
            foreach (var item in dictionary.OrderBy(x => x.Key))
            {
                result = result + "\"" + item.Key;
                if (item.Value.GetType() == typeof(int))
                {
                    result = result + "\":" + item.Value + ",";
                }
                else if (item.Value.GetType() == typeof(Dictionary<string, object>))
                {
                    result = result + "\":" + FormatDictionary((Dictionary<string, object>)item.Value) + ",";
                }
                else
                {
                    result = result + "\":\"" + item.Value + "\",";
                }
            };

            return (result.Substring(0, result.Length - 1) + "}");
        }
    }
}
