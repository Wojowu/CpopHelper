local checkSubpixel = {}

checkSubpixel.name = "checkSubpixelTrigger"
checkSubpixel.fieldInformation = {
	action = {fieldType = "string", options = {"KillOnIncorrect","SetFlagOnCorrect","SetFlagOnIncorrect"}, editable = false}
}
checkSubpixel.placements = {
    name = "checkSubpixel",
    data = {
        valueX = 0.5,
        valueY = 0.5,
		marginX = 0,
		marginY = 0,
        checkX = true,
        checkY = true,
		action = "KillOnIncorrect",
		flag = "",
		flagValue = true,
    }
}
checkSubpixel.fieldOrder = {"x","y","width","height","valueX","valueY","marginX","marginY","action","flag","checkX","checkY","flagValue"}

return checkSubpixel