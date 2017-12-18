/// CSIC-0640 Operating Systems - Summer/2016 - Prof. Dr.Simco
/// Assignment #2
/// Andre Vieira - N01693058
////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMMAC
{
    class Program
    {
        static void Main(string[] args)
        {
            //Default quantum if not informed
            int quantum = 1;
            int lastInstLine = 0;
            int indexLine = 0;
                                    
            //List of files read
            List<String> filesList = new List<String>();
                        
            //Create Job List
            List<Job> jobList = new List<Job>();
            
            try
            {
                {
                    //Get command line parameters                     
                    if (args.Count<string>() == 0)
                    {
                        // Check for null array
                        Console.WriteLine("No argument provided. If you want, enter information as requested. Press Enter.....:"); 
                        Console.ReadLine();

                        //Enter Quantum Value
                        Console.Write("Enter quantum value (default quantum = 1):");
                        quantum = Convert.ToInt32(Console.ReadLine());

                        //Enter Files                        
                        Console.WriteLine("Enter file(s) full path. Type <exit> when finished");
                        while (true) // Loop indefinitely
                        {
                            string line = Console.ReadLine(); // Get string from user
                            if (line == "exit") // Check string                            
                                break;
                            else
                                filesList.Add(line);
                        }
                        Console.WriteLine("Press Any Key to Continue....");
                        Console.ReadLine();
                    }
                    else
                    {
                        for (int i = 0; i < args.Length; i++) // Loop through args
                        {
                            if (i == 0)
                                quantum = Convert.ToInt32(args[0]);
                            else
                            {
                                filesList.Add(args[i]);
                            }                           
                        }
                    }

                    //Process the file SIMMAC assembly commands and decode
                    foreach (String file in filesList)
                    {
                        //Create a the empty Array of Primary Memory
                        ArrayList storage = new ArrayList();
                        for (int i = 0; i < 512; i++)
                        {
                            storage.Add(null);
                        }

                        //Open and read text file with program
                        var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                        {                          
                            string line;

                            //Foreach Program read Program Lines and decode instruction
                            while ((line = streamReader.ReadLine()) != null)
                            {
                                // process the line
                                Instruction decodedInst = new Instruction(line);

                                //Set the index of line  to store and store one decoded line in the List of instructions
                                if (decodedInst.Opcode == 70) // If instruction is LDI, store directly to assigned address
                                {
                                    indexLine = lastInstLine;                                                                                                                                              
                                }
                                else
                                {
                                    indexLine = decodedInst.Operand;                                   
                                }
                                storage.Add(decodedInst);

                                //Check if contents is not null and write, otherwise. Overwrites the content
                                if (storage[indexLine] != null)
                                {
                                    decodedInst.Status = false;

                                    storage.RemoveAt(indexLine);

                                    //Insert the decoded line into Array @ indexLine position
                                    storage.Insert(indexLine, decodedInst);                                    
                                }
                                //Increment the last instruction line
                                lastInstLine++;
                            }
                            //Add decoded file as a Job
                            Job aJob = new Job(storage);
                                                       
                            //Add job to a list of jobs
                            jobList.Add(aJob);                                                          
                        }
                    }
                    //Run the scheduler based on list of jobs
                    RoundRobin schd = new RoundRobin(jobList);

                    //Execute the RR method
                    schd.doRoundRobin(quantum);

                    Console.WriteLine("Press ENTER to finish....");
                    Console.ReadLine();
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }            
        }
    }
}
