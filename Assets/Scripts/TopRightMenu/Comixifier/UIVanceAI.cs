using Comixification.Comixifier;
using Zenject;

namespace TopRightMenu.Comixifier
{
    public class UIVanceAI : UIComixifier
    {
        private VanceAI _vanceAI;
        
        [Inject]
        public void Construct(VanceAI vanceAI)
        {
            _vanceAI = vanceAI;
        }
        
        public override void Activate()
        {
            
        }

        public override Comixification.Comixifier.Comixifier GetComixifier()
        {
            return _vanceAI;
        }
    }
}