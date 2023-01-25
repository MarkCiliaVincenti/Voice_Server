using Core;
using JetBrains.Annotations;

namespace Storage.BlobStoring;

public class BlobContainerConfigurations
{
    private BlobContainerConfiguration Default => GetConfiguration<DefaultContainer>();

    private readonly Dictionary<string, BlobContainerConfiguration> _containers;

    public BlobContainerConfigurations()
    {
        _containers = new Dictionary<string, BlobContainerConfiguration>
        {
            //Add default container
            [BlobContainerNameAttribute.GetContainerName<DefaultContainer>()] = new BlobContainerConfiguration()
        };
    }

    public BlobContainerConfigurations Configure<TContainer>(
        Action<BlobContainerConfiguration> configureAction)
    {
        return Configure(
            BlobContainerNameAttribute.GetContainerName<TContainer>(),
            configureAction
        );
    }

    public BlobContainerConfigurations Configure(
        [NotNull] string name,
        [NotNull] Action<BlobContainerConfiguration> configureAction)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(configureAction);

        configureAction(
            _containers.GetOrAdd(
                name,
                () => new BlobContainerConfiguration(Default)
            )
        );

        return this;
    }

    public BlobContainerConfigurations ConfigureDefault(Action<BlobContainerConfiguration> configureAction)
    {
        configureAction(Default);
        return this;
    }

    public BlobContainerConfigurations ConfigureAll(Action<string, BlobContainerConfiguration> configureAction)
    {
        foreach (var container in _containers)
        {
            configureAction(container.Key, container.Value);
        }

        return this;
    }

    [NotNull]
    public BlobContainerConfiguration GetConfiguration<TContainer>()
    {
        return GetConfiguration(BlobContainerNameAttribute.GetContainerName<TContainer>());
    }

    [NotNull]
    public BlobContainerConfiguration GetConfiguration([NotNull] string name)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(name);

        return _containers.GetOrDefault(name) ??
               Default;
    }
}
