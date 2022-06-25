# RgbColor

Namespace: Groundbeef.Drawing.ColorX

```csharp
public class RgbColor : ColorBase<TColorData>, System.IEquatable<RgbColor>
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ColorBase&lt;TColorData&gt;](ColorBase-1.md) → [RgbColor](RgbColor.md)

Implements [IConvertibleColor](IConvertibleColor.md), [IEquatable&lt;ColorBase&lt;TColorData&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.colorbase-1[[groundbeef.drawing.colorx.rgbcolordata, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]], groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]]), [IEquatable&lt;RgbColor&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.rgbcolor, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]])

## Properties

### ARGB

```csharp
public UInt32 ARGB { get; set; }
```

> #### Property Value
> 
> `UInt32`<br>
> 

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
public Byte R { get; set; }
```

> #### Property Value
> 
> `Byte`<br>
> 

### G

```csharp
public Byte G { get; set; }
```

> #### Property Value
> 
> `Byte`<br>
> 

### B

```csharp
public Byte B { get; set; }
```

> #### Property Value
> 
> `Byte`<br>
> 

### Data

```csharp
public RgbColorData Data { get; }
```

> #### Property Value
> 
> `RgbColorData`<br>
> 

---

## Constructors

### RgbColor(Byte, Byte, Byte, Byte)

```csharp
public RgbColor(Byte a, Byte r, Byte g, Byte b)
```

> #### Parameters
> 
> a : `Byte`<br>
> 
> r : `Byte`<br>
> 
> g : `Byte`<br>
> 
> b : `Byte`<br>
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

### Equals(RgbColor)

```csharp
public Boolean Equals(RgbColor other)
```

> #### Parameters
> 
> other : `RgbColor`<br>
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

### DataToScRgb()

```csharp
internal ScRgbColorData DataToScRgb()
```

> #### Returns
> 
> ScRgbColorData<br>
> 

### DataToSRgb()

```csharp
internal SRgbColorData DataToSRgb()
```

> #### Returns
> 
> SRgbColorData<br>
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
