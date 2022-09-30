using System;
using System.Linq;
using GridUtilities;
using NUnit.Framework;

namespace IteratorUtilities.Tests;

public class GridUtilitiesEdgeInformationTests
{
  [TestCaseSource(typeof(TestData.FunctionOverloads), nameof(TestData.FunctionOverloads.EdgeInformationFunctions))]
  public void EdgeInformation_DimensionArrayIsEmpty_ThrowsArgumentException(
    Func<int[], GridEdgeInformation[]> function) =>
    Assert.That(() => function(Array.Empty<int>()), Throws.ArgumentException);

  [TestCaseSource(typeof(TestData), nameof(TestData.EdgeInformationFunctionsAndInvalidSize))]
  public void EdgeInformation_ZeroOrNegativeDimension_ThrowsArgumentException(
    Func<int[], GridEdgeInformation[]> function, int[] size) =>
    Assert.That(() => function(size), Throws.ArgumentException);

  [TestCaseSource(typeof(TestData.FunctionOverloads),
    nameof(TestData.FunctionOverloads.EdgeInformationFunctionsWithOffset))]
  public void EdgeInformation_WrongOffsetArraySize_ThrowsArgumentException(
    Func<int[], int[], GridEdgeInformation[]> function)
  {
    // ARRANGE
    var shortArray = new[] { 1 };
    var longArray = new[] { 1, 2 };

    // ASSERT
    Assert.That(() => function(shortArray, longArray), Throws.ArgumentException);
    Assert.That(() => function(longArray, shortArray), Throws.ArgumentException);
  }

  [TestCaseSource(typeof(TestData), nameof(TestData.EdgeInformationFunctionsWithoutOffset))]
  public void EdgeInformation_ReturnsExpectedValues(Func<int[], GridEdgeInformation[]> function, int[] size,
    GridEdgeInformation[] expectedInformation) =>
    Assert.That(function(size), Is.EquivalentTo(expectedInformation));

  [TestCaseSource(typeof(TestData), nameof(TestData.EdgeInformationFunctionsWithOffset))]
  public void EdgeInformation_WithOffset_ReturnsExpectedValues(Func<int[], int[], GridEdgeInformation[]> function,
    int[] size, int[] offset, GridEdgeInformation[] expectedInformation) =>
    Assert.That(function(size, offset), Is.EquivalentTo(expectedInformation));

  [TestCaseSource(typeof(TestData), nameof(TestData.EdgeInformationFunctionsWithSingleWrap))]
  public void EdgeInformation_WithSingleWrap_ReturnsExpectedValues(
    Func<int[], bool, GridEdgeInformation[]> function, int[] size, bool wrapAll,
    GridEdgeInformation[] expectedInformation) =>
    Assert.That(function(size, wrapAll), Is.EquivalentTo(expectedInformation));

  [TestCaseSource(typeof(TestData), nameof(TestData.EdgeInformationFunctionsWithOffsetAndSingleWrap))]
  public void EdgeInformation_WithOffsetAndSingleWrap_ReturnsExpectedValues(
    Func<int[], int[], bool, GridEdgeInformation[]> function, int[] size, int[] offset, bool wrapAll,
    GridEdgeInformation[] expectedInformation) =>
    Assert.That(function(size, offset, wrapAll), Is.EquivalentTo(expectedInformation));

  [TestCaseSource(typeof(TestData), nameof(TestData.EdgeInformationFunctionsWithWrap))]
  public void EdgeInformation_WithWrap_ReturnsExpectedValues(
    Func<int[], bool[], GridEdgeInformation[]> function, int[] size, bool[] wrap,
    GridEdgeInformation[] expectedInformation) =>
    Assert.That(function(size, wrap), Is.EquivalentTo(expectedInformation));

  [TestCaseSource(typeof(TestData), nameof(TestData.EdgeInformationFunctionsWithOffsetAndWrap))]
  public void EdgeInformation_WithOffsetAndWrap_ReturnsExpectedValues(
    Func<int[], int[], bool[], GridEdgeInformation[]> function, int[] size, int[] offset, bool[] wrap,
    GridEdgeInformation[] expectedInformation) =>
    Assert.That(function(size, offset, wrap), Is.EquivalentTo(expectedInformation));

  [TestCaseSource(typeof(TestData), nameof(TestData.EdgeInformationFunctionsFromDimensionDefinition))]
  public void EdgeInformation_FromGridDefinition_ReturnsExpectedValues(
    Func<GridDimensionInformation[], GridEdgeInformation[]> function, GridDimensionInformation[] dimensionDefinition,
    GridEdgeInformation[] expectedInformation) =>
    Assert.That(function(dimensionDefinition), Is.EquivalentTo(expectedInformation));
}