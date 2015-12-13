-- Author      : Chucksta
-- Create Date : 9/26/2014 8:13:58 AM-


local myName = UnitName("player")

messageRead = "false"

function Frame1_OnLoad()
    --put your event handler logic here
	this:RegisterEvent("CHAT_MSG_SYSTEM")	
end

function getCoords(arg1)
	count = 0
	index = 0
	coordinatesAcquired = "false"
	Xcoordinate = ""
	Ycoordinate = ""
	Zcoordinate = ""
	Orientation = ""
	coordinates = ""
	character = string.sub(arg1,1,1) -- grab the 1st char
	while character ~= "" and coordinatesAcquired == "false" do
		index = index + 1
		character = string.sub(arg1,index,index)
		-- check for X coord
		if character == "X" then
			-- acquire X coordinate
			index = index + 2
			character = string.sub(arg1,index,index)
			while character ~= "Y" do
				Xcoordinate = Xcoordinate .. character
				index = index + 1
				character = string.sub(arg1,index,index)
			end
			-- acquire Y coord
			index = index + 2
			character = string.sub(arg1,index,index)
			while character ~= "Z" do
				Ycoordinate = Ycoordinate .. character
				index = index + 1
				character = string.sub(arg1,index,index)
			end
			-- acquire Z coord
			index = index + 2
			character = string.sub(arg1,index,index)
			while character ~= "O" do
				Zcoordinate = Zcoordinate .. character
				index = index + 1
				character = string.sub(arg1,index,index)
			end
			-- acquire Orientation (removed for now - too many of these was making the animation jerky)
	--		index = index + 12
	--		character = string.sub(arg1,index,index)
	--		while character ~= "g" do
	--			Orientation = Orientation .. character
	--			index = index + 1
	--			character = string.sub(arg1,index,index)
	--		end
			coordinatesAcquired = "true"
		end
	end
	
	coordinates = "XStart" .. Xcoordinate .. "XEnd" .. "YStart" ..  Ycoordinate .. "YEnd" .. "ZStart" ..  Zcoordinate .. "ZEnd"
	SendChatMessage(coordinates, "SAY")
end

function Frame1_OnEvent()
    --put your event handler logic here
	-- react to a system level message (yellow text)
	if event == "CHAT_MSG_SYSTEM" and messageRead == "false" then
		messageRead = "true"
		-- process the text 
		getCoords(arg1)
	else
		messageRead = "false"
	end
end

function Button1_OnClick()
    --put your event handler logic here
	SendChatMessage(".gps", "SAY") -- send out GM .gps request (gets continent level coodinates)
end
