﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoopringAPI
{
    public class Client
    {
        private string _apiKey;
        private string _ethPrivateKey;
        private string _loopringPrivateKey;
        private int _accountId;
        private SecureClient _client;

        /// <summary>
        /// The Object you need in order to communicate with the Loopring API. Recommended to use as a signleton.
        /// </summary>
        /// <param name="apiKey">Your wallet API Key, needed for almost all api calls</param>
        /// <param name="loopringPrivateKey">Your Layer 2 Private Key, needed for most api calls</param>
        /// <param name="ethPrivateKey">Your Layer 1, Ethereum Private Key, needed for some very specific API calls</param>
        /// <param name="accountId">Your Loopring Account ID, used for a surprising amount of calls</param>
        public Client(string apiKey, string loopringPrivateKey, string ethPrivateKey, int accountId, bool useTestNet)
        {
            _apiKey = apiKey;
            _loopringPrivateKey = loopringPrivateKey;
            _ethPrivateKey = ethPrivateKey;
            _client = new SecureClient(useTestNet);
            _accountId = accountId;
        }

        /// <summary>
        /// The Object you need in order to communicate with the Loopring API. Recommended to use as a signleton. Automatically gets the API Key using your Loopring Private Key
        /// </summary>        
        /// <param name="loopringPrivateKey">Your Layer 2 Private Key, needed for most api calls</param>
        /// <param name="ethPrivateKey">Your Layer 1, Ethereum Private Key, needed for some very specific API calls</param>
        /// <param name="accountId">Your Loopring Account ID, used for a surprising amount of calls</param>
        public Client(string loopringPrivateKey, string ethPrivateKey, int accountId, bool useTestNet)
        {
            _loopringPrivateKey = loopringPrivateKey;
            _ethPrivateKey = ethPrivateKey;
            _client = new SecureClient(useTestNet);
            _accountId = accountId;
            _apiKey = ApiKey().Result;
        }

        /// <summary>
        /// Returns the relayer's current time in millisecond
        /// </summary>
        /// <returns>Current time in milliseconds</returns>        
        public Task<long> Timestamp()
        {
            return _client.Timestamp();
        }

        /// <summary>
        /// Gets the current exchange prices between varius cryptos on the Loopring Protocol
        /// </summary>        
        /// <param name="pairs">The tickers to retreive. (Ex. LRC-USDT, LRC-ETH)</param>
        /// <returns>Returns a list of all the ticker details for your requested tickers</returns>
        public Task<List<Ticker>> Ticker(params string[] pairs)
        {
            return _client.Ticker(pairs);
        }

        /// <summary>
        /// Return various configurations of Loopring.io
        /// </summary>
        /// <returns>Fees, exchange address, all sort of useful stuff</returns>
        public Task<ExchangeInfo> ExchangeInfo()
        {
            return _client.ExchangeInfo();
        }

        /// <summary>
        /// Get the ApiKey associated with the user's account.
        /// </summary>
        /// <returns>The api key</returns>
        /// <exception cref="System.Exception">Gets thrown when there's a problem getting info from the Loopring API endpoint</exception>
        public Task<string> ApiKey()
        {
            return _client.ApiKey(_loopringPrivateKey, _accountId);
        }

        /// <summary>
        /// Submit an order to exchange two currencies, but with all the nonsense removed
        /// </summary>
        /// <param name="orderHash">The hash of the order you wish to nuke.</param>
        /// <param name="clientOrderId">The unique order ID of the client</param>
        /// <returns>Returns OrderResult which basically contains the status of your transaction after the cancel was succesfully requested</returns>
        public Task<OrderResult> CancelOrder(string orderHash, string clientOrderId)
        {
            return _client.DeleteOrder(_loopringPrivateKey, _apiKey, _accountId, orderHash, clientOrderId);
        }

        /// <summary>
        /// Submit an order to exchange two currencies, but with all the nonsense removed
        /// </summary>
        /// <param name="sellCurrency">The name of the token you are selling (ETH, LRC, USDT, etc)</param>
        /// <param name="sellAmmount">How much of that token you are selling</param>
        /// <param name="buyCurrency">The name of the token you are buying (ETH, LRC, USDT, etc)</param>
        /// <param name="buyAmmount">How much of that token you are buying</param>        
        /// <param name="orderType">Order types, can be AMM, LIMIT_ORDER, MAKER_ONLY, TAKER_ONLY</param>
        /// <param name="poolAddress">The AMM pool address if order type is AMM</param>
        /// <returns>Returns OrderResult which basically contains the status of your transaction after it was succesfully requested</returns>
        public Task<OrderResult> SubmitOrder(
        string sellCurrency,
        decimal sellAmmount,
        string buyCurrency,
        decimal buyAmmount,
        OrderType orderType,
        string poolAddress = null)
        {
            return _client.SubmitOrder(_loopringPrivateKey, _apiKey, _accountId, sellCurrency, sellAmmount, buyCurrency, buyAmmount, orderType, poolAddress);
        }

        /// <summary>
        /// Submit an order to exchange two currencies
        /// </summary>
        /// <param name="sellToken">The token you are selling</param>
        /// <param name="buyToken">The token you are buying</param>
        /// <param name="allOrNone">Whether the order supports partial fills or not.Currently only supports false as a valid value</param>
        /// <param name="fillAmountBOrS">Fill size by buy token or by sell token</param>
        /// <param name="validUntil">Order expiration time, accuracy is in seconds</param>
        /// <param name="maxFeeBips">Maximum order fee that the user can accept, value range (in ten thousandths) 1 ~ 63</param>
        /// <param name="clientOrderId">An arbitrary, client-set unique order identifier, max length is 120 bytes</param>
        /// <param name="orderType">Order types, can be AMM, LIMIT_ORDER, MAKER_ONLY, TAKER_ONLY</param>
        /// <param name="tradeChannel">	Order channel, can be ORDER_BOOK, AMM_POOL, MIXED</param>
        /// <param name="taker">Used by the P2P order which user specify the taker, so far its 0x0000000000000000000000000000000000000000</param>
        /// <param name="poolAddress">The AMM pool address if order type is AMM</param>
        /// <param name="affiliate">An accountID who will recieve a share of the fee of this order</param>
        /// <returns>Returns OrderResult which basically contains the status of your transaction after it was succesfully requested</returns>
        public Task<OrderResult> SubmitOrder(
        Token sellToken,
        Token buyToken,
        bool allOrNone,
        bool fillAmountBOrS,
        int validUntil,
        int maxFeeBips = 20,
        string clientOrderId = null,
        OrderType? orderType = null,
        TradeChannel? tradeChannel = null,
        string taker = null,
        string poolAddress = null,
        string affiliate = null)

        {
            return _client.SubmitOrder(_loopringPrivateKey, _apiKey, _accountId, sellToken, buyToken, allOrNone, fillAmountBOrS, validUntil, maxFeeBips, clientOrderId, orderType, tradeChannel, taker, poolAddress, affiliate);
        }

        /// <summary>
        /// Requests a new API key from Loopring and starts using it, also returns it for storage.
        /// </summary>
        /// <param name="l2Pk"></param>
        /// <param name="apiKey"></param>
        /// <param name="accountId"></param>
        /// <returns>The new API Key</returns>
        public async Task<string> UpdateApiKey()
        {
            _apiKey = await _client.UpdateApiKey(_loopringPrivateKey, _apiKey, _accountId);
            return _apiKey;
        }

        /// <summary>
        /// Fetches the next order id for a given sold token
        /// </summary>        
        /// <param name="accountId">Loopring account identifier</param>
        /// <param name="sellTokenId">The unique identifier of the token which the user wants to sell in the next order.</param>
        /// <param name="maxNext">Return the max of the next available storageId, so any storageId > returned value is avaliable, to help user manage storageId by themselves. for example, if [20, 60, 100] is avaliable, all other ids < 100 is used before, user gets 20 if flag is false (and 60 in next run), but gets 100 if flag is true, so he can use 102, 104 freely</param>
        /// <returns>Returns an object instance of StorageId which contains the next offchainId and orderId</returns>
        public Task<StorageId> StorageId(int sellTokenId, int maxNext = 0)
        {
            return _client.StorageId(_apiKey, _accountId, sellTokenId, maxNext);
        }

        /// <summary>
        /// Get the details of an order based on order hash.
        /// </summary>        
        /// <param name="orderHash">The hash of the worder for which you want details</param>
        /// <returns>OrderDetails object filled with awesome order details</returns>
        /// <exception cref="System.Exception">Gets thrown when there's a problem getting info from the Loopring API endpoint</exception>
        public Task<OrderDetails> OrderDetails(string orderHash)
        {
            return _client.OrderDetails(_apiKey, _accountId, orderHash);
        }

        /// <summary>
        /// Get how much fee you need to pay right now to carry out a transaction of a specified type
        /// </summary>        
        /// <param name="accountId">Loopring account identifier</param>
        /// <param name="requestType">Off-chain request type</param>
        /// <param name="tokenSymbol">Required only for withdrawls - The token you wish to withdraw</param>
        /// <param name="amount">Required only for withdrawls - how much of that token you wish to withdraw</param>
        /// <returns>Returns the fee amount</returns>
        public Task<OffchainFee> OffchainFee(OffChainRequestType requestType, string tokenSymbol, string amount)
        {
            return _client.OffchainFee(_apiKey, _accountId, requestType, tokenSymbol, amount);
        }

        /// <summary>
        /// Get a list of orders satisfying certain criteria.
        /// </summary>
        /// <param name="market">Trading pair (ex. Trading pair)</param>
        /// <param name="start">Lower bound of order's creation timestamp in millisecond (ex. 1567053142000)</param>
        /// <param name="end">Upper bound of order's creation timestamp in millisecond (ex. 1567053242000)</param>
        /// <param name="side">"BUY" or "SELL"</param>
        /// <param name="statuses">Order statuses to search by</param>
        /// <param name="orderTypes">Order types to search by</param>
        /// <param name="tradeChannels">Trade channels to search by</param>
        /// <param name="limit">How many results per call? Default 50</param>
        /// <param name="offset">How many results to skip? Default 0 </param>
        /// <returns>List of OrderDetails objects containing the searched-for items</returns>
        public Task<List<OrderDetails>> OrdersDetails(
            string market,
            long start,
            long end,
            Side? side,
            List<OrderStatus> statuses,
            List<OrderType> orderTypes,
            List<TradeChannel> tradeChannels,
            int limit = 50,
            int offset = 0)
        {
            return _client.OrdersDetails(_apiKey,_accountId, market,start, end, side, statuses, orderTypes, tradeChannels, limit, offset);
        }
    }
}
