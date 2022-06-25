# ConcurrentDictionaryExtention

Namespace: Groundbeef.Collections.Concurrent

```csharp
public static class ConcurrentDictionaryExtention : System.Object
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ConcurrentDictionaryExtention](ConcurrentDictionaryExtention.md)

## Methods

### Add&lt;TKey, TValue&gt;(ConcurrentDictionary&lt;,&gt;, TKey&, TValue)

```csharp
public static Void Add<TKey, TValue>(ConcurrentDictionary<TKey, TValue> dictionary, TKey& key, TValue value)
```

> #### Type Parameters
> 
> `TKey`<br>
> 
> `TValue`<br>
> 
> #### Parameters
> 
> dictionary : `ConcurrentDictionary<,>`<br>
> 
> key : `TKey&`<br>
> 
> value : `TValue`<br>
> 
> #### Returns
> 
> Void<br>
> 

### Add&lt;TKey, TValue&gt;(ConcurrentDictionary&lt;,&gt;, KeyValuePair&lt;,&gt;)

```csharp
public static Void Add<TKey, TValue>(ConcurrentDictionary<TKey, TValue> dictionary, KeyValuePair keyValuePair)
```

> #### Type Parameters
> 
> `TKey`<br>
> 
> `TValue`<br>
> 
> #### Parameters
> 
> dictionary : `ConcurrentDictionary<,>`<br>
> 
> keyValuePair : `KeyValuePair<,>`<br>
> 
> #### Returns
> 
> Void<br>
> 

### Remove&lt;TKey, TValue&gt;(ConcurrentDictionary&lt;,&gt;, TKey&)

```csharp
public static Boolean Remove<TKey, TValue>(ConcurrentDictionary<TKey, TValue> dictionary, TKey& key)
```

> #### Type Parameters
> 
> `TKey`<br>
> 
> `TValue`<br>
> 
> #### Parameters
> 
> dictionary : `ConcurrentDictionary<,>`<br>
> 
> key : `TKey&`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### AddRange&lt;TKey, TValue&gt;(ConcurrentDictionary&lt;,&gt;, IEnumerable&lt;&gt;)

```csharp
public static Void AddRange<TKey, TValue>(ConcurrentDictionary<TKey, TValue> dictionary, IEnumerable keyValuePairs)
```

> #### Type Parameters
> 
> `TKey`<br>
> 
> `TValue`<br>
> 
> #### Parameters
> 
> dictionary : `ConcurrentDictionary<,>`<br>
> 
> keyValuePairs : `IEnumerable<>`<br>
> 
> #### Returns
> 
> Void<br>
> 

---

[`< Index`](..\index.md)
