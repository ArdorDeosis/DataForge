using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Ardalis.GuardClauses;
using GridUtilities;

namespace IteratorUtilities.Tests;

internal static partial class TestData
{
  [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
  [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
  private static IEnumerable<GridDimensionInformation> MakeGridDimensionData(int[] size) =>
    size.Select(n => new GridDimensionInformation(n));

  [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
  [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
  private static IEnumerable<GridDimensionInformation> MakeGridDimensionData(int[] size, bool wrapAll) =>
    size.Select(n => new GridDimensionInformation(n, wrapAll));

  [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
  [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
  [SuppressMessage("ReSharper", "VariableHidesOuterVariable")]
  private static IEnumerable<GridDimensionInformation> MakeGridDimensionData(int[] size, bool[] wrap) =>
    size.Select((size, n) => new GridDimensionInformation(size, wrap[n]));

  [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
  [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
  [SuppressMessage("ReSharper", "VariableHidesOuterVariable")]
  private static IEnumerable<GridDimensionInformation> MakeGridDimensionData(int[] size, int[] offset) =>
    size.Select((size, n) => new GridDimensionInformation(size, offset[n]));

  [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
  [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
  [SuppressMessage("ReSharper", "VariableHidesOuterVariable")]
  private static IEnumerable<GridDimensionInformation> MakeGridDimensionData(int[] size, int[] offset, bool wrapAll) =>
    size.Select((size, n) => new GridDimensionInformation(size, offset[n], wrapAll));

  [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
  [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
  [SuppressMessage("ReSharper", "VariableHidesOuterVariable")]
  private static IEnumerable<GridDimensionInformation> MakeGridDimensionData(int[] size, int[] offset, bool[] wrap) =>
    size.Select((size, n) => new GridDimensionInformation(size, offset[n], wrap[n]));
}