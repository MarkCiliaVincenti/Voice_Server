namespace Core.IO;

/// <summary>
/// A helper class for Directory operations.
/// </summary>
public static class DirectoryHelper
{
    public static void CreateIfNotExists(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public static void DeleteIfExists(string directory)
    {
        if (Directory.Exists(directory))
        {
            Directory.Delete(directory);
        }
    }

    public static void DeleteIfExists(string directory, bool recursive)
    {
        if (Directory.Exists(directory))
        {
            Directory.Delete(directory, recursive);
        }
    }

    public static void CreateIfNotExists(DirectoryInfo directory)
    {
        if (!directory.Exists)
        {
            directory.Create();
        }
    }

    public static bool IsSubDirectoryOf(string parentDirectoryPath, string childDirectoryPath)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(parentDirectoryPath);
        ArgumentNullException.ThrowIfNullOrEmpty(childDirectoryPath);
        return IsSubDirectoryOf(
            new DirectoryInfo(parentDirectoryPath),
            new DirectoryInfo(childDirectoryPath)
        );
    }

    public static bool IsSubDirectoryOf(DirectoryInfo parentDirectory,
        DirectoryInfo childDirectory)
    {
        ArgumentNullException.ThrowIfNull(parentDirectory);
        ArgumentNullException.ThrowIfNull(childDirectory);

        if (parentDirectory.FullName == childDirectory.FullName)
        {
            return true;
        }

        var parentOfChild = childDirectory.Parent;
        if (parentOfChild == null)
        {
            return false;
        }

        return IsSubDirectoryOf(parentDirectory, parentOfChild);
    }

    public static IDisposable ChangeCurrentDirectory(string targetDirectory)
    {
        var currentDirectory = Directory.GetCurrentDirectory();

        if (currentDirectory.Equals(targetDirectory, StringComparison.OrdinalIgnoreCase))
        {
            return NullDisposable.Instance;
        }

        Directory.SetCurrentDirectory(targetDirectory);

        return new DisposeAction<string>(Directory.SetCurrentDirectory, currentDirectory);
    }
}