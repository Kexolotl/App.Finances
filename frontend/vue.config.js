module.exports = {
	configureWebpack: {
		// Merged into the final Webpack config
		devtool: "source-map"
	},
	devServer: {
		port: "8080"
	},
	chainWebpack: config => {
		config.plugins.delete("prefetch");
	}
};
