let math = require('mathjs')

console.log(math.derivative("x*sin(x)+cos(x)", "x").toString())