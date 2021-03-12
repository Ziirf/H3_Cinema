namespace Cinema.Converters
{
    public interface IConverter<Model, DTO>
    {
        DTO Convert(Model model);
        Model Convert(DTO dto);
    }
}