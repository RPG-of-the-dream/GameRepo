using UnityEngine;

namespace InputReader
{
    public class ExternalDevicesInputReader : IEntityInputSource
    {
        public Vector2 Direction => new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        public bool Attack { get; private set; }

        public void ResetOneTimeActions() => Attack = false;
    }
}