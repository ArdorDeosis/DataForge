using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DataForge.GridUtilities.Tests;

internal static partial class TestData
{
  [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
  [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
  private static IEnumerable<GridDimensionInformation> MakeGridDimensionData(int[] size) =>
    size.Select(n => new GridDimensionInformation(n));

  [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
  [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
  private static IEnumerable<GridDimensionInformation> MakeGridDimensionData(int[] size, bool wrapAll) =>
    size.Select(n => new GridDimensionInformation(n) { Wrap = wrapAll });


  [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
  [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
  [SuppressMessage("ReSharper", "VariableHidesOuterVariable")]
  private static IEnumerable<GridDimensionInformation> MakeGridDimensionData(int[] size, bool[] wrap) =>
    size.Select((size, n) => new GridDimensionInformation(size) { Wrap = wrap[n] });

  [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
  [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
  [SuppressMessage("ReSharper", "VariableHidesOuterVariable")]
  private static IEnumerable<GridDimensionInformation> MakeGridDimensionData(int[] size, int[] offset) =>
    size.Select((size, n) => new GridDimensionInformation(size) { Offset = offset[n] });

  [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
  [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
  [SuppressMessage("ReSharper", "VariableHidesOuterVariable")]
  private static IEnumerable<GridDimensionInformation> MakeGridDimensionData(int[] size, int[] offset, bool wrapAll) =>
    size.Select((size, n) => new GridDimensionInformation(size) { Offset = offset[n], Wrap = wrapAll });

  [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
  [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
  [SuppressMessage("ReSharper", "VariableHidesOuterVariable")]
  private static IEnumerable<GridDimensionInformation> MakeGridDimensionData(int[] size, int[] offset, bool[] wrap) =>
    size.Select((size, n) => new GridDimensionInformation(size) { Offset = offset[n], Wrap = wrap[n] });
}