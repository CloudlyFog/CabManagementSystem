﻿namespace CabManagementSystem.Models
{
    public class BankModel
    {
        public Guid ID { get; set; } = Guid.NewGuid(); // id for identification in the database
        public Guid BankID { get; set; }
        public string BankName { get; set; } = string.Empty;
        public decimal AccountAmount { get; set; }
    }
    public class OperationModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid BankID { get; set; } = Guid.NewGuid();
        public Guid ReceiverID { get; set; } = Guid.NewGuid();
        public Guid SenderID { get; set; } = Guid.NewGuid();
        public decimal TransferAmount { get; set; }
        public StatusOperationCode OperationStatus { get; set; } = StatusOperationCode.Successfull;
        public OperationKind OperationKind { get; set; }

    }
    public enum StatusOperationCode
    {
        Default = 100,
        Successfull = 200,
        Restricted = 300,
        Error = 400,
    }
    public enum OperationKind
    {
        Accrual = 1,
        Withdraw
    }
}
