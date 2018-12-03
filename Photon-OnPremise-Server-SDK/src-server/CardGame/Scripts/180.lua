function OnBeforePlayed(pCard)
	--SetEnhancement(pCard, 0x02, true);
	--SetEnhancement(pCard, 0x04, true);

	SetCanAttack(pCard, true);
	SetHasShield(pCard, true);
end