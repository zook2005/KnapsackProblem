using System;
using System.Collections.Generic;

// ReSharper disable UnusedMember.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace KnapsackProblem.Knapsack
{
    public class Item : IEquatable<Item>, IComparable<Item>
    {
        public Item(uint id, uint size, uint value)
        {
            Id = id;
            Size = size;
            Value = value;           
        }

        public uint Id { get; }
        public uint Size { get; }
        public uint Value { get; }
        public double Density => Value / (double)Size;
        public static IItemComparisonPolicy ItemComparisonPolicy = new CompareItemsByDensity(); //default comparison policy

        public bool Equals(Item other)
        {
            return other != null && Id.Equals(other.Id);
        }

        public int CompareTo(Item other)
        {
            return ItemComparisonPolicy.Compare(this, other);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            Item itemObj = obj as Item;

            return itemObj != null && Equals(itemObj);
        }

        public override int GetHashCode()
        {
            return (int) Id;
        }

        public static bool operator ==(Item left, Item right)
        {
            if (((object)left) == null || ((object)right) == null)
                return Equals(left, right);

            return left.Equals(right);
        }

        public static bool operator !=(Item left, Item right)
        {
            if (((object)left) == null || ((object)right) == null)
                return !Equals(left, right);

            return !(left.Equals(right));
        }

        public interface IItemComparisonPolicy : IComparer<Item> {  }

        public class CompareItemsBySize : IItemComparisonPolicy
        {
            public int Compare(Item x, Item y)
            {
                return x.Size.CompareTo(y.Size);
            }
        }

        public class CompareItemsByDensity : IItemComparisonPolicy
        {
            public int Compare(Item x, Item y)
            {
                var equals =  x.Density.CompareTo(y.Density); //items with the same density should be compared by their value
                if (@equals != 0) return equals;
                return new CompareItemsByValue().Compare(x, y);
                
            }
        }

        public class CompareItemsByValue : IItemComparisonPolicy
        {
            public int Compare(Item x, Item y)
            {
                var equals = x.Value.CompareTo(y.Value);
                if (@equals != 0) return equals;
                return new CompareItemsBySize().Compare(x, y)*(-1); //x is "bigger" then y if it's size is smaller
            }
        }

        public override string ToString()
        {
            return $"ID = {Id}\t" 
                  + $"Size = {Size} \t" 
                  + $"Value = {Value} \t" 
                  //+ $"Density = {Density}"
                  + $"";
        }

        public string ToOutportFormat()
        {
            return $"{Id}\t{Size}\t{Value}";
        }

        public class CompareItemsById : IItemComparisonPolicy
        {
            public int Compare(Item x, Item y)
            {
                return x.Id.CompareTo(y.Id);
            }
        }
    }
}