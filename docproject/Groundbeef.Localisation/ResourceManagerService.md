# ResourceManagerService

Namespace: Groundbeef.Localisation

```csharp
public static class ResourceManagerService : System.Object
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ResourceManagerService](ResourceManagerService.md)

## Properties

### CurrentLocale

```csharp
public static Locale CurrentLocale { get; private set; }
```

> #### Property Value
> 
> `Locale`<br>
> 

---

## Methods

### GetResourceString(String&, String&)

```csharp
public static String GetResourceString(String& managerName, String& resourceKey)
```

> #### Parameters
> 
> managerName : `String&`<br>
> 
> resourceKey : `String&`<br>
> 
> #### Returns
> 
> String<br>
> 

### ChangeLocale(String&)

```csharp
public static Void ChangeLocale(String& newLocaleName)
```

> #### Parameters
> 
> newLocaleName : `String&`<br>
> 
> #### Returns
> 
> Void<br>
> 

### Refresh()

```csharp
public static Void Refresh()
```

> #### Returns
> 
> Void<br>
> 

### RegisterManager(String&, ResourceManager&)

```csharp
public static Void RegisterManager(String& managerName, ResourceManager& manager)
```

> #### Parameters
> 
> managerName : `String&`<br>
> 
> manager : `ResourceManager&`<br>
> 
> #### Returns
> 
> Void<br>
> 

### RegisterManager(String&, ResourceManager&, Boolean)

```csharp
public static Void RegisterManager(String& managerName, ResourceManager& manager, Boolean refresh)
```

> #### Parameters
> 
> managerName : `String&`<br>
> 
> manager : `ResourceManager&`<br>
> 
> refresh : `Boolean`<br>
> 
> #### Returns
> 
> Void<br>
> 

### UnregisterManager(String&)

```csharp
public static Void UnregisterManager(String& name)
```

> #### Parameters
> 
> name : `String&`<br>
> 
> #### Returns
> 
> Void<br>
> 

---

## Events

### LocaleChanged

```csharp
public static event ValueChangedEventHandler<TValue> LocaleChanged;
```

---

[`< Index`](..\index.md)
