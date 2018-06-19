using System;
using System.Collections.Generic;
using System.Text;

namespace Blast_Calc
{
    class Hemispherical_Blast : Spherical_Blast
    {
        // This class inherits all members of its parent. 
        // On this way, full functionality is available only by changing the constants.
        // All calculations are based on the Kingery and Bulmash equation for blasts. 
        
        // Constructors:
        public Hemispherical_Blast()
        {
            Start();
        }

        public Hemispherical_Blast(double dsetR, double dsetW)
        {
            Start(dsetR, dsetW);
        }

        public Hemispherical_Blast(double dsetR, double dsetW, int iExplosiveTypeIO)
        {
            Start(dsetR, dsetW, iExplosiveTypeIO);
        }

        protected override void Set_Values()
        {
            // Coefficients for the incident overpressure of a spherical air burst:
            dCIP[0, 0] = 0.06; dCIP[0, 1] = 1.13; dCIP[0, 2] = 10.0; dCIP[0, 3] = 40.0;
            dCIP[1, 0] = 1.3295e-1; dCIP[1, 1] = -2.1712; dCIP[1, 2] = -1.3878; dCIP[1, 3] = -1.0401; dCIP[1, 4] = -3.7148e-1;
            dCIP[2, 0] = 1.3067e-1; dCIP[2, 1] = -2.0672; dCIP[2, 2] = -1.1750; dCIP[2, 3] = 2.1159; dCIP[2, 4] = -8.3460e-1;
            dCIP[3, 0] = 7.8363e-1; dCIP[3, 1] = -4.5738; dCIP[3, 2] = 2.6834; dCIP[3, 3] = -7.2311e-1; dCIP[3, 4] = 0.0;

            // Coefficients for the reflected pressure:
            dCRP[0, 0] = 0.06; dCRP[0, 1] = 0.42; dCRP[0, 2] = 3.45; dCRP[0, 3] = 40.0;
            dCRP[1, 0] = 1.3953; dCRP[1, 1] = 3.0058e-1; dCRP[1, 2] = 4.8121; dCRP[1, 3] = 4.7833; dCRP[1, 4] = 1.5439;
            dCRP[2, 0] = 9.0962e-1; dCRP[2, 1] = -2.6898; dCRP[2, 2] = -1.2237; dCRP[2, 3] = 8.5625e-1; dCRP[2, 4] = 1.4957;
            dCRP[3, 0] = 1.2511; dCRP[3, 1] = -4.7950; dCRP[3, 2] = 2.7741; dCRP[3, 3] = -7.3282e-1; dCRP[3, 4] = 0.0;

            // Coefficients for the incident momentum:
            dCIM[0, 0] = 0.06; dCIM[0, 1] = 0.95; dCIM[0, 2] = 5.97; dCIM[0, 3] = 40.0;
            dCIM[1, 0] = -6.0247e-1; dCIM[1, 1] = 1.1143; dCIM[1, 2] = 1.3760; dCIM[1, 3] = -1.5534; dCIM[1, 4] = -1.0651;
            dCIM[2, 0] = -6.3226e-1; dCIM[2, 1] = -4.1419e-1; dCIM[2, 2] = -2.2475; dCIM[2, 3] = 3.8761; dCIM[2, 4] = -2.2190;
            dCIM[3, 0] = -6.0392e-1; dCIM[3, 1] = -8.4947e-1; dCIM[3, 2] = -5.5334e-2; dCIM[3, 3] = 0.0; dCIM[3, 4] = 0.0;

            // Coefficients for the reflected impulse:
            dC0_RI = -5.3169e-2; dC1_RI = -1.3466; dC2_RI = 2.3258e-1; dC3_RI = -5.9534e-2;

            // Coefficients for the arrival time:
            dCAT[0, 0] = 0.06; dCAT[0, 1] = 1.46; dCAT[0, 2] = 10.0; dCAT[0, 3] = 40.0;
            dCAT[1, 0] = -3.3217e-1; dCAT[1, 1] = 1.8061; dCAT[1, 2] = 4.3653e-1; dCAT[1, 3] = 2.6277e-1; dCAT[1, 4] = 1.5906e-1;
            dCAT[2, 0] = -3.5217e-1; dCAT[2, 1] = 1.9914; dCAT[2, 2] = -1.3049e-1; dCAT[2, 3] = -1.7628e-1; dCAT[2, 4] = 2.4074e-1;
            dCAT[3, 0] = -7.4315e-2; dCAT[3, 1] = 1.5680; dCAT[3, 2] = -1.5812e-1; dCAT[3, 3] = 0.0; dCAT[3, 4] = 0.0;

            // Coefficients for the positive phase duration:
            dCPPD[0, 0] = 0.17; dCPPD[0, 1] = 0.69; dCPPD[0, 2] = 1.00; dCPPD[0, 3] = 2.88; dCPPD[0, 4] = 10.0; dCPPD[0, 5] = 40.0;
            dCPPD[1, 0] = 4.3227e-1; dCPPD[1, 1] = 6.1103; dCPPD[1, 2] = 12.418; dCPPD[1, 3] = 11.021; dCPPD[1, 4] = 3.8670;
            dCPPD[2, 0] = 2.4242e-1; dCPPD[2, 1] = 3.6673; dCPPD[2, 2] = 2.6397; dCPPD[2, 3] = 0.0; dCPPD[2, 4] = 0.0;
            dCPPD[3, 0] = 2.4255e-1; dCPPD[3, 1] = 2.1849; dCPPD[3, 2] = -14.917; dCPPD[3, 3] = 35.106; dCPPD[3, 4] = -23.852;
            dCPPD[4, 0] = -3.2552e-1; dCPPD[4, 1] = 2.7174; dCPPD[4, 2] = -2.7949; dCPPD[4, 3] = 1.0846; dCPPD[4, 4] = 0.0;
            dCPPD[5, 0] = 2.7214e-1; dCPPD[5, 1] = 4.8449e-1; dCPPD[5, 2] = -7.6501e-2; dCPPD[5, 3] = 0.0; dCPPD[5, 4] = 0.0;

            // Coefficients for the shock velocity:
            dCSV[0, 0] = 0.06; dCSV[0, 1] = 1.28; dCSV[0, 2] = 10.0; dCSV[0, 3] = 40.0;
            dCSV[1, 0] = 7.9911e-2; dCSV[1, 1] = -9.7917e-1; dCSV[1, 2] = -5.3612e-1; dCSV[1, 3] = -3.9288e-1; dCSV[1, 4] = -1.4662e-1;
            dCSV[2, 0] = 8.9984e-2; dCSV[2, 1] = -1.1228; dCSV[2, 2] = 3.1104e-1; dCSV[2, 3] = 7.3402e-1; dCSV[2, 4] = -4.5634e-1;
            dCSV[3, 0] = -4.1168e-1; dCSV[3, 1] = -3.4073e-2; dCSV[3, 2] = 0.0; dCSV[3, 3] = 0.0; dCSV[3, 4] = 0.0;

        }

        protected override void Set_Coefficients_PPD(double[,] dVals)
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
            else if (dZ >= dVals[0, 4] && dZ < dVals[0, 5])
            {
                dC0 = dVals[5, 0];
                dC1 = dVals[5, 1];
                dC2 = dVals[5, 2];
                dC3 = dVals[5, 3];
                dC4 = dVals[5, 4];
            }
            else
            {
                Console.OpenStandardError();
            }
        }
    }
}
