# TypeExtention

Namespace: Groundbeef.Reflection

```csharp
public static class TypeExtention : System.Object
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [TypeExtention](TypeExtention.md)

## Methods

### HasInterface&lt;T&gt;(Type, Boolean)

```csharp
public static Boolean HasInterface<T>(Type type, Boolean preferGenericTypeDefinition)
```

> #### Type Parameters
> 
> `T`<br>
> 
> #### Parameters
> 
> type : `Type`<br>
> 
> preferGenericTypeDefinition : `Boolean`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### HasInterface(Type, Type, Boolean)

```csharp
public static Boolean HasInterface(Type type, Type interfaceType, Boolean preferGenericTypeDefinition)
```

> #### Parameters
> 
> type : `Type`<br>
> 
> interfaceType : `Type`<br>
> 
> preferGenericTypeDefinition : `Boolean`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### GetEnumerableElementType(Type&)

```csharp
public static Type GetEnumerableElementType(Type& type)
```

> #### Parameters
> 
> type : `Type&`<br>
> 
> #### Returns
> 
> Type<br>
> 

### WithAttribute&lt;TAttribute&gt;(IEnumerable&lt;&gt;)

```csharp
public static IEnumerable<MethodInfo> WithAttribute<TAttribute>(IEnumerable<MethodInfo> methods)
```

> #### Type Parameters
> 
> `TAttribute`<br>
> 
> #### Parameters
> 
> methods : `IEnumerable<>`<br>
> 
> #### Returns
> 
> IEnumerable<MethodInfo><br>
> 

### WithAttribute(IEnumerable&lt;&gt;, Type)

```csharp
public static IEnumerable<MethodInfo> WithAttribute(IEnumerable<MethodInfo> methods, Type attributeType)
```

> #### Parameters
> 
> methods : `IEnumerable<>`<br>
> 
> attributeType : `Type`<br>
> 
> #### Returns
> 
> IEnumerable<MethodInfo><br>
> 

---

[`< Index`](..\index.md)
