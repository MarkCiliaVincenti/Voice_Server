﻿using Abstraction.Storage;
using Core;

namespace Storage.BlobStoring;

public static class BlobContainerExtensions
{
    public static async Task SaveAsync(
        this IBlobContainer container,
        string name,
        byte[] bytes,
        bool overrideExisting = false,
        CancellationToken cancellationToken = default
    )
    {
        using var memoryStream = new MemoryStream(bytes);
        await container.SaveAsync(
            name,
            memoryStream,
            overrideExisting,
            cancellationToken
        );
    }

    public static async Task<byte[]> GetAllBytesAsync(
        this IBlobContainer container,
        string name,
        CancellationToken cancellationToken = default)
    {
        await using var stream = await container.GetAsync(name, cancellationToken);
        return await stream.GetAllBytesAsync(cancellationToken);
    }

    public static async Task<byte[]> GetAllBytesOrNullAsync(
        this IBlobContainer container,
        string name,
        CancellationToken cancellationToken = default)
    {
        var stream = await container.GetOrNullAsync(name, cancellationToken);
        if (stream == null)
        {
            return null;
        }

        await using (stream)
        {
            return await stream.GetAllBytesAsync(cancellationToken);
        }
    }
}
