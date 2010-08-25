using System.IO;

namespace fs4net.Memory.Node
{
    internal class FileNode : FileSystemNode
    {
        private readonly NonDisposingStream _content;
        private bool _isOpen;

        public FileNode(FolderNode parent, string name)
            : base(parent, name)
        {
            _content = new NonDisposingStream(new MemoryStream(), this);
        }

        public override void Dispose()
        {
            _content.DisposeForReal();
        }

        internal Stream CreateReadStream()
        {
            _isOpen = true;
            _content.Seek(0, SeekOrigin.Begin);
            return _content;
        }

        internal Stream CreateWriteStream()
        {
            _isOpen = true;
            _content.Seek(0, SeekOrigin.Begin);
            return _content;
        }

        public override void VerifyCanBeRemoved()
        {
            if (_isOpen) throw new IOException("The file is in use.");
        }

        private void NotifyClose()
        {
            _isOpen = false;
        }


        // Note: This class is really scary and should not be used in production code.
        private class NonDisposingStream : Stream
        {
            private readonly Stream _inner;
            private readonly FileNode _creator;

            public NonDisposingStream(Stream inner, FileNode creator)
            {
                _inner = inner;
                _creator = creator;
            }

            public void DisposeForReal()
            {
                _inner.Dispose();
            }

            protected override void Dispose(bool disposing)
            {
                // Do nothing... that's the charm!
            }

            public override void Close()
            {
                base.Close();
                _creator.NotifyClose();
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