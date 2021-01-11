package.path = package.path .. ";D:/School/Year 3/Minor/Card_Game_Repository/Photon-OnPremise-Server-SDK/src-server/CardGame/Scripts/?.lua"
require 'spell_default'

function OnPlayed(pSpell, pTargets)
	local amount = #pTargets;

	if (amount == 0) then
		math.randomseed(os.time());
		local target = pTargets[0];
		
		if (IsEnemy(target) == false and IsMonster(target)) then
			local rnd = math.random(0, 1);
			if (rnd == 0) then
				AddEnhancement(target, "Add_Health", target:GetHealth());
				AddEnhancement(target, "Add_Attack", target:GetAttack());
			else
				local healthToAdd = math.ceil(target:GetHealth() / 2);
				local attackToAdd = math.ceil(target:GetAttack() / 2);
			
				AddEnhancement(target, "Add_Health", -healthToAdd);
				AddEnhancement(target, "Add_Attack", -attackToAdd);
			end
			return true;
		end
	end
	return false;
end