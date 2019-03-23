using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.Linq;

namespace ReshaperCore.Utils.Extensions
{
	public static class MefExtensions
	{

		public static IEnumerable<Type> GetExportsTypes(this CompositionContainer container, params Type[] types)
		{
			IQueryable<ComposablePartDefinition> parts =
			container.Catalog.Parts.Concat(container.Providers.OfType<CatalogExportProvider>().SelectMany(provider => provider.Catalog.Parts));

			return parts.Select(part => ReflectionModelServices.GetPartType(part).Value).Where(type => types.All(expectedType => expectedType.IsAssignableFrom(type)));
		}

		public static IEnumerable<Type> GetDistinctExportsTypes(this CompositionContainer container, params Type[] types)
		{
			IQueryable<ComposablePartDefinition> parts =
			container.Catalog.Parts.Concat(container.Providers.OfType<CatalogExportProvider>().SelectMany(provider => provider.Catalog.Parts)).GroupBy(part => part.ExportDefinitions.FirstOrDefault()).Select(group => group.First());
			
			return parts.Select(part => ReflectionModelServices.GetPartType(part).Value).Where(type => types.All(expectedType => expectedType.IsAssignableFrom(type)));
		}

		public static IEnumerable<Type> GetExportedTypes(this CompositionContainer container)
		{
			IQueryable<ComposablePartDefinition> parts = container.Catalog.Parts.Union(container.Providers.OfType<CatalogExportProvider>().SelectMany(provider => provider.Catalog.Parts));
			return parts.Select(part => ReflectionModelServices.GetPartType(part).Value);
		}
	}
}
