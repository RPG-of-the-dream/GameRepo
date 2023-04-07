using UnityEngine;

namespace InputReader
{
    public interface IEntityInputSource
    {
        Vector2 Direction { get; }
        bool Attack { get;}

        void ResetOneTimeActions();
    }
}