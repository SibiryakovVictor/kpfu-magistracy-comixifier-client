using Comixification.Comixifier;
using Zenject;

namespace TopRightMenu.Comixifier
{
    public class UICutOut : UIComixifier
    {
        private CutOut _comixifier;
        
        [Inject]
        public void Construct(CutOut comixifier)
        {
            _comixifier = comixifier;
        }
        
        public override void Activate()
        {
            
        }

        public override Comixification.Comixifier.Comixifier GetComixifier()
        {
            return _comixifier;
        }
    }
}