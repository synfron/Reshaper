namespace ReshaperCore.Providers
{
	public class SelfProvider : SingletonProvider<ISelf>
	{
		protected override ISelf CreateInstance()
		{
			return new Self();
		}
	}
}
