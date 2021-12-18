using System;

namespace Model
{
    public class Payment
    {
        public int Payment_Id { get; set; }
        public int UserID { get; set; }
        public int Payment_Amt { get; set; }
        public string Payment_Sts { get; set; }
        public string Payment_DT { get; set; }
    }
}
