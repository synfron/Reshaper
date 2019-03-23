namespace ReshaperCore.Networking
{
	public class Buffer<T>
	{

		public T[] Array { get; set; }
		public int Length { get; set; }
		public int Position { get; set; }

		public Buffer(T[] array, int position, int length)
		{
			this.Array = array;
			this.Position = position;
			this.Length = length;
		}

		public Buffer(T[] array)
		{
			this.Array = array;
			this.Position = 0;
			this.Length = array.Length;
		}

		public byte[] GetBytes()
		{
			byte[] subArray = new byte[Length];
			System.Buffer.BlockCopy(Array, Position, subArray, 0, Length);
			return subArray;
		}
	}
}
