# Invoker

Namespace: Groundbeef.Reflection

```csharp
public sealed class Invoker : System.MulticastDelegate
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Delegate](https://docs.microsoft.com/en-us/dotnet/api/system.delegate) → [MulticastDelegate](https://docs.microsoft.com/en-us/dotnet/api/system.multicastdelegate) → [Invoker](Invoker.md)

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

### Invoker(Object, IntPtr)

```csharp
public Invoker(Object object, IntPtr method)
```

> #### Parameters
> 
> object : `Object`<br>
> 
> method : `IntPtr`<br>
> 

---

## Methods

### Invoke(Object, Object[])

```csharp
public Object Invoke(Object obj, Object[] parameters)
```

> #### Parameters
> 
> obj : `Object`<br>
> 
> parameters : `Object[]`<br>
> 
> #### Returns
> 
> Object<br>
> 

### BeginInvoke(Object, Object[], AsyncCallback, Object)

```csharp
public IAsyncResult BeginInvoke(Object obj, Object[] parameters, AsyncCallback callback, Object object)
```

> #### Parameters
> 
> obj : `Object`<br>
> 
> parameters : `Object[]`<br>
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
public Object EndInvoke(IAsyncResult result)
```

> #### Parameters
> 
> result : `IAsyncResult`<br>
> 
> #### Returns
> 
> Object<br>
> 

---

[`< Index`](..\index.md)
