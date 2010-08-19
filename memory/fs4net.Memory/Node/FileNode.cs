using System.IO;

namespace fs4net.Memory.Node
{
    internal class FileNode : FileSystemNode
    {
        private readonly NonDisposingStream _content = new NonDisposingStream(new MemoryStream());

        public FileNode(FolderNode parent, string name)
            : base(parent, name)
        {
        }

        public override void Dispose()
        {
            _content.DisposeForReal();
        }

        internal Stream CreateReadStream()
        {
            _content.Seek(0, SeekOrigin.Begin);
            return _content;
        }

        internal Stream CreateWriteStream()
        {
            _content.Seek(0, SeekOrigin.Begin);
            return _content;
        }


        // Note: This class is really scary and should not be used in production code.
        private class NonDisposingStream : Stream
        {
            private readonly Stream _inner;

            public NonDisposingStream(Stream inner)
            {
                _inner = inner;
            }

            public void DisposeForReal()
            {
                _inner.Dispose();
            }

            protected override void Dispose(bool disposing)
            {
                // Do nothing... that's the charm!
            }

            public override void Flush() { _inner.Flush(); }
            public override long Seek(long offset, SeekOrigin origin) { return _inner.Seek(offset, origin); }
            public override void SetLength(long value) { _inner.SetLength(value); }
            public override int Read(byte[] buffer, int offset, int count) { return _inner.Read(buffer, offset, count); }
            public override void Write(byte[] buffer, int offset, int count) { _inner.Write(buffer, offset, count); }
            public override bool CanRead { get { return _inner.CanRead; } }
            public override bool CanSeek { get { return _inner.CanSeek; } }
            public override bool CanWrite { get { return _inner.CanWrite; } }
            public override long Length { get { return _inner.Length; } }
            public override long Position { get { return _inner.Position; } set { _inner.Position = value; } }
        }
    }
}