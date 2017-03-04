using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class SurroundingExplorer
    {
        public bool Bonus = false;
        public BonusTypeEnum BonusType;

        private const float NodeSize = 5.5f;

        public void GetSurroundings(int radius, Turret turret)
        {
            var newRad = radius*NodeSize;
            var selfTag = turret.gameObject.tag;
            var temp = turret.transform.position;

            var cast = Physics.OverlapSphere(temp, newRad);
            
            var activateBonus = cast.Count(c => c.tag == selfTag);

            if (activateBonus < 2) return;

            SetBonusType(selfTag);
        }

        private void SetBonusType(string selfTag)
        {
            switch (selfTag)
            {
                case "Jaqueira":
                    BonusType = BonusTypeEnum.Jaqueira;
                    break;
                case "Ipe":
                    BonusType = BonusTypeEnum.Ipe;
                    break;
                case "Araucaria":
                    BonusType = BonusTypeEnum.Araucaria;
                    break;
            }
            Bonus = true;
        }

        public enum BonusTypeEnum
        {
            Araucaria,
            //normal: dano na range 
            //bonus: slow (não tem efeito ainda)

            Ipe,
            //normal: atira normalmente
            //bonus: gera uma seed a cada tantos segundos (1 seed - 30 s estipulado) 

            Jaqueira
            //normal: jaqueira explode 
            //bonus: inimigo leva dano continuo (poison) (effect: maquina soltando faisca)
        }

    }
}
