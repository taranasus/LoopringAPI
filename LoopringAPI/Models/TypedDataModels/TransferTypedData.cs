﻿using System.Collections.Generic;
using System.Numerics;

namespace LoopringSharp
{
    public class WithdrawTypedData
    {
        public Domain domain { get; set; }
        public Message message { get; set; }
        public string primaryType { get; set; }
        public Types types { get; set; }
        public class Domain
        {
            public BigInteger chainId { get; set; }
            public string name { get; set; }
            public string verifyingContract { get; set; }
            public string version { get; set; }
        }
        public class Types
        {
            public List<Type> EIP712Domain { get; set; }
            public List<Type> Transfer { get; set; }
        }
        public class Message
        {
            public string owner { get; set; }
            public int accountID { get; set; }
            public int tokenID { get; set; }
            public string amount { get; set; }
            public int feeTokenID { get; set; }
            public string maxFee { get; set; }
            public string to { get; set; }
            public string extraData { get; set; }
            public int minGas { get; set; }
            public int validUntil { get; set; }
            public int storageID { get; set; }
        }
    }
    public class TransferTypedData
    {
        public Domain domain { get; set; }
        public Message message { get; set; }
        public string primaryType { get; set; }
        public Types types { get; set; }

        public class Domain
        {
            public BigInteger chainId { get; set; }
            public string name { get; set; }
            public string verifyingContract { get; set; }
            public string version { get; set; }
        }

        public class Message
        {
            public string from { get; set; }
            public string to { get; set; }
            public int tokenID { get; set; }
            public string amount { get; set; }
            public int feeTokenID { get; set; }
            public string maxFee { get; set; }
            public int validUntil { get; set; }
            public int storageID { get; set; }
        }       

        public class Types
        {
            public List<Type> EIP712Domain { get; set; }
            public List<Type> Transfer { get; set; }
        }
    }
}
