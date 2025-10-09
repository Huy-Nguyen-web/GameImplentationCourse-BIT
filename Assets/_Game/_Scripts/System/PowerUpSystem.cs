using UnityEngine.Rendering;

namespace _Game._Scripts.System
{
    public class PowerUpSystem : Singleton<PowerUpSystem>
    {
        private BasePowerUp _currentPowerUp;

        public void PerformPowerUp()
        {
            if (_currentPowerUp == null) return;
            _currentPowerUp.Perform();
            Invoke(nameof(StopPowerUp), _currentPowerUp.duration);
        }

        public void StopPowerUp()
        {
            if(_currentPowerUp == null) return;
            _currentPowerUp.Stop();
            _currentPowerUp = null;
        }

        public void SetPowerUp(BasePowerUp powerUp)
        {
            if (powerUp == null) return;
            StopPowerUp();
            _currentPowerUp = powerUp;
        }
        
        public BasePowerUp GetPowerUp() => _currentPowerUp;
    }
}