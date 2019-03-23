using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Newtonsoft.Json.Serialization;
using ReshaperCore.Providers;
using ReshaperCore.Utils.Extensions;

namespace ReshaperCore.Utils
{
	public class SerializationBinder : DefaultSerializationBinder
	{
        public CompositionContainer Container { get; set; } = new CompositionContainerProvider().GetInstance();

		public override Type BindToType(string assemblyName, string typeName)
		{
			Type type = null;
			try
			{
				type = base.BindToType(assemblyName, typeName);
			}
			catch
			{
				type = Container.GetExportedTypes().Select(exportedType => exportedType.Assembly).Distinct().Select(assembly => assembly.DefinedTypes.FirstOrDefault(exportedType => exportedType.FullName == typeName && assembly.GetName().Name == assemblyName)).FirstOrDefault();
				if (type == null)
				{
					throw;
				}
			}
			return type;
		}
	}
}
