using General;

namespace Economy.Farm_House
{
    public class PersonCell : MenuCell
    {
        private ItemBunch _bunch;
        private Inventory _seller;
        private Inventory _buyer;
        private ItemBunch _sellerWallet;
        private ItemBunch _buyerWallet;
        
        public void SetCell(ItemBunch bunch, Inventory invFrom, Inventory invTo)
        {
            // _bunch = bunch;
            // _seller = invFrom;
            // _buyer = invTo;
            //
            // RefreshButton();
        }

        public void Hire()
        {
            
        }
        
        // public void Exchange()
        // {
        //     Item item = _bunch.GetItem();
        //     
        //     var price = item.GetPrice();
        //     
        //     if (!CheckWallets(_buyer, _seller)) return;;
        //     if (!IsEnoughMoney(_buyer, price)) return;
        //
        //     RefreshButton();
        //     CheckCount();
        // }

        private void RefreshButton() =>
                SetButton(_bunch.GetItemIcon(),
                        _bunch.GetItemName(),
                        _bunch.GetCount());
        
        private bool IsEnoughMoney(Inventory inv, int value) =>
                inv.IsExistsItems(GlobalConstants.Money, value);
        
        
        private bool CheckWallets(Inventory buyer, Inventory seller) =>
                buyer.TryGetBunch(GlobalConstants.Money, out _buyerWallet) &&
                seller.TryGetBunch(GlobalConstants.Money, out _sellerWallet);

        private void CheckCount()
        {
            if (_bunch.GetCount() <= 0) Destroy(gameObject);
        }
    }
}