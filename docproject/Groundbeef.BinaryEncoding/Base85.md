# Base85

Namespace: Groundbeef.BinaryEncoding

```csharp
public static class Base85 : System.Object
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Base85](Base85.md)

## Methods

### Decode(ReadOnlySpan&lt;&gt;)

```csharp
public static Span<Byte> Decode(ReadOnlySpan<Char> chars)
```

> #### Parameters
> 
> chars : `ReadOnlySpan<>`<br>
> 
> #### Returns
> 
> Span<Byte><br>
> 

### Decode(ReadOnlySpan&lt;&gt;, Int32)

```csharp
public static Span<Byte> Decode(ReadOnlySpan<Char> chars, Int32 offset)
```

> #### Parameters
> 
> chars : `ReadOnlySpan<>`<br>
> 
> offset : `Int32`<br>
> 
> #### Returns
> 
> Span<Byte><br>
> 

### Decode(ReadOnlySpan&lt;&gt;, Int32, Int32)

```csharp
public static Span<Byte> Decode(ReadOnlySpan<Char> chars, Int32 offset, Int32 count)
```

> #### Parameters
> 
> chars : `ReadOnlySpan<>`<br>
> 
> offset : `Int32`<br>
> 
> count : `Int32`<br>
> 
> #### Returns
> 
> Span<Byte><br>
> 

### Encode(ReadOnlySpan&lt;&gt;)

```csharp
public static Span<Char> Encode(ReadOnlySpan<Byte> bytes)
```

> #### Parameters
> 
> bytes : `ReadOnlySpan<>`<br>
> 
> #### Returns
> 
> Span<Char><br>
> 

### Encode(ReadOnlySpan&lt;&gt;, Int32)

```csharp
public static Span<Char> Encode(ReadOnlySpan<Byte> bytes, Int32 offset)
```

> #### Parameters
> 
> bytes : `ReadOnlySpan<>`<br>
> 
> offset : `Int32`<br>
> 
> #### Returns
> 
> Span<Char><br>
> 

### Encode(ReadOnlySpan&lt;&gt;, Int32, Int32)

```csharp
public static Span<Char> Encode(ReadOnlySpan<Byte> bytes, Int32 offset, Int32 count)
```

> #### Parameters
> 
> bytes : `ReadOnlySpan<>`<br>
> 
> offset : `Int32`<br>
> 
> count : `Int32`<br>
> 
> #### Returns
> 
> Span<Char><br>
> 

---

[`< Index`](..\index.md)
