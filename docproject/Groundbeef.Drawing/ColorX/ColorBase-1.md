# ColorBase&lt;&gt;

Namespace: Groundbeef.Drawing.ColorX

```csharp
public abstract class ColorBase<TColorData> : System.Object, IConvertibleColor, System.IEquatable<ColorBase<TColorData>>
```

#### Type Parameters

`TColorData`<br>

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ColorBase&lt;TColorData&gt;](ColorBase-1.md)

Implements [IConvertibleColor](IConvertibleColor.md), [IEquatable&lt;ColorBase&lt;TColorData&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1)

## Properties

### Data

```csharp
public TColorData Data { get; }
```

> #### Property Value
> 
> `TColorData`<br>
> 

---

## Methods

### Equals(Object)

```csharp
public Boolean Equals(Object obj)
```

> #### Parameters
> 
> obj : `Object`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### Equals(ColorBase&lt;&gt;)

```csharp
public Boolean Equals(ColorBase<TColorData> other)
```

> #### Parameters
> 
> other : `ColorBase<>`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### GetHashCode()

```csharp
public Int32 GetHashCode()
```

> #### Returns
> 
> Int32<br>
> 

### DataToAdobeRgb()

```csharp
internal abstract AdobeRgbColorData DataToAdobeRgb()
```

> #### Returns
> 
> AdobeRgbColorData<br>
> 

### DataToCmyk()

```csharp
internal abstract CmykColorData DataToCmyk()
```

> #### Returns
> 
> CmykColorData<br>
> 

### DataToHsl()

```csharp
internal abstract HslColorData DataToHsl()
```

> #### Returns
> 
> HslColorData<br>
> 

### DataToHsv()

```csharp
internal abstract HsvColorData DataToHsv()
```

> #### Returns
> 
> HsvColorData<br>
> 

### DataToRgb()

```csharp
internal abstract RgbColorData DataToRgb()
```

> #### Returns
> 
> RgbColorData<br>
> 

### DataToScRgb()

```csharp
internal abstract ScRgbColorData DataToScRgb()
```

> #### Returns
> 
> ScRgbColorData<br>
> 

### DataToSRgb()

```csharp
internal abstract SRgbColorData DataToSRgb()
```

> #### Returns
> 
> SRgbColorData<br>
> 

### DataToXyz()

```csharp
internal abstract XyzColorData DataToXyz()
```

> #### Returns
> 
> XyzColorData<br>
> 

### ToAdobeRgb()

```csharp
public AdobeRgbColor ToAdobeRgb()
```

> #### Returns
> 
> AdobeRgbColor<br>
> 

### ToCmyk()

```csharp
public CmykColor ToCmyk()
```

> #### Returns
> 
> CmykColor<br>
> 

### ToHsl()

```csharp
public HslColor ToHsl()
```

> #### Returns
> 
> HslColor<br>
> 

### ToHsv()

```csharp
public HsvColor ToHsv()
```

> #### Returns
> 
> HsvColor<br>
> 

### ToRgb()

```csharp
public RgbColor ToRgb()
```

> #### Returns
> 
> RgbColor<br>
> 

### ToScRgb()

```csharp
public ScRgbColor ToScRgb()
```

> #### Returns
> 
> ScRgbColor<br>
> 

### ToSRgb()

```csharp
public SRgbColor ToSRgb()
```

> #### Returns
> 
> SRgbColor<br>
> 

### ToXyz()

```csharp
public XyzColor ToXyz()
```

> #### Returns
> 
> XyzColor<br>
> 

---

[`< Index`](..\..\index.md)
