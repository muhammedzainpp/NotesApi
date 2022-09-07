namespace Notes.Application.Common.Exceptions;

public class NotFoundException : Exception
{
	public NotFoundException(string ex) :base(ex)
	{}
}
