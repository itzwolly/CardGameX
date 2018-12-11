package.path = package.path .. ";D:/School/Year 3/Minor/Card_Game_Repository/Photon-OnPremise-Server-SDK/src-server/CardGame/Scripts/?.lua"
require 'monster_default'

function OnPlayed(pCard)
	AddEnhancement(pCard, "Can_Attack", 1);
	AddEnhancement(pCard, "Has_Single_Shield", 1);
end


