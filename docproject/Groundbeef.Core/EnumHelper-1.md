# EnumHelper&lt;&gt;

Namespace: Groundbeef.Core

```csharp
public static class EnumHelper<T> : System.Object
```

#### Type Parameters

`T`<br>

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [EnumHelper&lt;T&gt;](EnumHelper-1.md)

## Methods

### GetValues()

```csharp
public static T[] GetValues()
```

> #### Returns
> 
> T[]<br>
> 

### Parse(ReadOnlySpan&lt;&gt;)

```csharp
public static T Parse(ReadOnlySpan value)
```

> #### Parameters
> 
> value : `ReadOnlySpan<>`<br>
> 
> #### Returns
> 
> T<br>
> 

### Parse(String&)

```csharp
public static T Parse(String& value)
```

> #### Parameters
> 
> value : `String&`<br>
> 
> #### Returns
> 
> T<br>
> 

### GetNames()

```csharp
public static IEnumerable<String> GetNames()
```

> #### Returns
> 
> IEnumerable<String><br>
> 

### GetDisplayValues()

```csharp
public static IEnumerable<String> GetDisplayValues()
```

> #### Returns
> 
> IEnumerable<String><br>
> 

### GetDisplayValue(T)

```csharp
public static String GetDisplayValue(T value)
```

> #### Parameters
> 
> value : `T`<br>
> 
> #### Returns
> 
> String<br>
> 

### GetDisplayValue(T, CultureInfo)

```csharp
public static String GetDisplayValue(T value, CultureInfo cultureInfo)
```

> #### Parameters
> 
> value : `T`<br>
> 
> cultureInfo : `CultureInfo`<br>
> 
> #### Returns
> 
> String<br>
> 

---

[`< Index`](..\index.md)
