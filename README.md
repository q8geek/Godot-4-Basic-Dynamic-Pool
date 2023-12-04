

[![License: Unlicense](https://img.shields.io/badge/Unlicense-gray?style=for-the-badge&logo=Unlicense&color=gray)](http://unlicense.org/)

[![Engine: Godot](https://img.shields.io/badge/Godot-478CBF?style=for-the-badge&logo=GodotEngine&logoColor=white
)](https://godotengine.org)

[![Language: C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white
)](https://learn.microsoft.com/en-us/dotnet/csharp/)


# Godot Basic Dynamic Pool

I wrote this Dynamic Objects Pooling C# code to make it easier for other programmers to pool their objects and nodes.

The code is not optomized yet, but hopefully I'll make it as optimized as I can by next update.

---
## Demo

Here's a demo of how objects are pooling:

![](https://github.com/q8geek/Godot-4-Basic-Dynamic-Pool/blob/main/demo.gif)

---
## Features

- A dictionary for PackedScenes and their queues.
- Store and retreive objects.
- Get queues' root Node's names and counts.
- ToString() method to retreive Pool's counts.
- A static Pool.instance reference to make life slightly easier. It also checks if the instance is empty or not before Initializing. 

---
## Installation

To use this script:

1. Copy `Pool.cs` to your project's folder
2. To initialize the Pool, write `Pool pool = new Pool(PackedScene[], parentNode);` Where:
* `PackedScene[]` is an array of the PackedScenes you want to pool.
* `parentNode` is a node that all Instantiated scenes from the PackedScenes will be child of.

#### Note:

Generally speaking, you'd want to Initialize the pool in early stages of your project. So probably somewhere inside a `public override void _EnterTree()` or something like that.


## Usage/Examples

Its preferrable to initialize the script inside `_EnterTree()` with the following calls:

```
Pool pool = New Pool(PackedScene[], parentNode);
```

There's a static `Pool.instance` where the class constructor would check if its empty and use it if it was empty.

`PackedScene[]` must be an array of PackedScenes, whether provided from an `[Export]`-ed array or directly read from a directory.

`parentNode` is used as an initial parent to the Scenes instantiated from PackedScenes, and to reparent unwanted nodes.


### Get a node from the pool
To get a `Node` from the pool, you can simply do the following:

```
Node node = Pool.instance.Get("NAME");
```

Where `NAME` is the `PackedScene`'s root node's name (Case sensitive).

### Store a node inside the pool
To store a node back in the pool, you can simply do the following:

```
Pool.instance.Store(NODE, "NAME");
```

Where `NODE` is the node you want to store in the pool.
#### NOTE: 
Make sure that you hide the stored object (Usually by `.visible = false`).


And, as mentioned above, `NAME` is the `PackedScene`'s root node's name (Case sensitive).

#### NOTE:

I know adding "NAME" to store a node back to the pool is inconvenient at best, but it beats having multiple lists/Dictionaries and try to match names with PackedScenes and have some additional loops.

---
## License

[The Unlicense](https://choosealicense.com/licenses/unlicense/)
## What's next?

Find better ways to retreive and store Nodes without having to use the PackedScenes root's Node name as a lookup key.

Also, learning how to write better readme.so files is necessary.