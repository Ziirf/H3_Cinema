namespace Cinema.Domain.Converters
{
    public interface IConverter<TSource, TDestination>
    {
        TDestination Convert(TSource source);
        TSource Convert(TDestination destination);
    }
}