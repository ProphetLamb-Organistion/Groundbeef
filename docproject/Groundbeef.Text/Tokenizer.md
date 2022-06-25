# Tokenizer

Namespace: Groundbeef.Text

```csharp
public class Tokenizer : System.Object, ITokenizer
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Tokenizer](Tokenizer.md)

Implements [ITokenizer](ITokenizer.md)

## Properties

### Current

```csharp
public String Current { get; }
```

> #### Property Value
> 
> `String`<br>
> 

---

## Constructors

### Tokenizer(String&)

```csharp
public Tokenizer(String& text)
```

> #### Parameters
> 
> text : `String&`<br>
> 

### Tokenizer(String&, CharEqualityComparisions&)

```csharp
public Tokenizer(String& text, CharEqualityComparisions& mode)
```

> #### Parameters
> 
> text : `String&`<br>
> 
> mode : `CharEqualityComparisions&`<br>
> 

### Tokenizer(String&, IEqualityComparer&lt;&gt;)

```csharp
public Tokenizer(String& text, IEqualityComparer comparer)
```

> #### Parameters
> 
> text : `String&`<br>
> 
> comparer : `IEqualityComparer<>`<br>
> 

---

## Methods

### First(ReadOnlySpan&lt;&gt;)

```csharp
public Boolean First(ReadOnlySpan<Char> token)
```

> #### Parameters
> 
> token : `ReadOnlySpan<>`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### Next(ReadOnlySpan&lt;&gt;)

```csharp
public Boolean Next(ReadOnlySpan<Char> token)
```

> #### Parameters
> 
> token : `ReadOnlySpan<>`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### Reset()

```csharp
public Void Reset()
```

> #### Returns
> 
> Void<br>
> 

---

[`< Index`](..\index.md)
