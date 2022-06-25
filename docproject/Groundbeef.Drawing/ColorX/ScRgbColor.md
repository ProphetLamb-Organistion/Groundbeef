# ScRgbColor

Namespace: Groundbeef.Drawing.ColorX

```csharp
public class ScRgbColor : ColorBase<TColorData>, System.IEquatable<ScRgbColor>
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ColorBase&lt;TColorData&gt;](ColorBase-1.md) → [ScRgbColor](ScRgbColor.md)

Implements [IConvertibleColor](IConvertibleColor.md), [IEquatable&lt;ColorBase&lt;TColorData&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.colorbase-1[[groundbeef.drawing.colorx.scrgbcolordata, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]], groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]]), [IEquatable&lt;ScRgbColor&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.scrgbcolor, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]])

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
public ScRgbColorData Data { get; }
```

> #### Property Value
> 
> `ScRgbColorData`<br>
> 

---

## Constructors

### ScRgbColor(Byte, Single, Single, Single)

```csharp
public ScRgbColor(Byte a, Single r, Single g, Single b)
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

### Equals(ScRgbColor)

```csharp
public Boolean Equals(ScRgbColor other)
```

> #### Parameters
> 
> other : `ScRgbColor`<br>
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
