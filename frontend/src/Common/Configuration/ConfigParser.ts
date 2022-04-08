import { ISpaConfiguration } from "./ConfigurationModel";

export default class ConfigParser {
    public static getInstance(): ConfigParser {
        if (!ConfigParser.instance) {
            ConfigParser.instance = new ConfigParser();
        }
        return ConfigParser.instance;
    }
    private static instance: ConfigParser;

    public configuration: ISpaConfiguration;
    public initialize(): ISpaConfiguration {
        const config: ISpaConfiguration = require("../../../spa-config.json");
        return config;
    }
}
