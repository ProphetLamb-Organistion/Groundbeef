# ProphetLamb.Tools

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

**EnumHelper**

* GetValues: Returns a generic list of all values of the Enum.
* Parse: Parses a enum value from a string.
* GetNames: Returns a list of string represenations of all values of the Enum.
* GetDisplayValues: Returns a list of the displayvalue-attribute strings of all values of the Enum.
* GetDisplayValue: Returns the value of the displayvalue-attribute (with the specified culture).

**Int32Range**
Simpler implementation of a range structure then System.Range

**MathHelper**

NonNegativemodulus: Returns 'a' modulus 'n' as a non negative value.

**TaskHelper**

* STAThreadTask
  Runs a task in STA thread instead of MTA.
* Task Collections
  Extention method to Task arrays and lists adding Task.WaitAll, Task.WaitAny, Task.WhenAll, and Task.WhenAny.

**TypeHelper**

* T CastObject<T>: Casts the object to the desired type.
* T ConvertObject<T>: Converts the object the desired type using Convert.ChangeType.

### Collections (.NET Standard)

#### Generic

**Arrays & Spans**

* SortByKeys: Sorts a one-dimesional array into a new array by swapping each element to the index indicated by keys without shuffeling the keys.
  Returns the sorted array.
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

**Enumerable** (non generic)

* Count: Returns the number of elements in a sequence.

**GenericCollectionConversion**

* ToGenericList: Converts the generic IEnumerable to a generic List of the same element type.
* ToGenericArray: Converts the generic IEnumerable to a generic Array of the same element type.

**Map**
A bi-directionally accessible dictionary implementation.

* Indexer: A wrapper class of Dictionary tailored for the needs of Map.

#### Concurrent

**Concurrent dictionary**
The purpose here is to streamline the code produced for Dictionarys with ConcurrentDictionaries

* Add(key, value)(keyvalue-pair): Adds the specified key and value to the concurrent dictionary. If the key already exists overwrites the exisiting value.
  Calls AddOrUpdate.
* Remove(key): Removes the value with the specified key from the ConcurrentDictionary.
* AddRange: Adds a range of keyvalue-pairs to the dictionary.

### Converters (.NET Standard)

**Base85**
Encodes and decodes base85 strings to bytes

### Events (.NET Standard)

* ValueChangedEvent: Generic event for a changed property value.
* PropertyChangedEvent: Generic event based on ValueChangedEvent with a property name.

### IO (.NET Standard)

**Binary reader**

* ReadAllBytes: Reads the binary reader to the end.

**FileHelper**

* Create: Creates or overwrites an existing file allowing other programs read and write access to the created file.
* Open: Opens or creates a file allowing other programs read and write access to the file.
* GetFileHash: Returns the hash of a file, using the HashAlorithm specified.

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

**CollectionsConvert**
Static class that can convert IEnumerable<T> to List<T> and T[] based on a type variable using reflection.

**Enumerables**
* IsGenericIEnumerable: Returns whether the type implements the IEnumerable<T> interface.
* GetEnumerableElementType: Returns the generic type argument of any IEnumerable<T> type.

**EqualityComparers**
* GetDefaultEqualityComparer: Returns the default <see cref="IEqualityComparer"/> for the type specified.
  Same as IEqualityComparer<T>.Default but with a type variable using reflection.

### Text (.NET Standard)

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

**StringHelper**
* FastAllocateString: Calls the internal FastAllocateString method using reflection.
* RandomString: Returns a new randomly generated string using the provided Random with the specified length containing characters in alphabet.

### WPF (.NET Core WPF)

**Colors**
For System.Media & System.Drawing colors.

* GetLightness: Returns the lightness of a color using the HVP alogrithm.
* ToDrawingColor: Converts the System.Media.Color to a System.Drawing.Color.
* ToMediaColor: Converts System.Drawing.Color to a System.Media.Color.

**DynamicResourceBindingExtention**
From Mark A. Donohoe https://stackoverflow.com/a/49159501/6401643

**SimpleCommand**
ICommand implementation.

**ViewModelBase**
Abstract class implementing INotifyPropertyChanged as a lightweight alternative to DependencyObjects

**VisualTreeTraverseHelper**

* GetSuperElement: Traverses the VisualTree parentage until the first element of the type is traversed, then returns the instance of that DependencyObject.
