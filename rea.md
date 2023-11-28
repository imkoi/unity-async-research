# Unity Async Research
Research of available async libraries for unity engine
This test will cover 4 different libraries:
* .Net Task
* Unity Awaitable
* Cysharp UniTask

### Setup
Tests should cover:
* Allocation size and speed
* Processing of tasks
* Codegen size

So I created base class for my "tests"
```c#
public abstract class ExampleBase : MonoBehaviour
{
    [SerializeField] private int _capacity;
    [SerializeField] private int _acitveSeconds;

    protected Stopwatch Stopwatch { get; private set; }
    protected int Capacity => _capacity;
    protected int ActiveMilliseconds => _acitveSeconds * 1000;
    protected CancellationToken CancellationToken => destroyCancellationToken;
    protected bool IsDone => Stopwatch.ElapsedTicks / 10000f >= ActiveMilliseconds;

    protected abstract void Execute();

    private void Awake()
    {
        Stopwatch = Stopwatch.StartNew();
        Execute();
        
        Debug.Log($"{GetType().Name} STARTUP: {Stopwatch.ElapsedTicks / 10000f}");
    }
}
```

Implementations foreach library could be found in Assets/Tasks:
* UniTaskExample.cs
* TaskExample.cs
* AwaitableExample.cs

Strategy:
1. Set tasks amount into _capacity field in inspector
2. Set some task duration into _activeSeconds field in inspector
3. Create implementations foreach task library that will run simple logic
4. Check startup allocations and alloc per frame
5. Check PlayerLoop performance (each library will use its own player loop to schedule and invoke completion events on main thread)

### Allocations and Frametime
Note: I dont cover hard cases here, such awaiting .Net Task
that will make allocation every frame, because i would have raw results.
To have better picture of processing speed i will test frametime only
for 50000 background tasks

Each task is not allocate garbage each frame because we not await
method that return Task(to have only processing speed), so i dont test it

### 100 background tasks:
##### Allocations on execute:
* UnityEngine.Awaitable: 2.2 kb / **122%**
* System.Threading.Task: 2.4 kb / **133%**
* Cysharp.UniTask: 1.8 kb / **100%**

### 500 background tasks:
##### Allocations on execute:
* UnityEngine.Awaitable: 8.3 kb / **105%**
* System.Threading.Task: 11.8 kb / **149%**
* Cysharp.UniTask: 7.9 kb / **100%**

### 50000 background tasks:
##### Frametime per frame:
* UnityEngine.Awaitable: 12.11 ms / **197%**
* System.Threading.Task: 883 ms / **14428%**
* Cysharp.UniTask: 6.12 ms / **100%**

Here we can see that UniTask is better for runtime performance and allocations

### Codegen size

CodeGen size was tested with generation 250 number of 
tasks that call each other,
so state machine will not be small. CodeGenerator is located in 
_Assets/CodeGen/CodeGenerator.cs_

On builds i also inspect il2cpp project to
ensure that codegen code is not stripped.

Results of codegen code could be founded at:
* _Assets/CodeGen/Awaitable/AwaitableGenerated.cs_
* _Assets/CodeGen/Task/TaskGenerated.cs_
* _Assets/CodeGen/UniTask/UniTaskGenerated.cs_

Results:
* Empty: 22.256 mb
* UnityEngine.Awaitable: 29.026 mb / **100%**
* System.Threading.Task: 29.331 mb / **102%**
* Cysharp.UniTask: 30.933 mb / **107%**

By result we see that cpp compiler can strip even mscore.dll code that 
not used, because **UnityEngine.Awaitable** codegen size are less 
than with **System.Threading.Tasks**

Also as we see there is not a big codegen hit for Cysharp.UniTask,
but they have much better runtime performance and less allocations
