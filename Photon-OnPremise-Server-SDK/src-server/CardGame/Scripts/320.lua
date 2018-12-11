package.path = package.path .. ";D:/School/Year 3/Minor/Card_Game_Repository/Photon-OnPremise-Server-SDK/src-server/CardGame/Scripts/?.lua"
require 'spell_default'

function OnPlayed(pSpell, pTargets)
	local targets = GetTargetCollection(pSpell, "All_Monsters");
	
	for i = 0, #targets do
		local target = targets[i];
		
		if (target:GetHealth() <= 2) then
			AddEnhancement(target, "Add_Health", -target:GetHealth());
		end
	end
	
	return true;
end