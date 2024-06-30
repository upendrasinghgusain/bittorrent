using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bittorrent.Tests
{
    public class InfoTestsB
    {
        [Fact]
        public void InfoTest()
        {
            var projectDirectory = Directory.GetCurrentDirectory();
            var filepath = Path.Combine(projectDirectory, "TestFiles", "sample.torrent");

            var result = new CommandProcessor().Process(["info", filepath]);
            Assert.NotNull(result);
            //got to figure out recursive dictionray
        }
    }
}
