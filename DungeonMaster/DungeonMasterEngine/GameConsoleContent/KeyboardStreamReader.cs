using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DungeonMasterEngine.GameConsoleContent
{
    public class KeyboardStreamReader : TextReader
    {
        public KeyboardStream Stream { get; }
        public KeyboardStreamReader(KeyboardStream stream)
        {
            Stream = stream;
        }

        readonly byte[] buffer = new byte[1024];

        private string notFinishedLine = "";
        private readonly Queue<string> pendingLines = new Queue<string>();

        public override async Task<string> ReadLineAsync()
        {
            if (pendingLines.Any())
                return pendingLines.Dequeue();

            int readCount = 0;
            while ((readCount = await Stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                string readString = new string(buffer.Take(readCount).Select(x => (char)x).ToArray());
                var newLines = readString.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                if (newLines.Length == 1) //no new line found 
                {
                    notFinishedLine += newLines[0];

                    //for case NewLine is spited between \r and \n
                    var segments = notFinishedLine.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    if (segments.Length == 2)
                    {
                        notFinishedLine = segments[1];
                        return segments[0];
                    }
                    else if (segments.Length != 1)
                    {
                        throw new InvalidOperationException("Not possible.");
                    }
                }
                else// at least one line found 
                {
                    newLines[0] = notFinishedLine + newLines[0];
                    notFinishedLine = "";

                    if (newLines.Last() != "") //last line is not finished
                    {
                        notFinishedLine = newLines.Last();
                    }

                    EnqueueRange(newLines.Take(newLines.Length - 1));

                    if (pendingLines.Any())
                        break;
                }
            }

            return pendingLines.Dequeue();
        }

        private void EnqueueRange(IEnumerable<string> newLines)
        {
            foreach (string line in newLines)
                pendingLines.Enqueue(line);
        }

        public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
        {
            throw new NotSupportedException();
        }

        public override string ReadToEnd()
        {
            throw new NotSupportedException();
        }

        public override int ReadBlock(char[] buffer, int index, int count)
        {
            throw new NotSupportedException();
        }

        public override Task<int> ReadAsync(char[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override int Read()
        {
            throw new NotSupportedException();
        }

        public override int Read(char[] buffer, int index, int count)
        {
            throw new NotSupportedException();
        }

        public override int Peek()
        {
            throw new NotSupportedException();
        }

        public override string ReadLine()
        {
            throw new NotSupportedException();
        }
    }
}