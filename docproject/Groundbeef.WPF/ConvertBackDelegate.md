# ConvertBackDelegate

Namespace: Groundbeef.WPF

```csharp
public sealed class ConvertBackDelegate : System.MulticastDelegate
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Delegate](https://docs.microsoft.com/en-us/dotnet/api/system.delegate) → [MulticastDelegate](https://docs.microsoft.com/en-us/dotnet/api/system.multicastdelegate) → [ConvertBackDelegate](ConvertBackDelegate.md)

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

### ConvertBackDelegate(Object, IntPtr)

```csharp
public ConvertBackDelegate(Object object, IntPtr method)
```

> #### Parameters
> 
> object : `Object`<br>
> 
> method : `IntPtr`<br>
> 

---

## Methods

### Invoke(Object, Type[], Object, CultureInfo)

```csharp
public Object[] Invoke(Object value, Type[] targetTypes, Object parameter, CultureInfo culture)
```

> #### Parameters
> 
> value : `Object`<br>
> 
> targetTypes : `Type[]`<br>
> 
> parameter : `Object`<br>
> 
> culture : `CultureInfo`<br>
> 
> #### Returns
> 
> Object[]<br>
> 

### BeginInvoke(Object, Type[], Object, CultureInfo, AsyncCallback, Object)

```csharp
public IAsyncResult BeginInvoke(Object value, Type[] targetTypes, Object parameter, CultureInfo culture, AsyncCallback callback, Object object)
```

> #### Parameters
> 
> value : `Object`<br>
> 
> targetTypes : `Type[]`<br>
> 
> parameter : `Object`<br>
> 
> culture : `CultureInfo`<br>
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
public Object[] EndInvoke(IAsyncResult result)
```

> #### Parameters
> 
> result : `IAsyncResult`<br>
> 
> #### Returns
> 
> Object[]<br>
> 

---

[`< Index`](..\index.md)
