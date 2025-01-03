const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const { sveltePreprocess } = require("svelte-preprocess");

module.exports = {
    entry: "./Frontend/src/Components/MessageWindow.svelte",
    output: {
        path: path.resolve(__dirname, "wwwroot"),
        filename: "[name].[chunkhash].js",
        publicPath: "/",
    },
    resolve: {
        // see below for an explanation
        alias: {
          svelte: path.resolve('node_modules', 'svelte/src/runtime') // Svelte 3: path.resolve('node_modules', 'svelte')
        },
        extensions: ['.mjs', '.js', '.svelte'],
        mainFields: ['svelte', 'browser', '...'],
        conditionNames: ['svelte', 'browser', '...'],
      },
      module: {
        rules: [
          // The following two loader entries are only needed if you use Svelte 5+ with TypeScript.
          // Also make sure your tsconfig.json includes `"target": "ESNext"` in order to not downlevel syntax
          {
            test: /\.svelte\.ts$/,
            use: [ "svelte-loader", "ts-loader"],
          },
          // This is the config for other .ts files - the regex makes sure to not process .svelte.ts files twice
          {
            test: /(?<!\.svelte)\.ts$/,
            loader: "ts-loader",
          },
          {
            // Svelte 5+:
            // test: /\.(svelte|svelte\.js)$/,
            // Svelte 3 or 4:
            test: /\.svelte$/,
            // In case you write Svelte in HTML (not recommended since Svelte 3):
            // test: /\.(html|svelte)$/,
            use: 'svelte-loader'
          },
          {
            // required to prevent errors from Svelte on Webpack 5+, omit on Webpack 4
            // test: /node_modules\/svelte\/.*\.mjs$/,
            // resolve: {
            //   fullySpecified: false
            // }
          },
            {
                test: /\.(svelte)$/,
                use: {
                    loader: 'svelte-loader',
                    options: {
                        emitCss: true,
                        hotReload: true,
                        preprocess: sveltePreprocess({
                          typescript: true,
                        }),
                    },
                },
            },
          ]
    },
    devtool: 'source-map',
    plugins: [
        new CleanWebpackPlugin(),
        new HtmlWebpackPlugin({
            template: "./Frontend/src/app.html",
        }),
    ],
};
