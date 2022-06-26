using UnityEngine;

namespace Assets.CodeBase.Infrastructure.Bank
{
    public class BankRepository : Repository
    {
        private const string Key = "Coins";

        public int coins { get; set; }

        public override void Initialize()
        {
            coins = PlayerPrefs.GetInt(Key);
        }

        public override void Save()
        {
            PlayerPrefs.SetInt(Key, coins);
        }
    }
}


