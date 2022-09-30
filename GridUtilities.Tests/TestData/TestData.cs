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

  /// [ function, size ]  
  internal static IEnumerable<object[]> CoordinateFunctionsAndInvalidSize() =>
    from function in FunctionOverloads.CoordinateFunctions()
    from size in InvalidGridSizes()
    select new object[] { function, size };

  /// [ function, size, expectedCoordinates ]
  internal static IEnumerable<object[]> CoordinateFunctionsWithoutOffset() =>
    from function in FunctionOverloads.CoordinateFunctions()
    from testDataTuple in ExpectedValues.Coordinates.ForSize()
    select new[] { function, testDataTuple[0], testDataTuple[1] };

  /// [ function, size, offset, expectedCoordinates ]
  internal static IEnumerable<object[]> CoordinateFunctionsWithOffset() =>
    from function in FunctionOverloads.CoordinateFunctionsWithOffset()
    from testData in ExpectedValues.Coordinates.ForSizeAndOffset()
    select new[] { function, testData[0], testData[1], testData[2] };

  /// [ function, dimensionDefinition, expectedCoordinates ]
  internal static IEnumerable<object[]> CoordinateFunctionsFromDimensionDefinition() =>
    from function in FunctionOverloads.RawCoordinateFunctions()
    from testData in ExpectedValues.Coordinates.ForDimensionDefinition()
    select new[] { function, testData[0], testData[1] };

  /// [ function, size ]
  internal static IEnumerable<object[]> EdgeInformationFunctionsAndInvalidSize() =>
    from function in FunctionOverloads.EdgeInformationFunctions()
    from gridSize in InvalidGridSizes()
    select new object[] { function, gridSize };

  /// [ function, size, expectedInfo ]
  internal static IEnumerable<object[]> EdgeInformationFunctionsWithoutOffset() =>
    from function in FunctionOverloads.EdgeInformationFunctions()
    from testDataPair in ExpectedValues.EdgeInformation.ForSize()
    select new[] { function, testDataPair[0], testDataPair[1] };

  /// [ function, size, offset, expectedInfo ]
  internal static IEnumerable<object[]> EdgeInformationFunctionsWithOffset() =>
    from function in FunctionOverloads.EdgeInformationFunctionsWithOffset()
    from testData in ExpectedValues.EdgeInformation.ForSizeAndOffset()
    select new[] { function, testData[0], testData[1], testData[2] };

  /// [ function, size, wrapAll, expectedInfo ]
  internal static IEnumerable<object[]> EdgeInformationFunctionsWithSingleWrap() =>
    // ReSharper disable once InvokeAsExtensionMethod
    Enumerable.Concat(
      from function in FunctionOverloads.EdgeInformationFunctionsWithSingleWrap()
      from testData in ExpectedValues.EdgeInformation.ForSize()
      select new[] { function, testData[0], false, testData[1] },
      from function in FunctionOverloads.EdgeInformationFunctionsWithSingleWrap()
      from testDataPair in ExpectedValues.EdgeInformation.ForSizeAllWrapped()
      select new[] { function, testDataPair[0], true, testDataPair[1] }
    );

  /// [ function, size, wrap, expectedInfo ]
  internal static IEnumerable<object[]> EdgeInformationFunctionsWithWrap() =>
    from function in FunctionOverloads.EdgeInformationFunctionsWithWrap()
    from testData in ExpectedValues.EdgeInformation.ForSizeAndWrap()
    select new[] { function, testData[0], testData[1], testData[2] };

  /// [ function, size, offset, wrapAll, expectedInfo ]
  internal static IEnumerable<object[]> EdgeInformationFunctionsWithOffsetAndSingleWrap() =>
    // ReSharper disable once InvokeAsExtensionMethod
    Enumerable.Concat(
      from function in FunctionOverloads.EdgeInformationFunctionsWithOffsetAndSingleWrap()
      from testDataPair in ExpectedValues.EdgeInformation.ForSizeAndOffset()
      select new[] { function, testDataPair[0], testDataPair[1], false, testDataPair[2] },
      from function in FunctionOverloads.EdgeInformationFunctionsWithOffsetAndSingleWrap()
      from testDataPair in ExpectedValues.EdgeInformation.ForSizeAndOffsetAllWrapped()
      select new[] { function, testDataPair[0], testDataPair[1], true, testDataPair[2] }
    );

  /// [ function, size, offset, wrap, expectedInfo ]
  internal static IEnumerable<object[]> EdgeInformationFunctionsWithOffsetAndWrap() =>
    from function in FunctionOverloads.EdgeInformationFunctionsWithOffsetAndWrap()
    from testData in ExpectedValues.EdgeInformation.ForSizeAndOffsetAndWrap()
    select new[] { function, testData[0], testData[1], testData[2], testData[3] };

  /// [ function, dimensionDefinition, expectedInfo ]
  internal static IEnumerable<object[]> EdgeInformationFunctionsFromDimensionDefinition() =>
    from function in FunctionOverloads.RawEdgeInformationFunctions()
    from testData in ExpectedValues.EdgeInformation.ForDimensionDefinition()
    select new[] { function, testData[0], testData[1] };
}