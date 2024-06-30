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
                return DecodeNumber(encodedValue).decodedValue;
            }
            else if (encodedValue[0] == 'l' && encodedValue[encodedValue.Length - 1] == 'e')
            {
                return DecodeList(encodedValue);
            }
            else if (encodedValue[0] == 'd' && encodedValue[encodedValue.Length - 1] == 'e')
            {
                // todo: configure serializer to not enclose int in quotes
                return JsonSerializer.Serialize(DecodeDictionary(encodedValue).dictionary);
            }

            throw new InvalidOperationException("Unhandled encoded value: " + encodedValue);
        }

        private (string decodedValue, int position) DecodeNumber(string encodedValue, int currentPosition = 0)
        {
            // Example: i42e => 42

            var strNumber = GetNumber(encodedValue.Substring(1));
            return (strNumber, currentPosition + strNumber.Length + 1);
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
            string strNumber = "";

            for (int i = 0; i < encodedValue.Length; i++)
            {
                if (!char.IsDigit(encodedValue[i]))
                {
                    if (encodedValue[i] != 'e' || encodedValue[i] != ':')
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
                    var (decodedString, position) = DecodeString(encodedValue.Substring(i));
                    result = result + "\"" + decodedString + "\",";
                    //i = i + decodedString.Length + (decodedString.Length.ToString().Length);
                    i = position;
                }
                else if (c == 'i')
                {
                    var (decodedNumber, position) = DecodeNumber(encodedValue.Substring(i));
                    result = result + decodedNumber + ",";
                    //i = i + decodedNumber.Length + 1;
                    i = position;
                }
                else
                {
                    throw new InvalidOperationException("Invalid encoded value in List: " + encodedValue);
                }
            }

            return result.Substring(0, result.Length - 1) + "]";
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
