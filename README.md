[![Version](https://img.shields.io/nuget/v/MyNihongo.Expressions?style=plastic)](https://www.nuget.org/packages/MyNihongo.Expressions/)  
Utility methods for `System.Linq.Expressions` and objects. The main goal of the package is reducing usage of the reflection code.
## PropertyOf
Access properties of an onject by the property names.
```cs
object obj = new MyClass();

// Get a property MyProp which is a string
var prop = PropertyOf.Get<string>(obj, "MyProp");

// Set a property MyProp which is a string
PropertyOf.Set(obj, "MyProp", "new value");
```
## CastTo
Cast an object to another type
```cs
var obj = new MyDerivedClass();
var baseObj = CastTo<MyBaseClass>.From(obj);
```
## MethodOf
Invoke methods of an object by the method names.
```cs
object obj = new MyClass();

// Invoke a method without parameters
var result = MethodOf.Invoke<string>(obj, "GetStringValue");

// Invoke a method with parameters
var result = MethodOf.Invoke<string>(obj, "GetStringValue", "input1", 123);
```
# Limitations
* Utility `MethodOf` will probably fail with overloaded methods because of simple caching keys.