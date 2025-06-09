using System;

namespace SAM.Core.IAPWS
{
    /// <summary>
    /// Provides thermodynamic property calculations for Region 1 (compressed liquid water) 
    /// of the IAPWS-IF97 standard (valid for T ≤ 623.15 K and P ≤ 100 MPa).
    /// </summary>
    /// <remarks>
    /// Based on IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam.
    /// Reference: https://www.iapws.org/relguide/IF97-Rev.pdf
    /// </remarks>
    public class Region1Calculator : RegionCalculator
    {
        private const double SpecificGasConstant = 0.461526; // [kJ/kg·K], R for water
        private const double ReferenceTemperature = 1386.0;   // [K]
        private const double ReferencePressure = 16.53;       // [MPa]

        // Coefficients for dimensionless Helmholtz function phi (34 terms)
        // Source: IAPWS-IF97 Region 1
        private static readonly int[] I = {
            0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1,
            2, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 8, 8, 21, 23, 29, 30
        };

        private static readonly int[] J = {
            -2, -1, 0, 1, 2, 3, 4, 5,
            -9, -7, -1, 0, 1, 3, 7, 9,
            -3, 0, 1, 3, 17, -4, 0, 6,
            -5, -2, 10, -8, -11, -6, -29, -31, -38, -39
        };

        private static readonly double[] n = {
            0.14632971213167, -0.84548187169114, -3.756360367204, 3.3855169168385,
            -0.95791963387872, 0.15772038513228, -0.016616417199501,
            0.00081214629983568, 0.00028319080123804, -0.00060706301565874,
            -0.018990068218419, -0.032529748770505, -0.021841717175414,
            -5.283835796993e-05, -4.7184321073267e-04, -3.0001780793026e-04,
            4.7661393906987e-05, -4.4141845330846e-06, -7.2694996297594e-06,
            -3.1679644845054e-05, -2.8270797985312e-10, -8.5205128120103e-10,
            -2.2425281908000e-06, -6.5171222895601e-07, -1.4341729937924e-13,
            -4.0516996860117e-07, -1.2734301741641e-09, -1.7424871230634e-10,
            -6.8762131295531e-19, 1.4478307828521e-20, 2.6335781662795e-23,
            -1.1947622640071e-23, 1.8228094581404e-24, -9.3537087292458e-26
        };

        /// <summary>
        /// Calculates the dimensionless Helmholtz energy function φ.
        /// </summary>
        private static double CalculatePhi(double pi, double tau)
        {
            double sum = 0.0;
            for (int k = 0; k < n.Length; k++)
                sum += n[k] * Math.Pow(7.1 - pi, I[k]) * Math.Pow(tau - 1.222, J[k]);
            return sum;
        }

        /// <summary>
        /// First derivative of φ with respect to τ.
        /// </summary>
        private static double CalculatePhiTau(double pi, double tau)
        {
            double sum = 0.0;
            for (int k = 0; k < n.Length; k++)
                sum += n[k] * Math.Pow(7.1 - pi, I[k]) * J[k] * Math.Pow(tau - 1.222, J[k] - 1);
            return sum;
        }

        /// <summary>
        /// First derivative of φ with respect to π.
        /// </summary>
        private static double CalculatePhiPi(double pi, double tau)
        {
            double sum = 0.0;
            for (int k = 0; k < n.Length; k++)
                sum += -n[k] * I[k] * Math.Pow(7.1 - pi, I[k] - 1) * Math.Pow(tau - 1.222, J[k]);
            return sum;
        }

        /// <summary>
        /// Second derivative of φ with respect to τ.
        /// </summary>
        private static double CalculatePhiTauTau(double pi, double tau)
        {
            double sum = 0.0;
            for (int k = 0; k < n.Length; k++)
                sum += n[k] * Math.Pow(7.1 - pi, I[k]) * J[k] * (J[k] - 1) * Math.Pow(tau - 1.222, J[k] - 2);
            return sum;
        }

        /// <summary>
        /// Calculates specific enthalpy [kJ/kg] for Region 1 given pressure [MPa] and temperature [K].
        /// </summary>
        public static double CalculateSpecificEnthalpy(double pressureMegapascal, double temperatureKelvin)
        {
            double pi = pressureMegapascal / ReferencePressure;
            double tau = ReferenceTemperature / temperatureKelvin;
            return SpecificGasConstant * temperatureKelvin * (tau * CalculatePhiTau(pi, tau));
        }

        /// <summary>
        /// Calculates specific entropy [kJ/kg·K] for Region 1 given pressure [MPa] and temperature [K].
        /// </summary>
        public static double CalculateSpecificEntropy(double pressureMegapascal, double temperatureKelvin)
        {
            double pi = pressureMegapascal / ReferencePressure;
            double tau = ReferenceTemperature / temperatureKelvin;
            return SpecificGasConstant * (tau * CalculatePhiTau(pi, tau) - CalculatePhi(pi, tau));
        }

        /// <summary>
        /// Calculates specific volume [m³/kg] for Region 1 given pressure [MPa] and temperature [K].
        /// </summary>
        public static double CalculateSpecificVolume(double pressureMegapascal, double temperatureKelvin)
        {
            double pi = pressureMegapascal / ReferencePressure;
            double tau = ReferenceTemperature / temperatureKelvin;
            return SpecificGasConstant * temperatureKelvin / (pressureMegapascal * 1000) * pi * CalculatePhiPi(pi, tau);
        }

        /// <summary>
        /// Calculates specific heat capacity at constant pressure cp [kJ/kg·K] for Region 1.
        /// </summary>
        public static double CalculateSpecificHeatCp(double pressureMegapascal, double temperatureKelvin)
        {
            double pi = pressureMegapascal / ReferencePressure;
            double tau = ReferenceTemperature / temperatureKelvin;
            return -SpecificGasConstant * tau * tau * CalculatePhiTauTau(pi, tau);
        }

        /// <summary>
        /// Inverts specific enthalpy to estimate temperature [K] given pressure [MPa] and enthalpy [kJ/kg].
        /// Uses Newton-Raphson iteration.
        /// </summary>
        public static double InvertTemperatureFromEnthalpy(double pressureMegapascal, double targetEnthalpy)
        {
            double T = 300.0; // initial guess in K
            for (int i = 0; i < 10; i++)
            {
                double h = CalculateSpecificEnthalpy(pressureMegapascal, T);
                double h1 = CalculateSpecificEnthalpy(pressureMegapascal, T + 0.01);
                double derivative = (h1 - h) / 0.01;
                T -= (h - targetEnthalpy) / derivative;
            }
            return T;
        }

        /// <summary>
        /// Inverts specific entropy to estimate temperature [K] given pressure [MPa] and entropy [kJ/kg·K].
        /// Uses Newton-Raphson iteration.
        /// </summary>
        public static double InvertTemperatureFromEntropy(double pressureMegapascal, double targetEntropy)
        {
            double T = 300.0; // initial guess in K
            for (int i = 0; i < 10; i++)
            {
                double s = CalculateSpecificEntropy(pressureMegapascal, T);
                double s1 = CalculateSpecificEntropy(pressureMegapascal, T + 0.01);
                double derivative = (s1 - s) / 0.01;
                T -= (s - targetEntropy) / derivative;
            }
            return T;
        }

        /// <summary>
        /// Inverts density to estimate temperature [K] given pressure [MPa] and density [kg/m³].
        /// Uses Newton-Raphson iteration.
        /// </summary>
        public static double InvertTemperatureFromDensity(double pressureMegapascal, double targetDensity)
        {
            double T = 300.0; // initial guess in K
            for (int i = 0; i < 10; i++)
            {
                double rho = CalculateDensity(pressureMegapascal, T);
                double rho1 = CalculateDensity(pressureMegapascal, T + 0.01);
                double derivative = (rho1 - rho) / 0.01;
                T -= (rho - targetDensity) / derivative;
            }
            return T;
        }

        public static double CalculateDensity(double pressureMegapascal, double temperature)
        {
            return 1.0 / CalculateSpecificVolume(pressureMegapascal, temperature);
        }
    }
}
