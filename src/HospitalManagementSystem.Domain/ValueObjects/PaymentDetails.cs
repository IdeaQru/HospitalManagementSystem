namespace HospitalManagementSystem.Domain.ValueObjects
{
    public class PaymentDetails
    {
        // Credit Card details
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string CVV { get; set; }
        public DateTime ExpiryDate { get; set; }

        // Insurance details
        public string PolicyNumber { get; set; }
        public decimal CoverageLimit { get; set; }
        public string InsuranceProvider { get; set; }

        // BPJS details
        public string BPJSCardNumber { get; set; }
        public int TreatmentClass { get; set; }
    }
}
