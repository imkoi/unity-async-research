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
6. Check code gen in ilCode, since IL2CPP will generate same code for MethodStateMachines

### Allocations and Frametime
Note: I dont cover hard cases here, such awaiting .Net Task
that will make allocation every frame, because i would have raw results.
To have better picture of processing speed i will test frametime only
for 50000 background tasks

Each task is not allocate garbage each frame because we not await
method that return Task(to have only processing speed), so i dont test it

### 100 background tasks:
##### Allocations on execute:
.Net Task

##### Allocations per frame:
.Net Task

### 500 background tasks:
##### Allocations on execute:
.Net Task

##### Allocations per frame:
.Net Task

### 50000 background tasks:
##### Allocations on execute:
.Net Task

##### Allocations per frame:
.Net Task

##### Frametime on execute:
.Net Task

##### Frametime per frame:
.Net Task

