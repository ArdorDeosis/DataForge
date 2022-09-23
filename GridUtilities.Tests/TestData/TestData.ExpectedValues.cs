using System;
using System.Collections.Generic;
using GridUtilities;

namespace IteratorUtilities.Tests;

internal static partial class TestData
{
  /// <summary><tt>[gridSize, expectedCoordinates]</tt></summary>
  private static IEnumerable<object[]> GridSizeAndCoordinatePairs()
  {
    yield return new object[]
    {
      new[] { 1 },
      new[] { new[] { 0 } },
    };
    yield return new object[]
    {
      new[] { 3 },
      new[] { new[] { 0 }, new[] { 1 }, new[] { 2 } },
    };
    yield return new object[]
    {
      new[] { 2, 2 },
      new[] { new[] { 0, 0 }, new[] { 1, 0 }, new[] { 0, 1 }, new[] { 1, 1 } },
    };
    yield return new object[]
    {
      new[] { 2, 3 },
      new[]
      {
        new[] { 0, 0 }, new[] { 1, 0 },
        new[] { 0, 1 }, new[] { 1, 1 },
        new[] { 0, 2 }, new[] { 1, 2 },
      },
    };
    yield return new object[]
    {
      new[] { 3, 2 },
      new[]
      {
        new[] { 0, 0 }, new[] { 1, 0 }, new[] { 2, 0 },
        new[] { 0, 1 }, new[] { 1, 1 }, new[] { 2, 1 },
      },
    };
    yield return new object[]
    {
      new[] { 2, 2, 2 },
      new[]
      {
        new[] { 0, 0, 0 }, new[] { 1, 0, 0 },
        new[] { 0, 1, 0 }, new[] { 1, 1, 0 },
        new[] { 0, 0, 1 }, new[] { 1, 0, 1 },
        new[] { 0, 1, 1 }, new[] { 1, 1, 1 },
      },
    };
  }

  /// <summary><tt>[gridSize, offset, expectedCoordinates]</tt></summary>
  private static IEnumerable<object[]> GridSizeAndOffsetsAndCoordinatePairs()
  {
    yield return new object[]
    {
      new[] { 1 },
      new[] { 0xC0FFEE },
      new[] { new[] { 0xC0FFEE } },
    };
    yield return new object[]
    {
      new[] { 3 },
      new[] { 7 },
      new[] { new[] { 7 }, new[] { 8 }, new[] { 9 } },
    };
    yield return new object[]
    {
      new[] { 2, 2 },
      new[] { 2, -2 },
      new[] { new[] { 2, -2 }, new[] { 3, -2 }, new[] { 2, -1 }, new[] { 3, -1 } },
    };
    yield return new object[]
    {
      new[] { 2, 3 },
      new[] { 0, 100 },
      new[]
      {
        new[] { 0, 100 }, new[] { 1, 100 },
        new[] { 0, 101 }, new[] { 1, 101 },
        new[] { 0, 102 }, new[] { 1, 102 },
      },
    };
    yield return new object[]
    {
      new[] { 3, 2 },
      new[] { 10, 10 },
      new[]
      {
        new[] { 10, 10 }, new[] { 11, 10 }, new[] { 12, 10 },
        new[] { 10, 11 }, new[] { 11, 11 }, new[] { 12, 11 },
      },
    };
    yield return new object[]
    {
      new[] { 2, 2, 2 },
      new[] { 0, 1, 2 },
      new[]
      {
        new[] { 0, 1, 2 }, new[] { 1, 1, 2 },
        new[] { 0, 2, 2 }, new[] { 1, 2, 2 },
        new[] { 0, 1, 3 }, new[] { 1, 1, 3 },
        new[] { 0, 2, 3 }, new[] { 1, 2, 3 },
      },
    };
  }

  /// <summary><tt>[gridSize, expectedEdgeInfo]</tt></summary>
  private static IEnumerable<object[]> GridSizeAndEdgeInformationPairs()
  {
    yield return new object[]
    {
      new[] { 1 },
      Array.Empty<GridEdgeInformation>(),
    };
    yield return new object[]
    {
      new[] { 3 },
      new[]
      {
        new GridEdgeInformation(new[] { 0 }, 0),
        new GridEdgeInformation(new[] { 1 }, 0),
      },
    };
    yield return new object[]
    {
      new[] { 2, 2 },
      new[]
      {
        new GridEdgeInformation(new[] { 0, 0 }, 0),
        new GridEdgeInformation(new[] { 0, 0 }, 1),
        new GridEdgeInformation(new[] { 0, 1 }, 0),
        new GridEdgeInformation(new[] { 1, 0 }, 1),
      },
    };
    yield return new object[]
    {
      new[] { 2, 3 },
      new[]
      {
        new GridEdgeInformation(new[] { 0, 0 }, 0),
        new GridEdgeInformation(new[] { 0, 0 }, 1),
        new GridEdgeInformation(new[] { 1, 0 }, 1),
        new GridEdgeInformation(new[] { 0, 1 }, 0),
        new GridEdgeInformation(new[] { 0, 1 }, 1),
        new GridEdgeInformation(new[] { 1, 1 }, 1),
        new GridEdgeInformation(new[] { 0, 2 }, 0),
      },
    };
    yield return new object[]
    {
      new[] { 3, 2 },
      new[]
      {
        new GridEdgeInformation(new[] { 0, 0 }, 0),
        new GridEdgeInformation(new[] { 0, 0 }, 1),
        new GridEdgeInformation(new[] { 1, 0 }, 0),
        new GridEdgeInformation(new[] { 1, 0 }, 1),
        new GridEdgeInformation(new[] { 0, 1 }, 0),
        new GridEdgeInformation(new[] { 1, 1 }, 0),
        new GridEdgeInformation(new[] { 2, 0 }, 1),
      },
    };
    yield return new object[]
    {
      new[] { 2, 2, 2 },
      new[]
      {
        new GridEdgeInformation(new[] { 0, 0, 0 }, 0),
        new GridEdgeInformation(new[] { 0, 0, 0 }, 1),
        new GridEdgeInformation(new[] { 0, 0, 0 }, 2),
        new GridEdgeInformation(new[] { 0, 0, 1 }, 0),
        new GridEdgeInformation(new[] { 0, 0, 1 }, 1),
        new GridEdgeInformation(new[] { 0, 1, 0 }, 0),
        new GridEdgeInformation(new[] { 0, 1, 0 }, 2),
        new GridEdgeInformation(new[] { 1, 0, 0 }, 1),
        new GridEdgeInformation(new[] { 1, 0, 0 }, 2),
        new GridEdgeInformation(new[] { 0, 1, 1 }, 0),
        new GridEdgeInformation(new[] { 1, 0, 1 }, 1),
        new GridEdgeInformation(new[] { 1, 1, 0 }, 2),
      },
    };
  }

  /// <summary><tt>[gridSize, offset, expectedEdgeInfo]</tt></summary>
  private static IEnumerable<object[]> GridSizeAndOffsetAndEdgeInformationPairs()
  {
    yield return new object[]
    {
      new[] { 1 },
      new[] { 0xC0FFEE },
      Array.Empty<GridEdgeInformation>(),
    };
    yield return new object[]
    {
      new[] { 3 },
      new[] { 7 },
      new[]
      {
        new GridEdgeInformation(new[] { 7 }, 0),
        new GridEdgeInformation(new[] { 8 }, 0),
      },
    };
    yield return new object[]
    {
      new[] { 2, 2 },
      new[] { 2, -2 },
      new[]
      {
        new GridEdgeInformation(new[] { 2, -2 }, 0),
        new GridEdgeInformation(new[] { 2, -2 }, 1),
        new GridEdgeInformation(new[] { 2, -1 }, 0),
        new GridEdgeInformation(new[] { 3, -2 }, 1),
      },
    };
    yield return new object[]
    {
      new[] { 2, 3 },
      new[] { 0, 100 },
      new[]
      {
        new GridEdgeInformation(new[] { 0, 100 }, 0),
        new GridEdgeInformation(new[] { 0, 100 }, 1),
        new GridEdgeInformation(new[] { 1, 100 }, 1),
        new GridEdgeInformation(new[] { 0, 101 }, 0),
        new GridEdgeInformation(new[] { 0, 101 }, 1),
        new GridEdgeInformation(new[] { 1, 101 }, 1),
        new GridEdgeInformation(new[] { 0, 102 }, 0),
      },
    };
    yield return new object[]
    {
      new[] { 3, 2 },
      new[] { 10, 10 },
      new[]
      {
        new GridEdgeInformation(new[] { 10, 10 }, 0),
        new GridEdgeInformation(new[] { 10, 10 }, 1),
        new GridEdgeInformation(new[] { 11, 10 }, 0),
        new GridEdgeInformation(new[] { 11, 10 }, 1),
        new GridEdgeInformation(new[] { 10, 11 }, 0),
        new GridEdgeInformation(new[] { 11, 11 }, 0),
        new GridEdgeInformation(new[] { 12, 10 }, 1),
      },
    };
    yield return new object[]
    {
      new[] { 2, 2, 2 },
      new[] { 0, 1, 2 },
      new[]
      {
        new GridEdgeInformation(new[] { 0, 1, 2 }, 0),
        new GridEdgeInformation(new[] { 0, 1, 2 }, 1),
        new GridEdgeInformation(new[] { 0, 1, 2 }, 2),
        new GridEdgeInformation(new[] { 0, 1, 3 }, 0),
        new GridEdgeInformation(new[] { 0, 1, 3 }, 1),
        new GridEdgeInformation(new[] { 0, 2, 2 }, 0),
        new GridEdgeInformation(new[] { 0, 2, 2 }, 2),
        new GridEdgeInformation(new[] { 1, 1, 2 }, 1),
        new GridEdgeInformation(new[] { 1, 1, 2 }, 2),
        new GridEdgeInformation(new[] { 0, 2, 3 }, 0),
        new GridEdgeInformation(new[] { 1, 1, 3 }, 1),
        new GridEdgeInformation(new[] { 1, 2, 2 }, 2),
      },
    };
  }
}