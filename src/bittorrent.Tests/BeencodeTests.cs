namespace bittorrent.Tests
{
    public class BeencodeTests
    {
        [Theory]
        [InlineData("i42e", "42")]
        [InlineData("i1234567e", "1234567")]
        [InlineData("i-42e", "-42")]
        [InlineData("i0e", "0")]
        public void Decode_Number(string encodedString, string expectedResult)
        {
            var actualResult = new Beencode().Decode(encodedString);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("1:a", "a")]
        [InlineData("3:123", "123")]
        [InlineData("26:HelloMyNameIsUpendraGusain", "HelloMyNameIsUpendraGusain")]
        public void Decode_String(string encodedString, string expectedResult)
        {
            var actualResult = new Beencode().Decode(encodedString);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("l5:helloi52ee", "[\"hello\",52]")]
        [InlineData("li123e7:upendrae", "[123,\"upendra\"]")]
        [InlineData("li1ei23ei456ee", "[1,23,456]")]
        [InlineData("l5:ethan26:HelloMyNameIsUpendraGusain1:Ie", "[\"ethan\",\"HelloMyNameIsUpendraGusain\",\"I\"]")]
        public void Decode_List(string encodedString, string expectedResult)
        {
            var actualResult = new Beencode().Decode(encodedString);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("l6:mondayl4:worki42ee7:tuesdayl4:homeee", "[\"monday\",[\"work\",42],\"tuesday\",[\"home\"]]")]
        public void Decode_List_Recursion(string encodedString, string expectedResult)
        {
            var actualResult = new Beencode().Decode(encodedString);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("d3:foo3:bar5:helloi52ee", "{\"foo\":\"bar\",\"hello\":52}")]
        [InlineData("d5:helloi52e3:foo3:bare", "{\"foo\":\"bar\",\"hello\":52}")]
        [InlineData("d3:bar4:spam3:fooi42ee", "{\"bar\":\"spam\",\"foo\":42}")]
        public void Decode_Dictionary_Simple(string encodedString, string expectedResult)
        {
            var actualResult = new Beencode().Decode(encodedString);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("d3:Zatd4:2keyi42e4:1key5:hello4:1234i100ee4:keyx3:ha!e", "{\"keyx\":\"ha!\",\"Zat\":{\"1234\":100,\"1key\":\"hello\",\"2key\":42}}")]
        //[InlineData("d4:key33:ha!4:key2di1234ei100e4:1key5:hello4:2keyi42e10:innerinnerd1:v2:a11:b5:hello1:1i42eee4:key1d2:1xi100e4:1key5:hello3:11xi42eeee",
        //    "{\"key3\":\"ha!\",\"key2\":{\"1234\":100,\"1key\":\"hello\",\"2key\":42,\"innerinner\":{\"v\":\"a1\",\"b\":\"hello\",\"1\":42}},\"key1\":{\"1x\":100,\"1key\":\"hello\",\"11x\":42}}")]
        public void Decode_Dictionary_Recursion(string encodedString, string expectedResult)
        {
            var actualResult = new Beencode().Decode(encodedString);

            Assert.Equal(expectedResult, actualResult);
        }

        //todo: Dictionary with list in it
        //todo: List with Dictionary in it

        [Fact]
        public void Test()
        {
            var x = new Dictionary<string, List<string>>();
            var y = new List<Dictionary<string, string>>();
        }
    }
}