# ProphetLamb.Tools
Collection of personal tools and utility functionality written in C#

## Table of contents
### Core

<strong>AOT</strong>
Functionallity to preload an assembly or a specific method.

<strong>CultureInfo</strong>
* VerifyCultureName: Verifes that the culture name (en_US etc.) of a given culture is valid, optionally throws an exception.

<strong>DateTime</strong>
* StartOfWeek: Returns the first day in the week at 00:00:00.000 relative the the provided DateTime.
* EndOfWeek: Returns the last day in the week at 23:59:59.999 relative to the provided DateTime.
* StartOfMonth: Return the first day in the month at 00:00:00.000 relative to the provided DateTime.
* EndOfMonth: Return the last day in the month at 23:59:59.999 relative to the provided DateTime.

<strong>DesignHelper</strong>
* IsInDesignModeStatic: Gets a value indicating whether the control is in design mode (running in Blend or Visual Studio).

<strong>EnumHelper</strong>
* GetValues: Returns a generic list of all values of the Enum.
* Parse: Parses a enum value from a string.
* GetNames: Returns a list of string represenations of all values of the Enum.
* GetDisplayValues: Returns a list of the displayvalue-attribute strings of all values of the Enum.
* GetDisplayValue: Returns the value of the displayvalue-attribute (with the specified culture).

<strong>Int32Range</strong>
Simpler implementation of a range structure then System.Range

<strong>MathHelper</strong>
* NonNegativemodulus: Returns a modulus n as a positive value.

<strong>StringHelper</strong>
* FastAllocateString: Expose internal function String.FastAllocateString.

<strong>STAThreadTask</strong>
Runs a task in STA thread instead of MTA.

<strong>Task Collections</strong>
Extention method to Task arrays and lists adding Task.WaitAll, Task.WaitAny, Task.WhenAll, and Task.WhenAny.

<strong>TypeHelper</strong>
* T CastObject<T>: Casts the object to the desired type.
* T ConvertObject<T>: Converts the object the desired type using Convert.ChangeType.
* IsGenericEnumerable: Returns whether the type implements the IEnumerable<> interface.
* GetEnumerableElementType: Returns the generic type argument of any enumerable type.
  
### Collections
#### Generic
<strong>Arrays & Spans</strong>
* SortByKeys: Sorts a one-dimesional array into a new array by swapping each element to the index indicated by keys without shuffeling the keys.
  Returns the sorted array.
* Array optimized predicate logic: FindFirst, FindLast, FindAll, IndexOf, IndexOfLast, IndexOfAll
  Find and return the requested element or index of that element using a match predicate.
* GetHashCode: Additional parameter [bool]fromValues, if true:
  Generates the hashcode of the array or span using the GetHashCode method of thier elements instead and combines these into one.
  
<strong>Collections</strong>
* AddRange: Adds a range of elements to the collection by repeatetly calling the Collection.Add function.
* Collection<Date>.AddDays: Adds all days between two specific dates to the collection.
* Filter: Adds and Removes elements from the target collection so that it only contains elements from source that match the predicate filter.
* Arraylist optimized predicate logic: FindFirst, FindLast, FindAll, IndexOf, IndexOfLast, IndexOfAll
  Find and return the requested element or index of that element using a match predicate.
* Sort: Sorts the elements in the collection using an introspective sort algorithm.
  
<strong>Dictionary</strong>
* AddRange: Adds a range of keyvalue-pairs to the dictionary.
* Add(KeyValuePair): Adds the specified ky and value to the dictionary
* GetDictionaryEnumerator: Returns an DictionaryEnumerator that iterates through the collection.
* DictionaryEnumerator: Implementation of IDictionaryEnumerator.

<strong>Enumerable</strong> (non generic)
* Count: Returns the number of elements in a sequence.
<strong>GenericCollectionConversion</strong>
* object ToGenericList(object, type): Converts the generic IEnumerable to a generic List of the same element type.
* object ToGenericArray(object, type): Converts the generic IEnumerable to a generic Array of the same element type.

<strong>Map</strong>
A bi-directionally accessible dictionary implementation.
* Indexer: A wrapper class of Dictionary tailored for the needs of Map.

#### Concurrent
<string>Concurrent dictionary</strong>
The purpose here is to streamline the code produced for Dictionarys with ConcurrentDictionaries
* Add(key, value)(keyvalue-pair): Adds the specified key and value to the concurrent dictionary. If the key already exists overwrites the exisiting value.
  Calls AddOrUpdate.
* Remove(key): Removes the value with the specified key from the ConcurrentDictionary.
* AddRange: Adds a range of keyvalue-pairs to the dictionary.
### Converters
<strong>Base85</strong>
Encodes and decodes base85 strings to bytes
### Events 
* ValueChangedEvent: Generic event for a changed property value, more lightweight then WPFs, DependencyPropertyChanged.
### IO
<strong>Binary reader</strong>
* ReadAllBytes: Reads the binary reader to the end.

<strong>FileHelper</strong>
* FileStream Create: Creates a file ad a specified directory with rwx access without locking the file.
### Json Resources
Simmilar API to System.Resources, but json backed.
<strong>ReourceManager</strong>
Manages locale ResourceSets

<strong>ResourceWriter</strong>
Writes a ResourceSet to a location derived from the associated ResourceManager.

<strong>ResourceReader</strong>
Reads a Culture from the device to the ResourceManager.
### Localisation
WPF localisation helpers ported from some blog post I found, extened it somewhat.

<strong>LocaslisationHelper</strong>
Helper class for binding to resource strings.

<strong>ResourceManagerService</strong>
Static class multiple ResourceManager can be registered to in order to access thier data.
### WPF
<strong>Colors</strong>
For System.Media & System.Drawing colors.
* GetLightness: Returns the lightness of a color using the W3C alogrithm.
* ToDrawingColor: Converts the System.Media.Color to a System.Drawing.Color.
* ToMediaColor: Converts System.Drawing.Color to a System.Media.Color.

<strong>DynamicResourceBindingExtention</strong>
From Mark A. Donohoe https://stackoverflow.com/a/49159501/6401643

<strong>SimpleCommand</strong>
ICommand implementation.

<strong>ViewModelBase</strong>
Abstract class implementing INotifyPropertyChanged as a lightweight alternative to DependencyObjects

<strong>VisualTreeTraverseHelper</strong>
* GetSuperElement: Traverses the VisualTree parentage until the first element of the type is traversed, then returns the instance of that DependencyObject.
