using System;

namespace SAM.Core.IAPWS
{
    /// <summary>
    /// Provides saturation pressure, temperature, and saturated state properties for Region 4 (liquid-vapor boundary)
    /// based on the IAPWS-IF97 standard. Includes both empirical approximations and IAPWS-based calculations.
    /// </summary>
    /// <remarks>
    /// Region 4 defines the saturation curve (liquid-vapor equilibrium). 
    /// Saturation pressure and temperature are calculated using the Wagner & Pruß formulation.
    /// Reference: https://www.iapws.org/relguide/IF97-Rev.pdf
    /// </remarks>
    public class Region4Calculator : RegionCalculator
    {
        /// <summary>
        /// Calculates saturation pressure [Pa] for a given temperature [°C] using Region 4 formulation.
        /// </summary>
        public static double SaturationPressure(double dryBulbTemperature)
        {
            double T = dryBulbTemperature + 273.15; // Convert to Kelvin
            double[] n = new double[]
            {
                0.11670521452767e4,
               -0.72421316703206e6,
               -0.17073846940092e2,
                0.12020824702470e5,
               -0.32325550322333e7,
                0.14915108613530e2,
               -0.48232657361591e4,
                0.40511340542057e6,
               -0.23855557567849,
                0.65017534844798e3
            };

            double theta = T + n[8] / (T - n[9]);
            double A = theta * theta + n[0] * theta + n[1];
            double B = n[2] * theta * theta + n[3] * theta + n[4];
            double C = n[5] * theta * theta + n[6] * theta + n[7];
            double p = Math.Pow((2 * C) / (-B + Math.Sqrt(B * B - 4 * A * C)), 4);

            return p * 1e6; // MPa to Pa
        }

        /// <summary>
        /// Estimates saturation temperature [°C] for a given vapor pressure [Pa] using Newton-Raphson method.
        /// </summary>
        public static double InvertSaturationTemperature(double vapourPressure)
        {
            double T = 373.15; // Initial guess [K] (100 °C)
            for (int i = 0; i < 10; i++)
            {
                double f = SaturationPressure(T - 273.15) - vapourPressure;
                double f1 = SaturationPressure(T + 0.01 - 273.15) - vapourPressure;
                double df = (f1 - f) / 0.01;
                T -= f / df;
            }
            return T - 273.15; // Convert back to °C
        }

        /// <summary>
        /// Calculates approximate saturated liquid enthalpy [kJ/kg] using a constant cp-based linear fit.
        /// Commonly used in engineering approximations where precise thermodynamic modeling is not required.
        /// </summary>
        public static double CalculateSaturatedLiquidEnthalpy(double dryBulbTemperature)
        {
            return 419.0 + 4.18 * dryBulbTemperature;
        }

        /// <summary>
        /// Calculates approximate saturated vapor enthalpy [kJ/kg] using linear latent heat drop.
        /// Commonly used in engineering approximations where precise thermodynamic modeling is not required.
        /// </summary>
        public static double CalculateSaturatedVaporEnthalpy(double dryBulbTemperature)
        {
            double h_fg = 2500.0 - 20.0 * dryBulbTemperature;
            return CalculateSaturatedLiquidEnthalpy(dryBulbTemperature) + h_fg;
        }

        /// <summary>
        /// Calculates accurate saturated liquid enthalpy [kJ/kg] using IAPWS-IF97 Region 1.
        /// </summary>
        public static double CalculateSaturatedLiquidEnthalpyIAPWS(double dryBulbTemperature)
        {
            double T = dryBulbTemperature + 273.15;
            double p = SaturationPressure(dryBulbTemperature) / 1e6; // Pa to MPa
            return Region1Calculator.CalculateSpecificEnthalpy(p, T);
        }

        /// <summary>
        /// Calculates accurate saturated vapor enthalpy [kJ/kg] using IAPWS-IF97 Region 2.
        /// </summary>
        public static double CalculateSaturatedVaporEnthalpyIAPWS(double dryBulbTemperature)
        {
            double T = dryBulbTemperature + 273.15;
            double p = SaturationPressure(dryBulbTemperature) / 1e6; // Pa to MPa
            return Region2Calculator.CalculateSpecificEnthalpy(p, T);
        }

        /// <summary>
        /// Inverts saturated liquid enthalpy [kJ/kg] to estimate saturation temperature [°C] using IAPWS Region 1.
        /// </summary>
        public static double InvertTemperatureFromSaturatedLiquidEnthalpy(double targetEnthalpy)
        {
            double T = 373.15; // Initial guess [K]
            for (int i = 0; i < 10; i++)
            {
                double tC = T - 273.15;
                double h = CalculateSaturatedLiquidEnthalpyIAPWS(tC);
                double h1 = CalculateSaturatedLiquidEnthalpyIAPWS(tC + 0.01);
                double dh = (h1 - h) / 0.01;
                T -= (h - targetEnthalpy) / dh;
            }
            return T - 273.15;
        }

        /// <summary>
        /// Inverts saturated vapor enthalpy [kJ/kg] to estimate saturation temperature [°C] using IAPWS Region 2.
        /// </summary>
        public static double InvertTemperatureFromSaturatedVaporEnthalpy(double targetEnthalpy)
        {
            double T = 373.15; // Initial guess [K]
            for (int i = 0; i < 10; i++)
            {
                double tC = T - 273.15;
                double h = CalculateSaturatedVaporEnthalpyIAPWS(tC);
                double h1 = CalculateSaturatedVaporEnthalpyIAPWS(tC + 0.01);
                double dh = (h1 - h) / 0.01;
                T -= (h - targetEnthalpy) / dh;
            }
            return T - 273.15;
        }
    }
}
