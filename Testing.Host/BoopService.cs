namespace Testing.Host;

public interface IAmAService
{
    public void Boop();
}

public class BoopService : IAmAService
{
    public void Boop(){}
}