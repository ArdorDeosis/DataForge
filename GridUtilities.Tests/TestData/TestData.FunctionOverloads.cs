using System;
using System.Collections.Generic;
using System.Linq;
using GridUtilities;

namespace IteratorUtilities.Tests;

internal static partial class TestData
{
  internal static IEnumerable<Func<int[], int[][]>> CoordinateFunctionsWithoutOffset()
  {
    yield return sizes => Grid.Coordinates(sizes).ToArray();
    yield return sizes => Grid.Coordinates(sizes.ToList()).ToArray();
    yield return sizes => Grid.Coordinates(MakeGridDimensionData(sizes).ToArray()).ToArray();
    yield return sizes => Grid.Coordinates(MakeGridDimensionData(sizes).ToList()).ToArray();
  }

  internal static IEnumerable<Func<int[], int[], int[][]>> CoordinateFunctionsWithOffset()
  {
    yield return (sizes, offset) => Grid.Coordinates(sizes, offset).ToArray();
    yield return (sizes, offset) => Grid.Coordinates(MakeGridDimensionData(sizes, offset).ToArray()).ToArray();
    yield return (sizes, offset) => Grid.Coordinates(MakeGridDimensionData(sizes, offset).ToList()).ToArray();
  }

  internal static IEnumerable<Func<int[], GridEdgeInformation[]>> EdgeInformationFunctionsWithoutOffset()
  {
    yield return sizes => Grid.EdgeInformation(sizes).ToArray();
    yield return sizes => Grid.EdgeInformation(sizes.ToList()).ToArray();
    yield return sizes => Grid.EdgeInformation(MakeGridDimensionData(sizes).ToArray()).ToArray();
    yield return sizes => Grid.EdgeInformation(MakeGridDimensionData(sizes).ToList()).ToArray();
  }

  internal static IEnumerable<Func<int[], int[], GridEdgeInformation[]>> EdgeInformationFunctionsWithOffset()
  {
    yield return (sizes, offset) => Grid.EdgeInformation(sizes, offset).ToArray();
    yield return (sizes, offset) => Grid.EdgeInformation(MakeGridDimensionData(sizes, offset).ToArray()).ToArray();
    yield return (sizes, offset) => Grid.EdgeInformation(MakeGridDimensionData(sizes, offset).ToList()).ToArray();
  }
}