# SyncronizedBitList

Namespace: Groundbeef.Collections.BitCollections

```csharp
private class SyncronizedBitList : BitList
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [BitArrayList](BitArrayList.md) → [BitList](BitList.md) → [SyncronizedBitList](SyncronizedBitList.md)

Implements [IReadOnlyCollection&lt;Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlycollection-1[[system.boolean, system.private.corelib, version=5.0.0.0, culture=neutral, publickeytoken=7cec85d7bea7798e]]), [IEnumerable&lt;Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1[[system.boolean, system.private.corelib, version=5.0.0.0, culture=neutral, publickeytoken=7cec85d7bea7798e]]), [IEnumerable](https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable), [ICollection](https://docs.microsoft.com/en-us/dotnet/api/system.collections.icollection), [IList&lt;Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1[[system.boolean, system.private.corelib, version=5.0.0.0, culture=neutral, publickeytoken=7cec85d7bea7798e]]), [ICollection&lt;Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1[[system.boolean, system.private.corelib, version=5.0.0.0, culture=neutral, publickeytoken=7cec85d7bea7798e]])

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

### IsReadOnly

```csharp
public Boolean IsReadOnly { get; }
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

### Item

```csharp
public Boolean Item { get; set; }
```

> #### Property Value
> 
> `Boolean`<br>
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

### SyncronizedBitList(BitList)

```csharp
public SyncronizedBitList(BitList list)
```

> #### Parameters
> 
> list : `BitList`<br>
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

### GetValue(Int32)

```csharp
protected Boolean GetValue(Int32 index)
```

> #### Parameters
> 
> index : `Int32`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### SetValue(Int32, Boolean)

```csharp
protected Void SetValue(Int32 index, Boolean value)
```

> #### Parameters
> 
> index : `Int32`<br>
> 
> value : `Boolean`<br>
> 
> #### Returns
> 
> Void<br>
> 

### Add(Boolean)

```csharp
public Void Add(Boolean item)
```

> #### Parameters
> 
> item : `Boolean`<br>
> 
> #### Returns
> 
> Void<br>
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

### IndexOf(Boolean)

```csharp
public Int32 IndexOf(Boolean item)
```

> #### Parameters
> 
> item : `Boolean`<br>
> 
> #### Returns
> 
> Int32<br>
> 

### Insert(Int32, Boolean)

```csharp
public Void Insert(Int32 index, Boolean item)
```

> #### Parameters
> 
> index : `Int32`<br>
> 
> item : `Boolean`<br>
> 
> #### Returns
> 
> Void<br>
> 

### Remove(Boolean)

```csharp
public Boolean Remove(Boolean item)
```

> #### Parameters
> 
> item : `Boolean`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### RemoveAt(Int32)

```csharp
public Void RemoveAt(Int32 index)
```

> #### Parameters
> 
> index : `Int32`<br>
> 
> #### Returns
> 
> Void<br>
> 

---

[`< Index`](..\index.md)
