/// CSIC-0640 Operating Systems - Summer/2016 - Prof. Dr.Simco
/// Assignment #2
/// Andre Vieira - N01693058
////////////////////////////////////////////////////////////////

using System;
using System.Collections;

namespace SIMMAC
{
    class Job
    {
        private int processID;
        private int startPoint = 0;
        private int timeQuantum = 0;
        private bool tempDone = false;
        private int counter = 0;
        private bool done;
        private int processCounter = 0;
        private int count = 0;
        private int accum = 0;
        
        public ArrayList instList; 
        
        public bool getTempDone() { return tempDone; }
        
        public Job(ArrayList iList)
        {
            instList = iList;            
        }        

        public void process(ArrayList store) 
        {
            //Create process ID from instList 
            this.processID = store.GetHashCode();                 

            //Create an object Register
            Register reg = new Register();

            //Read ach line of Primary Storage and Process
            foreach (Instruction inst in store)
            {
                if (inst!= null)
                { 
                    //Process program line
                    reg.opc(inst);
                    
                    //Update instruction flag
                    inst.Status = true;

                    //Update instructions counter
                    count++;

                    //Output
                    //Console.WriteLine("");
                    Console.WriteLine("****** Output Line " + count.ToString() + " *********");
                    Console.WriteLine("Opcode: " + inst.Opcode.ToString() + "  " + "Operand: " + inst.Operand.ToString());
                    Console.WriteLine("ACC ------> " + reg.ACC.ToString());
                    Console.WriteLine("MIR ------> " + reg.MIR.ToString());
                    Console.WriteLine("CSIAR ----> " + reg.CSIAR.ToString());
                    Console.WriteLine("PSIAR ----> " + reg.PSIAR.ToString());
                    Console.WriteLine("TMPR -----> " + reg.CSIAR.ToString());
                    Console.WriteLine("SAR ------> " + reg.SAR.ToString());
                    Console.WriteLine("------------------------------------------------------");                    
                }
            }

            

            //Update process counter
            processCounter++;

            //returns information about the process
            this.processInf();
        }
        
        private void processInf()
        {
            Console.WriteLine("Process " + this.processID + " Entering process " + " StartPoint: " + getStartPoint() + " time Quantum: " + getTimeQuantum());

            for (int i = getStartPoint(); i < getTimeQuantum(); i++)
            {
                Console.WriteLine(i + " Written by Process # " + processID);
                counter++;
            }

            tempDone = true;

            Console.WriteLine("Process " + processID + " Exiting loop with startPoint: " + getStartPoint());            
            
            if (counter == processCounter)
            {
                Console.WriteLine("Process: " + processID + " is done." + " counter = " + processCounter + "\n");            
                done = true;
            }
            if (processCounter % 2 != 0 && counter > processCounter)
            {
                counter = counter - 1;
                if (counter == processCounter)
                {
                    Console.WriteLine("Process: " + processID + " is done." + " counter = " + processCounter + "\n");
                    done = true;
                }
            }
            tempDone = true;            
        }        

        public void setStartPoint(int startPoint)
        {
            this.startPoint = startPoint;
        }

        public int getStartPoint()
        {
            return startPoint;
        }

        public bool isDone()
        {
            return done;
        }

        public void setTimeQuantum(int time)
        {
            this.timeQuantum = time;
        }

        public int getTimeQuantum()
        {
            tempDone = false;
            return this.timeQuantum;
        }

        public int getProcessID()
        {
            return processID;
        }
    }
}
