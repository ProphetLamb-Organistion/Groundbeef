# Groundbeef

Collection of personal tools and utility functionality written in C# using .NET Standard 2.1, .NET Core 3.1, WPF, and Nullable context.

## Table of contents

### Core (.NET Standard)

**AOT**
Functionallity to preload an assembly or a specific method.

**CultureInfo**

VerifyCultureName: Verifes that the culture name (en_US etc.) of a given culture is valid, optionally throws an exception.

**DateTime**

* StartOfWeek: Returns the first day in the week at 00:00:00.000 relative the the provided DateTime.
* EndOfWeek: Returns the last day in the week at 23:59:59.999 relative to the provided DateTime.
* StartOfMonth: Return the first day in the month at 00:00:00.000 relative to the provided DateTime.
* EndOfMonth: Return the last day in the month at 23:59:59.999 relative to the provided DateTime.
* GetTimzonesBetween: Enumerates all timezones between the UTC timezone offsets.

**EnumHelper**

* GetValues: Returns a generic list of all values of the Enum.
* Parse: Parses a enum value from a string.
* GetNames: Returns a list of string represenations of all values of the Enum.
* GetDisplayValues: Returns a list of the displayvalue-attribute strings of all values of the Enum.
* GetDisplayValue: Returns the value of the displayvalue-attribute (with the specified culture).

**EnumExtentions**
* HasAnyFlag: Indicates whether a enum value has any flags.
* HasAllFlags: Indicates whether a enum value has all flags.

**Int32Range**
Simpler implementation of a range structure then System.Range

**MathHelper**

IsPowerOfTwo: Indicates whether the value is a power of two.
RoundToInt: Rounds the value to the nearest 32/64bit signed integer.
MinMax(params): Returns the minimum and maximum value of the provided value.
Max(params): Returns the highest value of the provided values.
Min(params): Returns the lowerst value of the provided values.

**TaskHelper**

* STAThreadTask
  Runs a task in STA thread instead of MTA.
* Task Collections
  Extention method to Task arrays and lists adding Task.WaitAll, Task.WaitAny, Task.WhenAll, and Task.WhenAny.

**ObjectExtention**

* Cast: Casts the object to the desired type.
* Convert: Converts the object the desired type using Convert.ChangeType.
* Guid Combine: Creates a new Guid by combining a Guid with another. By appling the XOR operation to the first and last 8 bytes, of the 16-element byte arrays, crosswise.
* ThrowOnNull: Throws an ArgumentNullException if the object is null.
* DerefOrThrow: Dereferences the Nullable object, if the object is null throws a ArgumentNullException; otherwise returns the dereferenced object.

### Collections (.NET Standard)

#### Generic

**Spans**

* SortByKeys: Sorts a one-dimesional array into a new array by swapping each element to the index indicated by keys without shuffeling the keys.
  Returns the sorted array.
* Span optimized predicate logic: FindFirst, FindLast, FindAll, IndexOf, IndexOfLast, IndexOfAll
  Find and return the requested element or index of that element using a match predicate.
* GetHashCode: Additional parameter: fromValues, if true:
  Generates the hashcode of the array or span using the GetHashCode method of thier elements instead and combines these into one.

**SpanSplitEnumerator**
Based on https://github.com/dotnet/runtime/issues/934.

* Split: Enumerates slices of a portion of a Span separated by a specific separator.
* SplitWhere: Enumerates slices of a portion of a Span separated when a specified condition is met.

**EnumerableExtentions**

* QuickCount: Returns the number of elements in a sequence, attempts to cast the enumerable to a ICollection.
* CastList: Casts all elements in the IList to the type specified.
* Partition: Partitions the IEnumerable<T> by the partitioner.

**IPartitionedEnumerator**
```
public interface IPartitionedEnumerator<T> : IEnumerator<T>
{
    bool IsSatisfied { get; }
    bool MoveNextSatisfied();
    bool MoveNextFalsified();
    IEnumerator<T> SynchronizedSatisfiedEnumerator();
    IEnumerator<T> SynchronizedFalisifiedEnumerator();
}
```

**IPartitionedEnumerable**
```
public interface IPartitionedEnumerable<T> : IEnumerable<T>
{
    new IPartitionedEnumerator<T> GetEnumerator();
}
```

**PartitionedEnumerator | PartitionedEnumerable**
Enumerates a sequence, partitioning elements based on a condition.

**IRange**
```
public interface IRange
{
    object? Minimum { get; }
    object? Maximum { get; }
    bool HasValue { get; }
    
    bool Contains(object value);
}
```

**Range**
Generic Range valuetype

* Unify: Returns a new Range<T> unifing the Range<T> with another.
* Expand: Returns a new Range<T> encompassing the value.
* Intersects: Indicates whether the other Range<T> intersects with this instance.
* Contains: Indicates whether the value is contained widthin the Range<T>.

**RangeExtention**

* ToIntRange: Returns a Range<Int32> with the Minumum equal to the Range.Start, and the Maximum equal to the Range.End. 
* Cast<T>: Casts the IRange to the specified type.
* ToIndexedRange: Returns a new Range with the Start equal to Range<Int32>.Minimum, and the End equal to Range<Int32>.Maximum.

**Arrays**

* SortByKeys: Sorts a one-dimesional array into a new array by swapping each element to the index indicated by keys without shuffeling the keys.
  Returns the sorted array.
* GetRange: Returns a array containing a portion of the elements in the array.
* Array optimized predicate logic: FindFirst, FindLast, FindAll, IndexOf, IndexOfLast, IndexOfAll
  Find and return the requested element or index of that element using a match predicate.
* GetHashCode: Additional parameter: fromValues, if true:
  Generates the hashcode of the array or span using the GetHashCode method of thier elements instead and combines these into one.

**Collections**

* AddRange: Adds a range of elements to the collection by repeatetly calling the Collection.Add function.
* Collection<DateTime>.AddDays: Adds all days between two specific dates to the collection.
* Filter: Adds and Removes elements from the target collection so that it only contains elements from source that match the predicate filter.
* Arraylist optimized predicate logic: FindFirst, FindLast, FindAll, IndexOf, IndexOfLast, IndexOfAll
  Find and return the requested element or index of that element using a match predicate.
* Sort: Sorts the elements in the collection using an introspective sort algorithm.

**Dictionary**

* AddRange: Adds a range of keyvalue-pairs to the dictionary.
* Add(KeyValuePair): Adds the specified key and value to the dictionary
* GetDictionaryEnumerator: Returns an DictionaryEnumerator that iterates through the collection.
* DictionaryEnumerator: Implementation of IDictionaryEnumerator.

**Enumerable**(non generic)

* Count: Returns the number of elements in a sequence.

**GenericCollectionConversion**

* ToGenericList: Converts the generic IEnumerable to a generic List of the same element type.
* ToGenericArray: Converts the generic IEnumerable to a generic Array of the same element type.

**Map**
A bi-directionally accessible dictionary implementation.

* Indexer: A wrapper class of Dictionary tailored for the needs of Map.

**EqualityComparison**
Delegate indicating whether two values are equal
```
public delegate bool EqualityComparison<T>(T left, T right);
```

* ToComparer: Returns a new instance of a GenericEqualityComparer{T} with the specified comparison.

**HashCodeFunction**
Delegate returning the hashcode of a value using a specific function.
```
public delegate int HashCodeFunction<T>(T value);
```

**GenericEqualityComparer**
Warpper class for lamba EqualityComparison functions.

#### Concurrent

**Concurrent dictionary**
The purpose here is to streamline the code produced for Dictionarys with ConcurrentDictionaries

* Add(key, value)(keyvalue-pair): Adds the specified key and value to the concurrent dictionary. If the key already exists overwrites the exisiting value.
  Calls AddOrUpdate.
* Remove(key): Removes the value with the specified key from the ConcurrentDictionary.
* AddRange: Adds a range of keyvalue-pairs to the dictionary.

### BinaryEncoding (.NET Standard)

**Base85**
Encodes and decodes base85 strings to bytes.

### Drawing (.NET Standard)

**ColorExtention**

* ToRgb: Converts the color to an RgbColor
* ToSRgb: Converts the color to an SRgbColor
* ToScRgb: Converts the color to an ScRgbColor
* ToHsl: Converts the color to an HslColor
* ToHsv: Converts the color to an HsvColor
* ToCmyk: Converts the color to an CmykColor
* ToXyz: Converts the color to an XyzColor
* ToAdobeRgb: Converts the color to an AdobeRgbColor

### Events (.NET Standard)

* ValueChangedEvent: Generic event for a changed property value.
* PropertyChangedEvent: Generic event based on ValueChangedEvent with a property name.

### IO (.NET Standard)

**Binary reader**

* ReadAllBytes: Reads the binary reader to the end.
* ReadBytes: Enumerates bytes read from the BinaryReader in blocks with a maximum size of one page (4096).

**FileHelper**

* Create: Creates or overwrites an existing file allowing other programs read and write access to the created file.
* Open: Opens or creates a file allowing other programs read and write access to the file.
* IsDirectory: Indicates whether the file at the path specified is a directory or not.
* GetFileHash: Returns the hash of a file, using the HashAlorithm specified.
* IsRelativePath: Indicates whether the path is a relative path.

### Json Resources (.NET Standard)

Simmilar API to System.Resources, but json backed.

**ReourceManager**
Manages locale ResourceSets

**ResourceWriter**
Writes a ResourceSet to a location derived from the associated ResourceManager.

**ResourceReader**
Reads a Culture from the device to the ResourceManager.

### Json Settings (.NET Standard)

**SettingsProvider**
Syncronizes a JSON settings file with a SettingsStorage POCO. Allows reading and writing.

**ISettingsProvider**
```
public interface ISettingsProvider
{
    event PropertyChangedEventHandler? SettingsChanged;
    string FileName { get; }
    object? GetValue(string propertyName);
    void SetValue(string propertyName, object? value);
}
```

**SettingsManagerService**
Static class multiple SettingsProviders can be registered to in order to access thier data.

### Localisation (.NET Core WPF)

WPF localisation helpers ported from some blog post I found, extened it somewhat.

**LocaslisationHelper**
Helper class for binding to resource strings.

**ResourceManagerService**
Static class multiple ResourceManager can be registered to in order to access thier data.

**DesignHelper**

IsInDesignModeStatic: Gets a value indicating whether the control is in design mode (running in Blend or Visual Studio

### Reflection (.NET Standard)

**CollectionsReflect**
Static class that can convert IEnumerable<T> to List<T> and T[] based on a type variable using reflection.
  
  
**Invoker**
Delegate that invokes the underlying MethodInfo.Invoke method.
```
public delegate object? Invoker(object? obj, object?[]? parameters);
```

**EqualityComparers**

* GetDefaultEqualityComparer: Returns the default IEqualityComparer for the type specified.
  Same as IEqualityComparer<T>.Default but with a type variable using reflection.
  
**TypeEntention**

* HasInterface: Indicates whether the interface specified is implemented by the type.
* IsGenericIEnumerable: Returns whether the type implements the IEnumerable<T> interface.
* GetEnumerableElementType: Returns the generic type argument of any IEnumerable<T> type.
* WithAttribute: Filters a sequence by methods with the Attribute specified.

### Text (.NET Standard)

**StringHelper**
* FastAllocateString: Calls the internal FastAllocateString method using reflection.
* RandomString: Returns a new randomly generated string using the provided Random with the specified length containing characters in alphabet.
* GetBytes: Encodes all characters in the String into a squence of bytes using the specified encoding.
* IsASCIIString: Indicates whether the string only contains ASCII characters.
* Append: Returns a StringBuilder appending the value to the String.
* IsEmpty: Indicates whether the string is empty ("").
* IsWhiteSpace: Indicates whether the string is empty or consisits only of white-space characters.
* EqualsIgnoreCase, EqualsInvariant, EqualsInvariantIgnoreCase, EqualsOrdinal, EqualsOrdinalIgnoreCase
* NullSafe: Dereferences a ullable<String>, by coalescing the value with String.Empty on null.
* String to type conversion functions and thier respective inverse functions for the following types:
  Byte, SByte, UInt16, Int16, UInt32, Int32, UInt64, Int64, Single, Double, Enum, DateTime, Color, IEnumerable, KeyValuePair, Dictionary, Range.
* String to type conversions based on methods in the type with the "FromString" and "ToString" attribute.
  FromString requires the signature `static T function(string)`,
  ToString requires the signature `static string function(T)` where T is the gerneric type of the class of these members.
* SplitAndTrim: Splits a String into substrings based on the separator".

**BulkTokenizer**
Splits a string by tokens and returns the elements inbetween tokens.

**Tokenizer**
Splits a string by tokens and enumerates the elements inbetween tokens.

**ITokenizer**
```
public interface ITokenizer
{
    string? Current { get; }
    bool First(ReadOnlySpan<char> token);
    bool Next(ReadOnlySpan<char> token);
}
```

### WPF (.NET Core WPF)

**Colors**
For System.Media & System.Drawing colors.

* ToDrawingColor: Converts the System.Media.Color to a System.Drawing.Color.
* ToMediaColor: Converts System.Drawing.Color to a System.Media.Color.

**DynamicResourceBindingExtention**
From Mark A. Donohoe https://stackoverflow.com/a/49159501/6401643

**SyncronizedFilter**
Syncronizes the ICollectionView.Filter property of multiple ICollectionViews with the Predicate<object?> provided.

**SyncronizedFilterManagerService**
Manages SyncronizedFilters allowing ItemsControl to be added and Predicates to be changed.

**SimpleCommand**
ICommand implementation.

**ViewModelBase**
Abstract class implementing INotifyPropertyChanged as a lightweight alternative to DependencyObjects

**VisualTreeTraverseHelper**

* GetSuperElement: Traverses the VisualTree parentage until the first element of the type is traversed, then returns the instance of that DependencyObject.
