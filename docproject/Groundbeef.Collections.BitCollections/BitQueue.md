# BitQueue

Namespace: Groundbeef.Collections.BitCollections

```csharp
public class BitQueue : BitArrayList, System.ICloneable
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [BitArrayList](BitArrayList.md) → [BitQueue](BitQueue.md)

Implements [IReadOnlyCollection&lt;Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlycollection-1[[system.boolean, system.private.corelib, version=5.0.0.0, culture=neutral, publickeytoken=7cec85d7bea7798e]]), [IEnumerable&lt;Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1[[system.boolean, system.private.corelib, version=5.0.0.0, culture=neutral, publickeytoken=7cec85d7bea7798e]]), [IEnumerable](https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable), [ICollection](https://docs.microsoft.com/en-us/dotnet/api/system.collections.icollection), [ICloneable](https://docs.microsoft.com/en-us/dotnet/api/system.icloneable)

## Properties

### IsEmpty

```csharp
public Boolean IsEmpty { get; }
```

> #### Property Value
> 
> `Boolean`<br>
> 

### Count

```csharp
public Int32 Count { get; }
```

> #### Property Value
> 
> `Int32`<br>
> 

### DataBlockCount

```csharp
public Int32 DataBlockCount { get; }
```

> #### Property Value
> 
> `Int32`<br>
> 

### IsSynchronized

```csharp
public Boolean IsSynchronized { get; }
```

> #### Property Value
> 
> `Boolean`<br>
> 

### SyncRoot

```csharp
public Object SyncRoot { get; }
```

> #### Property Value
> 
> `Object`<br>
> 

---

## Constructors

### BitQueue()

```csharp
public BitQueue()
```

> 

---

## Methods

### Clone()

```csharp
public Object Clone()
```

> #### Returns
> 
> Object<br>
> 

### Enqueue(Boolean)

```csharp
public Void Enqueue(Boolean item)
```

> #### Parameters
> 
> item : `Boolean`<br>
> 
> #### Returns
> 
> Void<br>
> 

### Dequeue()

```csharp
public Boolean Dequeue()
```

> #### Returns
> 
> Boolean<br>
> 

### Peek()

```csharp
public Boolean Peek()
```

> #### Returns
> 
> Boolean<br>
> 

### Contains(Boolean)

```csharp
public Boolean Contains(Boolean item)
```

> #### Parameters
> 
> item : `Boolean`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### EnsureCapacity(Int32)

```csharp
protected Void EnsureCapacity(Int32 elementCount)
```

> #### Parameters
> 
> elementCount : `Int32`<br>
> 
> #### Returns
> 
> Void<br>
> 

### Syncronized(BitQueue)

```csharp
public static BitQueue Syncronized(BitQueue queue)
```

> #### Parameters
> 
> queue : `BitQueue`<br>
> 
> #### Returns
> 
> BitQueue<br>
> 

---

[`< Index`](..\index.md)
