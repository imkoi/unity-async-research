using System.Threading.Tasks;
using DefaultNamespace;

public class ValueTaskExample : ExampleBase
{
    protected override void Execute()
    {
        for (var i = 0; i < Capacity; i++)
        {
            Delay().Forget();
        }
    }

    private async ValueTask Delay()
    {
        while (!IsDone && !CancellationToken.IsCancellationRequested)
        {
            await Task.Yield();
        }
    }
}