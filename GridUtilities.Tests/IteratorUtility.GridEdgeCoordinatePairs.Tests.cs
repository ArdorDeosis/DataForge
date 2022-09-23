using System;
using System.Linq;
using GridUtilities;
using NUnit.Framework;

namespace IteratorUtilities.Tests;

public class IteratorUtilityGridEdgeCoordinatePairsTests
{
  [TestCaseSource(typeof(TestData), nameof(TestData.CoordinateFunctionsWithoutOffset))]
  public void GridEdgeInformation_DimensionArrayIsEmpty_ThrowsArgumentException(Func<int[], int[][]> function)
  {
    // ASSERT
    Assert.That(() => function(Array.Empty<int>()), Throws.ArgumentException);
  }

  [TestCaseSource(typeof(TestData), nameof(TestData.InvalidGridSizes))]
  public void GridEdgeInformation_ZeroOrNegativeDimension_ThrowsArgumentException(int[] dimensionSizes)
  {
    // ARRANGE
    var dimensionsInformation = dimensionSizes.Select(dimension => new GridDimensionInformation(dimension));

    // ASSERT
    Assert.That(() => Grid.EdgeInformation(dimensionSizes), Throws.ArgumentException);
    Assert.That(() => Grid.EdgeInformation(dimensionSizes.ToList()), Throws.ArgumentException);
    Assert.That(() => Grid.EdgeInformation(dimensionsInformation.ToList()), Throws.ArgumentException);
    Assert.That(() => Grid.EdgeInformation(dimensionsInformation.ToArray()), Throws.ArgumentException);
    // dimension size dubbing as offsets
    Assert.That(() => Grid.EdgeInformation(dimensionSizes, dimensionSizes), Throws.ArgumentException);
  }

  [Test]
  public void GridEdgeInformation_DifferentArraySizes_ThrowsArgumentException()
  {
    // ARRANGE
    var dimensionsSizeArray = new[] { 1 };
    var offsetArray = new[] { 1, 2 };

    // ASSERT
    Assert.That(() => Grid.EdgeInformation(dimensionsSizeArray, offsetArray), Throws.ArgumentException);
  }

  [TestCaseSource(typeof(TestData), nameof(TestData.EdgeInformationWithoutOffsetTestData))]
  public void GridEdgeInformation_WithoutOffsets_ReturnsExpectedValues(int[] dimensionSizes, GridEdgeInformation[] expectedEdgeInformation,
    Func<int[], GridEdgeInformation[]> function)
  {
    // ARRANGE
    var returnedEdgeInformation = function(dimensionSizes);

    // ASSERT
    Assert.That(returnedEdgeInformation, Is.EquivalentTo(expectedEdgeInformation));
  }

  [TestCaseSource(typeof(TestData), nameof(TestData.EdgeInformationWithOffsetTestData))]
  public void GridEdgeInformation_WithOffsets_ReturnsExpectedValues(int[] dimensionSizes, int[] offset,
    GridEdgeInformation[] expectedEdgeInformation,
    Func<int[], int[], GridEdgeInformation[]> function)
  {
    // ARRANGE
    var returnedEdgeInformation = function(dimensionSizes, offset);
  
    // ASSERT
    Assert.That(returnedEdgeInformation, Is.EquivalentTo(expectedEdgeInformation));
  }
}