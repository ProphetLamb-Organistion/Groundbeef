# STAThreadTask

Namespace: Groundbeef.Core

```csharp
public static class STAThreadTask : System.Object
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [STAThreadTask](STAThreadTask.md)

## Methods

### Run&lt;T&gt;(Func&lt;&gt;)

```csharp
public static Task<T> Run<T>(Func<T> func)
```

> #### Type Parameters
> 
> `T`<br>
> 
> #### Parameters
> 
> func : `Func<>`<br>
> 
> #### Returns
> 
> Task<T><br>
> 

### Run(Action)

```csharp
public static Task Run(Action func)
```

> #### Parameters
> 
> func : `Action`<br>
> 
> #### Returns
> 
> Task<br>
> 

---

[`< Index`](..\index.md)
