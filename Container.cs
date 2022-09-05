using JetBrains.Annotations;

namespace XwContainer;

[UsedImplicitly]
public class Container : IDisposable
{
    private readonly Dictionary<Type, object> _container = new();
    private readonly List<IDisposable> _disposables = new();

    [UsedImplicitly]
    public TRegister Register<TRegister>(TRegister instance) where TRegister : class
    {
        _container.Add(typeof(TRegister), instance);
        return instance;
    }

    [UsedImplicitly]
    public TRegister Register<[MeansImplicitUse] TRegister>() where TRegister : class
    {
        var instance = (TRegister?)Activator.CreateInstance(typeof(TRegister), this);
        if (instance == null) throw new Exception("Could not find requested service.");
        return Register(instance);
    }

    [UsedImplicitly]
    public TRegister RegisterDisposable<TRegister>(TRegister instance) where TRegister : IDisposable
    {
        _container.Add(typeof(TRegister), instance);
        _disposables.Add(instance);
        return instance;
    }

    // next level lazy
    [UsedImplicitly]
    public TRegister RegisterDisposable<[MeansImplicitUse] TRegister>() where TRegister : IDisposable
    {
        var instance = (TRegister?)Activator.CreateInstance(typeof(TRegister), this);
        if (instance == null) throw new Exception("Could not find requested service.");
        return RegisterDisposable(instance);
    }

    [UsedImplicitly]
    public TRegister Resolve<TRegister>() where TRegister : class
    {
        return _container[typeof(TRegister)] as TRegister ?? throw new InvalidOperationException();
    }

    [UsedImplicitly]
    public void Dispose()
    {
        _disposables.Reverse();
        foreach (var disposable in _disposables) disposable.Dispose();
        GC.SuppressFinalize(this);
    }
}