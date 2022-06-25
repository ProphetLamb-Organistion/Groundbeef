# HslColorData

Namespace: Groundbeef.Drawing.ColorX

```csharp
public struct HslColorData
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [HslColorData](HslColorData.md)

Implements [IEquatable&lt;HslColorData&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.hslcolordata, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]])

## Constructors

### HslColorData(Byte, Single, Single, Single)

```csharp
HslColorData(Byte a, Single h, Single s, Single l)
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

### Equals(HslColorData)

```csharp
Boolean Equals(HslColorData other)
```

> #### Parameters
> 
> other : `HslColorData`<br>
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

### ToSRgb(HslColorData&)

```csharp
SRgbColorData ToSRgb(HslColorData& hsl)
```

> #### Parameters
> 
> hsl : `HslColorData&`<br>
> 
> #### Returns
> 
> SRgbColorData<br>
> 

### FromSRgb(SRgbColorData&)

```csharp
HslColorData FromSRgb(SRgbColorData& sRgb)
```

> #### Parameters
> 
> sRgb : `SRgbColorData&`<br>
> 
> #### Returns
> 
> HslColorData<br>
> 

---

[`< Index`](..\..\index.md)
