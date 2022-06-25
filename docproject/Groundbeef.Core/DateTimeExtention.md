# DateTimeExtention

Namespace: Groundbeef.Core

```csharp
public static class DateTimeExtention : System.Object
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [DateTimeExtention](DateTimeExtention.md)

## Methods

### FirstOfWeek(DateTime, DayOfWeek)

```csharp
public static DateTime FirstOfWeek(DateTime dt, DayOfWeek startOfWeek)
```

> #### Parameters
> 
> dt : `DateTime`<br>
> 
> startOfWeek : `DayOfWeek`<br>
> 
> #### Returns
> 
> DateTime<br>
> 

### LastOfWeek(DateTime, DayOfWeek)

```csharp
public static DateTime LastOfWeek(DateTime dt, DayOfWeek endOfWeek)
```

> #### Parameters
> 
> dt : `DateTime`<br>
> 
> endOfWeek : `DayOfWeek`<br>
> 
> #### Returns
> 
> DateTime<br>
> 

### FirstOfMonth(DateTime)

```csharp
public static DateTime FirstOfMonth(DateTime dt)
```

> #### Parameters
> 
> dt : `DateTime`<br>
> 
> #### Returns
> 
> DateTime<br>
> 

### LastOfMonth(DateTime)

```csharp
public static DateTime LastOfMonth(DateTime dt)
```

> #### Parameters
> 
> dt : `DateTime`<br>
> 
> #### Returns
> 
> DateTime<br>
> 

### GetTimzonesBetween(DateTimeOffset, DateTimeOffset, Boolean)

```csharp
public static IEnumerable<TimeZoneInfo> GetTimzonesBetween(DateTimeOffset offset1, DateTimeOffset offset2, Boolean distinct)
```

> #### Parameters
> 
> offset1 : `DateTimeOffset`<br>
> 
> offset2 : `DateTimeOffset`<br>
> 
> distinct : `Boolean`<br>
> 
> #### Returns
> 
> IEnumerable<TimeZoneInfo><br>
> 

### GetTimzonesBetween(TimeSpan, TimeSpan, Boolean)

```csharp
public static IEnumerable<TimeZoneInfo> GetTimzonesBetween(TimeSpan utcOffset1, TimeSpan utcOffset2, Boolean distinct)
```

> #### Parameters
> 
> utcOffset1 : `TimeSpan`<br>
> 
> utcOffset2 : `TimeSpan`<br>
> 
> distinct : `Boolean`<br>
> 
> #### Returns
> 
> IEnumerable<TimeZoneInfo><br>
> 

### GetTimzonesBetween(Int32, Int32, Boolean)

```csharp
public static IEnumerable<TimeZoneInfo> GetTimzonesBetween(Int32 utcOffset1, Int32 utcOffset2, Boolean distinct)
```

> #### Parameters
> 
> utcOffset1 : `Int32`<br>
> 
> utcOffset2 : `Int32`<br>
> 
> distinct : `Boolean`<br>
> 
> #### Returns
> 
> IEnumerable<TimeZoneInfo><br>
> 

---

[`< Index`](..\index.md)
