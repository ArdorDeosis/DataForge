using System;
using System.Linq;
using GridUtilities;
using NUnit.Framework;

namespace IteratorUtilities.Tests;

public class IteratorUtilityGridCoordinatesTests
{
  [TestCaseSource(typeof(TestData), nameof(TestData.CoordinateFunctionsWithoutOffset))]
  public void GridCoordinates_DimensionArrayIsEmpty_ThrowsArgumentException(Func<int[], int[][]> function)
  {
    // ASSERT
    Assert.That(() => function(Array.Empty<int>()), Throws.ArgumentException);
  }

  [TestCaseSource(typeof(TestData), nameof(TestData.InvalidGridSizes))]
  public void GridCoordinates_ZeroOrNegativeDimension_ThrowsArgumentException(int[] dimensionSizes)
  {
    // ARRANGE
    var dimensionsInformation = dimensionSizes.Select(dimension => new GridDimensionInformation(dimension));

    // ASSERT
    Assert.That(() => Grid.Coordinates(dimensionSizes), Throws.ArgumentException);
    Assert.That(() => Grid.Coordinates(dimensionSizes.ToList()), Throws.ArgumentException);
    Assert.That(() => Grid.Coordinates(dimensionsInformation.ToList()), Throws.ArgumentException);
    Assert.That(() => Grid.Coordinates(dimensionsInformation.ToArray()), Throws.ArgumentException);
    // dimension size dubbing as offsets
    Assert.That(() => Grid.Coordinates(dimensionSizes, dimensionSizes), Throws.ArgumentException);
  }

  [Test]
  public void GridCoordinates_DifferentArraySizes_ThrowsArgumentException()
  {
    // ARRANGE
    var dimensionsSizeArray = new[] { 1 };
    var offsetArray = new[] { 1, 2 };

    // ASSERT
    Assert.That(() => Grid.Coordinates(dimensionsSizeArray, offsetArray), Throws.ArgumentException);
  }

  [TestCaseSource(typeof(TestData), nameof(TestData.CoordinatesWithoutOffsetTestData))]
  public void GridCoordinates_WithoutOffsets_ReturnsExpectedValues(int[] dimensionSizes, int[][] expectedCoordinates,
    Func<int[], int[][]> function)
  {
    // ARRANGE
    var returnedCoordinates = function(dimensionSizes);

    // ASSERT
    Assert.That(returnedCoordinates, Is.EqualTo(expectedCoordinates));
  }

  [TestCaseSource(typeof(TestData), nameof(TestData.CoordinatesWithOffsetTestData))]
  public void GridCoordinates_WithOffsets_ReturnsExpectedValues(int[] dimensionSizes, int[] offset,
    int[][] expectedCoordinates,
    Func<int[], int[], int[][]> function)
  {
    // ARRANGE
    var returnedCoordinates = function(dimensionSizes, offset);

    // ASSERT
    Assert.That(returnedCoordinates, Is.EqualTo(expectedCoordinates));
  }
}