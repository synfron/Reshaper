using System;

namespace ReshaperUI.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class SourceModelAttribute : Attribute
	{
	}
}
