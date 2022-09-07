using AutoMapper;
using Notes.Application.Interfaces;
using System.Reflection;

namespace Notes.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().
                Where(a => a.GetName()?.Name?.Equals("Notes.Application") ?? false).First();

            ApplyMappingsFromAssembly(assembly);
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(IMapFrom<>) ||
                                        i.GetGenericTypeDefinition() == typeof(IMapTo<>))))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var mapTo = type.GetMethod("MapTo") ??
                            instance!.GetType()
                                .GetInterface("IMapTo`1")?
                                .GetMethod("MapTo");
                var mapFrom = type.GetMethod("MapFrom") ??
                                instance!.GetType()
                                    .GetInterface("IMapFrom`1")?
                                    .GetMethod("MapFrom");

                mapTo?.Invoke(instance, new object[] { this });
                mapFrom?.Invoke(instance, new object[] { this });
            }
        }
    }
}
