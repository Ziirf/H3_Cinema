namespace Cinema.Converters
{
    // Wanted to use in case we wanted to convert multiple different things at once, never got to use it, but is implemented.
    public interface IConverter<Model, DTO>
    {
        DTO Convert(Model model);
        Model Convert(DTO dto);
    }
}