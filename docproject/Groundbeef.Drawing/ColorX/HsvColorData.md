# HsvColorData

Namespace: Groundbeef.Drawing.ColorX

```csharp
public struct HsvColorData
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [HsvColorData](HsvColorData.md)

Implements [IEquatable&lt;HsvColorData&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.hsvcolordata, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]])

## Constructors

### HsvColorData(Byte, Single, Single, Single)

```csharp
HsvColorData(Byte a, Single h, Single s, Single v)
```

> #### Parameters
> 
> a : `Byte`<br>
> 
> h : `Single`<br>
> 
> s : `Single`<br>
> 
> v : `Single`<br>
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

### Equals(HsvColorData)

```csharp
Boolean Equals(HsvColorData other)
```

> #### Parameters
> 
> other : `HsvColorData`<br>
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

### ToSRgb(HsvColorData&)

```csharp
SRgbColorData ToSRgb(HsvColorData& hsv)
```

> #### Parameters
> 
> hsv : `HsvColorData&`<br>
> 
> #### Returns
> 
> SRgbColorData<br>
> 

### FromSRgb(SRgbColorData&)

```csharp
HsvColorData FromSRgb(SRgbColorData& sRgb)
```

> #### Parameters
> 
> sRgb : `SRgbColorData&`<br>
> 
> #### Returns
> 
> HsvColorData<br>
> 

---

[`< Index`](..\..\index.md)
