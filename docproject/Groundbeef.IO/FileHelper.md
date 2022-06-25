# FileHelper

Namespace: Groundbeef.IO

```csharp
public static class FileHelper : System.Object
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [FileHelper](FileHelper.md)

## Methods

### Create(String)

```csharp
public static FileStream Create(String fileName)
```

> #### Parameters
> 
> fileName : `String`<br>
> 
> #### Returns
> 
> FileStream<br>
> 

### Open(String)

```csharp
public static FileStream Open(String fileName)
```

> #### Parameters
> 
> fileName : `String`<br>
> 
> #### Returns
> 
> FileStream<br>
> 

### Open(String, Boolean)

```csharp
public static FileStream Open(String fileName, Boolean create)
```

> #### Parameters
> 
> fileName : `String`<br>
> 
> create : `Boolean`<br>
> 
> #### Returns
> 
> FileStream<br>
> 

### IsDirectory(String)

```csharp
public static Boolean IsDirectory(String filePath)
```

> #### Parameters
> 
> filePath : `String`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### IsDirectory(FileInfo)

```csharp
public static Boolean IsDirectory(FileInfo fileInfo)
```

> #### Parameters
> 
> fileInfo : `FileInfo`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### GetFileHash(String, HashAlgorithm)

```csharp
public static Byte[] GetFileHash(String fileName, HashAlgorithm hashAlgorithm)
```

> #### Parameters
> 
> fileName : `String`<br>
> 
> hashAlgorithm : `HashAlgorithm`<br>
> 
> #### Returns
> 
> Byte[]<br>
> 

### IsRelativePath(ReadOnlySpan&lt;&gt;)

```csharp
public static Boolean IsRelativePath(ReadOnlySpan path)
```

> #### Parameters
> 
> path : `ReadOnlySpan<>`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

### IsRelativePath(ReadOnlySpan&lt;&gt;, Int32, Int32&)

```csharp
internal static Boolean IsRelativePath(ReadOnlySpan path, Int32 startIndex, Int32& relativeIndicatorEndIndex)
```

> #### Parameters
> 
> path : `ReadOnlySpan<>`<br>
> 
> startIndex : `Int32`<br>
> 
> relativeIndicatorEndIndex : `Int32&`<br>
> 
> #### Returns
> 
> Boolean<br>
> 

---

[`< Index`](..\index.md)
