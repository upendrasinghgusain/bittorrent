using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bittorrent
{
    public class Beencode
    {
        public string Decode(string encodedValue)
        {
            if (char.IsDigit(encodedValue[0]))
            {
                return DecodeString(encodedValue);
            }
            else if (encodedValue[0] == 'i' && encodedValue[encodedValue.Length - 1] == 'e')
            {
                return DecodeNumber(encodedValue);
            }
            else if (encodedValue[0] == 'l' && encodedValue[encodedValue.Length - 1] == 'e')
            {
                return DecodeList(encodedValue);
            }
            else if (encodedValue[0] == 'd' && encodedValue[encodedValue.Length - 1] == 'e')
            {
                return DecodeDictionary(encodedValue);
            }

            throw new InvalidOperationException("Unhandled encoded value: " + encodedValue);
        }

        private string DecodeNumber(string encodedValue)
        {
            // Example: i42e => 42

            return GetNumber(encodedValue.Substring(1), 'e');
        }

        private string DecodeString(string encodedValue)
        {
            // Example: "5:hello" -> "hello"
            // Example: "13:asdfertgyhuji" -> "asdfertgyhuji"

            //get the number first as could be a loong one
            var number = GetNumber(encodedValue, ':');
            var length = int.Parse(number);

            return encodedValue.Substring(number.Length + 1, length);
        }

        private string GetNumber(string encodedValue, char lastChar)
        {
            string strNumber = "";

            for (int i = 0; i < encodedValue.Length; i++)
            {
                if (!char.IsDigit(encodedValue[i]))
                {
                    if (encodedValue[i] != lastChar)
                    {
                        throw new InvalidOperationException("Invalid encoded value: " + encodedValue);
                    }
                    break;
                }

                strNumber = strNumber + encodedValue[i];
            }

            return strNumber;
        }

        private string DecodeList(string encodedValue)
        {
            // l5:helloi52ee => ["Hello",52]

            /*
             * loop
             * read first character
             * route to respective decoder
             */

            string result = "[";

            for (int i = 1; i <= encodedValue.Length - 2; i++)
            {
                char c = encodedValue[i];
                if (char.IsDigit(c))
                {
                    var decodedString = DecodeString(encodedValue.Substring(i));
                    result = result + "\"" + decodedString + "\",";
                    i = i + decodedString.Length + (decodedString.Length.ToString().Length);
                }
                else if (c == 'i')
                {
                    var decodedNumber = DecodeNumber(encodedValue.Substring(i));
                    result = result + decodedNumber + ",";
                    i = i + decodedNumber.Length + 1;
                }
                else
                {
                    throw new InvalidOperationException("Invalid encoded value in List: " + encodedValue);
                }
            }

            return result.Substring(0, result.Length - 1) + "]";
        }

        private string DecodeDictionary(string encodedValue)
        {
            /*
             * Example: d3:foo3:bar5:helloi52ee => {"foo":"bar","hello":52}
             */

            var dict = new Dictionary<string, string>();
            for (int i = 1; i <= encodedValue.Length - 2; i++)
            {
                var key = DecodeString(encodedValue.Substring(i));
                i = i + key.Length + (key.Length.ToString().Length) + 1;

                string value = "";
                if (char.IsDigit(encodedValue[i]))
                {
                    value = DecodeString(encodedValue.Substring(i));
                    i = i + value.Length + (value.Length.ToString().Length);
                }
                else if (encodedValue[i] == 'i')
                {
                    value = DecodeNumber(encodedValue.Substring(i));
                    i = i + value.Length + 1;
                }
                else
                {
                    throw new InvalidOperationException("Invalid encoded value in Dictionary: " + encodedValue);
                }

                dict.Add(key, value);
            }

            string result = "{";
            foreach (var item in dict.OrderBy(x => x.Key))
            {
                result = result + "\"" + item.Key;
                int val = 0;
                if(int.TryParse(item.Value, out val))
                {
                    result = result + "\":" + val + ",";
                }
                else
                {
                    result = result + "\":\"" + item.Value + "\",";
                }
                
            };

            return result.Substring(0, result.Length - 1) + "}";

        }
    }
}
