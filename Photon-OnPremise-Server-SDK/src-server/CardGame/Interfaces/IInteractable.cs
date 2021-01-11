namespace CardGame {
    public interface IInteractable {
        string GetName();
        int GetId();
        int GetOwnerId();
        int GetBoardIndex();
        int? GetHealth();
        int? GetAttack();
        void SetHealth(int pAmount);
        void SetAttack(int pAmount);
    }
}

