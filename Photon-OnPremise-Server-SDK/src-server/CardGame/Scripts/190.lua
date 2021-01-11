package.path = package.path .. ";D:/School/Year 3/Minor/Card_Game_Repository/Photon-OnPremise-Server-SDK/src-server/CardGame/Scripts/?.lua"
require 'monster_default'

function OnPlayed(pCard)
	AddAuraEnhancement(pCard, "Adjacent", "Add_Health", 1);
	AddAuraEnhancement(pCard, "Adjacent", "Add_Attack", 1);
end
