using System.Threading.Tasks;

public interface IEHService
{
    public Task SendEvent(int count = 1);

    public Task<string> GetStats();
}