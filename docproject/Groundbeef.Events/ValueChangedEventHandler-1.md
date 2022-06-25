# ValueChangedEventHandler&lt;&gt;

Namespace: Groundbeef.Events

```csharp
public sealed class ValueChangedEventHandler<TValue> : System.MulticastDelegate
```

#### Type Parameters

`TValue`<br>

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Delegate](https://docs.microsoft.com/en-us/dotnet/api/system.delegate) → [MulticastDelegate](https://docs.microsoft.com/en-us/dotnet/api/system.multicastdelegate) → [ValueChangedEventHandler&lt;TValue&gt;](ValueChangedEventHandler-1.md)

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

### ValueChangedEventHandler(Object, IntPtr)

```csharp
public ValueChangedEventHandler(Object object, IntPtr method)
```

> #### Parameters
> 
> object : `Object`<br>
> 
> method : `IntPtr`<br>
> 

---

## Methods

### Invoke(Object, ValueChangedEventArgs&lt;&gt;)

```csharp
public Void Invoke(Object sender, ValueChangedEventArgs<TValue> e)
```

> #### Parameters
> 
> sender : `Object`<br>
> 
> e : `ValueChangedEventArgs<>`<br>
> 
> #### Returns
> 
> Void<br>
> 

### BeginInvoke(Object, ValueChangedEventArgs&lt;&gt;, AsyncCallback, Object)

```csharp
public IAsyncResult BeginInvoke(Object sender, ValueChangedEventArgs<TValue> e, AsyncCallback callback, Object object)
```

> #### Parameters
> 
> sender : `Object`<br>
> 
> e : `ValueChangedEventArgs<>`<br>
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
