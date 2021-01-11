package.path = package.path .. ";D:/School/Year 3/Minor/Card_Game_Repository/Photon-OnPremise-Server-SDK/src-server/CardGame/Scripts/?.lua"
require 'monster_default'

function OnStartTurn(pCard)
	math.randomseed(os.time());

	local targets = GetTargetCollection(pCard, "Enemy_Board");
	local randomIndex = math.random(0, #targets);
	
	local target = targets[randomIndex];
	if (target ~= nil) then
		AddEnhancement(pCard, "Add_Health", -pCard:GetHealth());
		AddEnhancement(target, "Add_Health", -target:GetHealth());
	end
end