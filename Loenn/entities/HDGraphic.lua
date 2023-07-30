local HDGraphic = {}

HDGraphic.name = "HDGraphic"
HDGraphic.fieldInformation = {
	path = {fieldType = "string",},
}
HDGraphic.placements = {
    name = "HDGraphic",
    data = {
		path = "",
		displayScale = 1,
    }
}
HDGraphic.depth = -10000
HDGraphic.canResize = {false, false}
HDGraphic.ignoredFields = {"x","y","_id","_name"}
HDGraphic.fieldOrder = {"path","displayScale"}
HDGraphic.justification = {0.0,0.0}
HDGraphic.texture = "HDGraphic"

return HDGraphic