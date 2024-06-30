using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bittorrent
{
    public class CommandProcessor
    {
        public string Process(string[] args)
        {
            var (command, param) = args.Length switch
            {
                0 => throw new InvalidOperationException("Usage: your_bittorrent.sh <command> <param>"),
                1 => throw new InvalidOperationException("Usage: your_bittorrent.sh <command> <param>"),
                _ => (args[0], args[1])
            };

            if (command == "decode")
            {
                return new Beencode().Decode(param);
            }
            else if (command == "info")
            {
                var bytes = new FileReader().Read(param);
                var content = System.Text.Encoding.UTF8.GetString(bytes);
                return new Beencode().Decode(content);
            }

            throw new InvalidOperationException($"Invalid command: {command}");
        }
    }
}
