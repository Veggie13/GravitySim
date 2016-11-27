using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitySim
{
    struct Vector3<TUnit>
    {
        public double X;
        public double Y;
        public double Z;

        public Q<TUnit> QX
        {
            get { return new Q<TUnit>(X); }
        }
        public Q<TUnit> QY
        {
            get { return new Q<TUnit>(Y); }
        }
        public Q<TUnit> QZ
        {
            get { return new Q<TUnit>(Z); }
        }

        public Q<TUnit> Magnitude
        {
            get { return new Q<TUnit>() { Value = Math.Sqrt(this.Dot(this).Value) }; }
        }

        public Q<X<TUnit, TUnit>> Mag2
        {
            get { return this.Dot(this); }
        }

        public void Add(Vector3<TUnit> other)
        {
            X += other.X;
            Y += other.Y;
            Z += other.Z;
        }

        public void Sub(Vector3<TUnit> other)
        {
            X -= other.X;
            Y -= other.Y;
            Z -= other.Z;
        }

        public void Scale(double scalar)
        {
            X *= scalar;
            Y *= scalar;
            Z *= scalar;
        }
    }

    struct Q<TUnit>
    {
        public Q(double value)
        {
            Value = value;
        }

        public double Value;

        public static Q<TUnit> operator +(Q<TUnit> a, Q<TUnit> b)
        {
            return new Q<TUnit>(a.Value + b.Value);
        }

        public static Q<TUnit> operator -(Q<TUnit> a, Q<TUnit> b)
        {
            return new Q<TUnit>(a.Value - b.Value);
        }

        public static bool operator <(Q<TUnit> a, Q<TUnit> b)
        {
            return (a.Value < b.Value);
        }

        public static bool operator >(Q<TUnit> a, Q<TUnit> b)
        {
            return (a.Value > b.Value);
        }

        public static bool operator ==(Q<TUnit> a, Q<TUnit> b)
        {
            return (a.Value == b.Value);
        }

        public static bool operator !=(Q<TUnit> a, Q<TUnit> b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is Q<TUnit>)
                return (this == (Q<TUnit>)obj);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    class X<T1, T2>
    { }

    class Per<T1, T2>
    { }

    class Unitless
    { }

    class M
    { }

    class S
    { }

    class KG
    { }

    static class Vector3Extensions
    {
        #region Quantity
        #region Inverse
        private static Q<TOut> inverse<T1, TOut>(this Q<T1> @this)
        {
            return new Q<TOut>(1 / @this.Value);
        }

        public static Q<Per<Unitless, TUnit>> Inv<TUnit>(this Q<TUnit> @this)
        {
            return @this.inverse<TUnit, Per<Unitless, TUnit>>();
        }

        public static Q<Per<T2, T1>> Inv<T1, T2>(this Q<Per<T1, T2>> @this)
        {
            return @this.inverse<Per<T1, T2>, Per<T2, T1>>();
        }
        #endregion

        #region ScaledBy
        private static Q<TOut> scaledBy<T1, T2, TOut>(this Q<T1> @this, Q<T2> scalar)
        {
            return new Q<TOut>(@this.Value * scalar.Value);
        }

        public static Q<TUnit> X<TUnit>(this Q<TUnit> @this, double scalar)
        {
            return new Q<TUnit>(@this.Value * scalar);
        }

        public static Q<X<T1, T2>> X<T1, T2>(this Q<T1> @this, Q<T2> scalar)
        {
            return @this.scaledBy<T1, T2, X<T1, T2>>(scalar);
        }

        public static Q<Per<X<T1, T3>, T2>> X<T1, T2, T3>(this Q<Per<T1, T2>> @this, Q<T3> scalar)
        {
            return @this.scaledBy<Per<T1, T2>, T3, Per<X<T1, T3>, T2>>(scalar);
        }

        public static Q<Per<T1, T2>> X<T1, T2, T3>(this Q<Per<T1, X<T2, T3>>> @this, Q<T3> scalar)
        {
            return @this.scaledBy<Per<T1, X<T2, T3>>, T3, Per<T1, T2>>(scalar);
        }

        public static Q<Per<T2, T3>> X<T1, T2, T3>(this Q<Per<X<T1, T2>, T3>> @this, Q<Per<Unitless, T1>> scalar)
        {
            return @this.scaledBy<Per<X<T1, T2>, T3>, Per<Unitless, T1>, Per<T2, T3>>(scalar);
        }

        public static Q<Unitless> X<TUnit>(this Q<TUnit> @this, Q<Per<Unitless, TUnit>> scalar)
        {
            return @this.scaledBy<TUnit, Per<Unitless, TUnit>, Unitless>(scalar);
        }
        #endregion

        #region IEnumerable
        public static Q<TUnit> Sum<TUnit>(this IEnumerable<Q<TUnit>> items)
        {
            return new Q<TUnit>(items.Sum(q => q.Value));
        }
        #endregion
        #endregion

        #region Vector3
        #region Unit
        public static Vector3<Unitless> Unit<TUnit>(this Vector3<TUnit> @this)
        {
            double mag = @this.Magnitude.Value;
            return new Vector3<Unitless>()
            {
                X = @this.X / mag,
                Y = @this.Y / mag,
                Z = @this.Z / mag
            };
        }
        #endregion

        #region ScaledBy
        public static Vector3<TOut> scaledBy<T1, T2, TOut>(this Vector3<T1> @this, Q<T2> scalar)
        {
            return new Vector3<TOut>()
            {
                X = @this.X * scalar.Value,
                Y = @this.Y * scalar.Value,
                Z = @this.Z * scalar.Value
            };
        }

        public static Vector3<TUnit> X<TUnit>(this Vector3<TUnit> @this, double scalar)
        {
            return new Vector3<TUnit>()
            {
                X = @this.X * scalar,
                Y = @this.Y * scalar,
                Z = @this.Z * scalar
            };
        }

        public static Vector3<TUnit> X<TUnit>(this Vector3<Unitless> @this, Q<TUnit> scalar)
        {
            return @this.scaledBy<Unitless, TUnit, TUnit>(scalar);
        }

        public static Vector3<T1> X<T1, T2>(this Vector3<Per<T1, T2>> @this, Q<T2> scalar)
        {
            return @this.scaledBy<Per<T1, T2>, T2, T1>(scalar);
        }

        public static Vector3<Per<X<T1, T2>, T3>> X<T1, T2, T3, T4>(this Vector3<Per<X<T1, T2>, X<T3, T4>>> @this, Q<T4> scalar)
        {
            return @this.scaledBy<Per<X<T1, T2>, X<T3, T4>>, T4, Per<X<T1, T2>, T3>>(scalar);
        }

        public static Vector3<Per<T1, T2>> X<T1, T2, T3>(this Vector3<Per<T1, X<T2, T3>>> @this, Q<T3> scalar)
        {
            return @this.scaledBy<Per<T1, X<T2, T3>>, T3, Per<T1, T2>>(scalar);
        }

        public static Vector3<Per<X<T1, T3>, T2>> X<T1, T2, T3>(this Vector3<Per<T1, T2>> @this, Q<T3> scalar)
        {
            return @this.scaledBy<Per<T1, T2>, T3, Per<X<T1, T3>, T2>>(scalar);
        }

        public static Vector3<Per<T1, T3>> X<T1, T2, T3>(this Vector3<Per<X<T1, T2>, T3>> @this, Q<Per<Unitless, T2>> scalar)
        {
            return @this.scaledBy<Per<X<T1, T2>, T3>, Per<Unitless, T2>, Per<T1, T3>>(scalar);
        }

        public static Vector3<T1> X<T1, T2>(this Vector3<X<T1, T2>> @this, Q<Per<Unitless, T2>> scalar)
        {
            return @this.scaledBy<X<T1, T2>, Per<Unitless, T2>, T1>(scalar);
        }

        public static Vector3<X<T1, T2>> X<T1, T2>(this Vector3<T1> @this, Q<T2> scalar)
        {
            return @this.scaledBy<T1, T2, X<T1, T2>>(scalar);
        }
        #endregion

        #region Dot
        private static Q<TOut> dot<T1, T2, TOut>(this Vector3<T1> @this, Vector3<T2> other)
        {
            return new Q<TOut>()
            {
                Value = @this.X * other.X + @this.Y * other.Y + @this.Z * other.Z
            };
        }

        public static Q<X<X<T1, T2>, T3>> Dot<T1, T2, T3>(this Vector3<T1> @this, Vector3<X<T2, T3>> other)
        {
            return @this.dot<T1, X<T2, T3>, X<X<T1, T2>, T3>>(other);
        }

        public static Q<Per<X<T1, T2>, T3>> Dot<T1, T2, T3>(this Vector3<T1> @this, Vector3<Per<T2, T3>> other)
        {
            return @this.dot<T1, Per<T2, T3>, Per<X<T1, T2>, T3>>(other);
        }

        public static Q<Per<X<T1, T3>, X<T2, T4>>> Dot<T1, T2, T3, T4>(this Vector3<Per<T1, T2>> @this, Vector3<Per<T3, T4>> other)
        {
            return @this.dot<Per<T1, T2>, Per<T3, T4>, Per<X<T1, T3>, X<T2, T4>>>(other);
        }

        public static Q<X<T1, T2>> Dot<T1, T2>(this Vector3<T1> @this, Vector3<T2> other)
        {
            return @this.dot<T1, T2, X<T1, T2>>(other);
        }
        #endregion

        #region Cross
        private static Vector3<TOut> cross<T1, T2, TOut>(this Vector3<T1> @this, Vector3<T2> other)
        {
            return new Vector3<TOut>()
            {
                X = @this.Y * other.Z - @this.Z * other.Y,
                Y = @this.Z * other.X - @this.X * other.Z,
                Z = @this.X * other.Y - @this.Y * other.X
            };
        }

        public static Vector3<Per<X<X<T1, T2>, T3>, T4>> Cross<T1, T2, T3, T4>(this Vector3<T1> @this, Vector3<Per<X<T2, T3>, T4>> other)
        {
            return @this.cross<T1, Per<X<T2, T3>, T4>, Per<X<X<T1, T2>, T3>, T4>>(other);
        }

        public static Vector3<X<T1, T2>> Cross<T1, T2>(this Vector3<T1> @this, Vector3<T2> other)
        {
            return @this.cross<T1, T2, X<T1, T2>>(other);
        }
        #endregion

        #region Linear Operations
        public static Vector3<TUnit> Plus<TUnit>(this Vector3<TUnit> @this, Vector3<TUnit> other)
        {
            return new Vector3<TUnit>()
            {
                X = @this.X + other.X,
                Y = @this.Y + other.Y,
                Z = @this.Z + other.Z
            };
        }

        public static Vector3<TUnit> Minus<TUnit>(this Vector3<TUnit> @this, Vector3<TUnit> other)
        {
            return new Vector3<TUnit>()
            {
                X = @this.X - other.X,
                Y = @this.Y - other.Y,
                Z = @this.Z - other.Z
            };
        }
        #endregion

        #region IEnumerable
        public static Vector3<TUnit> Sum<TUnit>(this IEnumerable<Vector3<TUnit>> items)
        {
            return items.Aggregate(new Vector3<TUnit>(), (acc, item) => acc.Plus(item));
        }
        #endregion
        #endregion
    }
}
