using System;
using System.Collections.Generic;

namespace ReshaperCore.Utils
{
	public class MessageQueue<T> : Subject
	{
		private Deque<T> _deque;

		public MessageQueue() : base()
		{
			_deque = new Deque<T>();
		}

		public virtual void AddFirst(IList<T> messages)
		{
			lock (this)
			{
				try
				{
					for (int index = messages.Count - 1; index >= 0; index--)
					{
						_deque.AddFirst(messages[index]);
					}
				}
				catch (Exception ex)
				{
					Log.LogError(ex, "Core Error: Could not add message queue item");
				}
			}

			Notify();
		}

		public virtual void AddFirst(T message)
		{
			lock (this)
			{
				try
				{
					_deque.AddFirst(message);
				}
				catch (Exception ex)
				{
					Log.LogError(ex, "Core Error: Could not add message queue item");
				}
			}

			Notify();
		}

		public virtual void AddLast(T message)
		{
			lock (this)
			{
				try
				{
					_deque.AddLast(message);
				}
				catch (Exception ex)
				{
					Log.LogError(ex, "Core Error: Could not add message queue item");
				}
			}
			Notify();
		}

		public void AddLast(IList<T> messages)
		{
			lock (this)
			{
				try
				{
					foreach (T message in messages)
					{
						_deque.AddLast(message);
					}
				}
				catch (Exception ex)
				{
					Log.LogError(ex, "Core Error: Could not add message queue item");
				}
			}
			Notify();
		}

		public void Clear()
		{
			lock (this)
			{
				_deque.Clear();
			}
		}

		public T TakeFirst()
		{
			lock (this)
			{
				T message = default(T);
				try
				{
					if (_deque.Count > 0)
					{
						message = _deque.TakeFirst();
					}
				}
				catch (Exception ex)
				{
					Log.LogError(ex, "Core Error: Could not remove message queue item");
				}
				return message;
			}
		}

		public bool IsEmpty()
		{
			return (_deque.Count == 0);
		}
	}
}
