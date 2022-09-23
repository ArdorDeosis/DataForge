using System;
using GridUtilities;
using NUnit.Framework;

namespace IteratorUtilities.Tests;

public class GridEdgeInformationTests
{
  [Test]
  public void PublicParameterlessConstructor_ThrowsInvalidOperationException()
  {
    // ASSERT
    Assert.That(() => new GridEdgeInformation(), Throws.InvalidOperationException);
  }

  [Test]
  public void Constructor_EmptyLowerCoordinate_ThrowsArgumentException()
  {
    // ASSERT
    Assert.That(() => new GridEdgeInformation(Array.Empty<int>(), 0), Throws.ArgumentException);
  }
  
  [TestCase(new[]{0}, 1)]
  [TestCase(new[]{0}, -1)]
  [TestCase(new[]{0, 0, 0}, 3)]
  public void Constructor_OutOfBoundsDimensionOfChange_ThrowsArgumentOutOfRangeException(int[] coordinate, int dimensionOfChange)
  {
    // ASSERT
    Assert.That(() => new GridEdgeInformation(Array.Empty<int>(), 0), Throws.ArgumentException);
  }

  [TestCase(new[] { 0 }, 0, new[] { 1 })]
  [TestCase(new[] { 0, 0 }, 0, new[] { 1, 0 })]
  [TestCase(new[] { 0, 0 }, 1, new[] { 0, 1 })]
  public void Constructor_CalculatesUpperCoordinateCorrectly(int[] lowerCoordinate, int dimensionOfChange,
    int[] expectedUpperCoordinate)
  {
    // ACT
    var upperCoordinate = new GridEdgeInformation(lowerCoordinate, dimensionOfChange).UpperCoordinate;
    
    // ASSERT
    Assert.That(upperCoordinate, Is.EqualTo(expectedUpperCoordinate));
  }

  [Test]
  public void Equals_Self_True()
  {
    // ARRANGE
    var left = new GridEdgeInformation(new[] { 0xBEEF, 0xC0FFEE }, 0);

    // ASSERT
    Assert.That(left.Equals(left));
  }

  [Test]
  public void Equals_SameValues_True()
  {
    // ARRANGE
    var coordinate = new[] { 0xBEEF, 0xC0FFEE };
    var left = new GridEdgeInformation(coordinate, 0);
    var right = new GridEdgeInformation(coordinate, 0);

    // ASSERT
    Assert.That(left == right);
  }
  
  [Test]
  public void Equals_SameValuesAsObject_True()
  {
    // ARRANGE
    var coordinate = new[] { 0xBEEF, 0xC0FFEE };
    var left = new GridEdgeInformation(coordinate, 0);
    var right = new GridEdgeInformation(coordinate, 0);

    // ASSERT
    Assert.That(left.Equals((object)right));
  }

  [Test]
  public void Equals_DifferentCoordinates_False()
  {
    // ARRANGE
    var left = new GridEdgeInformation(new[] { 0xBEEF, 0xC0FFEE }, 0);
    var right = new GridEdgeInformation(new[] { 0xBEEF, 0xC0FFEE, 0xF00D }, 0);

    // ASSERT
    Assert.That(left != right);
  }

  [Test]
  public void Equals_DifferentDimensionOfChange_False()
  {
    // ARRANGE
    var coordinate = new[] { 0xBEEF, 0xC0FFEE };
    var left = new GridEdgeInformation(coordinate, 0);
    var right = new GridEdgeInformation(coordinate, 1);

    // ASSERT
    Assert.That(left != right);
  }

  [Test]
  public void GetHashCode_SameValues_SameHash()
  {
    // ARRANGE
    var coordinate = new[] { 0xBEEF, 0xC0FFEE };
    var left = new GridEdgeInformation(coordinate, 0);
    var right = new GridEdgeInformation(coordinate, 0);

    // ASSERT
    Assert.That(left.GetHashCode(), Is.EqualTo(right.GetHashCode()));
  }

  [Test]
  public void GetHashCode_SimpleValueSwitch_DifferentHashCodes()
  {
    // ARRANGE
    var left = new GridEdgeInformation(new[] { 0xBEEF, 0xC0FFEE }, 0);
    var right = new GridEdgeInformation(new[] { 0xC0FFEE, 0xBEEF }, 0);

    // ASSERT
    Assert.That(left.GetHashCode(), Is.Not.EqualTo(right.GetHashCode()));
  }
}