using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class SurroundingExplorer
    {
        public bool Bonus = false;
        public BonusTypeEnum BonusType;

        private const float NodeSize = 6.5f;

        public void GetSurroundings(int radius, Turret turret)
        {
            var newRad = radius*NodeSize;
            var selfTag = turret.gameObject.tag;
            var temp = turret.transform.position;
            var radiusVector = Vector3.one*newRad;

            var cast = Physics.OverlapBox(temp, radiusVector);

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
            //normal: dano na range (implementar)             //prioridade 1 (maior)
            //bonus: slow (azul - tipo congelado? )

            Ipe,
            //normal: atira normalmente
            //bonus: gera uma seed a cada tantos segundos (1 seed - 30 s estipulado) (light)

            Jaqueira
            //normal: jaqueira explode 
            //bonus: inimigo leva dano continuo (poison) (vermelho piscando)
        }

    }
}
