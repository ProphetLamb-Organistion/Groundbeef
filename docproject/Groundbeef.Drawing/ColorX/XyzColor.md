# XyzColor

Namespace: Groundbeef.Drawing.ColorX

```csharp
public class XyzColor : ColorBase<TColorData>, System.IEquatable<XyzColor>
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ColorBase&lt;TColorData&gt;](ColorBase-1.md) → [XyzColor](XyzColor.md)

Implements [IConvertibleColor](IConvertibleColor.md), [IEquatable&lt;ColorBase&lt;TColorData&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.colorbase-1[[groundbeef.drawing.colorx.xyzcolordata, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]], groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]]), [IEquatable&lt;XyzColor&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.xyzcolor, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]])

## Properties

### A

```csharp
public Byte A { get; set; }
```

> #### Property Value
> 
> `Byte`<br>
> 

### X

```csharp
public Single X { get; set; }
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

### Z

```csharp
public Single Z { get; set; }
```

> #### Property Value
> 
> `Single`<br>
> 

### Data

```csharp
public XyzColorData Data { get; }
```

> #### Property Value
> 
> `XyzColorData`<br>
> 

---

## Constructors

### XyzColor(Byte, Single, Single, Single)

```csharp
public XyzColor(Byte a, Single x, Single y, Single z)
```

> #### Parameters
> 
> a : `Byte`<br>
> 
> x : `Single`<br>
> 
> y : `Single`<br>
> 
> z : `Single`<br>
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

### Equals(XyzColor)

```csharp
public Boolean Equals(XyzColor other)
```

> #### Parameters
> 
> other : `XyzColor`<br>
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

---

[`< Index`](..\..\index.md)
