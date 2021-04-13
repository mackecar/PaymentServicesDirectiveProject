namespace Banks.ApplicationServiceInterfaces.DTOs
{
    public class DepositDto
    {
        public string PersonalNumber { get; set; }
        public decimal Amount { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }

        public DepositDto() { }
    }
}
