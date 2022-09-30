namespace GridUtilities;

public class temp
{
  int Calculate() => 1;

  void HandleError()
  { }

  bool Try<T>(Func<T> action, out T result, int retryAttempts = 1)
  {
    var exceptions = new List<Exception>();
    while (exceptions.Count < retryAttempts)
    {
      try
      {
        result = action();
        return true;
      }
      catch (Exception exception)
      {
        exceptions.Add(exception);
      }
    }

    throw new AggregateException(exceptions);
  }

  void Retry(Action action, int retryAttempts)
  {
    var exceptions = new List<Exception>();
    while (exceptions.Count < retryAttempts)
    {
      try
      {
        action();
        return;
      }
      catch (Exception exception)
      {
        exceptions.Add(exception);
      }
    }

    throw new AggregateException();
  }

  public enum KOKS : uint
  {
    Kaffee = 0xC0FFEE,
    Obst = 0xF00D,
    Kekse,
    Süßigkeiten
  }

  public enum InTechValues : byte
  {
    ThinkAhead = 0x0,
    GetShitDone = 0x1,
    LoveYourTeam = 0x2,
    LoveYourWork = 0x3,
    FeelResponsible = 0x4,
    SpeakYourMind = 0x5,
  }
}