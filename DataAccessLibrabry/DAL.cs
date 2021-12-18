using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using Model;
using System.Net;
using System.Net.Mail;


namespace DataAccessLibrabry
{
    public class DAL
    {
        static string constr = "data source=DESKTOP-912JH4M\\SQLEXPRESS;initial catalog=SMART_WASHER_DRYER;integrated security=True;";
        DataTable table = new DataTable("allPrograms");

        string emailAdd;
        string firstname;
        public DataTable ExecuteData(string Query)
        {
            DataTable result = new DataTable();

            try
            {
                using (SqlConnection sqlcon = new SqlConnection(constr))
                {
                    sqlcon.Open();
                    SqlCommand cmd = new SqlCommand(Query, sqlcon);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(result);
                    sqlcon.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }

        public bool ExecuteCommand(string queury)
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(constr))
                {
                    sqlcon.Open();
                    SqlCommand cmd = new SqlCommand(queury, sqlcon);
                    cmd.ExecuteNonQuery();
                    sqlcon.Close();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
                throw;
            }
            return true;
        }

        //Payment pmt = new Payment();
        public User GetUser()
        {
            string Fname = string.Empty;
            string Lname = string.Empty;
            string Email = string.Empty;

            Console.Write("First Name: ");
            Fname = Console.ReadLine();

            Console.Write("Last Name: ");
            Lname = Console.ReadLine();

            Console.Write("Email Address: ");
            Email = Console.ReadLine();

            firstname = Fname;
            emailAdd = Email;
            
            User user = new User()
            {
                Fname = Fname,
                Lname = Lname,
                Email = Email
            };
            SendEmail(emailAdd, firstname);
            return user;
        }
        public void AddUser(User user)
        {
            ExecuteCommand(String.Format("Insert into UserInfo(Fname, Lname, Email) values ('{0}','{1}','{2}')", user.Fname, user.Lname, user.Email));
            Console.WriteLine();
            Console.WriteLine("User has been register in SMD.");
        }
        public void Get_User()
        {
            DataTable DT = ExecuteData("select * from UserInfo");// where UserID="+ user.UserID);
            if (DT.Rows.Count > 0)
            {

                Console.Write(Environment.NewLine);
                Console.WriteLine("===================================================================================================================================================================================================================");
                Console.WriteLine("*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*User Details*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*|*");
                Console.WriteLine("===================================================================================================================================================================================================================");
                Console.WriteLine(" UserID |  First Name  |  Last Name  | Email Address  ");
                foreach (DataRow row in DT.Rows)
                {
                    Console.WriteLine(" {0,5} | {1,-15} | {2,-15} | {3,-30} ", row["UserID"].ToString(), row["Fname"].ToString(), row["Lname"].ToString(), row["Email"].ToString());
                    Console.WriteLine();
                }
                Console.WriteLine("====================================================================================================================================================================================================================" + Environment.NewLine);
            }
            else
            {
                Console.Write(Environment.NewLine);
                Console.WriteLine("!!!No Records Found!!!");
                Console.Write(Environment.NewLine);
            }
        }
        public Payment GetPaymentInfo()
        {
            int Payment_Amt; 
            string Payment_Sts = string.Empty;
            string Payment_DT;
            int UserID;

            Console.WriteLine("Enter UserID: ");
            UserID = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter Email Address: ");
            emailAdd = Console.ReadLine();
            Console.WriteLine("Enter First Name: ");
            firstname = Console.ReadLine();

        Reapet:
            Console.WriteLine("Please $5 for start washing process: ");
            Payment_Amt = Convert.ToInt32(Console.ReadLine());

            if(Payment_Amt == 5)
            {
                //Console.WriteLine("Payment Succesfull!!!");
                Payment_Sts = "Successfull";
                
            }
            else
            {
                Console.WriteLine("Please Enter Expected Amount($5)!!!");
                Payment_Sts = "Faild";
                goto Reapet;
            }

            //Payment_DT = DateTime.Now;
            Payment_DT =Convert.ToString( System.DateTime.Now);
            
                    

                //User u = new User();
                //UserID = u.UserID;
                Payment payment = new Payment()
                {
                    UserID = UserID,
                    Payment_Amt = Payment_Amt,
                    Payment_Sts = Payment_Sts,
                    Payment_DT = Payment_DT
                };
            SendEmailPayment(emailAdd, firstname);
            return payment;

            
        }
        public void DoPayment(Payment payment)
        {
            ExecuteCommand(String.Format("Insert into Payment(UserID, Payment_Amt, Payment_Sts, Payment_DT) values ({0},{1},'{2}','{3}')", payment.UserID, payment.Payment_Amt, payment.Payment_Sts, payment.Payment_DT));
            Console.WriteLine();
            Console.WriteLine("Payment Has been Successfuly done to SMD.");
            Console.WriteLine();
            Console.WriteLine("Washing Process has been started.");
        }

        public void Start_Washing()
        {
           


                Console.WriteLine("Washing Process has Started....");
            SendEmail_StartWash(emailAdd, firstname);
            Thread aNewThread = new Thread(() => OnGoingFunction());
                aNewThread.Start();
   
            
        }
        private void OnGoingFunction()
        {
            //Code....
            Thread.Sleep(15000); //100 ms, this is in the thead so it will not stop your winForm
                                 //More code....
            Console.WriteLine("Washing is Done..!");
            SendEmail_CompleteWash(emailAdd, firstname);
        }

        public void Starts_Drying()
        {

            Thread.Sleep(20000);

            Console.WriteLine("Drying Process has Started....");
            SendEmail_StartDrying(emailAdd, firstname);
            Thread aNewThread = new Thread(() => OnGoFunction());
            aNewThread.Start();

        }
        private void OnGoFunction()
        {
            //Code....
            Thread.Sleep(15000); //100 ms, this is in the thead so it will not stop your winForm
                                 //More code....
            Console.WriteLine("Drying is Done..!");
            SendEmail_CompleteDrying(emailAdd, firstname);
        }

        
        public  void SendEmail(string Email,string Fname)
        {
            
            var fromAddress = new MailAddress("jadhavvivek107@gmail.com");
            var fromPassword = "VJsweetdevil@1993";
            var toAddress = new MailAddress(Email);
           

            string subject = "User Credentials";
            string body = String.Format("Hi "+Fname+",\n\nYou have successfuly registered in SWD.\nYour Credentials as follows\nUser ID: \nThanks & Regards,\nSWD");

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })

                smtp.Send(message);

        }
        public void SendEmailPayment(string Email, string Fname)
        {
            var fromAddress = new MailAddress("jadhavvivek107@gmail.com");
            var fromPassword = "VJsweetdevil@1993";
            var toAddress = new MailAddress(Email);
            

            string subject = "Payment Status of SWD";
            string body = String.Format("Hi " + Fname + ",\n\nPayment has been Successfull to SWD.\n \nThanks & Regards,\nSWD");

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })

                smtp.Send(message);

        }
        public void SendEmail_StartWash(string Email, string Fname)
        {
            var fromAddress = new MailAddress("jadhavvivek107@gmail.com");
            var fromPassword = "VJsweetdevil@1993";
            var toAddress = new MailAddress(Email);
            

            string subject = "Washing Status of SWD";
            string body = String.Format("Hi " + Fname + ",\n\nYour washing process has started. It will 60 min. to wash.\n \nThanks & Regards,\nSWD");

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })

                smtp.Send(message);

        }
        public void SendEmail_CompleteWash(string Email, string Fname)
        {
            var fromAddress = new MailAddress("jadhavvivek107@gmail.com");
            var fromPassword = "VJsweetdevil@1993";
            var toAddress = new MailAddress(Email);
            

            string subject = "Washing Status of SWD";
            string body = String.Format("Hi " + Fname + ",\n\nYour washing process has Completed. Now it will go to drying process.\n \nThanks & Regards,\nSWD");

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })

                smtp.Send(message);

        }
        public void SendEmail_StartDrying(string Email, string Fname)
        {
            var fromAddress = new MailAddress("jadhavvivek107@gmail.com");
            var fromPassword = "VJsweetdevil@1993";
            var toAddress = new MailAddress(Email);
            

            string subject = "Drying Status of SWD";
            string body = String.Format("Hi " + Fname + ",\n\nYour drying process has started. It will 60 min. to dry.\n \nThanks & Regards,\nSWD");

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })

                smtp.Send(message);

        }
        public void SendEmail_CompleteDrying(string Email, string Fname)
        {
            var fromAddress = new MailAddress("jadhavvivek107@gmail.com");
            var fromPassword = "VJsweetdevil@1993";
            var toAddress = new MailAddress(Email);
            

            string subject = "Drying Status of SWD";
            string body = String.Format("Hi " + Fname + ",\n\nYour Drying process has Completed. You can take your cloths out of machine. thank you for using out service. Visit agaia.!!!\n \nThanks & Regards,\nSWD");

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })

                smtp.Send(message);

        }
        
    }
}
