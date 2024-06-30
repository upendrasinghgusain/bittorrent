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
        public void EncodeTest(string encodedString, string expectedResult)
        {
            var actualResult = new Beencode().Decode(encodedString);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}