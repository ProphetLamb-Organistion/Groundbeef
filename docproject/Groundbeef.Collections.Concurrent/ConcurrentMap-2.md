# ConcurrentMap&lt;,&gt;

Namespace: Groundbeef.Collections.Concurrent

```csharp
public class ConcurrentMap<TForward, TReverse> : System.Object, System.Collections.Generic.IEnumerable<KeyValuePair<TForward, TReverse>>, System.Collections.IEnumerable
```

#### Type Parameters

`TForward`<br>

`TReverse`<br>

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ConcurrentMap&lt;TForward, TReverse&gt;](ConcurrentMap-2.md)

Implements [IEnumerable&lt;KeyValuePair&lt;TForward, TReverse&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1), [IEnumerable](https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable)

## Properties

### Forward

```csharp
public ConcurrentIndexer<TKey, TValue> Forward { get; }
```

> #### Property Value
> 
> `ConcurrentIndexer<TKey, TValue>`<br>
> 

### Reverse

```csharp
public ConcurrentIndexer<TKey, TValue> Reverse { get; }
```

> #### Property Value
> 
> `ConcurrentIndexer<TKey, TValue>`<br>
> 

---

## Constructors

### ConcurrentMap()

```csharp
public ConcurrentMap()
```

> 

---

## Methods

### Add(TForward&, TReverse&)

```csharp
public Void Add(TForward& value1, TReverse& value2)
```

> #### Parameters
> 
> value1 : `TForward&`<br>
> 
> value2 : `TReverse&`<br>
> 
> #### Returns
> 
> Void<br>
> 

### Add(KeyValuePair&lt;,&gt;)

```csharp
public Void Add(KeyValuePair value)
```

> #### Parameters
> 
> value : `KeyValuePair<,>`<br>
> 
> #### Returns
> 
> Void<br>
> 

### GetEnumerator()

```csharp
public IEnumerator<KeyValuePair<TForward, TReverse>> GetEnumerator()
```

> #### Returns
> 
> IEnumerator<KeyValuePair<TForward, TReverse>><br>
> 

---

[`< Index`](..\index.md)
