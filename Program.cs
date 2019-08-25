using System;

namespace HammingCode
{
    class Program
    {
        
         static class Message
        {

            static public string ReadInfoFromConsole()
            {
                string result = "";

                Console.Write("Input info msg (four symbols \"0\" or \"1\"): ");
                string tmp_string = Console.ReadLine();

                return result = tmp_string;
            }

            static public byte GetInfoNibble(string Infa)
            {
                byte result = 0;
                
                if ((Infa.Length == 4) && StrToBinConvertabilityChecker(Infa))
                    result = Convert.ToByte(Infa, 2);
                Console.WriteLine("PureInfoMessage {0}", Convert.ToString((result), toBase: 2).PadLeft(4, '0'));
                return result;
            }


            static public uint ComputeCheck(uint InfoPart)
            {
                uint result = 0;
                uint mask = 1;
                uint ch0 = (InfoPart & mask) ^ ((InfoPart >> 1) & mask) ^ ((InfoPart >> 3) & mask);
                uint ch1 = (InfoPart & mask) ^ ((InfoPart >> 2) & mask) ^ ((InfoPart >> 3) & mask);
                uint ch2 = ((InfoPart >> 1) & mask) ^ ((InfoPart >> 2) & mask) ^ ((InfoPart >> 3) & mask);
                result = (byte)((ch0 << 4) | (ch1 << 5) | (ch2 << 6));
                
                Console.WriteLine("CheckMessage with zeroes instead of info bits {0} ", Convert.ToString((result), toBase: 2).PadLeft(7, '0'));
                
                return result;
            }

            static public uint ComputeSyndrome(uint Msg)
            {
                uint result = 0;

                uint s0 = (Msg & 1) ^ ((Msg & 2) >> 1) ^ ((Msg & 8) >> 3) ^ ((Msg & 16) >> 4);
                uint s1 = (Msg & 1) ^ ((Msg & 4) >> 2) ^ ((Msg & 8) >> 3) ^ ((Msg & 32) >> 5);
                uint s2 = ((Msg & 2) >> 1) ^ ((Msg & 4) >> 2) ^ ((Msg & 8) >> 3) ^ ((Msg & 64) >> 6);
                result = (byte)((s0 << 4) | (s1 << 5) | (s2 << 6));
                Console.WriteLine("Syndrome: {0} ", Convert.ToString((result), toBase: 2).PadLeft(7, '0'));


                return result;
            }


        }

        static bool StrToBinConvertabilityChecker(string strToCheck)
        {
            bool result = true;
            for(int i = 0; i<strToCheck.Length; i++)
            {
                if (!(  (strToCheck[i] == '0') || (strToCheck[i] == '1')  ))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }


        static uint ErrorGenerarion()
        {
            Random rnd = new Random();
            int result = rnd.Next(1, 7);  // creates a number between 1 and 7

            Console.WriteLine("Error: {0} ", Convert.ToString((1 << (result - 1)), toBase: 2).PadLeft(7, '0'));
            return (uint)result;
        }
        

        static void Main(string[] args)
        {
            string infoBits_4;
            uint CheckPart;
            uint ComplexMSG;
            uint Syndrome;
            uint ErrorBit;
            uint CorruptedMSG;
            
           // Message msg = new Message();
            while (true)
            {
                infoBits_4 = Message.ReadInfoFromConsole();
                if (infoBits_4 == "0000") break;
                uint pureInfo = Message.GetInfoNibble(infoBits_4);
                CheckPart = Message.ComputeCheck(pureInfo);
                ComplexMSG = (pureInfo | CheckPart);
                Console.WriteLine("Whole Message {0}", Convert.ToString(ComplexMSG, toBase: 2).PadLeft(7, '0'));
                Syndrome = Message.ComputeSyndrome(ComplexMSG);
                ErrorBit = ErrorGenerarion();
                CorruptedMSG = ComplexMSG ^ (uint)(1 << (int)(ErrorBit - 1));
                Console.WriteLine("Corrupted Message {0}", Convert.ToString(CorruptedMSG, toBase: 2).PadLeft(7, '0'));
                Syndrome = Message.ComputeSyndrome(CorruptedMSG);

            }
        }
    }
}
