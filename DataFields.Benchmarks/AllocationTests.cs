using BenchmarkDotNet.Attributes;
using DataForge.DataFields;

namespace DataFields.Benchmarks;

[MemoryDiagnoser]
public class AllocationTests
{
  private const int SizeX = 1000;
  private const int SizeY = 1000;

  [Benchmark]
  public void BaseLine()
  {
    var fieldA = new NumericDataField<int>(SizeX, SizeY);
    var fieldB = new NumericDataField<int>(SizeX, SizeY);
  }

  [Benchmark]
  public void AdditionOperator()
  {
    var fieldA = new NumericDataField<int>(SizeX, SizeY);
    var fieldB = new NumericDataField<int>(SizeX, SizeY);
    fieldA = fieldA + fieldB;
  }

  [Benchmark]
  public void CompoundAdditionOperator()
  {
    var fieldA = new NumericDataField<int>(SizeX, SizeY);
    var fieldB = new NumericDataField<int>(SizeX, SizeY);
    fieldA += fieldB;
  }

  [Benchmark]
  public void ForEach()
  {
    var fieldA = new NumericDataField<int>(SizeX, SizeY);
    var fieldB = new NumericDataField<int>(SizeX, SizeY);
    fieldA.ForEach((a, b) => a + b, fieldB);
  }
}