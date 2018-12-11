Monster = {}

function Monster.OnStartTurn(pCard)
	AddEnhancement(pCard, "Can_Attack", 1);
end

function Monster.OnPlayed(pCard)
	AddEnhancement(pCard, "Can_Attack", 0);
end

function Monster.BeforeAttack(pAttacker, pTarget)
	-- TODO: Do something here?
end

function Monster.OnAttack(pAttacker, pTarget)
	local attackerCanAttack = GetNewestEnhancementValue(pAttacker, "Can_Attack");
	
	if (attackerCanAttack == 1) then
		local targetHasShield = GetNewestEnhancementValue(pTarget, "Has_Single_Shield");
		local attackerHasShield = GetNewestEnhancementValue(pAttacker, "Has_Single_Shield");
		
		local targetHealth = pTarget:GetHealth();
		local targetAttack = pTarget:GetAttack();
		
		local attackerHealth = pAttacker:GetHealth();
		local attackerAttack = pAttacker:GetAttack();
		
		if (targetHasShield == 1 and attackerAttack > 0) then
			AddEnhancement(pTarget, "Has_Single_Shield", 0);
		else
			local newHealth = targetHealth - attackerAttack;
			AddEnhancement(pTarget, "Add_Health", -attackerAttack);
		end
		
		if (attackerHasShield == 1 and targetAttack > 0) then
			AddEnhancement(pAttacker, "Has_Single_Shield", 0);
		else
			local newHealth = attackerHealth - targetAttack;
			AddEnhancement(pAttacker, "Add_Health", -targetAttack);
		end
		
		return true;
	end
	
	return false;
end

function Monster.AfterAttack(pAttacker, pTarget)
	AddEnhancement(pAttacker, "Can_Attack", 0);
end

function Monster.OnDeath(pCard)
	CancelAuras(pCard);
end