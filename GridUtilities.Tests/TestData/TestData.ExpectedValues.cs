using System;
using System.Collections.Generic;
using System.Linq;

namespace DataForge.GridUtilities.Tests;

internal static partial class TestData
{
  internal static class ExpectedValues
  {
    internal static class Coordinates
    {
      /// <summary>
      /// <tt>[size, expectedCoordinates]</tt>
      /// </summary>
      internal static IEnumerable<object[]> ForSize()
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

      /// <summary>
      /// <tt>[size, offset, expectedCoordinates]</tt>
      /// </summary>
      internal static IEnumerable<object[]> ForSizeAndOffset()
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

      /// <summary>
      /// <tt>[dimensionDefinition, expectedCoordinates]</tt>
      /// </summary>
      internal static IEnumerable<object[]> ForDimensionDefinition()
      {
        // ReSharper disable once InvokeAsExtensionMethod
        return Enumerable.Concat(
          ForSize()
            .Select(data => new[] { MakeGridDimensionData((int[])data[0]).ToArray(), data[1] })
            .ToArray(),
          ForSizeAndOffset()
            .Select(data => new[] { MakeGridDimensionData((int[])data[0], (int[])data[1]).ToArray(), data[2] })
            .ToArray()
        ).ToArray();
      }
    }

    internal static class EdgeInformation
    {
      /// <summary>
      /// <tt>[size, expectedEdgeInfo]</tt>
      /// </summary>
      internal static IEnumerable<object[]> ForSize()
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

      /// <summary>
      /// <tt>[size, offset, expectedEdgeInfo]</tt>
      /// </summary>
      internal static IEnumerable<object[]> ForSizeAndOffset()
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
            new GridEdgeInformation(new[] { 0, 2, 3 }, 0),
            new GridEdgeInformation(new[] { 1, 1, 2 }, 1),
            new GridEdgeInformation(new[] { 1, 1, 2 }, 2),
            new GridEdgeInformation(new[] { 1, 1, 3 }, 1),
            new GridEdgeInformation(new[] { 1, 2, 2 }, 2),
          },
        };
      }

      /// <summary>
      /// <tt>[size, expectedEdgeInfo]</tt>
      /// </summary>
      internal static IEnumerable<object[]> ForSizeAllWrapped()
      {
        yield return new object[]
        {
          new[] { 1 },
          new[]
          {
            new GridEdgeInformation(new[] { 0 }, new[] { 0 }, 0),
          },
        };
        yield return new object[]
        {
          new[] { 1, 1, 1 },
          new[]
          {
            new GridEdgeInformation(new[] { 0, 0, 0 }, new[] { 0, 0, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 0, 0 }, new[] { 0, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 0, 0, 0 }, new[] { 0, 0, 0 }, 2),
          },
        };
        yield return new object[]
        {
          new[] { 3 },
          new[]
          {
            new GridEdgeInformation(new[] { 0 }, 0),
            new GridEdgeInformation(new[] { 1 }, 0),
            new GridEdgeInformation(new[] { 2 }, new[] { 0 }, 0),
          },
        };
        yield return new object[]
        {
          new[] { 3, 1 },
          new[]
          {
            new GridEdgeInformation(new[] { 0, 0 }, 0),
            new GridEdgeInformation(new[] { 1, 0 }, 0),
            new GridEdgeInformation(new[] { 2, 0 }, new[] { 0, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 0 }, new[] { 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 0 }, new[] { 1, 0 }, 1),
            new GridEdgeInformation(new[] { 2, 0 }, new[] { 2, 0 }, 1),
          },
        };
        yield return new object[]
        {
          new[] { 2, 2 },
          new[]
          {
            new GridEdgeInformation(new[] { 0, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 0 }, new[] { 0, 0 }, 0),
            new GridEdgeInformation(new[] { 1, 0 }, 1),
            new GridEdgeInformation(new[] { 0, 1 }, 0),
            new GridEdgeInformation(new[] { 0, 1 }, new[] { 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 1 }, new[] { 0, 1 }, 0),
            new GridEdgeInformation(new[] { 1, 1 }, new[] { 1, 0 }, 1),
          },
        };
        yield return new object[]
        {
          new[] { 2, 3 },
          new[]
          {
            new GridEdgeInformation(new[] { 0, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 0 }, new[] { 0, 0 }, 0),
            new GridEdgeInformation(new[] { 1, 0 }, 1),
            new GridEdgeInformation(new[] { 0, 1 }, 0),
            new GridEdgeInformation(new[] { 0, 1 }, 1),
            new GridEdgeInformation(new[] { 1, 1 }, new[] { 0, 1 }, 0),
            new GridEdgeInformation(new[] { 1, 1 }, 1),
            new GridEdgeInformation(new[] { 0, 2 }, 0),
            new GridEdgeInformation(new[] { 0, 2 }, new[] { 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 2 }, new[] { 0, 2 }, 0),
            new GridEdgeInformation(new[] { 1, 2 }, new[] { 1, 0 }, 1),
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
            new GridEdgeInformation(new[] { 0, 1 }, new[] { 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 1 }, 0),
            new GridEdgeInformation(new[] { 1, 1 }, new[] { 1, 0 }, 1),
            new GridEdgeInformation(new[] { 2, 0 }, new[] { 0, 0 }, 0),
            new GridEdgeInformation(new[] { 2, 0 }, 1),
            new GridEdgeInformation(new[] { 2, 1 }, new[] { 0, 1 }, 0),
            new GridEdgeInformation(new[] { 2, 1 }, new[] { 2, 0 }, 1),
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
            new GridEdgeInformation(new[] { 0, 0, 1 }, new[] { 0, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 1, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 1, 0 }, new[] { 0, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 0, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 0, 0 }, new[] { 0, 0, 0 }, 0),
            new GridEdgeInformation(new[] { 1, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 1, 1 }, 0),
            new GridEdgeInformation(new[] { 0, 1, 1 }, new[] { 0, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 0, 1, 1 }, new[] { 0, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 0, 1 }, new[] { 0, 0, 1 }, 0),
            new GridEdgeInformation(new[] { 1, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 1, 0, 1 }, new[] { 1, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 1, 0 }, new[] { 0, 1, 0 }, 0),
            new GridEdgeInformation(new[] { 1, 1, 0 }, new[] { 1, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 1, 1 }, new[] { 0, 1, 1 }, 0),
            new GridEdgeInformation(new[] { 1, 1, 1 }, new[] { 1, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 1, 1, 1 }, new[] { 1, 1, 0 }, 2),
          },
        };
      }

      /// <summary>
      /// <tt>[size, offset, expectedEdgeInfo]</tt>
      /// </summary>
      internal static IEnumerable<object[]> ForSizeAndOffsetAllWrapped()
      {
        yield return new object[]
        {
          new[] { 1 },
          new[] { 0xC0FFEE },
          new[]
          {
            new GridEdgeInformation(new[] { 0xC0FFEE }, new[] { 0xC0FFEE }, 0),
          },
        };
        yield return new object[]
        {
          new[] { 1, 1, 1 },
          new[] { 0xC0FFEE, 0xBEEF, 0xF00D },
          new[]
          {
            new GridEdgeInformation(new[] { 0xC0FFEE, 0xBEEF, 0xF00D }, new[] { 0xC0FFEE, 0xBEEF, 0xF00D }, 0),
            new GridEdgeInformation(new[] { 0xC0FFEE, 0xBEEF, 0xF00D }, new[] { 0xC0FFEE, 0xBEEF, 0xF00D }, 1),
            new GridEdgeInformation(new[] { 0xC0FFEE, 0xBEEF, 0xF00D }, new[] { 0xC0FFEE, 0xBEEF, 0xF00D }, 2),
          },
        };
        yield return new object[]
        {
          new[] { 3 },
          new[] { 7 },
          new[]
          {
            new GridEdgeInformation(new[] { 7 }, 0),
            new GridEdgeInformation(new[] { 8 }, 0),
            new GridEdgeInformation(new[] { 9 }, new[] { 7 }, 0),
          },
        };
        yield return new object[]
        {
          new[] { 3, 1 },
          new[] { 7, 7 },
          new[]
          {
            new GridEdgeInformation(new[] { 7, 7 }, 0),
            new GridEdgeInformation(new[] { 8, 7 }, 0),
            new GridEdgeInformation(new[] { 9, 7 }, new[] { 7, 7 }, 0),
            new GridEdgeInformation(new[] { 7, 7 }, new[] { 7, 7 }, 1),
            new GridEdgeInformation(new[] { 8, 7 }, new[] { 8, 7 }, 1),
            new GridEdgeInformation(new[] { 9, 7 }, new[] { 9, 7 }, 1),
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
            new GridEdgeInformation(new[] { 3, -2 }, new[] { 2, -2 }, 0),
            new GridEdgeInformation(new[] { 3, -2 }, 1),
            new GridEdgeInformation(new[] { 2, -1 }, 0),
            new GridEdgeInformation(new[] { 2, -1 }, new[] { 2, -2 }, 1),
            new GridEdgeInformation(new[] { 3, -1 }, new[] { 2, -1 }, 0),
            new GridEdgeInformation(new[] { 3, -1 }, new[] { 3, -2 }, 1),
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
            new GridEdgeInformation(new[] { 1, 100 }, new[] { 0, 100 }, 0),
            new GridEdgeInformation(new[] { 1, 100 }, 1),
            new GridEdgeInformation(new[] { 0, 101 }, 0),
            new GridEdgeInformation(new[] { 0, 101 }, 1),
            new GridEdgeInformation(new[] { 1, 101 }, new[] { 0, 101 }, 0),
            new GridEdgeInformation(new[] { 1, 101 }, 1),
            new GridEdgeInformation(new[] { 0, 102 }, 0),
            new GridEdgeInformation(new[] { 0, 102 }, new[] { 0, 100 }, 1),
            new GridEdgeInformation(new[] { 1, 102 }, new[] { 0, 102 }, 0),
            new GridEdgeInformation(new[] { 1, 102 }, new[] { 1, 100 }, 1),
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
            new GridEdgeInformation(new[] { 10, 11 }, new[] { 10, 10 }, 1),
            new GridEdgeInformation(new[] { 11, 11 }, 0),
            new GridEdgeInformation(new[] { 11, 11 }, new[] { 11, 10 }, 1),
            new GridEdgeInformation(new[] { 12, 10 }, new[] { 10, 10 }, 0),
            new GridEdgeInformation(new[] { 12, 10 }, 1),
            new GridEdgeInformation(new[] { 12, 11 }, new[] { 10, 11 }, 0),
            new GridEdgeInformation(new[] { 12, 11 }, new[] { 12, 10 }, 1),
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
            new GridEdgeInformation(new[] { 0, 1, 3 }, new[] { 0, 1, 2 }, 2),
            new GridEdgeInformation(new[] { 0, 2, 2 }, 0),
            new GridEdgeInformation(new[] { 0, 2, 2 }, new[] { 0, 1, 2 }, 1),
            new GridEdgeInformation(new[] { 0, 2, 2 }, 2),
            new GridEdgeInformation(new[] { 1, 1, 2 }, new[] { 0, 1, 2 }, 0),
            new GridEdgeInformation(new[] { 1, 1, 2 }, 1),
            new GridEdgeInformation(new[] { 1, 1, 2 }, 2),
            new GridEdgeInformation(new[] { 0, 2, 3 }, 0),
            new GridEdgeInformation(new[] { 0, 2, 3 }, new[] { 0, 1, 3 }, 1),
            new GridEdgeInformation(new[] { 0, 2, 3 }, new[] { 0, 2, 2 }, 2),
            new GridEdgeInformation(new[] { 1, 1, 3 }, new[] { 0, 1, 3 }, 0),
            new GridEdgeInformation(new[] { 1, 1, 3 }, 1),
            new GridEdgeInformation(new[] { 1, 1, 3 }, new[] { 1, 1, 2 }, 2),
            new GridEdgeInformation(new[] { 1, 2, 2 }, new[] { 0, 2, 2 }, 0),
            new GridEdgeInformation(new[] { 1, 2, 2 }, new[] { 1, 1, 2 }, 1),
            new GridEdgeInformation(new[] { 1, 2, 2 }, 2),
            new GridEdgeInformation(new[] { 1, 2, 3 }, new[] { 0, 2, 3 }, 0),
            new GridEdgeInformation(new[] { 1, 2, 3 }, new[] { 1, 1, 3 }, 1),
            new GridEdgeInformation(new[] { 1, 2, 3 }, new[] { 1, 2, 2 }, 2),
          },
        };
      }

      /// <summary>
      /// <tt>[size, wrap, expectedEdgeInfo]</tt>
      /// </summary>
      internal static IEnumerable<object[]> ForSizeAndWrap()
      {
        yield return new object[]
        {
          new[] { 2, 2 },
          new[] { true, false },
          new[]
          {
            new GridEdgeInformation(new[] { 0, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 0 }, new[] { 0, 0 }, 0),
            new GridEdgeInformation(new[] { 1, 0 }, 1),
            new GridEdgeInformation(new[] { 0, 1 }, 0),
            new GridEdgeInformation(new[] { 1, 1 }, new[] { 0, 1 }, 0),
          },
        };
        yield return new object[]
        {
          new[] { 2, 2 },
          new[] { false, true },
          new[]
          {
            new GridEdgeInformation(new[] { 0, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 0 }, 1),
            new GridEdgeInformation(new[] { 0, 1 }, 0),
            new GridEdgeInformation(new[] { 0, 1 }, new[] { 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 1 }, new[] { 1, 0 }, 1),
          },
        };
        yield return new object[]
        {
          new[] { 2, 2, 2 },
          new[] { true, false, false },
          new[]
          {
            new GridEdgeInformation(new[] { 0, 0, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 0, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 0, 1 }, 0),
            new GridEdgeInformation(new[] { 0, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 0, 1, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 0, 0 }, new[] { 0, 0, 0 }, 0),
            new GridEdgeInformation(new[] { 1, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 1, 1 }, 0),
            new GridEdgeInformation(new[] { 1, 0, 1 }, new[] { 0, 0, 1 }, 0),
            new GridEdgeInformation(new[] { 1, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 1, 1, 0 }, new[] { 0, 1, 0 }, 0),
            new GridEdgeInformation(new[] { 1, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 1, 1 }, new[] { 0, 1, 1 }, 0),
          },
        };
        yield return new object[]
        {
          new[] { 2, 2, 2 },
          new[] { false, true, false },
          new[]
          {
            new GridEdgeInformation(new[] { 0, 0, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 0, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 0, 1 }, 0),
            new GridEdgeInformation(new[] { 0, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 0, 1, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 1, 0 }, new[] { 0, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 0, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 1, 1 }, 0),
            new GridEdgeInformation(new[] { 0, 1, 1 }, new[] { 0, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 1, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 1, 1, 0 }, new[] { 1, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 1, 1 }, new[] { 1, 0, 1 }, 1),
          },
        };
        yield return new object[]
        {
          new[] { 2, 2, 2 },
          new[] { false, false, true },
          new[]
          {
            new GridEdgeInformation(new[] { 0, 0, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 0, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 0, 1 }, 0),
            new GridEdgeInformation(new[] { 0, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 0, 0, 1 }, new[] { 0, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 1, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 1, 1 }, 0),
            new GridEdgeInformation(new[] { 0, 1, 1 }, new[] { 0, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 1, 0, 1 }, new[] { 1, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 1, 1 }, new[] { 1, 1, 0 }, 2),
          },
        };
        yield return new object[]
        {
          new[] { 2, 2, 2 },
          new[] { true, true, false },
          new[]
          {
            new GridEdgeInformation(new[] { 0, 0, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 0, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 0, 1 }, 0),
            new GridEdgeInformation(new[] { 0, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 0, 1, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 1, 0 }, new[] { 0, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 0, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 0, 0 }, new[] { 0, 0, 0 }, 0),
            new GridEdgeInformation(new[] { 1, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 1, 1 }, 0),
            new GridEdgeInformation(new[] { 0, 1, 1 }, new[] { 0, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 1, 0, 1 }, new[] { 0, 0, 1 }, 0),
            new GridEdgeInformation(new[] { 1, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 1, 1, 0 }, new[] { 0, 1, 0 }, 0),
            new GridEdgeInformation(new[] { 1, 1, 0 }, new[] { 1, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 1, 1 }, new[] { 0, 1, 1 }, 0),
            new GridEdgeInformation(new[] { 1, 1, 1 }, new[] { 1, 0, 1 }, 1),
          },
        };
        yield return new object[]
        {
          new[] { 2, 2, 2 },
          new[] { true, false, true },
          new[]
          {
            new GridEdgeInformation(new[] { 0, 0, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 0, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 0, 1 }, 0),
            new GridEdgeInformation(new[] { 0, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 0, 0, 1 }, new[] { 0, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 1, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 0, 0 }, new[] { 0, 0, 0 }, 0),
            new GridEdgeInformation(new[] { 1, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 1, 1 }, 0),
            new GridEdgeInformation(new[] { 0, 1, 1 }, new[] { 0, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 0, 1 }, new[] { 0, 0, 1 }, 0),
            new GridEdgeInformation(new[] { 1, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 1, 0, 1 }, new[] { 1, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 1, 0 }, new[] { 0, 1, 0 }, 0),
            new GridEdgeInformation(new[] { 1, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 1, 1 }, new[] { 0, 1, 1 }, 0),
            new GridEdgeInformation(new[] { 1, 1, 1 }, new[] { 1, 1, 0 }, 2),
          },
        };
        yield return new object[]
        {
          new[] { 2, 2, 2 },
          new[] { false, true, true },
          new[]
          {
            new GridEdgeInformation(new[] { 0, 0, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 0, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 0, 1 }, 0),
            new GridEdgeInformation(new[] { 0, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 0, 0, 1 }, new[] { 0, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 1, 0 }, 0),
            new GridEdgeInformation(new[] { 0, 1, 0 }, new[] { 0, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 0, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 0, 1, 1 }, 0),
            new GridEdgeInformation(new[] { 0, 1, 1 }, new[] { 0, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 0, 1, 1 }, new[] { 0, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 1, 0, 1 }, new[] { 1, 0, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 1, 0 }, new[] { 1, 0, 0 }, 1),
            new GridEdgeInformation(new[] { 1, 1, 0 }, 2),
            new GridEdgeInformation(new[] { 1, 1, 1 }, new[] { 1, 0, 1 }, 1),
            new GridEdgeInformation(new[] { 1, 1, 1 }, new[] { 1, 1, 0 }, 2),
          },
        };
      }

      /// <summary>
      /// <tt>[size, offset, wrap, expectedEdgeInfo]</tt>
      /// </summary>
      internal static IEnumerable<object[]> ForSizeAndOffsetAndWrap()
      {
        yield return new object[]
        {
          new[] { 2, 2 },
          new[] { 10, 0 },
          new[] { true, false },
          new[]
          {
            new GridEdgeInformation(new[] { 10, 0 }, 0),
            new GridEdgeInformation(new[] { 10, 0 }, 1),
            new GridEdgeInformation(new[] { 11, 0 }, new[] { 10, 0 }, 0),
            new GridEdgeInformation(new[] { 11, 0 }, 1),
            new GridEdgeInformation(new[] { 10, 1 }, 0),
            new GridEdgeInformation(new[] { 11, 1 }, new[] { 10, 1 }, 0),
          },
        };
        yield return new object[]
        {
          new[] { 2, 2 },
          new[] { 10, 0 },
          new[] { false, true },
          new[]
          {
            new GridEdgeInformation(new[] { 10, 0 }, 0),
            new GridEdgeInformation(new[] { 10, 0 }, 1),
            new GridEdgeInformation(new[] { 11, 0 }, 1),
            new GridEdgeInformation(new[] { 10, 1 }, 0),
            new GridEdgeInformation(new[] { 10, 1 }, new[] { 10, 0 }, 1),
            new GridEdgeInformation(new[] { 11, 1 }, new[] { 11, 0 }, 1),
          },
        };
        yield return new object[]
        {
          new[] { 2, 2, 2 },
          new[] { 10, 100, 1000 },
          new[] { true, false, false },
          new[]
          {
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 0),
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 10, 100, 1001 }, 0),
            new GridEdgeInformation(new[] { 10, 100, 1001 }, 1),
            new GridEdgeInformation(new[] { 10, 101, 1000 }, 0),
            new GridEdgeInformation(new[] { 10, 101, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 100, 1000 }, new[] { 10, 100, 1000 }, 0),
            new GridEdgeInformation(new[] { 11, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 11, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 10, 101, 1001 }, 0),
            new GridEdgeInformation(new[] { 11, 100, 1001 }, new[] { 10, 100, 1001 }, 0),
            new GridEdgeInformation(new[] { 11, 100, 1001 }, 1),
            new GridEdgeInformation(new[] { 11, 101, 1000 }, new[] { 10, 101, 1000 }, 0),
            new GridEdgeInformation(new[] { 11, 101, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 101, 1001 }, new[] { 10, 101, 1001 }, 0),
          },
        };
        yield return new object[]
        {
          new[] { 2, 2, 2 },
          new[] { 10, 100, 1000 },
          new[] { false, true, false },
          new[]
          {
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 0),
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 10, 100, 1001 }, 0),
            new GridEdgeInformation(new[] { 10, 100, 1001 }, 1),
            new GridEdgeInformation(new[] { 10, 101, 1000 }, 0),
            new GridEdgeInformation(new[] { 10, 101, 1000 }, new[] { 10, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 10, 101, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 11, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 10, 101, 1001 }, 0),
            new GridEdgeInformation(new[] { 10, 101, 1001 }, new[] { 10, 100, 1001 }, 1),
            new GridEdgeInformation(new[] { 11, 100, 1001 }, 1),
            new GridEdgeInformation(new[] { 11, 101, 1000 }, new[] { 11, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 11, 101, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 101, 1001 }, new[] { 11, 100, 1001 }, 1),
          },
        };
        yield return new object[]
        {
          new[] { 2, 2, 2 },
          new[] { 10, 100, 1000 },
          new[] { false, false, true },
          new[]
          {
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 0),
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 10, 100, 1001 }, 0),
            new GridEdgeInformation(new[] { 10, 100, 1001 }, 1),
            new GridEdgeInformation(new[] { 10, 100, 1001 }, new[] { 10, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 10, 101, 1000 }, 0),
            new GridEdgeInformation(new[] { 10, 101, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 11, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 10, 101, 1001 }, 0),
            new GridEdgeInformation(new[] { 10, 101, 1001 }, new[] { 10, 101, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 100, 1001 }, 1),
            new GridEdgeInformation(new[] { 11, 100, 1001 }, new[] { 11, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 101, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 101, 1001 }, new[] { 11, 101, 1000 }, 2),
          },
        };
        yield return new object[]
        {
          new[] { 2, 2, 2 },
          new[] { 10, 100, 1000 },
          new[] { true, true, false },
          new[]
          {
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 0),
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 10, 100, 1001 }, 0),
            new GridEdgeInformation(new[] { 10, 100, 1001 }, 1),
            new GridEdgeInformation(new[] { 10, 101, 1000 }, 0),
            new GridEdgeInformation(new[] { 10, 101, 1000 }, new[] { 10, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 10, 101, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 100, 1000 }, new[] { 10, 100, 1000 }, 0),
            new GridEdgeInformation(new[] { 11, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 11, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 10, 101, 1001 }, 0),
            new GridEdgeInformation(new[] { 10, 101, 1001 }, new[] { 10, 100, 1001 }, 1),
            new GridEdgeInformation(new[] { 11, 100, 1001 }, new[] { 10, 100, 1001 }, 0),
            new GridEdgeInformation(new[] { 11, 100, 1001 }, 1),
            new GridEdgeInformation(new[] { 11, 101, 1000 }, new[] { 10, 101, 1000 }, 0),
            new GridEdgeInformation(new[] { 11, 101, 1000 }, new[] { 11, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 11, 101, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 101, 1001 }, new[] { 10, 101, 1001 }, 0),
            new GridEdgeInformation(new[] { 11, 101, 1001 }, new[] { 11, 100, 1001 }, 1),
          },
        };
        yield return new object[]
        {
          new[] { 2, 2, 2 },
          new[] { 10, 100, 1000 },
          new[] { true, false, true },
          new[]
          {
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 0),
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 10, 100, 1001 }, 0),
            new GridEdgeInformation(new[] { 10, 100, 1001 }, 1),
            new GridEdgeInformation(new[] { 10, 100, 1001 }, new[] { 10, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 10, 101, 1000 }, 0),
            new GridEdgeInformation(new[] { 10, 101, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 100, 1000 }, new[] { 10, 100, 1000 }, 0),
            new GridEdgeInformation(new[] { 11, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 11, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 10, 101, 1001 }, 0),
            new GridEdgeInformation(new[] { 10, 101, 1001 }, new[] { 10, 101, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 100, 1001 }, new[] { 10, 100, 1001 }, 0),
            new GridEdgeInformation(new[] { 11, 100, 1001 }, 1),
            new GridEdgeInformation(new[] { 11, 100, 1001 }, new[] { 11, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 101, 1000 }, new[] { 10, 101, 1000 }, 0),
            new GridEdgeInformation(new[] { 11, 101, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 101, 1001 }, new[] { 10, 101, 1001 }, 0),
            new GridEdgeInformation(new[] { 11, 101, 1001 }, new[] { 11, 101, 1000 }, 2),
          },
        };
        yield return new object[]
        {
          new[] { 2, 2, 2 },
          new[] { 10, 100, 1000 },
          new[] { false, true, true },
          new[]
          {
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 0),
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 10, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 10, 100, 1001 }, 0),
            new GridEdgeInformation(new[] { 10, 100, 1001 }, 1),
            new GridEdgeInformation(new[] { 10, 100, 1001 }, new[] { 10, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 10, 101, 1000 }, 0),
            new GridEdgeInformation(new[] { 10, 101, 1000 }, new[] { 10, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 10, 101, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 11, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 10, 101, 1001 }, 0),
            new GridEdgeInformation(new[] { 10, 101, 1001 }, new[] { 10, 100, 1001 }, 1),
            new GridEdgeInformation(new[] { 10, 101, 1001 }, new[] { 10, 101, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 100, 1001 }, 1),
            new GridEdgeInformation(new[] { 11, 100, 1001 }, new[] { 11, 100, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 101, 1000 }, new[] { 11, 100, 1000 }, 1),
            new GridEdgeInformation(new[] { 11, 101, 1000 }, 2),
            new GridEdgeInformation(new[] { 11, 101, 1001 }, new[] { 11, 100, 1001 }, 1),
            new GridEdgeInformation(new[] { 11, 101, 1001 }, new[] { 11, 101, 1000 }, 2),
          },
        };
      }

      /// <summary>
      /// <tt>[dimensionDefinition, expectedCoordinates]</tt>
      /// </summary>
      internal static IEnumerable<object[]> ForDimensionDefinition()
      {
        return ForSize()
          .Select(data => new[] { MakeGridDimensionData((int[])data[0]).ToArray(), data[1] })
          .Concat(
            ForSizeAndOffset()
              .Select(data => new[] { MakeGridDimensionData((int[])data[0], (int[])data[1]).ToArray(), data[2] }))
          .Concat(
            ForSizeAllWrapped()
              .Select(data => new[] { MakeGridDimensionData((int[])data[0], true).ToArray(), data[1] }))
          .Concat(
            ForSizeAndOffsetAllWrapped()
              .Select(data => new[] { MakeGridDimensionData((int[])data[0], (int[])data[1], true).ToArray(), data[2] }))
          .Concat(
            ForSizeAndWrap()
              .Select(data => new[] { MakeGridDimensionData((int[])data[0], (bool[])data[1]).ToArray(), data[2] }))
          .Concat(
            ForSizeAndOffsetAndWrap()
              .Select(data => new[]
                { MakeGridDimensionData((int[])data[0], (int[])data[1], (bool[])data[2]).ToArray(), data[3] }))
          .ToArray();
      }
    }
  }
}