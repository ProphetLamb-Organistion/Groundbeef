# ConcurrentIndexer&lt;,&gt;

Namespace: Groundbeef.Collections.Concurrent

```csharp
public class ConcurrentIndexer<TKey, TValue> : System.Object, System.Collections.Generic.IReadOnlyDictionary<TKey, TValue>, System.Collections.Generic.IEnumerable<KeyValuePair<TKey, TValue>>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection<KeyValuePair<TKey, TValue>>
```

#### Type Parameters

`TKey`<br>

`TValue`<br>

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ConcurrentIndexer&lt;TKey, TValue&gt;](ConcurrentIndexer-2.md)

Implements [IReadOnlyDictionary&lt;TKey, TValue&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2), [IEnumerable&lt;KeyValuePair&lt;TKey, TValue&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1), [IEnumerable](https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable), [IReadOnlyCollection&lt;KeyValuePair&lt;TKey, TValue&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlycollection-1)

## Properties

### Item

```csharp
public TValue Item { get; set; }
```

> #### Property Value
> 
> `TValue`<br>
> 

### Keys

```csharp
public IEnumerable<TKey> Keys { get; }
```

> #### Property Value
> 
> `IEnumerable<TKey>`<br>
> 

### Values

```csharp
public IEnumerable<TValue> Values { get; }
```

> #### Property Value
> 
> `IEnumerable<TValue>`<br>
> 

### Count

```csharp
public Int32 Count { get; }
```

> #### Property Value
> 
> `Int32`<br>
> 

---

## Methods

### ContainsKey(TKey)

```csharp
public Boolean ContainsKey(TKey key)
```

> #### Parameters
> 
> key : `TKey`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### TryGetValue(TKey, TValue&)

```csharp
public Boolean TryGetValue(TKey key, TValue& value)
```

> #### Parameters
> 
> key : `TKey`<br>
> 
> value : `TValue&`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### GetEnumerator()

```csharp
public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
```

> #### Returns
> 
> IEnumerator<KeyValuePair<TKey, TValue>><br>
> 

---

[`< Index`](..\index.md)
