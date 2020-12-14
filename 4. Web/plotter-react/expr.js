import React from 'react'
import { compile } from 'mathjs'

export default class Expression extends React.Component {
    constructor(props) {
        super(props)
        this.onChange.bind(this)
    }

    onChange(e, props) {
        console.log('Inside input change')
        console.log(e.target.value)
        try {
            let code = compile(e.target.value)
            let x = new Array(100)
            for(let i = 0;i<x.length;i++)
                x[i] = -1 + 2 * i / (x.length - 1)
            let y = x.map(xx => code.evaluate({ "x": xx }))
            console.log('New data is set')
            props.onNewData({x: x, y: y})
        }
        catch {
             return
        }  
    }

    render() {
        return (
            <input placeholder='expression' onChange={args => this.onChange(args, this.props)}></input>
        )
    }
}