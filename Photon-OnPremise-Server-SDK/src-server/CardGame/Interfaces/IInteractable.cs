namespace CardGame {
    public interface IInteractable {
        int GetId();
        int? GetHealth();
        int? GetAttack();
        void SetHealth(int pAmount);
        void SetAttack(int pAmount);
    }
}

