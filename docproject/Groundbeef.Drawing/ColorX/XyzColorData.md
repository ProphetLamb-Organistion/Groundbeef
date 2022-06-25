# XyzColorData

Namespace: Groundbeef.Drawing.ColorX

```csharp
public struct XyzColorData
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [XyzColorData](XyzColorData.md)

Implements [IEquatable&lt;XyzColorData&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1[[groundbeef.drawing.colorx.xyzcolordata, groundbeef.drawing, version=1.0.0.0, culture=neutral, publickeytoken=null]])

## Constructors

### XyzColorData(Byte, Single, Single, Single)

```csharp
XyzColorData(Byte a, Single x, Single y, Single z)
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

### Equals(XyzColorData)

```csharp
Boolean Equals(XyzColorData other)
```

> #### Parameters
> 
> other : `XyzColorData`<br>
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

### ToScRgb(XyzColorData&)

```csharp
ScRgbColorData ToScRgb(XyzColorData& xyz)
```

> #### Parameters
> 
> xyz : `XyzColorData&`<br>
> 
> #### Returns
> 
> ScRgbColorData<br>
> 

### FromScRgb(ScRgbColorData&)

```csharp
XyzColorData FromScRgb(ScRgbColorData& scRgb)
```

> #### Parameters
> 
> scRgb : `ScRgbColorData&`<br>
> 
> #### Returns
> 
> XyzColorData<br>
> 

---

[`< Index`](..\..\index.md)
