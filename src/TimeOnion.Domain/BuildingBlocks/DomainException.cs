namespace TimeOnion.Domain.BuildingBlocks;

public abstract class DomainException : Exception
{
    protected DomainException(string message)
    {
    }
}