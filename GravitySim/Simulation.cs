using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitySim
{
    class Simulation
    {
        // m/s2 = (m3/kg s2) kg/m2
        private static readonly Q<Per<X<X<M, M>, M>, X<X<S, S>, KG>>> G = new Q<Per<X<X<M, M>, M>, X<X<S, S>, KG>>>(6.67e-11);

        private List<Particle> _particles;
        private Q<KG> _mass;

        public const int N = 100;
        public static readonly Q<M> Size = new Q<M>(1.5e11);
        public static readonly Q<KG> TotalMass = new Q<KG>(2e30);
        public static readonly Q<KG> MinMass = new Q<KG>(1);
        public static readonly Q<KG> MaxMass = TotalMass.X(2.0 / N);
        public static readonly Q<Per<M, S>> MaxSpeed = new Q<Per<M, S>>(0);

        public Simulation()
        {
            var particles = Particles.Generate(MinMass, MaxMass, Size, MaxSpeed).Take(N).ToList();
            var totalMass = particles.Select(p => p.Mass).Sum();
            var center = particles.CenterOfMass();
            var momentum = particles.Momentum();
            var velocity = momentum.X(totalMass.Inv());
            foreach (var p in particles)
            {
                p.Position = p.Position.Minus(center);
                p.Velocity = p.Velocity.Minus(velocity);
            }
            particles.Sort((a, b) => b.Mass.Value.CompareTo(a.Mass.Value));

            _particles = particles;
            _mass = totalMass;

            doCollisions();
        }

        public IEnumerable<Particle> Items
        {
            get { return _particles; }
        }

        public Q<KG> Mass
        {
            get { return _mass; }
        }

        public void Step(Q<S> dt)
        {
            Vector3<Per<M, X<S, S>>>[] dV = new Vector3<Per<M, X<S, S>>>[_particles.Count];

            for (int i = 0; i < _particles.Count; i++)
            {
                var p = _particles[i];

                for (int j = i + 1; j < _particles.Count; j++)
                {
                    var op = _particles[j];
                    var R = op.Position.Minus(p.Position);

                    var x = G.X(op.Mass);
                    x.X(R.Mag2.Inv());
                    dV[i].Add(R.Unit().X(G.X(op.Mass).X(R.Mag2.Inv())));
                    dV[j].Add(R.Unit().X(G.X(p.Mass).X(R.Mag2.Inv()).X(-1)));
                }

                p.Update(dV[i], dt);
            }

            doCollisions();
        }

        private void doCollisions()
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                for (int j = i + 1; j < _particles.Count; j++)
                {
                    var diff = _particles[i].Position.Minus(_particles[j].Position);
                    if (diff.Magnitude < _particles[i].Radius + _particles[j].Radius)
                    {
                        _particles[i] = Particles.Combine(_particles[i], _particles[j]);
                        _particles.RemoveAt(j);
                        i--;
                        break;
                    }
                }
            }
        }
    }
}
