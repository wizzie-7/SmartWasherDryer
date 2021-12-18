using System;
using System.Data;
using System.Data.SqlClient;
using DataAccessLibrabry;
using Model;

namespace SmartWasherDryer
{
    class SmartWasherDryer
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("==================================================================================================================================================================================================================");
            Console.WriteLine("*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*||*Welcome To Smart Washer & Dryer|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|");
            Console.WriteLine("==================================================================================================================================================================================================================");
           
            DAL dAL = new DAL();

            char use = '\0';
            //char ch = '\0';

            //do
            //{
            repeat2:
                Console.WriteLine("\n");
                Console.WriteLine("Select Following option: ");
                Console.WriteLine("\n");
                Console.WriteLine("1. If you want to see details of Smart Washer & Dryer Press 'D' ");
                Console.WriteLine("\n");
                Console.WriteLine("2. If you are new user press 'N'");
                Console.WriteLine("\n");
                Console.WriteLine("3. If you are new user press Y");
                use = Convert.ToChar(Console.ReadLine());

                if (use == 'd' || use == 'D')
                {
                    Console.WriteLine("A washing machine is a machine which is also one of the home appliances used to wash laundry. The benefits are like;");
                    Console.WriteLine("•	It eliminates the effort needed to wash cloths. ");
                    Console.WriteLine("•	It saves time and makes the work easy. ");
                    Console.WriteLine("•	It saves time and makes the work easy. ");
                    goto repeat2;

                }
                else if (use == 'n' || use == 'N')
                {
                    Console.WriteLine("Enter Following Details.");

                    User user = dAL.GetUser();
                    dAL.AddUser(user);

                    dAL.Get_User();

                    Payment payment = dAL.GetPaymentInfo();
                    dAL.DoPayment(payment);


                    dAL.Start_Washing();
                    dAL.Starts_Drying();
                    Console.WriteLine();
                    //Console.WriteLine("For shutdown the machine press 'S': ");
                    //Console.WriteLine();
                    //Console.WriteLine("For continue the machine press 'C': ");
                    //ch = Convert.ToChar(Console.ReadLine());

                }
                else if (use == 'y' || use == 'Y')
                {

                    Payment payment = dAL.GetPaymentInfo();
                    dAL.DoPayment(payment);
                    dAL.Start_Washing();
                    dAL.Starts_Drying();
                    //Console.WriteLine();
                    //Console.WriteLine("For shutdown the machine press 'S': ");
                    //Console.WriteLine();
                    //Console.WriteLine("For continue the machine press 'C': ");
                    //ch = Convert.ToChar(Console.ReadLine());
                }
                else
                {
                    Console.WriteLine("Please Select Correct Option!!!!!!");
                    goto repeat2;
                }
            //}
            //while (ch == 'S' || ch == 's');


           
        }
    }
}
