# ObjectExtention

Namespace: Groundbeef.Core

```csharp
public static class ObjectExtention : System.Object
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ObjectExtention](ObjectExtention.md)

## Methods

### Cast&lt;T&gt;(Object)

```csharp
public static T Cast<T>(Object value)
```

> #### Type Parameters
> 
> `T`<br>
> 
> #### Parameters
> 
> value : `Object`<br>
> 
> #### Returns
> 
> T<br>
> 

### Convert&lt;T&gt;(Object)

```csharp
public static T Convert<T>(Object value)
```

> #### Type Parameters
> 
> `T`<br>
> 
> #### Parameters
> 
> value : `Object`<br>
> 
> #### Returns
> 
> T<br>
> 

### Combine(Guid, Guid)

```csharp
public static Guid Combine(Guid self, Guid other)
```

> #### Parameters
> 
> self : `Guid`<br>
> 
> other : `Guid`<br>
> 
> #### Returns
> 
> Guid<br>
> 

### ThrowOnNull(Object, String&)

```csharp
public static Void ThrowOnNull(Object self, String& parameterName)
```

> #### Parameters
> 
> self : `Object`<br>
> 
> parameterName : `String&`<br>
> 
> #### Returns
> 
> Void<br>
> 

### DerefOrThrow&lt;T&gt;(T, String&)

```csharp
public static T DerefOrThrow<T>(T self, String& paramerterName)
```

> #### Type Parameters
> 
> `T`<br>
> 
> #### Parameters
> 
> self : `T`<br>
> 
> paramerterName : `String&`<br>
> 
> #### Returns
> 
> T<br>
> 

### DerefOrThrow&lt;T&gt;(Nullable&lt;&gt;, String&)

```csharp
public static T DerefOrThrow<T>(Nullable<T> self, String& paramerterName)
```

> #### Type Parameters
> 
> `T`<br>
> 
> #### Parameters
> 
> self : `Nullable<>`<br>
> 
> paramerterName : `String&`<br>
> 
> #### Returns
> 
> T<br>
> 

---

[`< Index`](..\index.md)
