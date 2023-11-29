using System.Threading.Tasks;

namespace DefaultNamespace
{
    public class ValueTaskRuntime : RuntimeBase
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
                await Task.Yield();
                await Task.Yield();
            }
        }
    }
}