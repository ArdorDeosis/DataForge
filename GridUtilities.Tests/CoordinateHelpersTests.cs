using System.Collections.Generic;
using NUnit.Framework;

namespace GridUtilities.Tests;

internal class CoordinateHelpersTests
{
  [TestCase(null, new[] { 0 })]
  [TestCase(new[] { 0 }, null)]
  [TestCase(null, null)]
  public void AddCoordinates_OneCoordinateIsNull_ThrowsArgumentNullException(IReadOnlyList<int> left,
    IReadOnlyList<int> right)
  {
    // ACT + ASSERT
    Assert.That(() => left.AddCoordinates(right), Throws.ArgumentNullException);
  }

  [Test]
  public void AddCoordinates_CoordinatesHaveDifferentLengths_ThrowsArgumentException()
  {
    // ARRANGE
    var left = new[] { 0 };
    var right = new[] { 0, 0 };

    // ACT + ASSERT
    Assert.That(() => left.AddCoordinates(right), Throws.ArgumentException);
  }

  [TestCase(new int[0], new int[0], new int[0])]
  [TestCase(new[] { 0 }, new[] { 0 }, new[] { 0 })]
  [TestCase(new[] { 1 }, new[] { 2 }, new[] { 3 })]
  [TestCase(new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, new[] { 5, 7, 9 })]
  public void AddCoordinates_AddsCorrectly(IReadOnlyList<int> left, IReadOnlyList<int> right,
    IReadOnlyList<int> expectedResult)
  {
    // ACT
    var result = left.AddCoordinates(right);

    // ASSERT
    Assert.That(result, Is.EqualTo(expectedResult));
  }

  [TestCase(null, new int[0])]
  [TestCase(new int[0], null)]
  [TestCase(null, new[] { 0 })]
  [TestCase(new[] { 0 }, null)]
  [TestCase(new[] { 0 }, new[] { 1 })]
  [TestCase(new[] { 0, 1 }, new[] { 1, 0 })]
  [TestCase(new[] { 0, 1, 2 }, new[] { 0, 1 })]
  [TestCase(new[] { 0, 1 }, new[] { 0, 1, 2 })]
  [TestCase(new int[0], new[] { 0, 1, 2 })]
  [TestCase(new[] { 0, 1, 2 }, new int[0])]
  public void CoordinatesEqual_NotEqualCoordinates_ReturnsFalse(IReadOnlyList<int> left, IReadOnlyList<int> right)
  {
    // ASSERT
    Assert.That(left.CoordinatesEqual(right), Is.False);
  }

  [TestCase(null, null)]
  [TestCase(new int[0], new int[0])]
  [TestCase(new[] { 0 }, new[] { 0 })]
  [TestCase(new[] { 0xC0FFEE, 0xBEEF, 0xF00D }, new[] { 0xC0FFEE, 0xBEEF, 0xF00D })]
  public void CoordinatesEqual_EqualCoordinates_ReturnsTrue(IReadOnlyList<int> left, IReadOnlyList<int> right)
  {
    // ASSERT
    Assert.That(left.CoordinatesEqual(right));
  }

  [TestCase(new[] { 0 }, new[] { 0, 0 })]
  [TestCase(new[] { 0, 1 }, new[] { 1, 0 })]
  public void GetCoordinateHashCode_SimilarCoordinates_HaveDifferentHashCode(IReadOnlyList<int> left,
    IReadOnlyList<int> right)
  {
    // ACT
    var leftHash = left.GetCoordinateHashCode();
    var rightHash = right.GetCoordinateHashCode();

    // ASSERT
    Assert.That(leftHash, Is.Not.EqualTo(rightHash));
  }
}