# CmykColorData

Namespace: Groundbeef.Drawing.ColorX

```csharp
public struct CmykColorData
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [CmykColorData](CmykColorData.md)

Implements [IEquatable&lt;CmykColorData&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.cmykcolordata, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]])

## Constructors

### CmykColorData(Byte, Single, Single, Single, Single)

```csharp
CmykColorData(Byte a, Single c, Single m, Single y, Single k)
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

### Equals(CmykColorData)

```csharp
Boolean Equals(CmykColorData other)
```

> #### Parameters
> 
> other : `CmykColorData`<br>
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

### ToSRgb(CmykColorData&)

```csharp
SRgbColorData ToSRgb(CmykColorData& cmyk)
```

> #### Parameters
> 
> cmyk : `CmykColorData&`<br>
> 
> #### Returns
> 
> SRgbColorData<br>
> 

### FromSRgb(SRgbColorData&)

```csharp
CmykColorData FromSRgb(SRgbColorData& sRgb)
```

> #### Parameters
> 
> sRgb : `SRgbColorData&`<br>
> 
> #### Returns
> 
> CmykColorData<br>
> 

---

[`< Index`](..\..\index.md)
