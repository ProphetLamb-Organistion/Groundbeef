# HslColor

Namespace: Groundbeef.Drawing.ColorX

```csharp
public class HslColor : ColorBase<TColorData>, System.IEquatable<HslColor>
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ColorBase&lt;TColorData&gt;](ColorBase-1.md) → [HslColor](HslColor.md)

Implements [IConvertibleColor](IConvertibleColor.md), [IEquatable&lt;ColorBase&lt;TColorData&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.colorbase-1[[groundbeef.drawing.colorx.hslcolordata, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]], groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]]), [IEquatable&lt;HslColor&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.hslcolor, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]])

## Properties

### A

```csharp
public Byte A { get; set; }
```

> #### Property Value
> 
> `Byte`<br>
> 

### H

```csharp
public Single H { get; set; }
```

> #### Property Value
> 
> `Single`<br>
> 

### S

```csharp
public Single S { get; set; }
```

> #### Property Value
> 
> `Single`<br>
> 

### L

```csharp
public Single L { get; set; }
```

> #### Property Value
> 
> `Single`<br>
> 

### Data

```csharp
public HslColorData Data { get; }
```

> #### Property Value
> 
> `HslColorData`<br>
> 

---

## Constructors

### HslColor(Byte, Single, Single, Single)

```csharp
public HslColor(Byte a, Single h, Single s, Single l)
```

> #### Parameters
> 
> a : `Byte`<br>
> 
> h : `Single`<br>
> 
> s : `Single`<br>
> 
> l : `Single`<br>
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

### Equals(HslColor)

```csharp
public Boolean Equals(HslColor other)
```

> #### Parameters
> 
> other : `HslColor`<br>
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

---

[`< Index`](..\..\index.md)
