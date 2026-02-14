using DotNetEnv;

namespace AipsWorker.Utilities;

public static class LoadingDotEnv
{
    public static bool TryLoad()
    {
        string? dir = Directory.GetCurrentDirectory();

        while (dir != null && !File.Exists(Path.Combine(dir, ".env")))
        {
            dir = Directory.GetParent(dir)?.FullName;
        }

        if (dir != null)
        {
            Env.Load(Path.Combine(dir, ".env"));
            return true;
        }

        return false;
    }
}