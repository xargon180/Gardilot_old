
--PinMap = { D0=16, D1=5, D2=4, D3=0, D4=2, D5=14, D6=12, D7=13, D8=15, D9=3, D10=1, LED_BUILTIN=16 }

local function ternary ( cond , T , F )
    if cond then return T else return F end
end

local function getCodeWordA(sGroup, sDevice, bStatus)
    local sReturn = { }
    local nReturnPos = 1

    for i = 1,5,1 do 
        sReturn[nReturnPos] = ternary(sGroup:sub(i,i) == '0', 'F', '0')
        nReturnPos = nReturnPos + 1
    end
    
    for i = 1,5,1 do 
        sReturn[nReturnPos] = ternary(sDevice:sub(i,i) == '0', 'F', '0')
        nReturnPos = nReturnPos + 1
    end

  sReturn[nReturnPos] = ternary(bStatus, '0', 'F')
  nReturnPos = nReturnPos + 1
  
  sReturn[nReturnPos] = ternary(bStatus, 'F', '0')
  nReturnPos = nReturnPos + 1

  return table.concat(sReturn, "");
end

local function sendTriState(sCodeWord)
  -- turn the tristate code word into the corresponding bit pattern, then send it
    local code = 0;
    local length = 0;

    for i = 1, #sCodeWord do
        code = bit.lshift(code, 2)
        local c = sCodeWord:sub(i,i)

        if  c == 'F' then code = bit.bor(code, 1) end
        if  c == '1' then code = bit.bor(code, 3) end

        length = length + 2
    end

    print(code)
    print(length)

    local protocol = 1
    local pulse_length = 350
    local repat = 10
    rfswitch.send(protocol, pulse_length, repat, 7, code, length)
end

local function switchOn(sGroup, sDevice)
    local triState = getCodeWordA(sGroup, sDevice, true)
    print(triState)
    sendTriState(triState)
end

local function switchOff(sGroup, sDevice)
    local triState = getCodeWordA(sGroup, sDevice, false)
    print(triState)
    sendTriState(triState)
end

switchOn("11110", "10000")
print("switchOn")
tmr.delay(5000000)
switchOff("11110", "10000")
print("switchOff")
