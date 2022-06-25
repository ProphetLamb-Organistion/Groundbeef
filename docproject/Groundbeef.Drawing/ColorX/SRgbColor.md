# SRgbColor

Namespace: Groundbeef.Drawing.ColorX

```csharp
public class SRgbColor : ColorBase<TColorData>, System.IEquatable<SRgbColor>
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ColorBase&lt;TColorData&gt;](ColorBase-1.md) → [SRgbColor](SRgbColor.md)

Implements [IConvertibleColor](IConvertibleColor.md), [IEquatable&lt;ColorBase&lt;TColorData&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.colorbase-1[[groundbeef.drawing.colorx.srgbcolordata, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]], groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]]), [IEquatable&lt;SRgbColor&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.srgbcolor, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]])

## Properties

### A

```csharp
public Byte A { get; set; }
```

> #### Property Value
> 
> `Byte`<br>
> 

### R

```csharp
public Single R { get; set; }
```

> #### Property Value
> 
> `Single`<br>
> 

### G

```csharp
public Single G { get; set; }
```

> #### Property Value
> 
> `Single`<br>
> 

### B

```csharp
public Single B { get; set; }
```

> #### Property Value
> 
> `Single`<br>
> 

### Data

```csharp
public SRgbColorData Data { get; }
```

> #### Property Value
> 
> `SRgbColorData`<br>
> 

---

## Constructors

### SRgbColor(Byte, Single, Single, Single)

```csharp
public SRgbColor(Byte a, Single r, Single g, Single b)
```

> #### Parameters
> 
> a : `Byte`<br>
> 
> r : `Single`<br>
> 
> g : `Single`<br>
> 
> b : `Single`<br>
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

### Equals(SRgbColor)

```csharp
public Boolean Equals(SRgbColor other)
```

> #### Parameters
> 
> other : `SRgbColor`<br>
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
internal AdobeRgbColorData DataToAdobeRgb()
```

> #### Returns
> 
> AdobeRgbColorData<br>
> 

### DataToCmyk()

```csharp
internal CmykColorData DataToCmyk()
```

> #### Returns
> 
> CmykColorData<br>
> 

### DataToHsl()

```csharp
internal HslColorData DataToHsl()
```

> #### Returns
> 
> HslColorData<br>
> 

### DataToHsv()

```csharp
internal HsvColorData DataToHsv()
```

> #### Returns
> 
> HsvColorData<br>
> 

### DataToRgb()

```csharp
internal RgbColorData DataToRgb()
```

> #### Returns
> 
> RgbColorData<br>
> 

### DataToSRgb()

```csharp
internal SRgbColorData DataToSRgb()
```

> #### Returns
> 
> SRgbColorData<br>
> 

### DataToScRgb()

```csharp
internal ScRgbColorData DataToScRgb()
```

> #### Returns
> 
> ScRgbColorData<br>
> 

### DataToXyz()

```csharp
internal XyzColorData DataToXyz()
```

> #### Returns
> 
> XyzColorData<br>
> 

### ToString()

```csharp
public String ToString()
```

> #### Returns
> 
> String<br>
> 

### ToString(ColorStyles)

```csharp
public String ToString(ColorStyles style)
```

> #### Parameters
> 
> style : `ColorStyles`<br>
> 
> #### Returns
> 
> String<br>
> 

---

[`< Index`](..\..\index.md)
