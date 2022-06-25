# ExceptionHelper

Namespace: Groundbeef.Core

```csharp
public static class ExceptionHelper : System.Object
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ExceptionHelper](ExceptionHelper.md)

## Methods

### Try(Action, Action&lt;&gt;)

```csharp
public static Void Try(Action action, Action<Exception> handler)
```

> #### Parameters
> 
> action : `Action`<br>
> 
> handler : `Action<>`<br>
> 
> #### Returns
> 
> Void<br>
> 

### Try&lt;T&gt;(Action, Action&lt;&gt;)

```csharp
public static Void Try<T>(Action action, Action<T> handler)
```

> #### Type Parameters
> 
> `T`<br>
> 
> #### Parameters
> 
> action : `Action`<br>
> 
> handler : `Action<>`<br>
> 
> #### Returns
> 
> Void<br>
> 

---

[`< Index`](..\index.md)
