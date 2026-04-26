using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Scripts.Class.Base
{
    public abstract class ActionCard: Card
    {
        public ActionCard(string name, string effectDesc) : base(name, effectDesc)
        {
        }

        public abstract void Use(PolicyCard card);
    }
}
