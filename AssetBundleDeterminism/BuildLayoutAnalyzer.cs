using UnityEditor.AddressableAssets.Build.Layout;
using UnityEngine;

public static class BuildLayoutAnalyzer
{
    private static void CompareBuildLayouts(string buildLayoutPathA, string buildLayoutPathB)
    {
        var buildLayoutA = BuildLayout.Open(buildLayoutPathA, readFullFile: true);
        var buildLayoutB = BuildLayout.Open(buildLayoutPathB, readFullFile: true);

        foreach (var bundleA in BuildLayoutHelpers.EnumerateBundles(buildLayoutA))
        {
            foreach (var bundleB in BuildLayoutHelpers.EnumerateBundles(buildLayoutB))
            {
                if (bundleA.InternalName != bundleB.InternalName)
                    continue;

                if (bundleA.Hash == bundleB.Hash)
                    continue;

                Debug.LogWarning($"Asset Bundle Hash changed: {bundleA.Name}");

                foreach (var explicitAssetA in BuildLayoutHelpers.EnumerateAssets(bundleA))
                {
                    foreach (var explicitAssetB in BuildLayoutHelpers.EnumerateAssets(bundleB))
                    {
                        if (explicitAssetA.InternalId != explicitAssetB.InternalId)
                            continue;

                        if (explicitAssetA.AssetHash != explicitAssetB.AssetHash)
                            Debug.LogWarning($"Asset Hash changed: {explicitAssetA.InternalId}");
                    }
                }
            }
        }
    }
}