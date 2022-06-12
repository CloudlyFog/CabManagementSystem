﻿using System.ComponentModel.DataAnnotations.Schema;

namespace CabManagementSystem.Models
{
    public class BankModel
    {
        public Guid ID { get; set; } = Guid.NewGuid(); // id for identification in the database
        public Guid BankID { get; set; }
        public string BankName { get; set; } = string.Empty;
        public int MembersAmount { get; set; }
        public decimal AccountAmount { get; set; }

        [NotMapped]
        public UserModel? User { get; set; } = new();
    }
    public class OperationModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid BankID { get; set; } = Guid.NewGuid();
        public Guid ReceiverID { get; set; } = Guid.NewGuid();
        public Guid SenderID { get; set; } = Guid.NewGuid();
        public decimal TransferAmount { get; set; }
        public StatusOperationCode OperationStatus { get; set; } = StatusOperationCode.Default;
    }
    public enum StatusOperationCode
    {
        Default = 100,
        Successfull = 200,
        Restricted = 300,
        Error = 400,
    }
}
