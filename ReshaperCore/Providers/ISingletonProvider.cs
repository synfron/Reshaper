namespace ReshaperCore.Providers
{
	public interface ISingletonProvider<T> : ISingletonProvider<T, T>
	{
	}


	public interface ISingletonProvider<BaseT, SubT>
	{
		SubT GetInstance();
	}
}
