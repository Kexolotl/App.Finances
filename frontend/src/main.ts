import ConfirmationDialog from "./Common/Components/ConfirmationDialog/ConfirmationDialog.vue";

import Vue from "vue";
import App from "./App.vue";
import MyRouter from "./router";
import vuetify from "./vuetify";

import "vuetify/dist/vuetify.min.css";
import "@mdi/font/css/materialdesignicons.css";
import "./style.scss";

import HttpClient from "./Common/Http/HttpClient";
import { ChangeChecker } from "change-checker";
import { IConfirmationDialogOptions } from "./Common/Components/ConfirmationDialog/ConfirmationDialogModel";
import { MyFilters } from "./filters";
import ConfigParser from "./Common/Configuration/ConfigParser";
import { extend, ValidationObserver, ValidationProvider } from "vee-validate";
import { required, email } from "vee-validate/dist/rules";

const httpClient = HttpClient.getInstance();
const configParser = ConfigParser.getInstance();

const configuration = configParser.initialize();
httpClient.initialize(configuration);
const router = MyRouter.initialize();

// pass in your custom options as second parameter if applicable
Vue.config.productionTip = false;
Vue.component("confirmation-dialog", ConfirmationDialog);
Vue.component("ValidationProvider", ValidationProvider);
Vue.component("ValidationObserver", ValidationObserver);


// No message specified.
extend("email", email);

// Override the default message.
extend("required", required);

new Vue({
	vuetify,
	router,
	render: (h) => h(App)
}).$mount("#app");

Vue.prototype.$http = httpClient;
Vue.prototype.$changeChecker = new ChangeChecker();
Vue.prototype.$showConfirmationDialogAsync = async function(component: any, message: string, title: string, options?: IConfirmationDialogOptions): Promise<boolean> {
	// DO SOMETHING
	const dialog = component.$refs.confirmationDialog;
	return await dialog.open(message, title, options);
};

Vue.filter("formatDate", MyFilters.formatDate);
