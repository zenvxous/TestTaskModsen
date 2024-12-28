namespace TestTaskModsen.Core.Interfaces.Mappers;

public interface IMapper<TSource, TDestination>
{
    TDestination Map(TSource source);
    List<TDestination> Map(IEnumerable<TSource> source);
}