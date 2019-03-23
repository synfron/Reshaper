using System;
using System.Collections;
using System.Collections.Generic;

namespace ReshaperCore.Utils
{
	public class Deque<T> : IList<T>, ICollection
	{
		private int _firstIndex = 0;
		private int _lastIndex = 0;
		private T[] _storage;

		public Deque(int internalSize)
		{
			_storage = new T[internalSize];
		}

		public Deque() : this(50)
		{

		}

		public T this[int index]
		{
			get
			{
				if (index >= 0 && index < Count)
				{
					return _storage[GetStorageIndex(index)];
				}
				else
				{
					throw new IndexOutOfRangeException(index.ToString());
				}
			}
			set
			{
				if (index >= 0 && index < Count)
				{
					_storage[GetStorageIndex(index)] = value;
				}
				else
				{
					throw new IndexOutOfRangeException(index.ToString());
				}
			}
		}

		public int Capacity
		{
			get
			{
				return _storage.Length;
			}
		}

		public int Count
		{
			get;
			private set;
		}

		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		private int GetStorageIndex(int index)
		{
			return Mod(_firstIndex + index, Capacity);
		}

		public void Clear()
		{
			_firstIndex = 0;
			_lastIndex = 0;
			Count = 0;
			_storage = new T[Capacity];
		}

		public void AddFirst(T item)
		{
			int index = Mod(_firstIndex - 1, Capacity);
			if (index == _lastIndex)
			{
				Resize();
				index = Mod(_firstIndex - 1, Capacity);
			}
			_storage[index] = item;
			_firstIndex = index;
			if (Count == 0)
			{
				_lastIndex = _firstIndex;
			}
			Count++;
		}

		public void AddLast(T item)
		{
			int index = Mod(_lastIndex + 1, Capacity);
			if (index == _firstIndex)
			{
				Resize();
				index = Mod(_lastIndex + 1, Capacity);
			}
			_storage[index] = item;
			_lastIndex = index;
			if (Count == 0)
			{
				_firstIndex = _lastIndex;
			}
			Count++;
		}

		private void Resize()
		{
			T[] newStorage = new T[_storage.Length * 2];
			if (_firstIndex <= _lastIndex)
			{
				Array.Copy(_storage, _firstIndex, newStorage, 0, _lastIndex - _firstIndex + 1);
			}
			else
			{
				Array.Copy(_storage, _firstIndex, newStorage, 0, _storage.Length - _firstIndex);
				Array.Copy(_storage, 0, newStorage, _storage.Length - _firstIndex, _lastIndex + 1);
			}
			_firstIndex = 0;
			_lastIndex = _storage.Length - 1;
			_storage = newStorage;
		}

		public T TakeFirst()
		{
			if (Count > 0)
			{
				Count--;
				T item = _storage[_firstIndex];
				_storage[_firstIndex] = default(T);
				if (Count == 0)
				{
					_lastIndex = _firstIndex;
				}
				else
				{
					_firstIndex = Mod(_firstIndex + 1, Capacity);
				}
				return item;
			}
			return default(T);
		}

		public T TakeLast()
		{
			if (Count > 0)
			{
				Count--;
				T item = _storage[_lastIndex];
				_storage[_lastIndex] = default(T);
				if (Count == 0)
				{
					_firstIndex = _lastIndex;
				}
				else
				{
					_lastIndex = Mod(_lastIndex - 1, Capacity);
				}
				return item;
			}
			return default(T);
		}
		private int Mod(int number, int mod)
		{
			return (int)Mod((double)number, (double)mod);
		}

		private double Mod(double number, double mod)
		{
			return number - (Math.Floor(number / mod) * mod);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new DequeEnumerator<T>(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DequeEnumerator<T>(this);
		}

		public void CopyTo(Array array, int index)
		{
			if (_firstIndex <= _lastIndex)
			{
				Array.Copy(_storage, _firstIndex, array, index, _lastIndex - _firstIndex + 1);
			}
			else
			{
				Array.Copy(_storage, _firstIndex, array, index, _storage.Length - _firstIndex);
				Array.Copy(_storage, 0, array, index + _storage.Length - _firstIndex, _lastIndex + 1);
			}
		}

		public int IndexOf(T item)
		{
			int indexOf = -1;
			for (int index = 0; index < _storage.Length; index++)
			{
				if (this[index]?.Equals(item) ?? false)
				{
					indexOf = index;
					break;
				}
			}
			return indexOf;
		}

		public void Insert(int index, T item)
		{
			if (index >= 0 && index <= Capacity)
			{
				if (index == 0)
				{
					AddFirst(item);
				}
				else if (index == Count - 1)
				{
					AddLast(item);
				}
				else
				{
					T[] newStorage = new T[_storage.Length + 1];
					int storageIndex = GetStorageIndex(index);
					Array.Copy(_storage, newStorage, storageIndex);
					Array.Copy(_storage, storageIndex, newStorage, storageIndex + 1, Capacity - storageIndex);
					newStorage[storageIndex] = item;
					_storage = newStorage;
				}
			}
			else
			{
				throw new ArgumentOutOfRangeException($"Index {index} is out of range (0-{Capacity - 1})");
			}
		}

		public void RemoveAt(int index)
		{
			if (index >= 0 && index < Capacity)
			{
				if (index == 0)
				{
					TakeFirst();
				}
				else if (index == Count - 1)
				{
					TakeLast();
				}
				else
				{
					T[] newStorage = new T[_storage.Length];
					int storageIndex = GetStorageIndex(index);
					Array.Copy(_storage, newStorage, storageIndex);
					Array.Copy(_storage, storageIndex + 1, newStorage, storageIndex, Capacity - (storageIndex + 1));
					_storage = newStorage;
				}
			}
			else
			{
				throw new ArgumentOutOfRangeException($"Index {index} is out of range (0-{Capacity - 1})");
			}
		}

		public void Add(T item)
		{
			AddLast(item);
		}

		public bool Contains(T item)
		{

			return IndexOf(item) != -1;
		}

		public void CopyTo(T[] array, int index)
		{
			if (_firstIndex <= _lastIndex)
			{
				Array.Copy(_storage, _firstIndex, array, index, _lastIndex - _firstIndex + 1);
			}
			else
			{
				Array.Copy(_storage, _firstIndex, array, index, _storage.Length - _firstIndex);
				Array.Copy(_storage, 0, array, index + _storage.Length - _firstIndex, _lastIndex + 1);
			}
		}

		public bool Remove(T item)
		{
			int indexOf = IndexOf(item);
			if (indexOf != -1)
			{
				RemoveAt(indexOf);
				return true;
			}
			else
			{
				return false;
			}
		}

		private class DequeEnumerator<I> : IEnumerator<I>
		{
			private int currentIndex;
			private Deque<I> deque;

			public DequeEnumerator(Deque<I> deque)
			{
				this.deque = deque;
				this.currentIndex = 0;
			}

			public object Current
			{
				get
				{
					return deque[currentIndex];
				}
			}

			I IEnumerator<I>.Current
			{
				get
				{
					return deque[currentIndex];
				}
			}

			public void Dispose()
			{
				deque = null;
			}

			public bool MoveNext()
			{
				bool moved = currentIndex < deque.Count;
				if (moved)
				{
					currentIndex++;
				}
				return moved;
			}

			public void Reset()
			{
				currentIndex = 0;
			}
		}
	}
}
