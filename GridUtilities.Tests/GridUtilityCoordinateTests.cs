using System;
using NUnit.Framework;

namespace DataForge.GridUtilities.Tests;

internal class GridUtilityCoordinateTests
{
  [TestCaseSource(typeof(TestData.FunctionOverloads), nameof(TestData.FunctionOverloads.CoordinateFunctions))]
  public void Coordinates_SizeArrayIsEmpty_ThrowsArgumentException(Func<int[], int[][]> function)
  {
    Assert.That(() => function(Array.Empty<int>()), Throws.ArgumentException);
  }

  [TestCaseSource(typeof(TestData.FunctionOverloads), nameof(TestData.FunctionOverloads.RawCoordinateFunctions))]
  public void
    Coordinates_SizeArrayIsEmpty_ThrowsArgumentException(Func<GridDimensionInformation[], int[][]> function)
  {
    Assert.That(() => function(Array.Empty<GridDimensionInformation>()), Throws.ArgumentException);
  }

  [TestCaseSource(typeof(TestData), nameof(TestData.CoordinateFunctionsAndInvalidSize))]
  public void Coordinates_ZeroOrNegativeDimension_ThrowsArgumentException(Func<int[], int[][]> function,
    int[] dimensionSizes)
  {
    Assert.That(() => function(dimensionSizes), Throws.ArgumentException);
  }

  [TestCaseSource(typeof(TestData.FunctionOverloads), nameof(TestData.FunctionOverloads.CoordinateFunctionsWithOffset))]
  public void Coordinates_WrongOffsetArraySize_ThrowsArgumentException(Func<int[], int[], int[][]> function)
  {
    // ARRANGE
    var shortArray = new[] { 1 };
    var longArray = new[] { 1, 2 };

    // ASSERT
    Assert.That(() => function(shortArray, longArray), Throws.ArgumentException);
    Assert.That(() => function(longArray, shortArray), Throws.ArgumentException);
  }

  [TestCaseSource(typeof(TestData), nameof(TestData.CoordinateFunctionsWithoutOffset))]
  public void Coordinates_ReturnsExpectedValues(Func<int[], int[][]> function, int[] size,
    int[][] expectedCoordinates)
  {
    Assert.That(function(size), Is.EqualTo(expectedCoordinates));
  }

  [TestCaseSource(typeof(TestData), nameof(TestData.CoordinateFunctionsWithOffset))]
  public void GridCoordinates_WithOffsets_ReturnsExpectedValues(Func<int[], int[], int[][]> function,
    int[] size, int[] offset, int[][] expectedCoordinates)
  {
    Assert.That(function(size, offset), Is.EqualTo(expectedCoordinates));
  }

  [TestCaseSource(typeof(TestData), nameof(TestData.CoordinateFunctionsFromDimensionDefinition))]
  public void GridCoordinates_FromDimensionDefinition_ReturnsExpectedValues(
    Func<GridDimensionInformation[], int[][]> function, GridDimensionInformation[] dimensions,
    int[][] expectedCoordinates)
  {
    Assert.That(function(dimensions), Is.EqualTo(expectedCoordinates));
  }
}