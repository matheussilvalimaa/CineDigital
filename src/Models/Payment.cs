namespace CineDigital.Models
{
    public class Payment
    {
        public Guid Id { get; set;}
        public Guid ReservationId { get; set;}
        public Reservation Reservation { get; set;}
        public string PaymentId { get; set;}
        public decimal Amount { get; set;}
        public DateTime PaymentDate { get; set;}
        public string Status { get; set;}
    }
}