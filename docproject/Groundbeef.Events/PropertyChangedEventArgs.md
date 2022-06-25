# PropertyChangedEventArgs

Namespace: Groundbeef.Events

```csharp
public class PropertyChangedEventArgs : ValueChangedEventArgs
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [EventArgs](https://docs.microsoft.com/en-us/dotnet/api/system.eventargs) → [ValueChangedEventArgs](ValueChangedEventArgs.md) → [PropertyChangedEventArgs](PropertyChangedEventArgs.md)

## Properties

### Name

```csharp
public String Name { get; set; }
```

> #### Property Value
> 
> `String`<br>
> 

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

### PropertyChangedEventArgs(Object, Object, String)

```csharp
public PropertyChangedEventArgs(Object oldValue, Object newValue, String propertyName)
```

> #### Parameters
> 
> oldValue : `Object`<br>
> 
> newValue : `Object`<br>
> 
> propertyName : `String`<br>
> 

### PropertyChangedEventArgs(Object, Object, Type, String)

```csharp
public PropertyChangedEventArgs(Object oldValue, Object newValue, Type valuesType, String propertyName)
```

> #### Parameters
> 
> oldValue : `Object`<br>
> 
> newValue : `Object`<br>
> 
> valuesType : `Type`<br>
> 
> propertyName : `String`<br>
> 

---

[`< Index`](..\index.md)
