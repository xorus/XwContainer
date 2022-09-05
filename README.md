# XwContainer

A very simple container system that I use in a lot of my code.

````csharp
Container = new Container();
Container.Register(this);
_something = Container.Register(new Something());
Container.Register(otherThing);
Container.Register<SomeClassWithAContructorThatNeedsContainer>();
````

