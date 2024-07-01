using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace bittorrent
{
    public class Beencode
    {
        public string Decode(string encodedValue)
        {
            if (char.IsDigit(encodedValue[0]))
            {
                return DecodeString(encodedValue).decodedValue;
            }
            else if (encodedValue[0] == 'i' && encodedValue[encodedValue.Length - 1] == 'e')
            {
                return JsonSerializer.Serialize(DecodeNumber(encodedValue).decodedValue);
            }
            else if (encodedValue[0] == 'l' && encodedValue[encodedValue.Length - 1] == 'e')
            {
                return JsonSerializer.Serialize(DecodeList(encodedValue).decodedValue);
            }
            else if (encodedValue[0] == 'd' && encodedValue[encodedValue.Length - 1] == 'e')
            {
                return JsonSerializer.Serialize(DecodeDictionary(encodedValue).dictionary);
            }

            throw new InvalidOperationException("Unhandled encoded value: " + encodedValue);
        }

        private (int decodedValue, int position) DecodeNumber(string encodedValue, int currentPosition = 0)
        {
            // Example: i42e => 42

            var strNumber = GetNumber(encodedValue.Substring(1));
            return (int.Parse(strNumber), currentPosition + strNumber.Length + 1);
        }

        private (string decodedValue, int position) DecodeString(string encodedValue, int currentPosition = 0)
        {
            // Example: "5:hello" -> "hello"
            // Example: "13:asdfertgyhuji" -> "asdfertgyhuji"

            //get the number first as could be a loong one
            var strNumber = GetNumber(encodedValue);
            var length = int.Parse(strNumber);

            return (encodedValue.Substring(strNumber.Length + 1, length), currentPosition + strNumber.Length + length);
        }

        private string GetNumber(string encodedValue)
        {
            int i = 0;
            string strNumber = "";

            if (encodedValue[0] == '-')
            {
                strNumber = "-";
                i++;
            }

            for (; i < encodedValue.Length; i++)
            {
                if (!char.IsDigit(encodedValue[i]))
                {
                    if (encodedValue[i] is not 'e' and not ':')
                    {
                        throw new InvalidOperationException("Invalid encoded value: " + encodedValue);
                    }

                    break;
                }

                strNumber = strNumber + encodedValue[i];
            }

            return strNumber;
        }

        private (List<object> decodedValue, int position) DecodeList(string encodedValue, int currentPosition = 0)
        {
            // l5:helloi52ee => ["Hello",52]

            int i = currentPosition + 1;
            var list = new List<object>();
            object value = null;

            for (; i <= encodedValue.Length - 2; i++)
            {
                if (encodedValue[i] == 'e')// end of list
                    break;

                if (char.IsDigit(encodedValue[i]))
                {
                    (value, i) = DecodeString(encodedValue.Substring(i), i);
                }
                else if (encodedValue[i] == 'i')
                {
                    (value, i) = DecodeNumber(encodedValue.Substring(i), i);
                }
                else if (encodedValue[0] == 'l')
                {
                    (value, i) = DecodeList(encodedValue, i);
                }
                else
                {
                    throw new InvalidOperationException("Invalid encoded value in Dictionary: " + encodedValue);
                }

                list.Add(value);
            }

            return (list, i);
        }

        //todo:  return result and current position i the encoded string
        //todo: make a class for bittorrent file contents
        private (Dictionary<string, object> dictionary, int position) DecodeDictionary(string encodedValue, int currentPosition = 0)
        {
            /*
             * Example: d3:foo3:bar5:helloi52ee => {"foo":"bar","hello":52}
             */
            int i = currentPosition + 1;
            var dict = new Dictionary<string, object>();

            for (; i <= encodedValue.Length - 2; i++)
            {
                if (encodedValue[i] == 'e')// end of dictionary
                    break;

                var (key, position) = DecodeString(encodedValue.Substring(i), i);
                i = position + 1;

                object value = null;
                if (char.IsDigit(encodedValue[i]))
                {
                    (value, position) = DecodeString(encodedValue.Substring(i), i);
                }
                else if (encodedValue[i] == 'i')
                {
                    (value, position) = DecodeNumber(encodedValue.Substring(i), i);
                }
                else if (encodedValue[0] == 'd')
                {
                    (value, position) = DecodeDictionary(encodedValue, i);
                }
                else
                {
                    throw new InvalidOperationException("Invalid encoded value in Dictionary: " + encodedValue);
                }

                dict.Add(key, value);
                i = position;
            }

            return (dict.OrderBy(x => x.Key).ToDictionary(), i);
        }
    }
}
