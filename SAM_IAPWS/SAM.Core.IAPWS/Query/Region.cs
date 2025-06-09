using System;

namespace SAM.Core.IAPWS
{
    public static partial class Query
    {
        /// <summary>
        /// Returns the region for the given state point.
        /// </summary>
        public static Region Region(double dryBulbTemperature, double pressure)
        {
            double dryBulbTemperature_K = dryBulbTemperature + 273.15;

            // All constants converted from MPa to Pa
            if (dryBulbTemperature_K > 1073.15 && dryBulbTemperature_K <= 2273.15 && pressure <= 50e6)
                return IAPWS.Region.Region5;

            double saturationPressure = Region4Calculator.SaturationPressure(dryBulbTemperature);

            if (Math.Abs(pressure - saturationPressure) < 100.0)
                return IAPWS.Region.Region4;

            if (dryBulbTemperature_K <= 623.15 && pressure <= 100e6)
            {
                if (pressure > saturationPressure)
                    return IAPWS.Region.Region1;
                else
                    return IAPWS.Region.Region2;
            }

            if (dryBulbTemperature_K > 623.15 && dryBulbTemperature_K <= 863.15 && pressure > 16.5292e6 && pressure <= 100e6)
                return IAPWS.Region.Region3;

            return IAPWS.Region.Undefined;
        }
    }
}
