using AutoMapper;

namespace GrpcKopikoDemoServer.Extensions.MapperExtension
{
	public static class MapperExtension
	{
		public static TDestination Map<TSource, TDestination>(this TDestination destination, TSource source, IMapper mapper)
		{
			return mapper.Map(source, destination);
		}
	}
}
