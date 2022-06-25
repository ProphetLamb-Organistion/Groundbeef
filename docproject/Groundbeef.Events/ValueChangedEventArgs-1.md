# ValueChangedEventArgs&lt;&gt;

Namespace: Groundbeef.Events

```csharp
public class ValueChangedEventArgs<TValue> : System.EventArgs
```

#### Type Parameters

`TValue`<br>

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [EventArgs](https://docs.microsoft.com/en-us/dotnet/api/system.eventargs) → [ValueChangedEventArgs&lt;TValue&gt;](ValueChangedEventArgs-1.md)

## Properties

### OldValue

```csharp
public TValue OldValue { get; protected set; }
```

> #### Property Value
> 
> `TValue`<br>
> 

### NewValue

```csharp
public TValue NewValue { get; protected set; }
```

> #### Property Value
> 
> `TValue`<br>
> 

---

## Constructors

### ValueChangedEventArgs(TValue, TValue)

```csharp
public ValueChangedEventArgs(TValue oldValue, TValue newValue)
```

> #### Parameters
> 
> oldValue : `TValue`<br>
> 
> newValue : `TValue`<br>
> 

---

[`< Index`](..\index.md)
