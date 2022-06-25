# PropertyChangedEventArgs&lt;&gt;

Namespace: Groundbeef.Events

```csharp
public class PropertyChangedEventArgs<T> : ValueChangedEventArgs<TValue>
```

#### Type Parameters

`T`<br>

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [EventArgs](https://docs.microsoft.com/en-us/dotnet/api/system.eventargs) → [ValueChangedEventArgs&lt;TValue&gt;](ValueChangedEventArgs-1.md) → [PropertyChangedEventArgs&lt;T&gt;](PropertyChangedEventArgs-1.md)

## Properties

### Name

```csharp
public String Name { get; protected set; }
```

> #### Property Value
> 
> `String`<br>
> 

### OldValue

```csharp
public T OldValue { get; protected set; }
```

> #### Property Value
> 
> `T`<br>
> 

### NewValue

```csharp
public T NewValue { get; protected set; }
```

> #### Property Value
> 
> `T`<br>
> 

---

## Constructors

### PropertyChangedEventArgs(T, T, String)

```csharp
public PropertyChangedEventArgs(T oldValue, T newValue, String propertyName)
```

> #### Parameters
> 
> oldValue : `T`<br>
> 
> newValue : `T`<br>
> 
> propertyName : `String`<br>
> 

---

[`< Index`](..\index.md)
