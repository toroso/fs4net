using System.IO;

namespace fs4net.Memory.Node
{
    internal sealed class FileNode : FileSystemNode
    {
        private readonly MemoryStream _content;
        private bool _isOpen;

        public FileNode(FolderNode parent, string name)
            : base(parent, name)
        {
            _content = new MemoryStream();
        }

        private FileNode(FolderNode parent, string name, MemoryStream content)
            : base(parent, name)
        {
            _content = content;
        }

        public void CopyTo(FolderNode parentNode, string name)
        {
            new FileNode(parentNode, name, new MemoryStream(_content.GetBuffer(), 0, (int) _content.Length));
        }

        public override void Dispose()
        {
            _content.Dispose();
        }

        internal Stream CreateReadStream()
        {
            _isOpen = true;
            return WrappingStream.CreateReadStream(_content, this);
        }

        internal Stream CreateWriteStream()
        {
            _isOpen = true;
            return WrappingStream.CreateWriteStream(_content, this);
        }

        internal Stream CreateAppendStream()
        {
            _isOpen = true;
            return WrappingStream.CreateAppendStream(_content, this);
        }

        public Stream CreateModifyStream()
        {
            _isOpen = true;
            return WrappingStream.CreateModifyStream(_content, this);
        }

        public override void VerifyCanBeRemoved()
        {
            if (_isOpen) throw new IOException("The file is in use.");
        }

        private void NotifyClose()
        {
            _isOpen = false;
        }


        private class WrappingStream : Stream
        {
            private readonly Stream _inner;
            private readonly FileNode _creator;
            private readonly bool _canRead;
            private readonly bool _canWrite;
            private readonly bool _canSeek;

            private WrappingStream(Stream inner, FileNode creator, bool canRead, bool canWrite, bool canSeek, SeekOrigin initialStreamPosition)
            {
                _inner = inner;
                _creator = creator;
                _canRead = canRead;
                _canWrite = canWrite;
                _canSeek = canSeek;
                _inner.Seek(0, initialStreamPosition);
            }

            protected override void Dispose(bool disposing)
            {
                // Do nothing... we want the inner MemoryStream to remember it's content.
            }

            public override void Close()
            {
                base.Close();
                _creator.NotifyClose();
            }

            public override void Flush() { _inner.Flush(); }

            public override long Seek(long offset, SeekOrigin origin)
            {
                if (!CanSeek) throw new IOException("Unable seek backward to overwrite data that previously existed in a file opened in Append mode.");
                return _inner.Seek(offset, origin);
            }

            public override void SetLength(long value) { _inner.SetLength(value); }
            public override int Read(byte[] buffer, int offset, int count) { return _inner.Read(buffer, offset, count); }
            public override void Write(byte[] buffer, int offset, int count) { _inner.Write(buffer, offset, count); }
            public override bool CanRead { get { return _canRead; } }
            public override bool CanSeek { get { return _canSeek; } }
            public override bool CanWrite { get { return _canWrite; } }
            public override long Length { get { return _inner.Length; } }
            public override long Position { get { return _inner.Position; } set { _inner.Position = value; } }

            public static Stream CreateReadStream(Stream inner, FileNode creator)
            {
                return new WrappingStream(inner, creator, true, false, true, SeekOrigin.Begin);
            }

            public static Stream CreateWriteStream(Stream inner, FileNode creator)
            {
                return new WrappingStream(inner, creator, false, true, true, SeekOrigin.Begin);
            }

            public static Stream CreateAppendStream(Stream inner, FileNode creator)
            {
                return new WrappingStream(inner, creator, false, true, false, SeekOrigin.End);
            }

            public static Stream CreateModifyStream(Stream inner, FileNode creator)
            {
                return new WrappingStream(inner, creator, true, true, true, SeekOrigin.Begin);
            }
        }
    }
}