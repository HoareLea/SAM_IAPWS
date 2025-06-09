using System;

namespace SAM.Core.IAPWS
{
    /// <summary>
    /// Provides thermodynamic property calculations for Region 2 (superheated steam) 
    /// of the IAPWS-IF97 standard (valid for 273.15 K ≤ T ≤ 1073.15 K and P ≤ 10 MPa).
    /// </summary>
    /// <remarks>
    /// Based on IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam.
    /// Reference: https://www.iapws.org/relguide/IF97-Rev.pdf
    /// </remarks>
    public class Region2Calculator : RegionCalculator
    {
        private const double SpecificGasConstant = 0.461526; // [kJ/kg·K]
        private const double ReferenceTemperature = 540.0;    // [K]
        private const double ReferencePressure = 1.0;         // [MPa]

        // Coefficients for phi (43 terms)
        private static readonly int[] I = {
            0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
            1, 1, 1, 1, 1, 1, 2, 2, 2, 2,
            2, 3, 3, 3, 4, 4, 4, 5, 5, 6,
            6, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7
        };

        private static readonly int[] J = {
            -2, -1, 0, 1, 2, 3, 4, 5,
            -9, -7, -1, 0, 1, 3, 7, 9,
            -3, 0, 1, 3, 17, -4, 0, 6,
            -5, -2, 10, -8, -11, -6,
            -29, -31, -38, -39, -40, -41, -42, -43, -44, -45,
            -46, -47, -48
        };

        private static readonly double[] n = {
            -0.0017731742473213, -0.017834862292358, -0.045996013696365,
            -0.057581259083432, -0.050325278727930, -0.000033032641670203,
            0.000038121019556466, 0.000009625054091777,
            -1.5549816803930E-05, -2.3380475088780E-04,
            7.8446903174950E-04, 0.000055817453952460,
            -0.000052838357969930, -0.00047184321073267,
            -0.00030001780793026, 0.000047661393906987,
            0.000000652038560282, -0.000033403757351843,
            -0.00018948987516315, -0.0039392777243355,
            -0.043797295650573, 0.000029234704884379,
            0.000055503957506234, -0.0011510546521286,
            0.000004803985516251, -0.0000080074085099451,
            -0.0000096074065302306, 0.00000042266121651627,
            0.000000026695029230681, -0.00000000063224485677469,
            -0.00000000015782849022900, -0.00000000000062326295133224,
            -0.000000000000050869200432457, -0.000000000000005449273262538,
            0.00000000000000072703361518766, -0.000000000000000034460997892411,
            0.00000000000000000053082680122037, -0.000000000000000000047388203284103,
            0.0000000000000000000000054522823169655, -0.00000000000000000000000010285298484727,
            0.0000000000000000000000000011467615373799, -0.00000000000000000000000000001301057821257,
            0.00000000000000000000000000000011067722737359
        };

        private static double CalculatePhi(double pi, double tau)
        {
            double sum = 0.0;
            for (int k = 0; k < n.Length; k++)
                sum += n[k] * Math.Pow(7.1 - pi, I[k]) * Math.Pow(tau - 0.5, J[k]);
            return sum;
        }

        private static double CalculatePhiTau(double pi, double tau)
        {
            double sum = 0.0;
            for (int k = 0; k < n.Length; k++)
                sum += n[k] * Math.Pow(7.1 - pi, I[k]) * J[k] * Math.Pow(tau - 0.5, J[k] - 1);
            return sum;
        }

        private static double CalculatePhiPi(double pi, double tau)
        {
            double sum = 0.0;
            for (int k = 0; k < n.Length; k++)
                sum += -n[k] * I[k] * Math.Pow(7.1 - pi, I[k] - 1) * Math.Pow(tau - 0.5, J[k]);
            return sum;
        }

        private static double CalculatePhiTauTau(double pi, double tau)
        {
            double sum = 0.0;
            for (int k = 0; k < n.Length; k++)
                sum += n[k] * Math.Pow(7.1 - pi, I[k]) * J[k] * (J[k] - 1) * Math.Pow(tau - 0.5, J[k] - 2);
            return sum;
        }

        public static double CalculateSpecificEnthalpy(double pressureMegapascal, double temperatureKelvin)
        {
            double pi = pressureMegapascal / ReferencePressure;
            double tau = ReferenceTemperature / temperatureKelvin;
            return SpecificGasConstant * temperatureKelvin * (tau * CalculatePhiTau(pi, tau));
        }

        public static double CalculateSpecificEntropy(double pressureMegapascal, double temperatureKelvin)
        {
            double pi = pressureMegapascal / ReferencePressure;
            double tau = ReferenceTemperature / temperatureKelvin;
            return SpecificGasConstant * (tau * CalculatePhiTau(pi, tau) - CalculatePhi(pi, tau));
        }

        public static double CalculateSpecificVolume(double pressureMegapascal, double temperatureKelvin)
        {
            double pi = pressureMegapascal / ReferencePressure;
            double tau = ReferenceTemperature / temperatureKelvin;
            return SpecificGasConstant * temperatureKelvin / (pressureMegapascal * 1000) * pi * CalculatePhiPi(pi, tau);
        }

        public static double CalculateSpecificHeatCp(double pressureMegapascal, double temperatureKelvin)
        {
            double pi = pressureMegapascal / ReferencePressure;
            double tau = ReferenceTemperature / temperatureKelvin;
            return -SpecificGasConstant * tau * tau * CalculatePhiTauTau(pi, tau);
        }
    }
}
