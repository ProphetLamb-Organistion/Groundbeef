# TaskCollectionExtention

Namespace: Groundbeef.Core

```csharp
public static class TaskCollectionExtention : System.Object
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [TaskCollectionExtention](TaskCollectionExtention.md)

## Methods

### WaitAll(Task[])

Waits for all of the provided System.Threading.Tasks.Task objects to complete execution.

```csharp
public static Void WaitAll(Task[] collection)
```

> #### Parameters
> 
> collection : `Task[]`<br>The  to await.
> 
> #### Returns
> 
> Void<br>
> 

### WaitAll(IReadOnlyList&lt;&gt;)

```csharp
public static Void WaitAll(IReadOnlyList<Task> collection)
```

> #### Parameters
> 
> collection : `IReadOnlyList<>`<br>
> 
> #### Returns
> 
> Void<br>
> 

### WaitAny(Task[])

Waits for any of the provided System.Threading.Tasks.Task objects to complete execution.

```csharp
public static Void WaitAny(Task[] collection)
```

> #### Parameters
> 
> collection : `Task[]`<br>The  to await.
> 
> #### Returns
> 
> Void<br>
> 

### WaitAny(IReadOnlyList&lt;&gt;)

```csharp
public static Void WaitAny(IReadOnlyList<Task> collection)
```

> #### Parameters
> 
> collection : `IReadOnlyList<>`<br>
> 
> #### Returns
> 
> Void<br>
> 

### WhenAll(Task[])

Creates a task that will complete when all of the System.Threading.Tasks.Task objects in an array have completed.

```csharp
public static Task WhenAll(Task[] collection)
```

> #### Parameters
> 
> collection : `Task[]`<br>The  to await.
> 
> #### Returns
> 
> Task<br>
> 

### WhenAll(IReadOnlyList&lt;&gt;)

```csharp
public static Task WhenAll(IReadOnlyList<Task> collection)
```

> #### Parameters
> 
> collection : `IReadOnlyList<>`<br>
> 
> #### Returns
> 
> Task<br>
> 

### WhenAny(Task[])

Creates a task that will complete when any of the supplied tasks have completed.

```csharp
public static Task WhenAny(Task[] collection)
```

> #### Parameters
> 
> collection : `Task[]`<br>The  to await.
> 
> #### Returns
> 
> Task<br>
> 

### WhenAny(IReadOnlyList&lt;&gt;)

```csharp
public static Task WhenAny(IReadOnlyList<Task> collection)
```

> #### Parameters
> 
> collection : `IReadOnlyList<>`<br>
> 
> #### Returns
> 
> Task<br>
> 

---

[`< Index`](..\index.md)
