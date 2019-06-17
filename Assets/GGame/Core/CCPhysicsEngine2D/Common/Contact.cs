

namespace GGame.Core.Physics
{
    public class Contact
    {
        public string Id { get; set; }
        public Vertex Vertex { get; set; }
        public double NormalImpulse { get; set; }
        public double TangentImpulse { get; set; }
    }
}
