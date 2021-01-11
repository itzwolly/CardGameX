package.path = package.path .. ";D:/School/Year 3/Minor/Card_Game_Repository/Photon-OnPremise-Server-SDK/src-server/CardGame/Scripts/?.lua"
require 'monster_default'

function OnPlayed(pCard)
	local targets = GetTargetCollection(pCard, "Player_Board");
    
	for i = 0, #targets do
		AddEnhancement(targets[i], "Has_Single_Shield", 1);
	end
end