# SyncronizedBitQueue

Namespace: Groundbeef.Collections.BitCollections

```csharp
private class SyncronizedBitQueue : BitQueue
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [BitArrayList](BitArrayList.md) → [BitQueue](BitQueue.md) → [SyncronizedBitQueue](SyncronizedBitQueue.md)

Implements [IReadOnlyCollection&lt;Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlycollection-1[[system.boolean, system.private.corelib, version=5.0.0.0, culture=neutral, publickeytoken=7cec85d7bea7798e]]), [IEnumerable&lt;Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1[[system.boolean, system.private.corelib, version=5.0.0.0, culture=neutral, publickeytoken=7cec85d7bea7798e]]), [IEnumerable](https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable), [ICollection](https://docs.microsoft.com/en-us/dotnet/api/system.collections.icollection), [ICloneable](https://docs.microsoft.com/en-us/dotnet/api/system.icloneable)

## Properties

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

### IsEmpty

```csharp
public Boolean IsEmpty { get; }
```

> #### Property Value
> 
> `Boolean`<br>
> 

---

## Constructors

### SyncronizedBitQueue(BitQueue)

```csharp
public SyncronizedBitQueue(BitQueue queue)
```

> #### Parameters
> 
> queue : `BitQueue`<br>
> 

---

## Methods

### Clear()

```csharp
public Void Clear()
```

> #### Returns
> 
> Void<br>
> 

### Clone()

```csharp
public Object Clone()
```

> #### Returns
> 
> Object<br>
> 

### TrimToSize()

```csharp
public Void TrimToSize()
```

> #### Returns
> 
> Void<br>
> 

### CopyTo(Array, Int32)

```csharp
public Void CopyTo(Array array, Int32 arrayIndex)
```

> #### Parameters
> 
> array : `Array`<br>
> 
> arrayIndex : `Int32`<br>
> 
> #### Returns
> 
> Void<br>
> 

### CopyTo(Boolean[], Int32)

```csharp
public Void CopyTo(Boolean[] array, Int32 arrayIndex)
```

> #### Parameters
> 
> array : `Boolean[]`<br>
> 
> arrayIndex : `Int32`<br>
> 
> #### Returns
> 
> Void<br>
> 

### CopyTo(UInt64[], Int32)

```csharp
public Void CopyTo(UInt64[] array, Int32 arrayIndex)
```

> #### Parameters
> 
> array : `UInt64[]`<br>
> 
> arrayIndex : `Int32`<br>
> 
> #### Returns
> 
> Void<br>
> 

### GetEnumerator()

```csharp
public IEnumerator<Boolean> GetEnumerator()
```

> #### Returns
> 
> IEnumerator<Boolean><br>
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

---

[`< Index`](..\index.md)
