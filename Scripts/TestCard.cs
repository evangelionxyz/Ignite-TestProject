using Ignite;

namespace TestProject.Scripts
{
    public class TestCard : Entity
    {
        [SerializeField]
        protected int id = 0;

        AssetHandle cardConfig;
        public override void OnCreate()
        {
        }
    }
}
