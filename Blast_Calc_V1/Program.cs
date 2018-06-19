using System;
using System.Globalization;

namespace Blast_Calc
{
    class Program
    {
        
        static void Main(string[] args)
        {
            CultureInfo ic = new CultureInfo("en");
            double dR = 40.00;
            double dW = 2.0;
            int iExplosiveType = 1;
            int iBlastType = 1;

            InputOutput IO_Start = new InputOutput();
            iExplosiveType = IO_Start.Get_ExplosiveType();
            dR = IO_Start.Get_Range();
            dW = IO_Start.Get_Weight();
            iBlastType = IO_Start.Get_BlastType();

            if (iBlastType == 1)
            {
                Spherical_Blast Blast = new Spherical_Blast(dR, dW, iExplosiveType);
                Blast.Output_Calculation();
            }
            else
            {
                Hemispherical_Blast Blast = new Hemispherical_Blast(dR, dW, iExplosiveType);
                Blast.Output_Calculation();
            }
            //FreeBlast.Write_Output(5.0);
            //FreeBlast.Output_Range(5.0);
            Console.ReadKey();
        }

        
        
    }
}
