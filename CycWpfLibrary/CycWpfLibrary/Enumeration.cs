using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CycWpfLibrary
{
  public abstract class Enumeration<EnumType> :  IComparable where EnumType : Enumeration<EnumType>, new()
  {
    public Enumeration()
    {
    }
    protected Enumeration(int value, string displayName)
    {
      Value = value;
      DisplayName = displayName;
    }

    public int Value { get; protected set; }
    public string DisplayName { get; protected set; }

    public int CompareTo(object other)
    {
      return Value.CompareTo(((Enumeration<EnumType>)other).Value);
    }

    public override string ToString()
    {
      return DisplayName;
    }
    public override bool Equals(object obj)
    {
      var otherValue = obj as Enumeration<EnumType>;

      if (otherValue == null)
      {
        return false;
      }

      var typeMatches = GetType().Equals(obj.GetType());
      var valueMatches = Value.Equals(otherValue.Value);

      return typeMatches && valueMatches;
    }
    public override int GetHashCode()
    {
      return Value.GetHashCode();
    }

    public static int AbsoluteDifference(EnumType firstValue, EnumType secondValue)
    {
      var absoluteDifference = System.Math.Abs(firstValue.Value - secondValue.Value);
      return absoluteDifference;
    }
    public static IEnumerable<T> GetAll<T>() where T : EnumType, new()
    {
      var type = typeof(T);
      var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

      foreach (var info in fields)
      {
        var instance = new T();
        var locatedValue = info.GetValue(instance) as T;

        if (locatedValue != null)
        {
          yield return locatedValue;
        }
      }
    }
    public static T FromValue<T>(int value) where T : EnumType, new()
    {
      var matchingItem = parse<T, int>(value, "value", item => item.Value == value);
      return matchingItem;
    }
    public static T FromDisplayName<T>(string displayName) where T : EnumType, new()
    {
      var matchingItem = parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
      return matchingItem;
    }
    private static T parse<T, K>(K value, string description, Func<T, bool> predicate) where T : EnumType, new()
    {
      var allItems = GetAll<T>();
      var matchingItem = allItems.FirstOrDefault(predicate);

      if (matchingItem == null)
      {
        var message = $"'{value}' is not a valid {description} in {typeof(T)}";
        throw new ApplicationException(message);
      }

      return matchingItem;
    }

    public static EnumType operator &(Enumeration<EnumType> enumA, EnumType enumB) => FromValue<EnumType>(enumA.Value & enumB.Value);
    public static EnumType operator |(Enumeration<EnumType> enumA, EnumType enumB) => FromValue<EnumType>(enumA.Value | enumB.Value);
    public static EnumType operator ^(Enumeration<EnumType> enumA, EnumType enumB) => FromValue<EnumType>(enumA.Value ^ enumA.Value);
    public static bool operator ==(Enumeration<EnumType> enumA, EnumType enumB) => enumA is null ? enumB is null : enumA.Equals(enumB);

    public static bool operator !=(Enumeration<EnumType> enumA, EnumType enumB) => !(enumA == enumB);

    public bool Contain(EnumType enumB) => (this & enumB) == enumB;
    public EnumType Add(EnumType enumB) => this | enumB;
    public EnumType Remove(EnumType enumB) => this ^ enumB;
  }

  internal class EmployeeType : Enumeration<EmployeeType>
  {
    public static readonly EmployeeType Manager
        = new EmployeeType(0, "Manager");
    public static readonly EmployeeType Servant
        = new EmployeeType(1, "Servant");
    public static readonly EmployeeType AssistantToTheRegionalManager
        = new EmployeeType(2, "Assistant to the Regional Manager");

    public EmployeeType() : this(0, "Manager") { }
    private EmployeeType(int value, string displayName)
      : base(value, displayName) { }
  }

  internal class aa
  {
    public aa()
    {
      var a = EmployeeType.AssistantToTheRegionalManager;
      var b = EmployeeType.Manager;
      if (a.Contain(b))
      {
        a.Add(b);
      }
    }
  }
}
