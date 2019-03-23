using ReshaperCore.Settings;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace ReshaperCore.Providers
{
	public class CompositionContainerProvider : SingletonProvider<CompositionContainer>
	{
		private const string ExtensionDirectory = @"/Extensions";

		protected override CompositionContainer CreateInstance()
		{
            string extensionsPath = Path.Combine(SettingsStore.StoragePath, ExtensionDirectory);

            if (!Directory.Exists(ExtensionDirectory))
			{
				Directory.CreateDirectory(ExtensionDirectory);
			}

			DirectoryCatalog thirdPartyCatelog = new DirectoryCatalog(ExtensionDirectory);
			AggregateCatalog defaultCatalog = new AggregateCatalog();

			defaultCatalog.Catalogs.Add(new DirectoryCatalog("./", "Reshaper*.dll"));
			CatalogExportProvider defaultExportProvider = new CatalogExportProvider(defaultCatalog);

			CompositionContainer container = new CompositionContainer(thirdPartyCatelog, defaultExportProvider);
			defaultExportProvider.SourceProvider = container;
			return container;
		}
	}
}
