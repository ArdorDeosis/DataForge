using System.Collections.Generic;
using System.Linq;

namespace IteratorUtilities.Tests;

internal static partial class TestData
{
  internal static IEnumerable<int[]> InvalidGridSizes()
  {
    yield return new[] { 0 };
    yield return new[] { -1 };
    yield return new[] { 0, 1 };
    yield return new[] { -1, 1 };
    yield return new[] { 1, 0 };
    yield return new[] { 1, -1 };
  }

  internal static IEnumerable<object[]> CoordinatesWithoutOffsetTestData()
  {
    return
      from function in CoordinateFunctionsWithoutOffset()
      from testDataPair in GridSizeAndCoordinatePairs()
      select new[] { testDataPair[0], testDataPair[1], function };
  }

  internal static IEnumerable<object[]> CoordinatesWithOffsetTestData()
  {
    return
      from function in CoordinateFunctionsWithOffset()
      from testData in GridSizeAndOffsetsAndCoordinatePairs()
      select new[] { testData[0], testData[1], testData[2], function };
  }
  internal static IEnumerable<object[]> EdgeInformationWithoutOffsetTestData()
  {
    return
      from function in EdgeInformationFunctionsWithoutOffset()
      from testDataPair in GridSizeAndEdgeInformationPairs()
      select new[] { testDataPair[0], testDataPair[1], function };
  }
  internal static IEnumerable<object[]> EdgeInformationWithOffsetTestData()
  {
    return
      from function in EdgeInformationFunctionsWithOffset()
      from testData in GridSizeAndOffsetAndEdgeInformationPairs()
      select new[] { testData[0], testData[1], testData[2], function };
  }
}