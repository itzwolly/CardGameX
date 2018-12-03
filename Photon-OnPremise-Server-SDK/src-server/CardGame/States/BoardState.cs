using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CardGame {

    public class BoardState {
        public const int BOARD_SIDE_SIZE = 5;
        private const int MAX_STACK_SIZE = 128;

        public readonly PlayerState PlayerState;
        
        private int _stackSize;
        private int[] _stack;

        public BoardState(PlayerState pPlayerState) {
            PlayerState = pPlayerState;
            _stack = new int[MAX_STACK_SIZE];
            _stackSize = 0;
        }

        // Attack: Target id, Target Owner id, Target Index (onboard), Attacker id, Attacker Index, Attacker Owner Id, GetAttack, Target id, Target Index, Target Owner Id, GetHealth, Subtract, Set_Health
        public void DamageCalculation(IInteractable pAttacker, IInteractable pTarget) {
            byte[] code = new byte[22];

            code[0] = (byte) BoardKeys.Instruction.Literal;
            code[1] = (byte) pTarget.GetId();
            code[2] = (byte) BoardKeys.Instruction.Literal;
            code[3] = (byte) pTarget.GetOwnerId();
            code[4] = (byte) BoardKeys.Instruction.Literal;
            code[5] = (byte) pTarget.GetBoardIndex();

            code[6] = (byte) BoardKeys.Instruction.Literal;
            code[7] = (byte) pAttacker.GetId();
            code[8] = (byte) BoardKeys.Instruction.Literal;
            code[9] = (byte) pAttacker.GetBoardIndex();
            code[10] = (byte) BoardKeys.Instruction.Literal;
            code[11] = (byte) pAttacker.GetOwnerId();
            code[12] = (byte) BoardKeys.Instruction.Get_Attack;

            code[13] = (byte) BoardKeys.Instruction.Literal;
            code[14] = (byte) pTarget.GetId();
            code[15] = (byte) BoardKeys.Instruction.Literal;
            code[16] = (byte) pTarget.GetBoardIndex();
            code[17] = (byte) BoardKeys.Instruction.Literal;
            code[18] = (byte) pTarget.GetOwnerId();
            code[19] = (byte) BoardKeys.Instruction.Get_Health;

            code[20] = (byte) BoardKeys.Instruction.Subtract;
            code[21] = (byte) BoardKeys.Instruction.Set_Health;

            Interpret(code);
        }

        // EXAMPLE:     Rush 
        // Bytecode:    0x01 (Literal) | 5 (Monster Id), 0x01 (Literal) | 1 (Can Attack, yes or no), 0x13 (Set_Can_Attack)
        //
        // Attack:      Target Index, Attacker Index, GetAttack, Target Index, GetHealth, Subtract, Set_Health
        // Stack:       5           | 4        | 8    | 8-4 = 4  | target = 5, health = 4
        //
        // Attack:      Target id, Target Owner id, Target Index (onboard), Attacker id, Attacker Index, Attacker Owner Id, GetAttack, Target id, Target Index, Target Owner Id, GetHealth, Subtract, Set_Health
        // Stack:       
        public void Interpret(byte[] pByteCode) {
            for (int i = 0; i < pByteCode.Length; i++) {
                byte instruction = pByteCode[i];

                switch (instruction) {
                    case (byte) BoardKeys.Instruction.Literal: {
                            int value = pByteCode[++i];
                            push(value);
                            break;
                        }
                    case (byte) BoardKeys.Instruction.Get_Attack: {
                            int playerOwnerId = pop(); // Owner id
                            int targetIndex = pop(); // Board index
                            int targetId = pop(); // Monster Id

                            if (PlayerState.IsPlayerId(targetId)) { // Check if target is the player
                                Player player = PlayerState.GetPlayerById(targetId);
                                push((int) player.GetAttack());
                            } else { // its a monster
                                Player owner = PlayerState.GetPlayerById(playerOwnerId);
                                MonsterCard monster = owner.BoardSide.Slots[targetIndex];

                                if (monster.Id == targetId) {
                                    push((int) monster.GetAttack());
                                } else {
                                    return; // Something went wrong
                                }
                            }
                            break;
                        }
                    case (byte) BoardKeys.Instruction.Get_Health: {
                            int playerOwnerId = pop(); // Owner id
                            int targetIndex = pop(); // Board index
                            int targetId = pop(); // Monster Id

                            if (PlayerState.IsPlayerId(targetId)) { // Check if target is the player
                                Player player = PlayerState.GetPlayerById(targetId);
                                push((int) player.GetHealth());
                            } else { // its a monster
                                Player owner = PlayerState.GetPlayerById(playerOwnerId);
                                MonsterCard monster = owner.BoardSide.Slots[targetIndex];

                                if (monster != null && monster.Id == targetId) {
                                    push((int) monster.GetHealth());
                                } else {
                                    return; // Something went wrong
                                }
                            }
                            break;
                        }
                    case (byte) BoardKeys.Instruction.Set_Health: {
                            int newHealth = pop();
                            int targetIndex = pop();
                            int playerOwnerId = pop();
                            int targetId = pop();

                            if (!PlayerState.IsPlayerId(targetId)) {
                                Player player = PlayerState.GetPlayerById(playerOwnerId);
                                player.BoardSide.AddOrModifyEnhancement(targetId, targetIndex, BoardSide.BoardEnhancements.Set_Health, newHealth, player.BoardSide.Slots[targetIndex].Health);
                            } else {
                                Player player = PlayerState.GetPlayerById(targetId);
                                player.BoardSide.AddOrModifyEnhancement(targetId, -1, BoardSide.BoardEnhancements.Set_Health, newHealth, player.GetHealth());
                            }
                            break;
                        }
                    case (byte) BoardKeys.Instruction.Set_Can_Attack: {
                            int value = pop();
                            int targetIndex = pop();
                            int playerOwnerId = pop();
                            int targetId = pop();

                            Player player = PlayerState.GetPlayerById(playerOwnerId);
                            object obj = player.BoardSide.GetEnhancementValue(targetId, BoardSide.BoardEnhancements.Can_Attack);

                            if (obj == null) {
                                player.BoardSide.AddOrModifyEnhancement(targetId, targetIndex, BoardSide.BoardEnhancements.Can_Attack, value, false);
                            } else {
                                player.BoardSide.AddOrModifyEnhancement(targetId, targetIndex, BoardSide.BoardEnhancements.Can_Attack, value, (int) obj);
                            }
                            break;
                        }
                    case (byte) BoardKeys.Instruction.Get_Can_Attack: {
                            int playerOwnerId = pop();
                            int targetIndex = pop();
                            int targetId = pop();

                            Player player = PlayerState.GetPlayerById(playerOwnerId);
                            object obj = player.BoardSide.GetEnhancementValue(targetId, BoardSide.BoardEnhancements.Can_Attack);

                            if (obj != null) {
                                push((int) obj);
                            } else {
                                return; // Something went wrong
                            }
                            break;
                        }
                    case (byte) BoardKeys.Instruction.Subtract: {
                            int b = pop();
                            int a = pop();
                            
                            push(b - a);
                            break;
                        }
                    case (byte) BoardKeys.Instruction.None:
                    default:
                        break;
                }
            }
        }

        private void push(int pValue) {
            Debug.Assert(_stackSize < MAX_STACK_SIZE);
            _stack[_stackSize++] = pValue;
        }
        
        private int pop() {
            Debug.Assert(_stackSize > 0);
            return _stack[--_stackSize];
        }

        public class BoardKeys {
            public enum Instruction {
                // Termination
                None = 0x00,
                // Basics bytecode
                Literal = 0x01,
                // Basics Gameplay
                Get_Health = 0x02,
                Set_Health = 0x03,
                Get_Attack = 0x04,
                Set_Attack = 0x05,
                Destroy_Monster = 0x06,
                // Math
                Add = 0x07,
                Subtract = 0x08,
                Divide = 0x09,
                Multiply = 0x10,
                Random = 0x11,
                // Rush
                Get_Can_Attack = 0x12,
                Set_Can_Attack = 0x13,
                // Shielded
                Get_Has_Shield = 0x14,
                Set_Has_Shield = 0x15,
                // Shield up
                Get_Player_Board_Has_Shield = 0x16,
                Set_Player_Board_Has_Shield = 0x17,
                // Empower 
                Get_Adjacent_Monster_Health = 0x18,
                Set_Adjacent_Monster_Health = 0x19,
                Get_Adjacent_Monster_Attack = 0x20,
                Set_Adjacent_Monster_Attack = 0x21,
                // Enraged
                Get_Opponent_Board_Health = 0x22,
                Get_Opponent_Board_Attack = 0x23,
                // Lure
                Get_Priority_Spells = 0x24,
                Set_Priority_Spells = 0x25,
                // Duplicates in deck
                Get_Card_Duplicate_Amount_In_Deck = 0x26,
                // Monster below x hp
                Get_All_Monster_Health = 0x27,
            }
        }
    }
}
