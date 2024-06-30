using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace bittorrent.Tests
{
    //todo: remove this
    public class UtilTests
    {
        [Fact]
        public void FormatDictionary()
        {
            var dictionary = new Dictionary<string, object>();

            dictionary["Z"] = "value1";
            dictionary["1"] = 42;
            dictionary["A"] = "word";

            var actualResult = Util.FormatDictionary(dictionary);

            Assert.Equal("{\"1\":42,\"A\":\"word\",\"Z\":\"value1\"}", actualResult);
        }

        [Fact]
        public void FormatDictionary_Recursion()
        {
            var dictionary = new Dictionary<string, object>();

            dictionary["z"] = "value1";
            dictionary["1"] = 42;

            var innerDictonary = new Dictionary<string, object>();
            innerDictonary["1"] = "value21";
            innerDictonary["12"] = 4242;
            innerDictonary["x"] = 100;

            dictionary["b"] = innerDictonary;

            var x = JsonSerializer.Serialize(dictionary);

            var actualResult = Util.FormatDictionary(dictionary);

            Assert.Equal("{\"1\":42,\"b\":{\"1\":\"value21\",\"12\":4242,\"x\":100},\"z\":\"value1\"}", actualResult);
        }
    }
}
