using System;
using System.IO;

namespace Blast_Calc
{
    public class Spherical_Blast
    {
        /* All calculations are based on the modified Kingery and Bulmash equation for blasts. 
         * For further information read: 
         * Jeon, DooJin, KiTae Kim, and SangEul Han. "Modified Equation of Shock Wave Parameters." Computation 5.3 (2017): 41.
         * 
         * With the following numerical values, the results are valid only for a free spherical burst.
         */

        protected double dR = 1.0;
        protected double dW = 6.0;
        protected double dZ = 0.0;
        protected double dlogZ = 0.0;

        // The int of explosive type is used to determine the explosive via imput.
        protected int iExplosiveType = 10;
        //protected double dExplosiveTypeValue = 1.0;

        private const double dP0 = 101.325;
        private const double dcsound = 343.0;
        private const double dAD0 = 0.00125e3;

        // All explosives are listed with their TNT equivalent value.
        private const double dAmatol = 0.99, dCompositB = 1.11, dCompositC3 = 1.08,
            dCompositC4 = 1.37, dHMX = 1.02, dOctol = 1.06, dPETN = 1.27, dRDX = 1.14,
            dTetryl = 1.07, dTNT = 1, dTritonal = 1.07;

        //protected double[,] dExplosives = new double[11, 2] { { 1, dAmatol },
        //    { 2, dCompositB }, {3, dCompositC3 }, {4, dCompositC4 }, {5, dHMX },
        //{6, dOctol }, {7, dPETN }, {8, dRDX }, {9, dTetryl }, {10, dTNT }, {11, dTritonal } };
        
        /* Coefficients for the physical values of the spherical air burst are
         * written as CXY. The letter X is the index of the coefficient and 
         * Y the identifier for the range: near 'n', mid-range 'm', far 'f'.
         * All coefficients are stored in multidimenional arrays.
         */
        // Coefficients for the incident pressure:
        protected double[,] dCIP = new double[4, 5];
        // Coefficients for the reflected pressure:
        protected double[,] dCRP = new double[4, 5];
        // Coefficients for the incident momentum:
        protected double[,] dCIM = new double[4, 5];
        // Coefficients for the arrival time:
        protected double[,] dCAT = new double[4, 5];
        // Coefficients for the positive phase duration:
        protected double[,] dCPPD = new double[6, 6];
        // Coefficients for the shock velocity:
        protected double[,] dCSV = new double[4, 5]; 

        // Coefficients for the reflected impulse:
        protected double dC0_RI = 0, dC1_RI = 0, dC2_RI = 0, dC3_RI = 0;
        
        // Temporary Coefficients:
        protected double dC0 = 0, dC1 = 0, dC2 = 0, dC3 = 0, dC4 = 0;
        
        // Constructors:
        public Spherical_Blast()
        {
            Start();
        }

        public Spherical_Blast(double dsetR, double dsetW)
        {
            Start(dsetR, dsetW);
        }

        public Spherical_Blast(double dsetR, double dsetW, int iExplosiveTypeIO)
        {
            Start(dsetR, dsetW, iExplosiveTypeIO);
        }

        protected void Start()
        {
            // Initializes the starting values
            Set_Values();
            dZ = Get_dZ();
            dlogZ = Math.Log10(dZ);
        }

        protected void Start(double dsetR, double dsetW)
        {
            // Initializes the starting values
            Set_Values();
            dR = dsetR;
            dW = dsetW;
            dZ = Get_dZ();
            dlogZ = Math.Log10(dZ);
        }

        protected void Start(double dsetR, double dsetW, int iExplosiveTypeInput)
        {
            // Initializes the starting values
            Set_Values();
            dR = dsetR;
            dW = dsetW;
            iExplosiveType = iExplosiveTypeInput;
            dZ = Get_dZ();
            dlogZ = Math.Log10(dZ);
        }


        protected virtual void Set_Values()
        {
            // Coefficients for the incident overpressure of a spherical air burst:
            dCIP[0, 0] = 0.05; dCIP[0, 1] = 0.67; dCIP[0, 2] = 10.0; dCIP[0, 3] = 40.0;
            dCIP[1, 0] = -6.6628e-2; dCIP[1, 1] = -2.5691; dCIP[1, 2] = -1.4213; dCIP[1, 3] = -5.0355e-1; dCIP[1, 4] = -9.4865e-2;
            dCIP[2, 0] = -2.8310e-2; dCIP[2, 1] = -2.2324; dCIP[2, 2] = -4.3379e-1; dCIP[2, 3] = 1.1615; dCIP[2, 4] = -4.2023e-1;
            dCIP[3, 0] = -2.8310e-2; dCIP[3, 1] = -2.2324; dCIP[3, 2] = -4.3379e-1; dCIP[3, 3] = 1.1615; dCIP[3, 4] = -4.2023e-1;

            //dCIP[3, 0] = -1.0569e-1; dCIP[3, 1] = -4.1582e-1; dCIP[3, 2] = -6.1361e-1; dCIP[3, 3] = 1.2882; dCIP[3, 4] = 0.0;
            //dCIP[3, 1] = -4.1582e-1;

            // Coefficients for the reflected pressure:
            dCRP[0, 0] = 0.05; dCRP[0, 1] = 1.05; dCRP[0, 2] = 10.0; dCRP[0, 3] = 40.0;
            dCRP[1, 0] = 6.9758e-1; dCRP[1, 1] = -2.9928; dCRP[1, 2] = -1.3840; dCRP[1, 3] = -2.5645e-1; dCRP[1, 4] = 0.0;
            dCRP[2, 0] = 6.9699e-1; dCRP[2, 1] = -2.8246; dCRP[2, 2] = -1.1613; dCRP[2, 3] = 2.8654; dCRP[2, 4] = -1.2088;
            dCRP[3, 0] = -2.4954e-1; dCRP[3, 1] = -1.3806; dCRP[3, 2] = 0.0; dCRP[3, 3] = 0.0; dCRP[3, 4] = 0.0;

            // Coefficients for the incident momentum:
            dCIM[0, 0] = 0.05; dCIM[0, 1] = 0.79; dCIM[0, 2] = 3.99; dCIM[0, 3] = 40.0;
            dCIM[1, 0] = -5.8967e-1; dCIM[1, 1] = 1.2467; dCIM[1, 2] = 7.2584e-1; dCIM[1, 3] = -2.1542; dCIM[1, 4] = -1.1542;
            dCIM[2, 0] = -7.5978e-1; dCIM[2, 1] = -7.4416e-1; dCIM[2, 2] = -1.4680; dCIM[2, 3] = 3.8777; dCIM[2, 4] = -3.1385;
            dCIM[3, 0] = -7.7508e-1; dCIM[3, 1] = -8.4083e-1; dCIM[3, 2] = 5.8847e-2; dCIM[3, 3] = 0.0; dCIM[3, 4] = 0.0;

            // Coefficients for the reflected impulse:
            dC0_RI = -2.5256e-1; dC1_RI = -1.3067; dC2_RI = 2.2166e-1; dC3_RI = -6.3474e-2;

            // Coefficients for the arrival time:
            dCAT[0, 0] = 0.05; dCAT[0, 1] = 0.71; dCAT[0, 2] = 10.0; dCAT[0, 3] = 40.0;
            dCAT[1, 0] = -2.4704e-1; dCAT[1, 1] = 2.1318; dCAT[1, 2] = 9.95e-1; dCAT[1, 3] = 6.1033e-1; dCAT[1, 4] = 1.8836e-1;
            dCAT[2, 0] = 2.7471e-1; dCAT[2, 1] = 1.8687; dCAT[2, 2] = 1.9437e-1; dCAT[2, 3] = -6.7341e-1; dCAT[2, 4] = 2.4074e-1;
            dCAT[3, 0] = 6.9208e-2; dCAT[3, 1] = 1.3812; dCAT[3, 2] = -9.3519e-2; dCAT[3, 3] = 0.0; dCAT[3, 4] = 0.0;

            // Coefficients for the positive phase duration:
            dCPPD[0, 0] = 0.14; dCPPD[0, 1] = 0.75; dCPPD[0, 2] = 1.15; dCPPD[0, 3] = 2.93; dCPPD[0, 4] = 40.0;
            dCPPD[1, 0] = 6.6547e-1; dCPPD[1, 1] = 6.0191; dCPPD[1, 2] = 8.2785; dCPPD[1, 3] = 3.5900; dCPPD[1, 4] = 0.0;
            dCPPD[2, 0] = 2.5418e-1; dCPPD[2, 1] = 2.4840e-1; dCPPD[2, 2] = -5.3442; dCPPD[2, 3] = 55.310; dCPPD[2, 4] = 0.0;
            dCPPD[3, 0] = 2.3966e-1; dCPPD[3, 1] = 8.4271e-1; dCPPD[3, 2] = -11.795; dCPPD[3, 3] = 45.212; dCPPD[3, 4] = -47.224;
            dCPPD[4, 0] = 8.4367e-2; dCPPD[4, 1] = 1.0610; dCPPD[4, 2] = -9.2091e-1; dCPPD[4, 3] = 5.0765e-1; dCPPD[4, 4] = -1.0921e-1;

            // Coefficients for the shock velocity:
            dCSV[0, 0] = 0.05; dCSV[0, 1] = 1.16; dCSV[0, 2] = 10.0; dCSV[0, 3] = 40.0;
            dCSV[1, 0] = 5.2658e-3; dCSV[1, 1] = -1.0266; dCSV[1, 2] = -2.3754e-1; dCSV[1, 3] = 1.4415e-1; dCSV[1, 4] = 7.3166e-2;
            dCSV[2, 0] = 1.1060e-2; dCSV[2, 1] = -1.0765; dCSV[2, 2] = 5.0854e-1; dCSV[2, 3] = 4.8259e-1; dCSV[2, 4] = -3.7621e-1;
            dCSV[3, 0] = -4.2546e-1; dCSV[3, 1] = -2.5850e-2; dCSV[3, 2] = 0.0; dCSV[3, 3] = 0.0; dCSV[3, 4] = 0.0;


        }

        public void Output_Calculation()
        {
            double dY = Get_dY();
            double dYM = Get_dIM();
            double dRP = Get_dRP();
            double dRI = Get_dRM();
            double dAT = Get_dAT();
            double dPPD = Get_dPPD();
            double dSFV = Get_dSFV();
            double dPV = Get_dPV();
            double dAD = Get_dAD();
            double dDP = Get_DP();
            Console.WriteLine("Value of Z = " + dZ + "\n"
                + "Value of R = " + dR + "\n" 
                + "Value of W = " + dW + "\n"
                + "Exlosive Type = " + iExplosiveType + "\n\n"
                + "Incident Overpressure = " + dY + " kPa" + "\n"
                + "Incident Impulse = " + dYM + " kPa ms" + "\n"
                + "Reflected Pressure = " + dRP + " kPa" + "\n"
                + "Reflected Impulse = " + dRI + " kPa ms" + "\n"
                + "Arrival Time = " + dAT + " ms" + "\n"
                + "Positive Phase Duration = " + dPPD + " ms" + "\n"
                + "Shock Front Velocity = " + dSFV + " m/s" + "\n\n");
        }

        public void Output_Range(double dsetRange)
        {
            double dtmp = 0.1;
            while (dtmp < dsetRange)
            {
                dR = dtmp;
                Console.WriteLine("Position r =" + dtmp + "; Incident Overpressure = " + Get_dY()
                    + " kPa" + "\n");
                dtmp += 0.1;
            }

        }

        public void Write_Output(double dsetRange)
        {
            FileStream fs = new FileStream("Data.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            double dtmp = 0.1;

            sw.WriteLine("Position / m  Incident Overpressure / kPa");
            while (dtmp < dsetRange)
            {
                dR = dtmp;
                sw.WriteLine(dtmp + ", " + Get_dY() + "\n");
                dtmp += 0.1;
            }
            sw.Close();
            fs.Close();
        }

        protected double Get_ExplosiveTypeValue()
        {
            switch (iExplosiveType)
            {
                case 1:
                    return dAmatol;
                case 2:
                    return dCompositB;
                case 3:
                    return dCompositC3;
                case 4:
                    return dCompositC4;
                case 5:
                    return dHMX;
                case 6:
                    return dOctol;
                case 7:
                    return dPETN;
                case 8:
                    return dRDX;
                case 9:
                    return dTetryl;
                case 10:
                    return dTNT;
                case 11:
                    return dTritonal;
                default:
                    return dTNT;
            }
        }


        protected double Get_dZ()
        {
            return dR / Math.Pow((Get_ExplosiveTypeValue() * dW), (1.0 / 3.0));
        }

        protected double Get_TNT_Equivalent()
        {
            return 0;
        }

        protected double Get_dY()
        {
            // Calculates the Incident Peak Overpressure in kPa.            
            Set_Coefficients(dCIP);

            // Returns MPa -> *1000 = kPa
            return Math.Pow(10, (dC0 + dC1 * dlogZ + dC2 * Math.Pow(dlogZ, 2) 
                + dC3 * Math.Pow(dlogZ, 3) + dC4 * Math.Pow(dlogZ, 4)))*1000;
        }

        protected double Get_dRP()
        {
            // Calculates the Reflected Pressure in kPa.            
            Set_Coefficients(dCRP);

            // Returns MPa -> *1000 = kPa
            return Math.Pow(10, (dC0 + dC1 * dlogZ + dC2 * Math.Pow(dlogZ, 2)
                + dC3 * Math.Pow(dlogZ, 3) + dC4 * Math.Pow(dlogZ, 4)))*1000;
        }

        protected double Get_dIM()
        {
            // Calculates the Incident Impulse in kPa*ms.            
            Set_Coefficients(dCIM);

            // Returns MPa*ms/kg^(1/3) -> *1000*dW^(1/3) = kPa*ms
            return Math.Pow(10, (dC0 + dC1 * dlogZ + dC2 * Math.Pow(dlogZ, 2)
                + dC3 * Math.Pow(dlogZ, 3) + dC4 * Math.Pow(dlogZ, 4))) * 1000 * Math.Pow(dW, 1.0/3.0);
        }

        protected double Get_dRM()
        {
            // Calculates the Reflected Impulse in kPa*ms.
            // Returns MPa*ms/kg^(1/3) -> *1000*dW^(1/3) = kPa*ms
            return (dC0_RI + dC1_RI * dlogZ
                    + dC2_RI * Math.Pow(dlogZ, 2)
                    + dC3_RI * Math.Pow(dlogZ, 3))* 1000 * Math.Pow(dW, 1.0 / 3.0);
        }

        protected double Get_dAT()
        {
            // Calculates the Arrival Time in ms.            
            Set_Coefficients(dCAT);

            // Returns ms/kg^(1/3) -> *dW^(1/3) = ms
            return Math.Pow(10, (dC0 + dC1 * dlogZ + dC2 * Math.Pow(dlogZ, 2)
                + dC3 * Math.Pow(dlogZ, 3) + dC4 * Math.Pow(dlogZ, 4))) * Math.Pow(dW, 1.0 / 3.0);
        }

        protected double Get_dPPD()
        {
            // Calculates the Positive Phase Duration in ms.            
            Set_Coefficients_PPD(dCPPD);

            // Returns ms/kg^(1/3) -> *dW^(1/3) = ms
            return Math.Pow(10, (dC0 + dC1 * dlogZ + dC2 * Math.Pow(dlogZ, 2)
                + dC3 * Math.Pow(dlogZ, 3) + dC4 * Math.Pow(dlogZ, 4))) * Math.Pow(dW, 1.0 / 3.0);
        }



        protected double Get_dSFV()
        {
            // Calculates the Shock Fronst Velocity in m/s.                        
            Set_Coefficients(dCSV);

            // Returns m/ms -> *1000 = m/s
            return Math.Pow(10, (dC0 + dC1 * dlogZ + dC2 * Math.Pow(dlogZ, 2)
                + dC3 * Math.Pow(dlogZ, 3) + dC4 * Math.Pow(dlogZ, 4))) * 1000;
        }

        protected double Get_dPV()
        {
            // Particle velocity 
            return ((5 * Get_dY()) / (7 * dP0)) * dcsound / Math.Sqrt(1 + (6 * Get_dY() * 7 * dP0));
        }

        protected double Get_dAD()
        {
            // Air density behind the shock front based on the density of the ambient air dAD0.
            return dAD0 * (7 + ((6 * Get_dY()) / (7 * dP0)));

        }

        protected double Get_DP()
        {
            // Dynamic Pressure
            return 5 * Math.Pow(Get_dY(), 2) + 2 * (Get_dY() + 7 * dP0);
        }

        protected void Set_Coefficients(double[,] dVals)
        {
            if (dZ >= dVals[0, 0] && dZ < dVals[0, 1])
            {
                dC0 = dVals[1, 0];
                dC1 = dVals[1, 1];
                dC2 = dVals[1, 2];
                dC3 = dVals[1, 3];
                dC4 = dVals[1, 4];
            }
            else if (dZ >= dVals[0, 1] && dZ < dVals[0, 2])
            {
                dC0 = dVals[2, 0];
                dC1 = dVals[2, 1];
                dC2 = dVals[2, 2];
                dC3 = dVals[2, 3];
                dC4 = dVals[2, 4];
            }
            else if (dZ >= dVals[0, 2] && dZ < dVals[0, 3])
            {
                dC0 = dVals[3, 0];
                dC1 = dVals[3, 1];
                dC2 = dVals[3, 2];
                dC3 = dVals[3, 3];
                dC4 = dVals[3, 4];
            }
            else
            {
                Console.OpenStandardError();
            }
        }

        protected virtual void Set_Coefficients_PPD(double[,] dVals)
        {
            if (dZ >= dVals[0, 0] && dZ < dVals[0, 1])
            {
                dC0 = dVals[1, 0];
                dC1 = dVals[1, 1];
                dC2 = dVals[1, 2];
                dC3 = dVals[1, 3];
                dC4 = dVals[1, 4];
            }
            else if (dZ >= dVals[0, 1] && dZ < dVals[0, 2])
            {
                dC0 = dVals[2, 0];
                dC1 = dVals[2, 1];
                dC2 = dVals[2, 2];
                dC3 = dVals[2, 3];
                dC4 = dVals[2, 4];
            }
            else if (dZ >= dVals[0, 2] && dZ < dVals[0, 3])
            {
                dC0 = dVals[3, 0];
                dC1 = dVals[3, 1];
                dC2 = dVals[3, 2];
                dC3 = dVals[3, 3];
                dC4 = dVals[3, 4];
            }
            else if (dZ >= dVals[0, 3] && dZ < dVals[0, 4])
            {
                dC0 = dVals[4, 0];
                dC1 = dVals[4, 1];
                dC2 = dVals[4, 2];
                dC3 = dVals[4, 3];
                dC4 = dVals[4, 4];
            }
            else
            {
                Console.OpenStandardError();
            }
        }

    }
}
