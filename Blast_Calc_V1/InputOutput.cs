using System;
using System.Collections.Generic;
using System.Text;

namespace Blast_Calc
{
    class InputOutput
    {
        private int iExplosiveType;
        private int iBlastType;
        private double dRange;
        private double dWeight;

        private string sM_notgood = "Invalid value!\n";
        private string sM_good = "Valid value. Next:\n";
        private string m1 = "\nType a string of numbers then press Enter. " +
                "Type '+' anywhere in the text to quit:\n";

        public InputOutput()
        {
            IO_ExplosiveType();
            IO_ExplosiveType_SetValue();
            IO_Range();
            IO_Weight();
            IO_BlastType();
        }

        public int Get_ExplosiveType()
        {
            return iExplosiveType;
        }

        public int Get_BlastType()
        {
            return iBlastType;
        }

        public double Get_Range()
        {
            return dRange;
        }

        public double Get_Weight()
        {
            return dWeight;
        }

        private void IO_ExplosiveType()
        {
            Console.WriteLine("Blast Parameter Calculator" + "\n"
                + "--------------------------" + "\n");
            Console.WriteLine("Possible Explosive Types are:" + "\n"
                + "(1) TNT, (2) Almatol, (3) Composite B, (4) Composite C3, (5) Composite C4, " +
                "(6) HMX, (7) Octol 75/25, (8) PETN, (9) RDX, (10) Cylotol, (11) Tetryl, "
                + "(12) Tritonal" + "\n");
            Console.WriteLine("Please enter a number of an explosive type: " + "\n");
        }

        private void IO_ExplosiveType_SetValue()
        {    
            string sTmp;

            sTmp = Console.ReadLine();
            try
            {
                iExplosiveType = Convert.ToInt32(sTmp);
                if (iExplosiveType >= 1 && iExplosiveType <= 12)
                {
                    Console.WriteLine(sM_good);
                    Console.WriteLine("Eingabe: " + iExplosiveType + "\n");

                }
                else
                    Console.WriteLine(sM_notgood);
            }
            catch (OverflowException e)
            {
                Console.WriteLine("{0} Value read = {1}.", e.Message, sTmp);

                Console.WriteLine(m1);
            }
        }

        private void IO_Range()
        {
            
            Console.WriteLine("Enter range (m): \n");
            if (!double.TryParse(Console.ReadLine(), out dRange))
            {
                // .. error with input
            }
            try
            {
                //dRange = Convert.ToDouble(sTmp);
                if (dRange >= 0.05 && dRange <= 40.0)
                {
                    Console.WriteLine(sM_good);
                    Console.WriteLine(dRange
                        );

                }
                else
                {
                    Console.WriteLine(sM_notgood);
                    Console.WriteLine("Eingabe: " + dRange + "\n");
                }
            }
            catch (OverflowException e)
            {
                Console.WriteLine("{0} Value read = {1}.", e.Message, dRange);

                Console.WriteLine(m1);
            }
        }

        private void IO_Weight()
        {
            string sTmp;

            Console.WriteLine("Enter weight (kg): \n");
            sTmp = Console.ReadLine();
            try
            {
                dWeight = Convert.ToDouble(sTmp);
                if (dWeight >= 0.01 && dRange <= 400.0)
                {
                    Console.WriteLine(sM_good);
                    Console.WriteLine(dWeight);

                }
                else
                    Console.WriteLine(sM_notgood);
            }
            catch (OverflowException e)
            {
                Console.WriteLine("{0} Value read = {1}.", e.Message, sTmp);

                Console.WriteLine(m1);
            }
        }

        private void IO_BlastType()
        {
            string sTmp;


            Console.WriteLine("Which kind of blast do you want to simulate?\n" 
                + "1. Spherical (Free Air) Blast" + "\n" 
                + "2. Hemispherical (Surface) Blast" + "\n");
            sTmp = Console.ReadLine();
            try
            {
                iBlastType = Convert.ToInt32(sTmp);
                if (iBlastType == 1 || iBlastType == 2)
                {
                    Console.WriteLine(sM_good);
                    Console.WriteLine("Eingabe: " + iBlastType + "\n");

                }
                else
                    Console.WriteLine(sM_notgood);
            }
            catch (OverflowException e)
            {
                Console.WriteLine("{0} Value read = {1}.", e.Message, sTmp);

                Console.WriteLine(m1);
            }
        }
    }
}
