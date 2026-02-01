using UnityEngine;
using DeviceType = Project.Enums.DeviceType;

namespace Project.UI
{
    public class DeviceChecker
    {
        private const float Size = 6.5f;
        private const float Ratio = 2.0f;

        public DeviceType CheckDevice()
        {
            var result = DeviceDiagonalSizeInInches() > Size && Screen.width / (float)Screen.height < Ratio;

            return result ? DeviceType.Tablet : DeviceType.Phone;
        }

        private float DeviceDiagonalSizeInInches()
        {
            float screenWidth = Screen.width / Screen.dpi;
            float screenHeight = Screen.height / Screen.dpi;
            
            float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
         
            return diagonalInches;
        }
    }
}