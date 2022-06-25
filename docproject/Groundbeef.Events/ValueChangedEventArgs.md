# ValueChangedEventArgs

Namespace: Groundbeef.Events

```csharp
public class ValueChangedEventArgs : System.EventArgs
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [EventArgs](https://docs.microsoft.com/en-us/dotnet/api/system.eventargs) → [ValueChangedEventArgs](ValueChangedEventArgs.md)

## Properties

### OldValue

```csharp
public Object OldValue { get; set; }
```

> #### Property Value
> 
> `Object`<br>
> 

### NewValue

```csharp
public Object NewValue { get; set; }
```

> #### Property Value
> 
> `Object`<br>
> 

### ValuesType

```csharp
public Type ValuesType { get; set; }
```

> #### Property Value
> 
> `Type`<br>
> 

---

## Constructors

### ValueChangedEventArgs(Object, Object)

```csharp
public ValueChangedEventArgs(Object oldValue, Object newValue)
```

> #### Parameters
> 
> oldValue : `Object`<br>
> 
> newValue : `Object`<br>
> 

### ValueChangedEventArgs(Object, Object, Type)

```csharp
public ValueChangedEventArgs(Object oldValue, Object newValue, Type valuesType)
```

> #### Parameters
> 
> oldValue : `Object`<br>
> 
> newValue : `Object`<br>
> 
> valuesType : `Type`<br>
> 

---

[`< Index`](..\index.md)
