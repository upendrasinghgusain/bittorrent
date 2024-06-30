namespace bittorrent.Tests
{
    public class BeencodeTests
    {
        [Theory]
        [InlineData("i42e", "42")]
        [InlineData("i1234567e", "1234567")]
        [InlineData("26:HelloMyNameIsUpendraGusain", "HelloMyNameIsUpendraGusain")]
        [InlineData("l5:helloi52ee", "[\"hello\",52]")]
        [InlineData("li123e7:upendrae", "[123,\"upendra\"]")]
        [InlineData("li1ei23ei456ee", "[1,23,456]")]
        [InlineData("l5:ethan26:HelloMyNameIsUpendraGusain1:Ie", "[\"ethan\",\"HelloMyNameIsUpendraGusain\",\"I\"]")]
        [InlineData("d3:foo3:bar5:helloi52ee", "{\"foo\":\"bar\",\"hello\":52}")]
        [InlineData("d5:helloi52e3:foo3:bare", "{\"foo\":\"bar\",\"hello\":52}")]
        [InlineData("d3:bar4:spam3:fooi42ee", "{\"bar\":\"spam\",\"foo\":42}")]
        public void Encode(string encodedString, string expectedResult)
        {
            var actualResult = new Beencode().Decode(encodedString);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("d3:keyd4:key1i42e4:key25:hello4:key3i100ee4:keyx3:ha!e", "{\"key\":{\"key1\":42,\"key2\":\"hello\",\"key3\",100},\"keyx\":\"ha!\"}")]
        public void Encode_Dictionary_Recursion(string encodedString, string expectedResult)
        {
            var actualResult = new Beencode().Decode(encodedString);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}