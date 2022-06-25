# RgbColorData

Namespace: Groundbeef.Drawing.ColorX

```csharp
public struct RgbColorData
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [RgbColorData](RgbColorData.md)

Implements [IEquatable&lt;RgbColorData&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.rgbcolordata, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]])

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

---

## Constructors

### RgbColorData(Byte, Byte, Byte, Byte)

```csharp
RgbColorData(Byte a, Byte r, Byte g, Byte b)
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
Boolean Equals(Object obj)
```

> #### Parameters
> 
> obj : `Object`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### Equals(RgbColorData)

```csharp
Boolean Equals(RgbColorData other)
```

> #### Parameters
> 
> other : `RgbColorData`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### GetHashCode()

```csharp
Int32 GetHashCode()
```

> #### Returns
> 
> Int32<br>
> 

### FromSRgb(SRgbColorData&)

```csharp
RgbColorData FromSRgb(SRgbColorData& sRgb)
```

> #### Parameters
> 
> sRgb : `SRgbColorData&`<br>
> 
> #### Returns
> 
> RgbColorData<br>
> 

### ToSRgb(RgbColorData&)

```csharp
SRgbColorData ToSRgb(RgbColorData& rgb)
```

> #### Parameters
> 
> rgb : `RgbColorData&`<br>
> 
> #### Returns
> 
> SRgbColorData<br>
> 

---

[`< Index`](..\..\index.md)
