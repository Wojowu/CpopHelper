local quizController = {}

quizController.name = "quizController"
quizController.fieldInformation = {
    options = {fieldType = "integer",},
    numberCorrect = {fieldType = "integer",},
    numberIncorrect = {fieldType = "integer",},
	questionID = {fieldType = "string",},
    revealAnswer = {fieldType = "boolean",},
	answerType = {fieldType = "string", options = {"Text","Image","HighResImage"}, editable = false},
	actionCorrect = {fieldType = "string", options = {"Confetti","SetFlagTrue","SetFlagFalse","None"}, editable = false},
	actionIncorrect = {fieldType = "string", options = {"Kill","SetFlagTrue","SetFlagFalse","None"}, editable = false},
	flag = {fieldType = "string",},
	controllerID = {fieldType = "string",},
}
quizController.placements = {
    name = "quizController",
    data = {
		options = 2,
		numberCorrect = 1,
		numberIncorrect = 1,
		questionID = "",
		revealAnswer = true,
		width = 24,
		height = 24,
		answerType = "Image",
		actionCorrect = "Confetti",
		actionIncorrect = "Kill",
		flag = "",
		controllerID = "",
		displayScale = 1,
    }
}
quizController.depth = -10000
quizController.canResize = {false, false}
quizController.ignoredFields = {"x","y","width","height","_id","_name"}
quizController.fieldOrder = {"questionID","options","numberCorrect","numberIncorrect","answerType","revealAnswer","actionCorrect","actionIncorrect","controllerID","flag","displayScale"}
quizController.justification = {0.0,0.0}
quizController.texture = "quizController"

quizController.nodeLimits = {0,-1}
quizController.nodeLineRenderType = "line"
quizController.nodeVisibility = "always"
quizController.nodeTexture = "quizTile"
quizController.nodeJustification = {0.0,0.0}

return quizController