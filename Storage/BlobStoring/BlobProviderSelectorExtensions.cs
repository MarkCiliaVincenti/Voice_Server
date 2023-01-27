using Core;

namespace Storage.BlobStoring;

public static class BlobProviderSelectorExtensions
{
    public static IBlobProvider Get<TContainer>(
        this IBlobProviderSelector selector)
    {
        Check.NotNull(selector, nameof(selector));

        return selector.Get(BlobContainerNameAttribute.GetContainerName<TContainer>());
    }
}
