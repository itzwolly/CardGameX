package.path = package.path .. ";D:/School/Year 3/Minor/Card_Game_Repository/Photon-OnPremise-Server-SDK/src-server/CardGame/Scripts/?.lua"
require 'monster_default'

function OnPlayed(pCard)
	local targets = GetTargetCollection(pCard, "Enemy_Board");
    
	local totalHealth = 0;
	local totalAttack = 0;
	
	if (#targets > 0) then
		for i = 0, #targets do
			totalHealth = totalHealth + targets[i]:GetHealth();
			totalAttack = totalAttack + targets[i]:GetAttack();
		end
	end
	
	if (totalAttack > 0 or totalHealth > 0) then
		AddEnhancement(pCard, "Add_Health", totalHealth);
		AddEnhancement(pCard, "Add_Attack", totalAttack);
	end
end