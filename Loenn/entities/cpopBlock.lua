local cpopBlock = {}

cpopBlock.name = "cpopBlock"
cpopBlock.fieldInformation = {
    timer1 = {fieldType = "integer",},
    timer2 = {fieldType = "integer",},
    timer3 = {fieldType = "integer",},
	direction = {fieldType = "string", options = {"Left","Right","Both"}, editable = false}
}
cpopBlock.placements = {
    name = "cpopBlock",
    data = {
		timer1 = 1,
		timer2 = 1,
		timer3 = 1,
		direction = "Both",
		width = 8,
		height = 8
    }
}
cpopBlock.canResize = {false, false}
cpopBlock.ignoredFields = {"width","height","_id","_name"}
cpopBlock.fieldOrder = {"x","y","timer1","timer2","timer3","direction"}
cpopBlock.texture = "cpopBlock"
cpopBlock.justification = {0,0}

return cpopBlock