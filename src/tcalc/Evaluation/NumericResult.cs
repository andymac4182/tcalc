using System;
using System.Globalization;

namespace tcalc.Evaluation
{
    public class NumericResult : Result, IEquatable<NumericResult>
    {
        public bool Equals(NumericResult other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NumericResult) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(NumericResult left, NumericResult right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NumericResult left, NumericResult right)
        {
            return !Equals(left, right);
        }

        public double Value { get; }

        public NumericResult(double value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}