using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TaskEntryPoint : MonoBehaviour
{
    private void Start()
    {
        var taskGenerated = new TaskGenerated();

        Forget(taskGenerated.Method10(CancellationToken.None));
    }

    private async void Forget(Task task)
    {
        await task;
    }
}
