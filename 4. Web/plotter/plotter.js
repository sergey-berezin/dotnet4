$(_ => {

    let exprBox = $("#expr") 

    exprBox.on('input', _ => {
        let text = exprBox.val()
        try {
            let myCompiledCode = math.compile(text)
            let x = new Array(100)
            for(let i = 0;i<x.length;i++)
                x[i] = -1 + 2 * i / (x.length - 1)
            let y = x.map(xx => myCompiledCode.evaluate({ "x": xx }))
    
            Plotly.newPlot('chart', [{
                x: x,
                y: y,
                type: 'scatter'
            }])
        }
        catch {
            return
        }  
    })
})