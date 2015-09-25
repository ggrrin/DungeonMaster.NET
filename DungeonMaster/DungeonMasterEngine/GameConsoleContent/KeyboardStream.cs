using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace DungeonMasterEngine.GameConsoleContent
{
    public class KeyboardStream : Stream
    {
        private MemoryStream stream;
        private StreamWriter inputWriter;

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => stream.Length;

        public override long Position { get; set; }
        public bool disposed { get; private set; }

        public KeyboardStream()
        {
            inputWriter = new StreamWriter(stream = new MemoryStream());
        }

        public override void Flush()
        {
            lock(streamLock) inputWriter.Flush();
        }

        object streamLock = new object();

        public override int Read(byte[] buffer, int offset, int count)
        {
            int readCount = Math.Min(WaitForData().Result, count);//blocking

            if (readCount > 0)
                lock (streamLock)
                    Array.Copy(stream.GetBuffer(), Position, buffer, offset, readCount);

            Position += readCount;
            return readCount;
        }

        TaskCompletionSource<bool> inputAvalible = new TaskCompletionSource<bool>();

        public void WriteLineToInput(string line)
        {
            lock (streamLock)
            {
                inputWriter.WriteLine(line);
                inputWriter.Flush();
            }

            if (line.Length > 0)
                inputAvalible.TrySetResult(true);
        }

        private async Task<int> WaitForData()
        {
            if (await inputAvalible.Task)
            {
                inputAvalible = new TaskCompletionSource<bool>();

                lock (streamLock)
                    return (int)Math.Min(int.MaxValue, stream.Length - Position);
            }
            else
                return 0;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                disposed = true;
                inputAvalible.TrySetResult(false);
                stream.Dispose();
            }
        }
    }
}