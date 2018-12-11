package.path = package.path .. ";D:/School/Year 3/Minor/Card_Game_Repository/Photon-OnPremise-Server-SDK/src-server/CardGame/Scripts/?.lua"
require 'monster_default'

function AfterAttack(pAttacker, pTarget)
	--local adjacentTargets = GetTargetCollection(pTarget, "Adjacent");
	if (IsMonster(pTarget)) then
		local left = GetMonsterUsingIndex(pTarget:GetOwnerId(), pTarget:GetBoardIndex() - 1);
		local right = GetMonsterUsingIndex(pTarget:GetOwnerId(), pTarget:GetBoardIndex() + 1);
		
		if (left ~= nil) then
			AddEnhancement(left, "Add_Health", -2);
		end
		if (right ~= nil) then
			AddEnhancement(right, "Add_Health", -2);
		end
	end
end

