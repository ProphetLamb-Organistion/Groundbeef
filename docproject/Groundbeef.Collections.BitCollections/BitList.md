# BitList

Namespace: Groundbeef.Collections.BitCollections

```csharp
public class BitList : BitArrayList, System.Collections.Generic.IList<Boolean>, System.Collections.Generic.ICollection<Boolean>
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [BitArrayList](BitArrayList.md) → [BitList](BitList.md)

Implements [IReadOnlyCollection&lt;Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlycollection-1[[system.boolean, system.private.corelib, version=5.0.0.0, culture=neutral, publickeytoken=7cec85d7bea7798e]]), [IEnumerable&lt;Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1[[system.boolean, system.private.corelib, version=5.0.0.0, culture=neutral, publickeytoken=7cec85d7bea7798e]]), [IEnumerable](https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable), [ICollection](https://docs.microsoft.com/en-us/dotnet/api/system.collections.icollection), [IList&lt;Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1[[system.boolean, system.private.corelib, version=5.0.0.0, culture=neutral, publickeytoken=7cec85d7bea7798e]]), [ICollection&lt;Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1[[system.boolean, system.private.corelib, version=5.0.0.0, culture=neutral, publickeytoken=7cec85d7bea7798e]])

## Properties

### Item

```csharp
public Boolean Item { get; set; }
```

> #### Property Value
> 
> `Boolean`<br>
> 

### IsReadOnly

```csharp
public Boolean IsReadOnly { get; }
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

### BitList()

```csharp
public BitList()
```

> 

### BitList(Int32)

```csharp
public BitList(Int32 capacity)
```

> #### Parameters
> 
> capacity : `Int32`<br>
> 

### BitList(IEnumerable&lt;&gt;)

```csharp
public BitList(IEnumerable<Boolean> collection)
```

> #### Parameters
> 
> collection : `IEnumerable<>`<br>
> 

---

## Methods

### FromData(Span&lt;&gt;)

```csharp
public static BitList FromData(Span<Byte> data)
```

> #### Parameters
> 
> data : `Span<>`<br>
> 
> #### Returns
> 
> BitList<br>
> 

### FromData(Span&lt;&gt;)

```csharp
public static BitList FromData(Span<UInt64> data)
```

> #### Parameters
> 
> data : `Span<>`<br>
> 
> #### Returns
> 
> BitList<br>
> 

### Clone()

```csharp
public Object Clone()
```

> #### Returns
> 
> Object<br>
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

### InsertLeftShift(Int32, Int32, Boolean)

```csharp
protected Void InsertLeftShift(Int32 longIndex, Int32 offset, Boolean value)
```

> #### Parameters
> 
> longIndex : `Int32`<br>
> 
> offset : `Int32`<br>
> 
> value : `Boolean`<br>
> 
> #### Returns
> 
> Void<br>
> 

### InsertRightShift(Int32, Int32, Boolean)

```csharp
protected Void InsertRightShift(Int32 longIndex, Int32 offset, Boolean value)
```

> #### Parameters
> 
> longIndex : `Int32`<br>
> 
> offset : `Int32`<br>
> 
> value : `Boolean`<br>
> 
> #### Returns
> 
> Void<br>
> 

### RemoveLeftShift(Int32, Int32)

```csharp
protected Void RemoveLeftShift(Int32 longIndex, Int32 offset)
```

> #### Parameters
> 
> longIndex : `Int32`<br>
> 
> offset : `Int32`<br>
> 
> #### Returns
> 
> Void<br>
> 

### RemoveRightShift(Int32, Int32)

```csharp
protected Void RemoveRightShift(Int32 longIndex, Int32 offset)
```

> #### Parameters
> 
> longIndex : `Int32`<br>
> 
> offset : `Int32`<br>
> 
> #### Returns
> 
> Void<br>
> 

### Syncronized(BitList)

```csharp
public BitList Syncronized(BitList list)
```

> #### Parameters
> 
> list : `BitList`<br>
> 
> #### Returns
> 
> BitList<br>
> 

---

[`< Index`](..\index.md)
