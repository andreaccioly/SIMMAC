using System;
using System.Collections.Generic;
using System.Threading;

namespace SIMMAC
{
    internal class Process
    {
        private List<Instruction> storage;

        public Process(List<Instruction> store)
        {
            this.storage = store;

            //Create an object Register
            Register reg = new Register();

            //Read ach line of Primary Storage and Process
            foreach (Instruction inst in storage)
            {
                //Process program line
                reg.opc(inst);                              
                
                //Output
                Console.WriteLine("****** Output Line " + storage.IndexOf(inst).ToString() + " *********");
                Console.WriteLine(inst.Opcode.ToString() + "  " + inst.Operand.ToString());
                Console.WriteLine("ACC ------>" + reg.ACC.ToString());
                Console.WriteLine("MIR ------>" + reg.MIR.ToString());
                Console.WriteLine("CSIAR----->" + reg.CSIAR.ToString());
                Console.WriteLine("SAR ------>" + reg.SAR.ToString());
                Console.WriteLine("Opcode --->" + inst.Opcode.ToString());
                Console.WriteLine("Operand -->" + inst.Operand.ToString());
                Console.WriteLine("------------------------------------------------------");
                               
            } 
        }
    }
}