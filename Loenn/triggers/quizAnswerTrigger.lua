local quizAnswerTrigger = {}

quizAnswerTrigger.fieldInformation = {
    index = {fieldType = "integer",},
	controllerID = {fieldType = "string",},
	auto = {fieldType = "boolean",}
}
quizAnswerTrigger.name = "quizAnswerTrigger"
quizAnswerTrigger.placements = {
    name = "quizAnswerTrigger",
    data = {
		index = 0,
		controllerID = "",
		auto = true,
    }
}
quizAnswerTrigger.fieldOrder = {"x","y","width","height","controllerID","index"}

return quizAnswerTrigger