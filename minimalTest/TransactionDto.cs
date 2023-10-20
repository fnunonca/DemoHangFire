namespace minimalTest
{
    public record TransactionDto
    {
        public string DateTimeTransaction { get; init; }
        public decimal Amount { get; init; }
        public string Currency { get; init; }
        public string CodeAuth { get; init; }
    }

    public record MerchantDto
    {
        public string MerchantCode { get; init; }
        public string PhoneNumber { get; init; }
        public string Address { get; init; }
        public string Email { get; init; }
        public string Name { get; init; }
    }

    public record TestIpnRequestDto
    {
        public string OrderNumber { get; init; }
        public string OrderStatus { get; init; }
        public string EndPointTransaction { get; init; }
        public TransactionDto Transaction { get; init; }
        public MerchantDto Merchant { get; init; }
    }

}
