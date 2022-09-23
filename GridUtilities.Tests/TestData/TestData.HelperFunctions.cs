using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GridUtilities;

namespace IteratorUtilities.Tests;

internal static partial class TestData
{
  [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
  private static IEnumerable<GridDimensionInformation> MakeGridDimensionData(int[] size) =>
    Enumerable.Range(0, size.Length).Select(n => new GridDimensionInformation(size[n]));

  [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
  private static IEnumerable<GridDimensionInformation> MakeGridDimensionData(int[] size, int[] offset) =>
    Enumerable.Range(0, size.Length).Select(n => new GridDimensionInformation(size[n], offset[n]));
}