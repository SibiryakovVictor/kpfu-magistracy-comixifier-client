using Comixification.Comixifier;
using Zenject;

namespace TopRightMenu.Comixifier
{
    public class UIFace2Comics : UIComixifier
    {
        private Face2Comics _face2Comics;
        
        [Inject]
        public void Construct(Face2Comics face2Comics)
        {
            _face2Comics = face2Comics;
        }
        
        public override void Activate()
        {
            
        }

        public override Comixification.Comixifier.Comixifier GetComixifier()
        {
            return _face2Comics;
        }
    }
}