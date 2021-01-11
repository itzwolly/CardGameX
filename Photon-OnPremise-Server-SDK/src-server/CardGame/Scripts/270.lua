package.path = package.path .. ";D:/School/Year 3/Minor/Card_Game_Repository/Photon-OnPremise-Server-SDK/src-server/CardGame/Scripts/?.lua"
require 'spell_default'

function OnPlayed(pSpell, pTargets)
	local amount = #pTargets;
	
	if (amount == 1) then
		local step = 0;
		local totalHealth = 0;
		local totalAttack = 0;
	
		for i = 0, amount do
			local target = pTargets[i];
			
			if (IsEnemy(target)) then
				step = step + 1;
			else 
				step = step - 1;
			end
			
			if (IsMonster(target)) then
				totalHealth = totalHealth + target:GetHealth();
				totalAttack = totalAttack + target:GetAttack();
			end
		end
		
		if (step == 0 and totalHealth > 0 and totalAttack > 0) then
			local newHealth = math.floor(totalHealth / 2);
			local newAttack = math.floor(totalAttack / 2);
			
			for i = 0, amount do
				local target = pTargets[i];
			
				local healthToAdd = newHealth - target:GetHealth();
				local attackToAdd = newAttack - target:GetAttack();
				
				AddEnhancement(target, "Add_Health", healthToAdd);
				AddEnhancement(target, "Add_Attack", attackToAdd);
			end
			
			return true;
		end
	end
	
	return false;
end