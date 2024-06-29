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
        public void EncodeTest(string encodedString, string expectedResult)
        {
            var actualResult = new Beencode().Decode(encodedString);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}