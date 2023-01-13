﻿namespace Graph;

public interface IIndexProvider<in TData, out TIndex> where TIndex : notnull
{
  TIndex GetIndex(TData data);
}