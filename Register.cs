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
    class Register
    {
        public int _acc;
        public int _psiar;
        public int _sar;
        public int[][] _sdr =  new int[2][] { new int[512], new int[512] };
        public int _tmpr;
        public int _csiar;
        public Instruction _ir=null;
        public int _mir;
        public int accAdr = 0;
                       
        int OPC = 0;
        
        public int ACC // Accumulator; A 32-bit register involved in all arithmetic operations.One of the operands in each arithmetic operation must be in the Accumulator; the other must be in primary storage.
        {
            get { return _acc; }
            set { _acc = value; }
        }
        public int PSIAR // Primary Storage Instruction Address Register; This 16-bit register points to the location in primary storage of the next machine language instruction to be executed.
        {
            get { return _psiar; }
            set { _psiar = value; }
        }
        public int SAR // Storage Address Register; This 16-bit register is involved in all references to primary storage.It holds the address of the location in primary storage being read from or written to.
        {
            get { return _sar; }
            set { _sar = value; }
        }
        public int[][] SDR // Storage Data Register; This 32-bit register is also involved in all references to primary storage.It holds the data being written to or receives the data being read from primary storage at the location specified in the SAR.
        {
            get { return _sdr; }
            set { _sdr = value;}
        }
        public int TMPR // Temporary Register; This 32-bit register is used to extract the address portion(rightmost 16-bits) of the machine instruction in the SDR so that it may be placed in the SAR. (No SDR to SAR transfer.)
        {
            get { return _tmpr; }
            set { _tmpr = value; }
        }
        public int CSIAR //Control Storage Instruction Address Register; This register points to the location of the next micro-instruction(in control storage) to be executed.
        {
            get { return _csiar; }
            set { _csiar = value; }
        }
        public Instruction IR // Instruction Register; This register contains the current instruction being executed.
        {
            get { return _ir; }
            set { _ir = value; }
        }
        public int MIR //Micro-instruction Register; This register contains the current micro-instruction being executed.
        {
            get { return _mir; }
            set { _mir = value; }
        }

        public void opc(Instruction oneInst)
        {
            int q = 1;
            while (q != 0)
            {
                MIR = CSIAR;

                //depending on the MIR value, set the appropriate op code
                switch (MIR)
                {
                    case 0: //Line 0 of fetch
                        OPC = 8;
                        break;
                    case 1:
                        OPC = 1;
                        break;
                    case 2: //Line 2 of fetch
                        OPC = 2;
                        break;
                    case 3:
                        OPC = 5;
                        break;
                    case 10:
                    case 20:
                    case 30:
                    case 40:
                    case 64:
                    case 70:
                        OPC = 21;
                        break;
                    case 11:
                    case 21:
                    case 31:
                    case 41:
                    case 65:
                    case 71:
                        OPC = 23;
                        break;
                    case 12:
                    case 22:
                    case 32:
                    case 42:
                    case 66:
                    case 72:
                        OPC = 20;
                        break;
                    case 13:
                    case 23:
                    case 33:
                    case 43:
                    case 67:
                    case 73:
                        OPC = 18;
                        break;
                    case 14:
                    case 24:
                    case 34:
                    case 44:
                        OPC = 6;
                        break;
                    case 15:
                    case 25:
                    case 35:
                    case 45:
                        OPC = 9;
                        break;
                    case 16:
                    case 26:
                    case 36:
                        OPC = 25;
                        break;
                    case 17:
                    case 27:
                        OPC = 6;
                        break;
                    case 18:
                        OPC = 12;
                        break;
                    case 19:
                    case 29:
                    case 38:
                    case 48:
                    case 51:
                    case 63:
                    case 68:
                    case 75:
                        OPC = 0;
                        break;
                    case 28:
                        OPC = 13;
                        break;
                    case 37:
                    case 74:
                        OPC = 14;
                        break;
                    case 46:
                        OPC = 15;
                        break;
                    case 47:
                        OPC = 26;
                        break;
                    case 50:
                    case 62:
                        OPC = 16;
                        break;
                    case 60:
                        OPC = 17;
                        break;
                    case 61:
                        OPC = 28;
                        break;
                    case 99:
                        OPC = 0;
                        break;
                }
                //depending on the op code, run the corresponding instructions
                switch (OPC)
                {
                    case 0:
                        //HALT the program
                        CSIAR = 0;
                        q = 0;
                        break;
                    case 1:
                        //Read
                        SDR[0][SAR] = oneInst.Opcode;
                        SDR[1][SAR] = oneInst.Operand;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 2:
                        //set CSIAR to SDR instruction address value
                        CSIAR = SDR[0][SAR];
                        //increment CSIAR
                        //CSIAR++;
                        break;
                    case 3:
                        //Read
                        SDR[1][SAR] = oneInst.Operand;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 4:
                        //Read
                        CSIAR = oneInst.Opcode;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 5:
                        IR = oneInst;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 6:
                        //set TEMP registers to value of SDR register value                        
                        TMPR = SDR[1][SAR];
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 8:
                        //set SAR value of PSIAR value
                        SAR = PSIAR;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 9:
                        //set SAR to TMPR value
                        SAR = TMPR;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 11:
                        //set ACC register from PSIAR
                        ACC = PSIAR;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 12:
                        //increment ACC value by TMPR                                                               
                        ACC = ACC + TMPR;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 13:
                        //Decrement ACC value by TMPR
                        ACC = ACC - TMPR;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 14:
                        //set ACC with SDR
                        ACC = SDR[1][SAR];
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 15:
                        //set SDR with ACC
                        SDR[1][SAR] = ACC;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 16:
                        //set PSIAR from SDR
                        PSIAR = SDR[0][SAR];
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 17:
                        //if ACC value is 0, set CSIAR + 2
                        if (ACC == 0)
                            CSIAR = CSIAR + 2;
                        else
                            //increment CSIAR
                            CSIAR = CSIAR + 1;
                        break;
                    case 18:
                        //set ACC from TMPR
                        ACC = TMPR;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 20:
                        //set PSIAR to ACC value
                        PSIAR = ACC;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 21:
                        //set TMPR values to ACC values                        
                        TMPR = ACC;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 22:
                        //accumulate acc values
                        ACC = ACC + 1;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 23:
                        //set  ACC value to PSIAR value plus 1
                        ACC = PSIAR + 1;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 24:
                        //set ACC with TMPR plus 1
                        ACC = TMPR + 1;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 25: //Read
                        //set SDR values to values at primary memory location SAR - Read
                        SDR[0][SAR] = oneInst.Opcode;
                        SDR[1][SAR] = oneInst.Operand;
                        //increment CSIAR
                        CSIAR++;
                        break;
                    case 26: //Write
                        //set values at primary memory location SAR to SDR values - Write
                        oneInst.Opcode  = SDR[0][SAR];
                        oneInst.Operand = SDR[1][SAR];
                        //increment CSIAR
                        CSIAR++;
                        break;
                    //case 27:
                    //    //set CSIAR with SDR
                    //    CSIAR = SDR[0][SAR];
                    //    //increment CSIAR
                    //    CSIAR++;
                    //    break;
                    case 28:
                        //set CSIAR to constant = 64
                        CSIAR = 64;
                        //increment CSIAR
                        CSIAR++;
                        break;
                }
            }
        }

        public int accRef(ref int ACC)
        {
            return ACC;
        }
        public int Add(int acc,ref int accum)
        {
            acc = acc + accum;
            return acc;
        }

        public int setValueByRef(Register reg, ref int address)
        {
            address = reg.ACC;
            return address;
        }

        public int getValueByRef(Register reg, ref int address)
        {
            int val;
            val = reg.ACC;
            return val;
        }
        
    }
}
