using System;

namespace com.amazon.device.iap.cpt
{
	public interface IAmazonIapV2
	{
		void AddGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate);

		void AddGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate);

		void AddGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate);

		void AddPurchaseResponseListener(PurchaseResponseDelegate responseDelegate);

		RequestOutput GetProductData(SkusInput skusInput);

		RequestOutput GetPurchaseUpdates(ResetInput resetInput);

		RequestOutput GetUserData();

		void NotifyFulfillment(NotifyFulfillmentInput notifyFulfillmentInput);

		RequestOutput Purchase(SkuInput skuInput);

		void RemoveGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate);

		void RemoveGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate);

		void RemoveGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate);

		void RemovePurchaseResponseListener(PurchaseResponseDelegate responseDelegate);

		void UnityFireEvent(string jsonMessage);
	}
}