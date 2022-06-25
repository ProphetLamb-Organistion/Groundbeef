# BitArrayList

Namespace: Groundbeef.Collections.BitCollections

```csharp
public abstract class BitArrayList : System.Object, System.Collections.Generic.IReadOnlyCollection<Boolean>, System.Collections.Generic.IEnumerable<Boolean>, System.Collections.IEnumerable, System.Collections.ICollection
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [BitArrayList](BitArrayList.md)

Implements [IReadOnlyCollection&lt;Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlycollection-1[[system.boolean, system.private.corelib, version=5.0.0.0, culture=neutral, publickeytoken=7cec85d7bea7798e]]), [IEnumerable&lt;Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1[[system.boolean, system.private.corelib, version=5.0.0.0, culture=neutral, publickeytoken=7cec85d7bea7798e]]), [IEnumerable](https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable), [ICollection](https://docs.microsoft.com/en-us/dotnet/api/system.collections.icollection)

## Properties

### Count

Gets the number of [bool](https://docs.microsoft.com/en-us/dotnet/api/system.boolean) elements in the collection.

```csharp
public Int32 Count { get; }
```

> #### Property Value
> 
> `Int32`<br>
> 

### DataBlockCount

Gets the number of [ulong](https://docs.microsoft.com/en-us/dotnet/api/system.uint64) chunks allocated to encompass all [bool](https://docs.microsoft.com/en-us/dotnet/api/system.boolean) elements in the collection. 
            `Groundbeef.Collections.BitCollections.BitArrayList.DataBlockCount` : `Groundbeef.Collections.BitCollections.BitArrayList.Count` converges to 1 : 64 on a 64bit platform and to 1 : 32 on a 32bit platform.

```csharp
public Int32 DataBlockCount { get; }
```

> #### Property Value
> 
> `Int32`<br>
> 

### IsSynchronized

Indicates that the collection is syncronized.

```csharp
public Boolean IsSynchronized { get; }
```

> #### Property Value
> 
> `Boolean`<br>
> 

### SyncRoot

Gets the [object](https://docs.microsoft.com/en-us/dotnet/api/system.object) that is the syncronization root for the current instance if `!:IsSyncronized`; otherwise, a reference to the instance itself.

```csharp
public Object SyncRoot { get; }
```

> #### Property Value
> 
> `Object`<br>
> 

---

## Methods

### Clear()

Clears all data from the collection.

```csharp
public Void Clear()
```

> #### Returns
> 
> Void<br>
> 

### TrimToSize()

Trims the `Groundbeef.Collections.BitCollections.BitArrayList.DataBlockCount` to minimally fit the `Groundbeef.Collections.BitCollections.BitArrayList.Count` by reallocating the storage array.

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

### EnforceOffsetRange()

```csharp
protected Void EnforceOffsetRange()
```

> #### Returns
> 
> Void<br>
> 

### EnsureCapacity(Int32)

Ensures that the storage array has enouth capacity to hoist the .

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

### RightShiftArrayLong(Span&lt;&gt;)

```csharp
protected static Void RightShiftArrayLong(Span sourceSpan)
```

> #### Parameters
> 
> sourceSpan : `Span<>`<br>
> 
> #### Returns
> 
> Void<br>
> 

### RightShiftLongRightAt(UInt64[]&, Int32, Int32)

```csharp
protected static Void RightShiftLongRightAt(UInt64[]& sourceArray, Int32 index, Int32 offset)
```

> #### Parameters
> 
> sourceArray : `UInt64[]&`<br>
> 
> index : `Int32`<br>
> 
> offset : `Int32`<br>
> 
> #### Returns
> 
> Void<br>
> 

### RightShiftLongLeftAt(UInt64[]&, Int32, Int32)

```csharp
protected static Void RightShiftLongLeftAt(UInt64[]& sourceArray, Int32 index, Int32 offset)
```

> #### Parameters
> 
> sourceArray : `UInt64[]&`<br>
> 
> index : `Int32`<br>
> 
> offset : `Int32`<br>
> 
> #### Returns
> 
> Void<br>
> 

### RightShiftArrayBit(Span&lt;&gt;)

```csharp
protected static Void RightShiftArrayBit(Span sourceSpan)
```

> #### Parameters
> 
> sourceSpan : `Span<>`<br>
> 
> #### Returns
> 
> Void<br>
> 

### LeftShiftArrayLong(Span&lt;&gt;)

```csharp
protected static Void LeftShiftArrayLong(Span sourceSpan)
```

> #### Parameters
> 
> sourceSpan : `Span<>`<br>
> 
> #### Returns
> 
> Void<br>
> 

### LeftShiftLongLeftAt(UInt64[]&, Int32, Int32)

```csharp
protected static Void LeftShiftLongLeftAt(UInt64[]& sourceArray, Int32 index, Int32 offset)
```

> #### Parameters
> 
> sourceArray : `UInt64[]&`<br>
> 
> index : `Int32`<br>
> 
> offset : `Int32`<br>
> 
> #### Returns
> 
> Void<br>
> 

### LeftShiftArrayBit(Span&lt;&gt;)

```csharp
protected static Void LeftShiftArrayBit(Span sourceSpan)
```

> #### Parameters
> 
> sourceSpan : `Span<>`<br>
> 
> #### Returns
> 
> Void<br>
> 

### UpdateElementsCount()

```csharp
protected Void UpdateElementsCount()
```

> #### Returns
> 
> Void<br>
> 

### WriteBitAt(Span&lt;&gt;, Int32, Boolean)

```csharp
protected static Void WriteBitAt(Span sourceSpan, Int32 offset, Boolean value)
```

> #### Parameters
> 
> sourceSpan : `Span<>`<br>
> 
> offset : `Int32`<br>
> 
> value : `Boolean`<br>
> 
> #### Returns
> 
> Void<br>
> 

### ReadBitAt(UInt64[]&, Int32)

```csharp
protected static Boolean ReadBitAt(UInt64[]& source, Int32 index)
```

> #### Parameters
> 
> source : `UInt64[]&`<br>
> 
> index : `Int32`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### CopyFromBooleanArray(Boolean[]&, UInt64[]&, Int32)

```csharp
protected static Void CopyFromBooleanArray(Boolean[]& sourceArray, UInt64[]& targetArray, Int32 length)
```

> #### Parameters
> 
> sourceArray : `Boolean[]&`<br>
> 
> targetArray : `UInt64[]&`<br>
> 
> length : `Int32`<br>
> 
> #### Returns
> 
> Void<br>
> 

---

[`< Index`](..\index.md)
