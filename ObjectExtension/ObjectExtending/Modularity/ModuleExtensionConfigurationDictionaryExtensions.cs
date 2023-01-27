using Core;
using JetBrains.Annotations;

namespace ObjectExtension.ObjectExtending.Modularity;

public static class ModuleExtensionConfigurationDictionaryExtensions
{
    public static ModuleExtensionConfigurationDictionary ConfigureModule<T>(
        [NotNull] this ModuleExtensionConfigurationDictionary configurationDictionary,
        [NotNull] string moduleName,
        [NotNull] Action<T> configureAction)
        where T : ModuleExtensionConfiguration, new()
    {
        Check.NotNull(moduleName, nameof(moduleName));
        Check.NotNull(configureAction, nameof(configureAction));

        configureAction(
            (T)configurationDictionary.GetOrAdd(
                moduleName,
                () => new T()
            )
        );

        return configurationDictionary;
    }
}
