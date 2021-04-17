using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Common;

namespace Assets.Scripts.Enemy
{
    public class EnemyHealthManager : MonoBehaviour
    {

        [SerializeField]
        private float _maxHealth = 1000;
        [SerializeField]
        private GameEvent _enemyDeathEvent;

        public GameObject HealthBar;
        public Slider Slider;

        private bool _started = false; // Indicates whether the enemy has started fighting

        public float CurrHealth { get; private set; }

        private void Start()
        {
            CurrHealth = _maxHealth;
            HealthBar.SetActive(false);
            Slider.minValue = 0;
            Slider.maxValue = _maxHealth;
            Slider.value = CurrHealth;
        }

        public void ActivateHealthBar()
        {
            _started = true;
            HealthBar.SetActive(true);
        }

        public void DecreaseHealth(float healthDecrease)
        {
            if (_started)
            {
                CurrHealth -= healthDecrease;

                if (CurrHealth < 0)
                {
                    CurrHealth = 0;
                    _enemyDeathEvent.Raise();
                    HealthBar.SetActive(false);
                    SharedInfo.KujengaBossDefeated = true;
                    GameObject.Find("AudioManager").GetComponent<SoundManager>().setChosen(0);
                }

                // Updating health bar in UI
                Slider.value = CurrHealth;
            }
        }
        public void IncreaseHealth(float healthDecrease)
        {
            if (_started)
            {
                CurrHealth += healthDecrease;

                if (CurrHealth > _maxHealth)
                    CurrHealth = _maxHealth;

                // Updating health bar in UI
                Slider.value = CurrHealth;
            }
        }
    }
}