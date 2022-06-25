# CollectionsReflect

Namespace: Groundbeef.Reflection

```csharp
public static class CollectionsReflect : System.Object
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [CollectionsReflect](CollectionsReflect.md)

## Methods

### Cast(IEnumerable, Type)

```csharp
public static IEnumerable Cast(IEnumerable source, Type resultType)
```

> #### Parameters
> 
> source : `IEnumerable`<br>
> 
> resultType : `Type`<br>
> 
> #### Returns
> 
> IEnumerable<br>
> 

### ToGenericList(Object&, Type&)

```csharp
public static Object ToGenericList(Object& enumerable, Type& sourceType)
```

> #### Parameters
> 
> enumerable : `Object&`<br>
> 
> sourceType : `Type&`<br>
> 
> #### Returns
> 
> Object<br>
> 

### ToGenericArray(Object&, Type&)

```csharp
public static Object ToGenericArray(Object& enumerable, Type& sourceType)
```

> #### Parameters
> 
> enumerable : `Object&`<br>
> 
> sourceType : `Type&`<br>
> 
> #### Returns
> 
> Object<br>
> 

### MakeToListMethod(Type&)

```csharp
internal static Invoker MakeToListMethod(Type& sourceType)
```

> #### Parameters
> 
> sourceType : `Type&`<br>
> 
> #### Returns
> 
> Invoker<br>
> 

### MakeToArrayMethod(Type&)

```csharp
internal static Invoker MakeToArrayMethod(Type& sourceType)
```

> #### Parameters
> 
> sourceType : `Type&`<br>
> 
> #### Returns
> 
> Invoker<br>
> 

### MakeList(Type)

```csharp
public static IList MakeList(Type elementType)
```

> #### Parameters
> 
> elementType : `Type`<br>
> 
> #### Returns
> 
> IList<br>
> 

### MakeList(Type, Int32)

```csharp
public static IList MakeList(Type elementType, Int32 capacity)
```

> #### Parameters
> 
> elementType : `Type`<br>
> 
> capacity : `Int32`<br>
> 
> #### Returns
> 
> IList<br>
> 

### MakeList(Type, IEnumerable)

```csharp
public static IList MakeList(Type elementType, IEnumerable collection)
```

> #### Parameters
> 
> elementType : `Type`<br>
> 
> collection : `IEnumerable`<br>
> 
> #### Returns
> 
> IList<br>
> 

---

[`< Index`](..\index.md)
