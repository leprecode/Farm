using Assets.CodeBase.Infrastructure.Bank;
using System.Collections;
using UnityEngine;

namespace Assets.CodeBase.Infrastructure
{
    public class GameEntryPoint : MonoBehaviour
    {
        public static GameEntryPoint instance { get; private set; }

        public BankInteractor _bankInteractor { get; private set; }

        private void Awake()
        {
            CheckInstance();

            _bankInteractor = new BankInteractor(new BankRepository());
            _bankInteractor.Initialize();
        }

        private void CheckInstance()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}


