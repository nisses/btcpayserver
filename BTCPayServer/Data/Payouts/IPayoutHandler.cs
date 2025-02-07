using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BTCPayServer.Abstractions.Models;
using BTCPayServer.Client.Models;
using BTCPayServer.Data;
using BTCPayServer.Payments;
using PayoutData = BTCPayServer.Data.PayoutData;
using Microsoft.AspNetCore.Mvc;
using StoreData = BTCPayServer.Data.StoreData;

public interface IPayoutHandler
{
    public bool CanHandle(PaymentMethodId paymentMethod);
    public Task TrackClaim(PaymentMethodId paymentMethodId, IClaimDestination claimDestination);
    //Allows payout handler to parse payout destinations on its own
    public Task<(IClaimDestination destination, string error)> ParseClaimDestination(PaymentMethodId paymentMethodId, string destination, bool validate);
    public IPayoutProof ParseProof(PayoutData payout);
    //Allows you to subscribe the main pull payment hosted service to events and prepare the handler 
    void StartBackgroundCheck(Action<Type[]> subscribe);
    //allows you to process events that the main pull payment hosted service is subscribed to
    Task BackgroundCheck(object o);
    Task<decimal> GetMinimumPayoutAmount(PaymentMethodId paymentMethod, IClaimDestination claimDestination);
    Dictionary<PayoutState, List<(string Action, string Text)>> GetPayoutSpecificActions();
    Task<StatusMessageModel> DoSpecificAction(string action, string[] payoutIds, string storeId);
    Task<IEnumerable<PaymentMethodId>> GetSupportedPaymentMethods(StoreData storeData);
    Task<IActionResult> InitiatePayment(PaymentMethodId paymentMethodId, string[] payoutIds);
}
