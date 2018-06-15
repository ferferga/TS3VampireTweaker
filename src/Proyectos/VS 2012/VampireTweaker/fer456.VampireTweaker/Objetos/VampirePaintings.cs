using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Objects.Decorations;
using Sims3.SimIFace;
namespace Sims3.Gameplay.VT.Paintings
{
    public class VampirePainting : Painting
    {
        [Tunable]
        private static GameObject.EnvironmentMotive kEnvironmentTuning = new GameObject.EnvironmentMotive();
        [Tunable]
        private static AbstractArtObject.ViewTuning kViewTuning = new AbstractArtObject.ViewTuning();
        public override GameObject.EnvironmentMotive EnvironmentTuning
        {
            get
            {
                return VampirePainting.kEnvironmentTuning;
            }
        }
        public override AbstractArtObject.ViewTuning TuningView
        {
            get
            {
                return VampirePainting.kViewTuning;
            }
        }
    }
}
