# Unity Async Research
Research of available async libraries for unity engine, where we will
cover 4 different types of tasks:
* System.Threading.Task
* System.Threading.ValueTask
* UnityEngine.Awaitable
* Cysharp.UniTask

### Setup
Tests should cover:
* Allocation size and speed
* Processing of tasks
* Codegen size

Base class that will explain runtime tests
```c#
public abstract class RuntimeBase : MonoBehaviour
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

Implementations foreach library could be found in Assets/Runtime/:
* UniTaskRuntime.cs
* TaskRuntime.cs
* ValueTaskRuntime.cs
* AwaitableRuntime.cs

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
for 50000+ background tasks

Note: All tests was done in **Unity 2023.1.20f1**

Each task is not allocate garbage each frame because we not await
method that return Task(to have only processing speed), so i dont test it

### 100 background tasks:
##### Allocations on execute:
* UnityEngine.Awaitable: 2.2 kb / **122%**
* System.Threading.Task: 2.4 kb / **133%**
* System.Threading.ValueTask: 2.4 kb / **133%**
* Cysharp.UniTask: 1.8 kb / **100%**

### 500 background tasks:
##### Allocations on execute:
* UnityEngine.Awaitable: 8.3 kb / **105%**
* System.Threading.Task: 11.8 kb / **149%**
* System.Threading.ValueTask: 11.8 kb / **149%**
* Cysharp.UniTask: 7.9 kb / **100%**

### 50000 background tasks:
##### Frametime per frame:
* UnityEngine.Awaitable: 12.11 ms / **197%**
* System.Threading.Task: 883 ms / **14428%**
* System.Threading.ValueTask: 883 ms / **14428%**
* Cysharp.UniTask: 6.12 ms / **100%**

**Here we can see that UniTask is better for runtime performance and allocations.**

**Also we can see same result for Task and ValueTask, thats
because operation is not completed, so it transforming into simple Task**

## Codegen

CodeGen size was tested with generation N amount of 
tasks that call each other,
so state machine will not be small. Builds IL2CPP project was inspected to
ensure that there is no unwanted added/removed code.

Source of CodeGenerator is located in 
_Assets/CodeGen/CodeGenerator.cs_

Results of codegen code could be founded at:
* _Assets/CodeGen/Awaitable/AwaitableGenerated.cs_
* _Assets/CodeGen/Task/TaskGenerated.cs_
* _Assets/CodeGen/UniTask/ValueTaskGenerated.cs_
* _Assets/CodeGen/UniTask/UniTaskGenerated.cs_

Note: _Upon the size of build I mean only GameAssembly.dll_

### Results for different Unity versions
[Results for Unity 2023.1.20f1](Unity2023Codegen.md)

[Results for Unity 2021.3.32f1](Unity2021Codegen.md)

**As we see there is not a big codegen hit for Cysharp.UniTask,
but they have much better runtime performance and less allocations.**

**UnityEngine.Awaitalbe could be a good choice for mobile games, but they miss
many basic features that required for this area,
such as Awaitable.WhenAny, Awaitable.WhenAll**
