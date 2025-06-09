using System;

namespace SAM.Core.IAPWS
{
    /// <summary>
    /// Provides thermodynamic property calculations for Region 5 (ideal-gas-like superheated vapor)
    /// of the IAPWS-IF97 standard.
    /// </summary>
    /// <remarks>
    /// Region 5 covers high-temperature, low-pressure steam in the range of:
    /// - T = 1073.15 K to 2273.15 K
    /// - P ≤ 50 MPa
    /// Reference: IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam
    /// https://www.iapws.org/relguide/IF97-Rev.pdf
    /// </remarks>
    public class Region5Calculator : RegionCalculator
    {
        private const double R = 0.461526; // [kJ/kg·K] specific gas constant for water
        private const double ReferenceTemperature = 1000.0; // [K]
        private const double ReferencePressure = 1.0;       // [MPa]

        // Coefficients for the dimensionless Gibbs free energy function (γ)
        private static readonly int[] I = { 1, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 };
        private static readonly int[] J = { 0, 1, 2, 0, 2, 0, 1, 0, 1, 0, 1 };
        private static readonly double[] n =
        {
            10.214165, -47.245215, 46.183473,
           -68.517495, 36.079910, 38.493460,
           -10.683200, -23.032400, 13.777480,
             3.846590, -3.374835
        };

        /// <summary>
        /// Calculates the dimensionless Gibbs free energy γ(pi, tau).
        /// </summary>
        private static double Gamma(double pi, double tau)
        {
            double sum = 0.0;
            for (int i = 0; i < n.Length; i++)
                sum += n[i] * Math.Pow(pi, I[i]) * Math.Pow(tau, J[i]);
            return sum;
        }

        /// <summary>
        /// First derivative of γ with respect to π.
        /// </summary>
        private static double GammaPi(double pi, double tau)
        {
            double sum = 0.0;
            for (int i = 0; i < n.Length; i++)
                sum += n[i] * I[i] * Math.Pow(pi, I[i] - 1) * Math.Pow(tau, J[i]);
            return sum;
        }

        /// <summary>
        /// First derivative of γ with respect to τ.
        /// </summary>
        private static double GammaTau(double pi, double tau)
        {
            double sum = 0.0;
            for (int i = 0; i < n.Length; i++)
                sum += n[i] * J[i] * Math.Pow(pi, I[i]) * Math.Pow(tau, J[i] - 1);
            return sum;
        }

        /// <summary>
        /// Second derivative of γ with respect to τ.
        /// </summary>
        private static double GammaTauTau(double pi, double tau)
        {
            double sum = 0.0;
            for (int i = 0; i < n.Length; i++)
                sum += n[i] * J[i] * (J[i] - 1) * Math.Pow(pi, I[i]) * Math.Pow(tau, J[i] - 2);
            return sum;
        }

        /// <summary>
        /// Calculates specific enthalpy [kJ/kg] for Region 5.
        /// </summary>
        public static double CalculateSpecificEnthalpy(double pressureMPa, double temperatureK)
        {
            double pi = pressureMPa / ReferencePressure;
            double tau = ReferenceTemperature / temperatureK;
            return R * temperatureK * (1 + tau * GammaTau(pi, tau));
        }

        /// <summary>
        /// Calculates specific entropy [kJ/kg·K] for Region 5.
        /// </summary>
        public static double CalculateSpecificEntropy(double pressureMPa, double temperatureK)
        {
            double pi = pressureMPa / ReferencePressure;
            double tau = ReferenceTemperature / temperatureK;
            return R * (tau * GammaTau(pi, tau) - Gamma(pi, tau));
        }

        /// <summary>
        /// Calculates specific volume [m³/kg] for Region 5.
        /// </summary>
        public static double CalculateSpecificVolume(double pressureMPa, double temperatureK)
        {
            double pi = pressureMPa / ReferencePressure;
            double tau = ReferenceTemperature / temperatureK;
            return R * temperatureK / (pressureMPa * 1000) * pi * GammaPi(pi, tau);
        }

        /// <summary>
        /// Calculates specific heat capacity at constant pressure [kJ/kg·K] for Region 5.
        /// </summary>
        public static double CalculateSpecificHeatCp(double pressureMPa, double temperatureK)
        {
            double pi = pressureMPa / ReferencePressure;
            double tau = ReferenceTemperature / temperatureK;
            return -R * tau * tau * GammaTauTau(pi, tau);
        }
    }
}
