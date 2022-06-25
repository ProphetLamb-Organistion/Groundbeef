# CmykColor

Namespace: Groundbeef.Drawing.ColorX

```csharp
public class CmykColor : ColorBase<TColorData>, System.IEquatable<CmykColor>
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ColorBase&lt;TColorData&gt;](ColorBase-1.md) → [CmykColor](CmykColor.md)

Implements [IConvertibleColor](IConvertibleColor.md), [IEquatable&lt;ColorBase&lt;TColorData&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.colorbase-1[[groundbeef.drawing.colorx.cmykcolordata, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]], groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]]), [IEquatable&lt;CmykColor&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.cmykcolor, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]])

## Properties

### A

```csharp
public Byte A { get; set; }
```

> #### Property Value
> 
> `Byte`<br>
> 

### C

```csharp
public Single C { get; set; }
```

> #### Property Value
> 
> `Single`<br>
> 

### M

```csharp
public Single M { get; set; }
```

> #### Property Value
> 
> `Single`<br>
> 

### Y

```csharp
public Single Y { get; set; }
```

> #### Property Value
> 
> `Single`<br>
> 

### K

```csharp
public Single K { get; set; }
```

> #### Property Value
> 
> `Single`<br>
> 

### Data

```csharp
public CmykColorData Data { get; }
```

> #### Property Value
> 
> `CmykColorData`<br>
> 

---

## Constructors

### CmykColor(Byte, Single, Single, Single, Single)

```csharp
public CmykColor(Byte a, Single c, Single m, Single y, Single k)
```

> #### Parameters
> 
> a : `Byte`<br>
> 
> c : `Single`<br>
> 
> m : `Single`<br>
> 
> y : `Single`<br>
> 
> k : `Single`<br>
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

### Equals(CmykColor)

```csharp
public Boolean Equals(CmykColor other)
```

> #### Parameters
> 
> other : `CmykColor`<br>
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
