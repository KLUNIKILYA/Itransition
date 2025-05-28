using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task3
{
    public class Dice
    {
        public int[] Faces { get; }
        public int Id { get; }

        public Dice(int id, int[] faces)
        {
            Id = id;
            Faces = faces;
        }

        public int Roll(int faceIndex) => Faces[faceIndex];

        public override string ToString() => $"[{string.Join(",", Faces)}]";
    }
}
