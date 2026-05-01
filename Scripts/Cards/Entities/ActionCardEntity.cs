using Ignite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Scripts.Cards.Entities
{
    public class ActionCardEntity : Entity
    {
        [SerializeField]
        private int _dataId;
        public ActionCard Data;

        [SerializeField]
        private string _name = "";
        [SerializeField]
        private string _effectDesc = "";

        public override void OnCreate()
        {
            Data = CardRegistry.Get<ActionCard>(_dataId);

            _name = Data.Name;
            _effectDesc = Data.EffectDesc;

            base.OnCreate();
        }
    }
}
