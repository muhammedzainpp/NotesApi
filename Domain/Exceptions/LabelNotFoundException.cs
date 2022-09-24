namespace Domain.Exceptions;

public class LabelNotFoundException : NotFoundException
{
    public LabelNotFoundException(int labelId)
        : base($"The label with the identifier {labelId} was not found.")
    {
    }
}
