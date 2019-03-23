using System;
using ReshaperCore.Utils;

namespace ReshaperCore.Providers
{
	public abstract class SingletonProvider<T> : SingletonProvider<T, T>, ISingletonProvider<T>
	{
	}

	public abstract class SingletonProvider<BaseT, SubT> : ISingletonProvider<BaseT, SubT> where SubT : BaseT
	{

		private bool HasInstance()
		{
			bool hasInstance = false;
			if (Singleton<BaseT>.Instance != null)
			{
				Type currentType = GetType();
				hasInstance = Singleton<BaseT>.Instance.GetType() == currentType || typeof(SubT).IsAssignableFrom(Singleton<BaseT>.Instance.GetType());
			}
			return hasInstance;
		}

		public virtual SubT GetInstance()
		{
			if (!HasInstance())
			{
				Singleton<SubT>.Instance = CreateInstance();
			}
			return Singleton<SubT>.Instance;
		}

		protected abstract SubT CreateInstance();
	}
}
