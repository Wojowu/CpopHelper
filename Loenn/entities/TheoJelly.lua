local drawableSprite = require("structs.drawable_sprite")
local drawableLine = require("structs.drawable_line")
local drawing = require("utils.drawing")

local TheoJelly = {}

TheoJelly.name = "TheoJelly"
TheoJelly.fieldInformation = {
    bubble = {fieldType = "boolean",},
    preventTransition = {fieldType = "boolean",},
    preventDownTransition = {fieldType = "boolean",},
    killPlayer = {fieldType = "boolean",},
}
TheoJelly.placements = {
    name = "TheoJelly",
    data = {
		bubble = true,
		preventTransition = true,
		preventDownTransition = true,
		killPlayer = true,
    }
}
TheoJelly.depth = -10000
TheoJelly.canResize = {false, false}
TheoJelly.texture = "objects/TheoJelly/idle0"

local texture = "objects/glider/idle0"
function TheoJelly.rectangle(room, entity)
    local sprite = drawableSprite.fromTexture(texture, entity)

    return sprite:getRectangle()
end

return TheoJelly