using Assets.CodeBase.BarnLogic;
using System.Collections;
using UnityEngine;

namespace Assets.CodeBase.Hero
{
    public class Player : MonoBehaviour
    {
        private const string _tagOfBarn = "Barn";
        [SerializeField] private Basket _basket;

        public Basket Basket { get => _basket;}

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_tagOfBarn))
            {
                GiveHarvestToBarn(other);
            }
        }

        private void GiveHarvestToBarn(Collider other)
        {
            Debug.Log("Barn!!");

            var harvestHeap = _basket.GetHarvestHeap();

            var barn = other.GetComponent<Barn>();
            barn.TakeHay(harvestHeap);
        }
    }
}