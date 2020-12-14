module.exports = {
    entry: './plotter.js',
    module: {
      rules: [
        {
          test: /\.js$/,
          exclude: /(node_modules|bower_components)/,
          use: {
            loader: 'babel-loader',
            options: {
              presets: [ '@babel/preset-react' ]
            }
          }
        }
      ]
    }
}