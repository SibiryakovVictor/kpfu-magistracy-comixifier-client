using UnityEngine;

namespace TopRightMenu.Comixifier
{
    public abstract class UIComixifier : MonoBehaviour
    {
        public abstract void Activate();

        public abstract Comixification.Comixifier.Comixifier GetComixifier();
    }
}