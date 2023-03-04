using System.Numerics;
using CommunityToolkit.HighPerformance;

namespace DataForge.DataFields;

public interface IDataField<T> where T : struct
{
  (int x, int y) Size { get; }
  ref T this[int x, int y] { get; }
  bool TryGetValue(int x, int y, ref T value);
  bool ContainsCoordinate(int x, int y);
  IEnumerable<T> Values { get; }
  IEnumerable<(int x, int y)> Coordinates { get; }

  public void ForEach(Func<T, T> operation);

  public void ForEach<TOther>(Func<T, TOther, T> operation, IDataField<TOther> other) where TOther : struct;
}

public interface INumericDataField<T> : IDataField<T> where T : struct, INumber<T>
{
  public static INumericDataField<T> operator +(INumericDataField<T> left, INumericDataField<T> right)
  {
    if (left.Size.x != right.Size.x)
      throw new Exception();
    if (left.Size.y != right.Size.y)
      throw new Exception();
    var result = new NumericDataField<T>(left.Size.x, left.Size.y);
    foreach (var coordinate in result.Coordinates)
      result[coordinate.x, coordinate.y] = left[coordinate.x, coordinate.y] + right[coordinate.x, coordinate.y];
    return result;
  }
}

public class DataField<T> : IDataField<T> where T : struct
{
  private readonly T[,] data;

  public DataField(int x, int y)
  {
    if (x < 0)
      throw new ArgumentException($"{nameof(x)} cannot be negative", nameof(x));
    if (y < 0)
      throw new ArgumentException($"{nameof(y)} cannot be negative", nameof(y));
    Size = (x, y);
    data = new T[x, y];
  }

  public Span2D<T>.Enumerator GetEnumerator() => data.AsSpan2D().GetEnumerator();

  public (int x, int y) Size { get; }

  public ref T this[int x, int y] => ref data[x, y];

  public bool TryGetValue(int x, int y, ref T value)
  {
    if (!ContainsCoordinate(x, y))
      return false;
    value = ref data[x, y];
    return true;
  }

  public bool ContainsCoordinate(int x, int y) => x >= 0 && x < Size.x && y >= 0 && y < Size.y;

  public IEnumerable<(int x, int y)> Coordinates
  {
    get
    {
      for (var y = 0; y < Size.x; y++)
      {
        for (var x = 0; x < Size.x; x++)
        {
          yield return (x, y);
        }
      }
    }
  }

  public void ForEach(Func<T, T> operation)
  {
    foreach (ref var value in this)
      value = operation(value);
  }

  public void ForEach<TOther>(Func<T, TOther, T> operation, IDataField<TOther> other) where TOther : struct
  {
    foreach (var coordinate in Coordinates)
      this[coordinate.x, coordinate.y] = operation(this[coordinate.x, coordinate.y], other[coordinate.x, coordinate.y]);
  }

  public IEnumerable<T> Values
  {
    get
    {
      var enumerator = (IEnumerator<T>)data.GetEnumerator();
      while (enumerator.MoveNext())
        yield return enumerator.Current;
    }
  }
}

public class NumericDataField<T> : DataField<T>, INumericDataField<T> where T : struct, INumber<T>
{
  public NumericDataField(int x, int y) : base(x, y) { }


  public static NumericDataField<T> operator +(NumericDataField<T> left, NumericDataField<T> right)
  {
    if (left.Size.x != right.Size.x)
      throw new Exception();
    if (left.Size.y != right.Size.y)
      throw new Exception();
    var result = new NumericDataField<T>(left.Size.x, left.Size.y);
    foreach (var coordinate in result.Coordinates)
      result[coordinate.x, coordinate.y] = left[coordinate.x, coordinate.y] + right[coordinate.x, coordinate.y];
    return result;
  }
}