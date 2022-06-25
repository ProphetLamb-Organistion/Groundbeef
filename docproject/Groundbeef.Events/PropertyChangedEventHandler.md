# PropertyChangedEventHandler

Namespace: Groundbeef.Events

```csharp
public sealed class PropertyChangedEventHandler : System.MulticastDelegate
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Delegate](https://docs.microsoft.com/en-us/dotnet/api/system.delegate) → [MulticastDelegate](https://docs.microsoft.com/en-us/dotnet/api/system.multicastdelegate) → [PropertyChangedEventHandler](PropertyChangedEventHandler.md)

Implements [ICloneable](https://docs.microsoft.com/en-us/dotnet/api/system.icloneable), [ISerializable](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.iserializable)

## Properties

### Target

```csharp
public Object Target { get; }
```

> #### Property Value
> 
> `Object`<br>
> 

### Method

```csharp
public MethodInfo Method { get; }
```

> #### Property Value
> 
> `MethodInfo`<br>
> 

---

## Constructors

### PropertyChangedEventHandler(Object, IntPtr)

```csharp
public PropertyChangedEventHandler(Object object, IntPtr method)
```

> #### Parameters
> 
> object : `Object`<br>
> 
> method : `IntPtr`<br>
> 

---

## Methods

### Invoke(Object, PropertyChangedEventArgs)

```csharp
public Void Invoke(Object sender, PropertyChangedEventArgs e)
```

> #### Parameters
> 
> sender : `Object`<br>
> 
> e : `PropertyChangedEventArgs`<br>
> 
> #### Returns
> 
> Void<br>
> 

### BeginInvoke(Object, PropertyChangedEventArgs, AsyncCallback, Object)

```csharp
public IAsyncResult BeginInvoke(Object sender, PropertyChangedEventArgs e, AsyncCallback callback, Object object)
```

> #### Parameters
> 
> sender : `Object`<br>
> 
> e : `PropertyChangedEventArgs`<br>
> 
> callback : `AsyncCallback`<br>
> 
> object : `Object`<br>
> 
> #### Returns
> 
> IAsyncResult<br>
> 

### EndInvoke(IAsyncResult)

```csharp
public Void EndInvoke(IAsyncResult result)
```

> #### Parameters
> 
> result : `IAsyncResult`<br>
> 
> #### Returns
> 
> Void<br>
> 

---

[`< Index`](..\index.md)
