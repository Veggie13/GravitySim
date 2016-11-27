using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitySim
{
    class Particle
    {
        private List<Vector3<M>> _history = new List<Vector3<M>>();

        public Q<KG> Mass;
        public Vector3<M> Position;
        public Vector3<Per<M, S>> Velocity;

        public Q<M> Radius
        {
            get { return new Q<M>(Math.Pow(Mass.Value, 1 / 3.0) / 6); }
        }

        public Vector3<Per<X<M, KG>, S>> Momentum
        {
            get { return Velocity.X(Mass); }
        }

        public Q<Per<X<X<M, M>, KG>, X<S, S>>> Energy
        {
            get { return Velocity.Dot(Velocity).X(Mass).X(0.5); }
        }

        public IEnumerable<Vector3<M>> History
        {
            get { return _history; }
        }

        public void Update(Vector3<Per<M, X<S, S>>> a, Q<S> dt)
        {
            if (!_history.Any())
            {
                _history.Add(Position);
            }

            Position.Add(Velocity.X(dt).Plus(a.X(dt).X(dt).X(0.5)));
            Velocity.Add(a.X(dt));

            _history.Add(Position);

            if (_history.Count > 10)
            {
                _history.RemoveRange(0, _history.Count - 10);
            }
        }
    }

    static class Particles
    {
        private static System.Random Rand = new System.Random();

        private static Vector3<Unitless> RandomInSphere()
        {
            double phi = Rand.NextDouble() * Math.PI * 2.0;
            double r = Math.Pow(Rand.NextDouble(), 1 / 3.0);
            double cosTheta = 2 * Rand.NextDouble() - 1;

            return new Vector3<Unitless>()
            {
                X = r * Math.Sqrt(1 - cosTheta * cosTheta) * Math.Cos(phi),
                Y = r * Math.Sqrt(1 - cosTheta * cosTheta) * Math.Sin(phi),
                Z = r * cosTheta
            };
        }

        public static Particle Random(Q<KG> minMass, Q<KG> maxMass, Q<M> maxRadius, Q<Per<M, S>> maxSpeed)
        {
            return new Particle()
            {
                Mass = minMass + (maxMass - minMass).X(Rand.NextDouble()),
                Position = RandomInSphere().X(maxRadius),
                Velocity = RandomInSphere().X(maxSpeed)
            };
        }

        public static IEnumerable<Particle> Generate(Q<KG> minMass, Q<KG> maxMass, Q<M> maxRadius, Q<Per<M, S>> maxSpeed)
        {
            while (true)
            {
                yield return Random(minMass, maxMass, maxRadius, maxSpeed);
            }
        }

        public static Vector3<M> CenterOfMass(this IEnumerable<Particle> items)
        {
            Q<KG> totalMass = items.Select(p => p.Mass).Sum();
            return items.Select(p => p.Position.X(p.Mass)).Sum().X(totalMass.Inv());
        }

        public static Vector3<Per<X<M, KG>, S>> Momentum(this IEnumerable<Particle> items)
        {
            return items.Select(p => p.Momentum).Sum();
        }

        public static Vector3<Per<X<X<M, M>, KG>, S>> AngularMomentum(this IEnumerable<Particle> items)
        {
            return items.Select(p => p.Position.Cross(p.Momentum)).Sum();
        }

        public static Particle Combine(this IEnumerable<Particle> items)
        {
            var totalMass = items.Select(p => p.Mass).Sum();

            return new Particle()
            {
                Mass = totalMass,
                Position = items.CenterOfMass(),
                Velocity = items.Momentum().X(totalMass.Inv())
            };
        }

        public static Particle Combine(params Particle[] items)
        {
            return items.Combine();
        }
    }
}
