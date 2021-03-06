require("complex")

local C = pd.Class:new():register("complex")

function C:initialize(sel, atoms)
  for k,v in pairs(complex) do
    pd.post("complex." .. tostring(k) .. " = " .. tostring(v))
  end
  self.inlets = 0
  self.outlets = 0
  return true
end
