namespace UI.InventoryUI
{
    public class InventoryScreenPresenter: ScreenController<InventoryScreenView>
    {
        public InventoryScreenPresenter(InventoryScreenView view) : base(view){}

        public override void Initialize()
        {
            View.CloseClicked += RequestClose;
            base.Initialize();
        }

        public override void Complete()
        {
            View.CloseClicked += RequestClose;
            base.Complete();
        }
    }
}