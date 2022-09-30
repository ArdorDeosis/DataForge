using System;
using System.Collections.Generic;
using System.Linq;
using GridUtilities;

namespace IteratorUtilities.Tests;

internal static partial class TestData
{
  internal static class FunctionOverloads
  {
    internal static IEnumerable<Func<int[], int[][]>> CoordinateFunctions()
    {
      yield return sizes => Grid.Coordinates(sizes).ToArray();
      yield return sizes => Grid.Coordinates(sizes.ToList()).ToArray();

      foreach (var function in CoordinateFunctionsWithOffset()
                 .Select<Func<int[], int[], int[][]>, Func<int[], int[][]>>(function => size =>
                   function(size, new int[size.Length])))
        yield return function;
    }

    internal static IEnumerable<Func<int[], int[], int[][]>> CoordinateFunctionsWithOffset()
    {
      yield return (sizes, offset) => Grid.Coordinates(sizes, offset).ToArray();
    }

    internal static IEnumerable<Func<GridDimensionInformation[], int[][]>> RawCoordinateFunctions()
    {
      yield return dimensions => Grid.Coordinates(dimensions).ToArray();
      yield return dimensions => Grid.Coordinates(dimensions.ToList()).ToArray();
    }

    internal static IEnumerable<Func<int[], GridEdgeInformation[]>> EdgeInformationFunctions()
    {
      yield return sizes => Grid.EdgeInformation(sizes.ToArray()).ToArray();
      yield return sizes => Grid.EdgeInformation(sizes.ToList()).ToArray();

      foreach (var function in EdgeInformationFunctionsWithOffset()
                 .Select<Func<int[], int[], GridEdgeInformation[]>, Func<int[], GridEdgeInformation[]>>(function =>
                   size => function(size, new int[size.Length])))
        yield return function;

      foreach (var function in EdgeInformationFunctionsWithSingleWrap()
                 .Select<Func<int[], bool, GridEdgeInformation[]>, Func<int[], GridEdgeInformation[]>>(function =>
                   size => function(size, false)))
        yield return function;

      foreach (var function in EdgeInformationFunctionsWithWrap()
                 .Select<Func<int[], bool[], GridEdgeInformation[]>, Func<int[], GridEdgeInformation[]>>(function =>
                   size => function(size, new bool[size.Length])))
        yield return function;
    }

    internal static IEnumerable<Func<int[], int[], GridEdgeInformation[]>> EdgeInformationFunctionsWithOffset()
    {
      yield return (sizes, offset) => Grid.EdgeInformation(sizes, offset).ToArray();

      foreach (var function in EdgeInformationFunctionsWithOffsetAndSingleWrap()
                 .Select<Func<int[], int[], bool, GridEdgeInformation[]>, Func<int[], int[], GridEdgeInformation[]>>(
                   function =>
                     (size, offset) => function(size, offset, false)))
        yield return function;
      foreach (var function in EdgeInformationFunctionsWithOffsetAndWrap()
                 .Select<Func<int[], int[], bool[], GridEdgeInformation[]>, Func<int[], int[], GridEdgeInformation[]>>(
                   function =>
                     (size, offset) => function(size, offset, new bool[size.Length])))
        yield return function;
    }

    internal static IEnumerable<Func<int[], bool, GridEdgeInformation[]>>
      EdgeInformationFunctionsWithSingleWrap()
    {
      yield return (sizes, wrapAll) => Grid.EdgeInformation(sizes, wrapAll).ToArray();

      foreach (var function in EdgeInformationFunctionsWithOffsetAndSingleWrap()
                 .Select<Func<int[], int[], bool, GridEdgeInformation[]>, Func<int[], bool, GridEdgeInformation[]>>(
                   function => (size, wrapAll) => function(size, new int[size.Length], wrapAll)))
      {
        yield return function;
      }
    }

    internal static IEnumerable<Func<int[], bool[], GridEdgeInformation[]>>
      EdgeInformationFunctionsWithWrap()
    {
      yield return (sizes, wrap) => Grid.EdgeInformation(sizes, wrap).ToArray();

      foreach (var function in EdgeInformationFunctionsWithOffsetAndWrap()
                 .Select<Func<int[], int[], bool[], GridEdgeInformation[]>, Func<int[], bool[], GridEdgeInformation[]>>(
                   function => (size, wrap) => function(size, new int[size.Length], wrap)))
      {
        yield return function;
      }
    }

    internal static IEnumerable<Func<int[], int[], bool, GridEdgeInformation[]>>
      EdgeInformationFunctionsWithOffsetAndSingleWrap()
    {
      yield return (sizes, offset, wrapAll) => Grid.EdgeInformation(sizes, offset, wrapAll).ToArray();
    }

    internal static IEnumerable<Func<int[], int[], bool[], GridEdgeInformation[]>>
      EdgeInformationFunctionsWithOffsetAndWrap()
    {
      yield return (sizes, offset, wrap) => Grid.EdgeInformation(sizes, offset, wrap).ToArray();
    }

    internal static IEnumerable<Func<GridDimensionInformation[], GridEdgeInformation[]>> RawEdgeInformationFunctions()
    {
      yield return dimensions => Grid.EdgeInformation(dimensions).ToArray();
      yield return dimensions => Grid.EdgeInformation(dimensions.ToList()).ToArray();
    }
  }
}