import React from 'react'
import Plot from 'react-plotly.js'
import Expression from './expr.js'

export default class App extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            x: [-3, -2, -1],
            y: [1, 2, 3]  
        }
    }

    render() {
        return (
            <div>
                <Plot data={[{ x: this.state.x, y: this.state.y, type: 'scatter' }]}></Plot>)
                <br></br>
                <Expression onNewData={a => this.setState({x: a.x, y: a.y})}></Expression> 
            </div>)
    }

              
  
}