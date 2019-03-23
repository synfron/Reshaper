using System.IO;
using System.Threading.Tasks;

namespace ReshaperCore.Networking
{
	public class DataReader
	{
		private int _offset = 0;
		private byte[] _loadBuffer = new byte[BufferSize];
		private const int BufferSize = 400382;

		public Stream Stream { get; private set; }

		private int BufferEmptyLength
		{
			get
			{
				return BufferSize - this._offset;
			}
		}

		public DataReader(Stream stream)
		{
			Stream = new BufferedStream(stream);
		}

		public async Task<Buffer<byte>> ReadAsync()
		{
			if (this.BufferEmptyLength < 1024)
			{
				this._loadBuffer = new byte[BufferSize];
				_offset = 0;
			}
			int origOffset = _offset;
			_offset += await Stream.ReadAsync(_loadBuffer, _offset, BufferEmptyLength);
			return new Buffer<byte>(_loadBuffer, origOffset, _offset - origOffset);
		}
	}
}
