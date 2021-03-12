namespace Cinema.Domain.Converters
{
    public interface IConverter<Model, DTO>
    {
        DTO Convert(Model source);
        Model Convert(DTO destination);
    }
}