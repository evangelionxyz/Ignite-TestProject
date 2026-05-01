using Ignite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Scripts.Cards.Entities
{
    public class PolicyCardEntity : Entity
    {
        [SerializeField]
        private int _dataId = 0;
        public PolicyCard Data;

        [SerializeField]
        private string _name = "";
        [SerializeField]
        private string _effectDesc = "";
        [SerializeField]
        private int _baseGdp = 0;
        [SerializeField]
        private int _bioEffect = 0;
        [SerializeField]
        private int _lowClassEFfect = 0;
        [SerializeField]
        private int _highClassEFfect = 0;

        public override void OnCreate()
        {
            Data = CardRegistry.Get<PolicyCard>(_dataId);

            _name = Data.Name;
            _effectDesc = Data.EffectDesc;
            _baseGdp = Data.BaseGdp;
            _bioEffect = Data.EffectToNature;
            _lowClassEFfect = Data.EffectToLowClass;
            _highClassEFfect = Data.EffectToHighClass;
        }
    }
}
