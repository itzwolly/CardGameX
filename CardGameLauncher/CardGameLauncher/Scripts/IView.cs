namespace CardGameLauncher.Scripts {

    public interface IView {
        IViewModel ViewModel {
            get;
            set;
        }

        void Show();
    }

}
