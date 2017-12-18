/// CSIC-0640 Operating Systems - Summer/2016 - Prof. Dr.Simco
/// Assignment #2
/// Andre Vieira - N01693058
////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMMAC
{
    class Instruction
    {
        public int _opcode;
        public int _operand;
        public bool _processed;
        
        public int Opcode // Operation
        {
            get { return _opcode; }
            set { _opcode = value; }
        }
        public int Operand // Operand (an integer value)
        {
            get { return _operand; }
            set { _operand = value; }
        }
        public bool Status // Instruction status
        {
            get { return _processed; }
            set { _processed = value; }
        }
        public Instruction(string line)
        {
            string[] ln = line.Split(' ');
            this.DecodeOpcd(ln[0]);
            this.DecodeOper(ln[1]);
            Status = false;            
        }

        public bool getStatus()
        {
            return this.Status;
        }

        public void DecodeOpcd(string code)
        {            
            switch (code)
            {
                case "ADD":
                    Opcode = 10;
                    break;
                case "SUB":
                    Opcode = 20;
                    break;
                case "LDA":
                    Opcode = 30;
                    break;
                case "STR": //Store
                    Opcode = 40;
                    break;
                case "BRH":
                    Opcode = 50;
                    break;
                case "CBR":
                    Opcode = 60;
                    break;
                case "LDI": //Load Imediate
                    Opcode = 70;
                    break;
                case "HLT": //HALT
                    Opcode = 0;
                    break;
            }
        }
        public void DecodeOper(string oper)
        {
            this.Operand = Convert.ToInt32(oper);
        }               
        
    }
}