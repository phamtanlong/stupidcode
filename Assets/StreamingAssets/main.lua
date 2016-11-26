print('@main')



local temp = iTweenEvent.__new()
local iTweenEvent_Type = temp.getType ()

local cube = GameObject.find ("Cube")

iTween.moveTo(cube, iTween.hash("position", Vector3.__new(0,13,0), "time", 1, "looptype", "pingPong"))



--[[

cube = GameObject.createPrimitive (PrimitiveType.Cube)

cube.transform.localScale = Vector3.__new(4,4,4) 
cube.transform.setParent (nil)

cubeWatcher = cube.gameObject.addComponent(Watcher_Type)

function handleStart (sender, arg)
	print ("start")
end

function handleOnDisable (sender, arg)
	print ("something is disable")
end

function handleOnEnable (sender, arg)
	print ("something is ENABLE")
end

cubeWatcher.startHappend.add (handleStart)
cubeWatcher.onDisableHappend.add (handleOnDisable)
cubeWatcher.onEnableHappend.add (handleOnEnable)


--]]